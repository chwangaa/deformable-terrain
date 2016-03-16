using System;
using Improbable.Core;
using UnityEngine;

namespace IoC
{
	public interface IGameObjectFactory
	{
		GameObject Build(Func<GameObject> constructor);
	}
	
	public class GameObjectFactory: IGameObjectFactory
	{
		[Inject] public IContainer container { set; private get; }
		
		public GameObject Build(Func<GameObject> constructor)
		{
			GameObject go = (GameObject)constructor();
			
			go.SetActive(true);
			
			MonoBehaviour[] components = go.GetComponentsInChildren<MonoBehaviour>();
			
			for (int i = 0; i < components.Length; ++i)
				container.Inject(components[i]);
			
			return go;
		}
	}
}

