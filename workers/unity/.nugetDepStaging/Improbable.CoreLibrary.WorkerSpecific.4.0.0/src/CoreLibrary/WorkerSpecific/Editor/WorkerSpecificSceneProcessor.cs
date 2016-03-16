using System;
using Assets.Improbable.Core.TemplateProviders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    /// <summary>
    /// Implements a Scene Processor which sets the WorkerSpecificTemplateProvider to look for Worker-Specific assets
    /// </summary>
    internal class WorkerSpecificSceneProcessor
    {
        public void ProcessScene(string sceneName)
        {
            WorkerSpecificTemplateProvider templateProvider;
            if (GetTemplateProviderFromScene(out templateProvider))
            {
                templateProvider.UseWorkerSpecificNames = true;
            }
            else
            {
                throw new ApplicationException(string.Format("Must add {0} to game object containing {1} component", typeof(WorkerSpecificTemplateProvider), typeof(Bootstrap)));
            }
        }

        private bool GetTemplateProviderFromScene(out WorkerSpecificTemplateProvider templateProvider)
        {
            templateProvider = null;
            var gameObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var gameObject in gameObjects)
            {
                if (GetTemplateProvider(gameObject, out templateProvider))
                {
                    return true;
                }
            }
            return false;
        }

        private bool GetTemplateProvider(GameObject obj, out WorkerSpecificTemplateProvider templateProvider)
        {
            templateProvider = obj.GetComponent<WorkerSpecificTemplateProvider>();
            return templateProvider != null;
        }
    }
}