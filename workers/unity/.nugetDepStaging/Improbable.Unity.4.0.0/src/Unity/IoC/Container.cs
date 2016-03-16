using System;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Util.Injection;

namespace IoC
{
    public class Container : IContainer, IInternalContainer
    {
        private readonly InjectionCache injectionCache;
        private readonly HashSet<object> _injectLater;
        private readonly Dictionary<Type, IProvider> _providers;
        private readonly Dictionary<IProvider, object> _uniqueInstances;

        public Container(InjectionCache injectionCache)
        {
            this.injectionCache = injectionCache;
            _providers = new Dictionary<Type, IProvider>();
            _uniqueInstances = new Dictionary<IProvider, object>();
            _injectLater = new HashSet<object>();
        }

        //
        // IContainer interface
        //

        public IBinder<TContractor> Bind<TContractor>()
        {
            IBinder<TContractor> binder = BinderProvider<TContractor>();

            binder.Bind<TContractor>(this);

            return binder;
        }

        public TContractor Build<TContractor>() where TContractor : class
        {
            Type contract = typeof(TContractor);

            var instance = Get(contract) as TContractor;
            if (instance == null)
            {
                throw new Exception("IoC.Container instance failed to be built (contractor not found)");
            }

            return instance;
        }

        public void Release<TContractor>() where TContractor : class
        {
            Type type = typeof(TContractor);

            if (_providers.ContainsKey(type))
            {
                IProvider provider = _providers[type];

                if (_uniqueInstances.ContainsKey(provider))
                {
                    _uniqueInstances.Remove(provider);
                }

                _providers.Remove(type);
            }
        }

        public void Inject<TContractor>(TContractor instance)
        {
            if (instance != null)
            {
                InternalInject(instance);
            }
            else
            {
                Console.WriteLine("Inject instance null");
            }
        }

        //
        // IInternalContainer interface
        //

        public virtual void Register(Type type, Type mapper)
        {
            if (!type.IsAssignableFrom(mapper))
            {
                throw new ArgumentException(string.Format("Canot register type {0} with mapper {1}.", type, mapper));
            }

            _providers[type] = new StandardProvider(mapper);
        }

        public virtual void Register(Type type, IProvider provider)
        {
            if (!type.IsAssignableFrom(provider.Contract))
            {
                throw new ArgumentException(string.Format("Cannot register type {0} with provider with contract {1}.", type, provider.Contract));
            }

            _providers[type] = provider;
        }

        public virtual void Register(Type type)
        {
            _providers[type] = new StandardProvider(type);
        }

        public virtual void Map(Type type, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            if (!type.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Trying to register an invalid instance");
            }

            _injectLater.Add(instance);

            _providers[type] = new StandardProvider(type);

            _uniqueInstances[_providers[type]] = instance;
        }

        //
        // Private Members
        //

        private void InternalInject(object injectable)
        {
            if (injectable == null)
            {
                throw new ArgumentNullException();
            }

            IMemberAdapter[] injectionAdapters = injectionCache.GetAdapters(injectable);
            for (int i = 0; i < injectionAdapters.Length; i++)
            {
                var adapter = injectionAdapters[i];
                Type adapterType = adapter.TypeOfMember;
                object injectee = adapterType == typeof(IContainer) ? this : Get(adapterType);
                if (injectee != null)
                {
                    adapter.SetValue(injectable, injectee);
                }
            }

            if (injectable is IInitialize)
            {
                (injectable as IInitialize).OnInject();
            }
        }

        protected virtual object Get(Type contract)
        {
            IProvider provider;
            if (_providers.TryGetValue(contract, out provider))
            {
                //take the provider linked to the contract
                //N.B. several contracts could be linked
                //to the provider of the same class
                //the contract is actually the provider type
                object instance;
                if (_uniqueInstances.TryGetValue(provider, out instance))
                {
                    if (_injectLater.Contains(instance))
                    {
                        InternalInject(instance);

                        _injectLater.Remove(instance);
                    }

                    return instance;
                }
                return CreateDependency(provider);
            }
            Console.WriteLine("Get not Found for contract: " + contract);
            return null;
        }

        protected virtual IBinder<TContractor> BinderProvider<TContractor>()
        {
            return new Binder<TContractor>();
        }

        private object CreateDependency(IProvider provider)
        {
            object obj = provider.Create();

            _uniqueInstances[provider] = obj; //seriously, this must be done before obj is injected to avoid circular dependencies

            InternalInject(obj);

            return obj;
        }
    }
}