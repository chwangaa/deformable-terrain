using System;
using System.Collections.Generic;
using Improbable.Util.Injection;
using UnityEngine;

namespace IoC
{
    public class UnityContainer : Container
    {
        public UnityContainer(InjectionCache injectionCache) : base(injectionCache)
        {
            Bind<IoC.GameObjectFactory>().AsSingle();
            Bind<IoC.IGameObjectFactory>().AsSingle<GameObjectFactory>();

            _mbcache = new Dictionary<Type, KeyValuePair<WeakReference, bool>>();
        }

        public override void Register(System.Type type, System.Type mapper)
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(mapper) == false)
            {
                base.Register(type, mapper);
            }
            else
            {
                throw new Exception("Monobehaviour can be registered only through instance");
            }
        }

        public override void Register(System.Type type)
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(type) == false)
            {
                base.Register(type);
            }
            else
            {
                throw new Exception("Monobehaviour can be registered only through instance");
            }
        }

        public override void Map(System.Type type, object instance)
        {
            if ((instance is MonoBehaviour) == false)
            {
                base.Map(type, instance);
            }
            else
            {
                if (!type.IsAssignableFrom(instance.GetType()))
                {
                    throw new ArgumentException("Trying to register an invalid instance");
                }

                KeyValuePair<WeakReference, bool> valuePair = new KeyValuePair<WeakReference, bool>(new WeakReference(instance), false);

                _mbcache[type] = valuePair;
            }
        }

        protected override object Get(Type contract)
        {
            KeyValuePair<WeakReference, bool> valuePair;
            if (_mbcache.TryGetValue(contract, out valuePair))
            {
                if (valuePair.Key.IsAlive)
                {
                    MonoBehaviour mb = valuePair.Key.Target as MonoBehaviour;

                    if (!valuePair.Value) //has been injected?
                    {
                        //note the cache must be set before the injection to avoid circular dependencies (To improve)
                        _mbcache[contract] = new KeyValuePair<WeakReference, bool>(new WeakReference(mb), true);

                        Inject(mb);
                    }

                    return mb;
                }

                return null;
            }
            return base.Get(contract);
        }

        private readonly Dictionary<Type, KeyValuePair<WeakReference, bool>> _mbcache;
    }
}