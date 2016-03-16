using System;
using Improbable.Core;
using UnityEngine;

namespace Improbable.Unity.Common.Core
{
    public class UnityEngineEvents : MonoBehaviour, IEngineEvents
    {
        public event Action GameEnd;

        public event Action Frame;

        private void Update()
        {
            if (Frame != null)
                Frame();
        }

        private void OnDestroy()
        {
            if (GameEnd != null)
                GameEnd();
        }
    }
}