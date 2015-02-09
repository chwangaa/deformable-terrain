using UnityEngine;

using System;

using WorldScene.Visualizers;


using Improbable.Demo;



namespace Improbable.Demo {

	class RedFlameVisualizer : MonoBehaviour, IVisualizer {
 
		[Data] private IFlammable Flammable;
		
		void OnEnable()
		{
			Flammable.IsOnFireUpdated += HandleIsOnFireUpdated;
		}

		void HandleIsOnFireUpdated (bool isOnFire)
		{
			if (isOnFire) {
				renderer.material.color = Color.red;
			} else {
				renderer.material.color = Color.blue;
			}
		}
	}
}