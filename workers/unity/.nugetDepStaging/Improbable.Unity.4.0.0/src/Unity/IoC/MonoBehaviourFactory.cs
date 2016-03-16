using System;
using Improbable.Core;
using UnityEngine;

namespace IoC
{
	public interface IMonoBehaviourFactory
	{
		M Build<M>(Func<M> constructor) where M:MonoBehaviour;
	}
	
	public class MonoBehaviourFactory: IMonoBehaviourFactory
	{
		[Inject] public IContainer container { set; private get; }
		
        // TODO may put initializer into constructor
		public M Build<M>(Func<M> constructor) where M:MonoBehaviour
		{
			M mb = (M)constructor();
			
			container.Inject(mb);
			
			return mb;
		}
	}
}

