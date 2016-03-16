using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Improbable.Entity.State;
using Improbable.Unity.State;
using Improbable.Util.Injection;
using IoC;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Visualizer
{
    internal interface IVisualizerMetadataLookup
    {
        InjectionCache StaticInjectionCache { get; }
        bool IsVisualizer(Type visualizerType);
        bool AreAllRequiredFieldsInjectable(Type visualizerType);
        bool DontEnableOnStart(Type visualizerType);
        IMemberAdapter GetFieldInfo(Type stateType, Type visualizerType);

        IMemberAdapter[] GetRequiredReadersWriters(Type visualizerType);
        IMemberAdapter[] GetRequiredWriters(Type visualizerType);
        string[] GetRequiredReaderStateNames(Type visualizerType); 
        
        bool IsWriter(IMemberAdapter fieldInfo);
        bool IsReader(IMemberAdapter fieldInfo);
    }

    internal sealed class VisualizerMetadataLookup : IVisualizerMetadataLookup
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VisualizerMetadataLookup));

        private static readonly InjectionCache InjectionCache =
            new InjectionCache(new MemberAdapterFactory(typeof(InjectAttribute)));

        private static readonly VisualizerMetadataLookup ObjectInstance =
            new VisualizerMetadataLookup(StateMetadataLookup.Instance);

        private readonly IStateMetadataLookup stateMetadataLookup;
        private readonly InjectionCache visualizerInjectionCache;

        private readonly HashSet<Type> visualizers;
        private readonly Dictionary<Type, IMemberAdapter[]> visualizerRequiredReadersWriters;
        private readonly Dictionary<Type, IMemberAdapter[]> visualizerRequiredWriters;
        private readonly Dictionary<Type, string[]> visualizerRequiredReaderStateNames;
        private readonly HashSet<Type> visualizersToNotAutoEnableOnStart;
        private readonly HashSet<Type> visualizersWithNonInjectableRequiredFields;

        private readonly HashSet<Type> readers;
        private readonly HashSet<Type> writers; 

        public static IVisualizerMetadataLookup Instance
        {
            get { return ObjectInstance; }
        }

        public InjectionCache StaticInjectionCache
        {
            get { return InjectionCache; }
        }

        private VisualizerMetadataLookup(IStateMetadataLookup stateMetadataLookup)
        {
            var stopWatch = new Stopwatch();

            Logger.Info("Generating Visualizer reflection lookups...");
            stopWatch.Start();
            this.stateMetadataLookup = stateMetadataLookup;
            visualizerInjectionCache = new InjectionCache(new MemberAdapterFactory(typeof(RequireAttribute)));
            visualizersWithNonInjectableRequiredFields = new HashSet<Type>();

            var types = GetAssemblyTypes();
            visualizers = new HashSet<Type>(types.Where(IsVisualizerInternal));
            CheckForVisualizerOnlyAttributes(types.Where(type => !visualizers.Contains(type)));

            var validVisualizers = visualizers.Where(visualizer => !visualizersWithNonInjectableRequiredFields.Contains(visualizer));

            var requiredFields = visualizerInjectionCache.GetAllInjectedTypes();
            //GetCustomAttributes doesn't walk hierarchy when passed true.
            writers = new HashSet<Type>(requiredFields.Where(fieldType => fieldType.GetCustomAttributes(typeof(WriterInterfaceAttribute), false).Any()));
            readers = new HashSet<Type>(requiredFields.Where(fieldType => fieldType.GetCustomAttributes(typeof(ReaderInterfaceAttribute), false).Any()));

            visualizersToNotAutoEnableOnStart = new HashSet<Type>(visualizers.Where(DontEnableOnStartInternal));

            visualizerRequiredReadersWriters = visualizers.ToDictionary<Type, Type, IMemberAdapter[]>(visualizerType => visualizerType, visualizerType => GetRequiredFieldsWithFilter(visualizerType, adapter => IsReader(adapter) || IsWriter(adapter)));
            visualizerRequiredWriters = visualizers.ToDictionary<Type, Type, IMemberAdapter[]>(visualizerType => visualizerType, memberAdapter => GetRequiredFieldsWithFilter(memberAdapter, IsWriter));
            visualizerRequiredReaderStateNames = visualizers.ToDictionary<Type, Type, string[]>(visualizer => visualizer, GetVisualizerRequiredReaderStateNames);

            visualizersWithNonInjectableRequiredFields.UnionWith(validVisualizers.Where(fieldType => !CheckAllRequiredFieldsInjectable(fieldType)));

            stopWatch.Stop();
            Logger.InfoFormat("Generating Visualizer reflection lookups took {0}s", stopWatch.Elapsed.TotalSeconds);
        }

        private void CheckForVisualizerOnlyAttributes(IEnumerable<Type> types)
        {
            foreach(var type in types)
            {
                if (type.GetCustomAttributes(typeof(DontAutoEnableAttribute), false).Any())
                {
                    Logger.WarnFormat("{0} uses DontAutoEnableAttribute but is not a managed behaviour as it has no [Require] or [Inject] fields. The attribute will be ignored.", type.FullName);
                }
            }
        }

        public bool IsVisualizer(Type visualizerType)
        {
            return visualizers.Contains(visualizerType);
        }

        public bool RequiresWriter(Type visualizer)
        {
            return visualizerRequiredWriters[visualizer].Length > 0;
        }

        public IMemberAdapter[] GetRequiredWriters(Type visualizerType)
        {
            return visualizerRequiredWriters[visualizerType];
        }

        public bool AreAllRequiredFieldsInjectable(Type visualizer)
        {
            return !visualizersWithNonInjectableRequiredFields.Contains(visualizer);
        }

        public string[] GetRequiredReaderStateNames(Type visualizerType)
        {
            return visualizerRequiredReaderStateNames[visualizerType];
        }

        public bool IsWriter(IMemberAdapter fieldInfo)
        {
            return writers.Contains(fieldInfo.TypeOfMember);
        }

        public bool IsReader(IMemberAdapter fieldInfo)
        {
            return readers.Contains(fieldInfo.TypeOfMember);
        }

        public IMemberAdapter[] GetRequiredReadersWriters(Type visualizerType)
        {
            return visualizerRequiredReadersWriters[visualizerType];
        }

        public bool DontEnableOnStart(Type visualizerType)
        {
            return visualizersToNotAutoEnableOnStart.Contains(visualizerType);
        }

        public IMemberAdapter GetFieldInfo(Type stateType, Type visualizerType)
        {
            return visualizerInjectionCache.GetAdapterForType(visualizerType, stateType);
        }

        private static bool DontEnableOnStartInternal(Type visualizerType)
        {
            return visualizerType.GetCustomAttributes(typeof(DontAutoEnableAttribute), true).Length > 0;
        }

        private bool CheckAllRequiredFieldsInjectable(Type visualizerType)
        {
            var badParams = visualizerInjectionCache
                .GetAdaptersForType(visualizerType)
                .Except(visualizerRequiredReadersWriters[visualizerType])
                .ToList();
            foreach (var nonReaderWriterRequire in badParams)
            {
                Logger.ErrorFormat("The [Require] attribute can only be used on state Readers and Writers. {0} {1} is not one of those in visualizer {2}. The visualizer won't be enabled.",
                                   nonReaderWriterRequire.TypeOfMember.FullName, nonReaderWriterRequire.Member.Name, visualizerType.FullName);
            }
            return badParams.Count == 0;
        }

        private string[] GetVisualizerRequiredReaderStateNames(Type visualizerType)
        {
            return GetRequiredFieldsWithFilter(visualizerType, IsReader)
                .Select(memberAdapter => stateMetadataLookup.GetCanonicalStateName(memberAdapter.TypeOfMember))
                .ToArray();
        }

        private IMemberAdapter[] GetRequiredFieldsWithFilter(Type visualizerType, Func<IMemberAdapter, bool> predicate)
        {
            return visualizerInjectionCache
                .GetAdaptersForType(visualizerType)
                .Where(predicate)
                .ToArray();
        }

        private IEnumerable<Type> GetAssemblyTypes()
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes());
        }
  
        private bool IsVisualizerInternal(Type visualizerType)
        {
            if (visualizerType.IsAbstract || visualizerType.IsInterface)
            {
                return false;
            }
            var registered = TryRegisterVisualizer(visualizerType);
            return
                !registered //If registering failed, must have been a visualizer
                || InjectionCache.GetAdaptersForType(visualizerType).Length > 0
                || visualizerInjectionCache.GetAdaptersForType(visualizerType).Length > 0; // Any type with either engine or Required, or Data attributes
        }

        private bool TryRegisterVisualizer(Type visualizer)
        {
            try
            {
                visualizerInjectionCache.RegisterType(visualizer);
                return true;
            }
            catch (ArgumentException e)
            {
                Logger.ErrorFormat(e.Message);
                visualizersWithNonInjectableRequiredFields.Add(visualizer);
                return false;
            }
        }
    }
}
