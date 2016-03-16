using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Improbable.Core.Serialization;
using log4net;

namespace Improbable.Unity.State
{
    public interface IStateMetadataLookup
    {
        string GetCanonicalStateName(Type state);
    }

    internal sealed class StateMetadataLookup : IStateMetadataLookup
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StateMetadataLookup));
        private static readonly IStateMetadataLookup StateMetadata = new StateMetadataLookup();
        private readonly Dictionary<Type, string> StateNames = new Dictionary<Type, string>();
        
        private StateMetadataLookup()
        {
            var stopWatch = new Stopwatch();

            Logger.Info("Generating State name lookup...");
            stopWatch.Start(); 
            foreach (var type in GetAllTypes())
            {
                GetCanonicalStateName(type);
            }
            stopWatch.Stop();
            Logger.InfoFormat("Generating State name lookup took {0}s", stopWatch.Elapsed.TotalSeconds);
        }


        public static IStateMetadataLookup Instance
        {
            get { return StateMetadata; }
        }

        private static Type[] GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {         
                Logger.ErrorFormat("While loading {0}", assembly.Location);
                foreach (var inner in ex.LoaderExceptions)
                {
                    Logger.ErrorFormat("{0}", inner);
                }
                throw;
            }
        }

        private static IEnumerable<Type> GetAllTypes()
        {
            return AppDomain.CurrentDomain.
                             GetAssemblies().
                SelectMany<Assembly, Type>(GetTypes);
        }

        public string GetCanonicalStateName(Type stateType)
        {
            string result;
            if (StateNames.TryGetValue(stateType, out result))
            {
                return result;
            }

            var nameAttribute = stateType.GetCustomAttributes(typeof(CanonicalNameAttribute), true).
                                          OfType<CanonicalNameAttribute>().
                                          SingleOrDefault();
            if (nameAttribute != null)
            {
                result = nameAttribute.Name;
                StateNames[stateType] = result;
                return result;
            }

            if (stateType.BaseType != null)
            {
                result = GetCanonicalStateName(stateType.BaseType);
                if (result != null)
                {
                    StateNames[stateType] = result;
                    return result;
                }
            }

            foreach (var @interface in stateType.GetInterfaces())
            {
                result = GetCanonicalStateName(@interface);
                if (result != null)
                {
                    StateNames[stateType] = result;
                    return result;
                }
            }

            StateNames[stateType] = null; // Negative caching.
            return null;
        }
    }
}