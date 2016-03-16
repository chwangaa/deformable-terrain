using Improbable.Math;
using Improbable.Unity.Common.Core.Math;
using UnityEngine;

namespace Improbable.Unity.RovingOrigin
{
    /// <summary>
    /// Selects the origin based on the mid point of all game objects
    /// </summary>
    public class GameObjectMidPointRovingOrigin : DefaultRovingOriginBase
    {
        protected override Coordinates NewOrigin
        {
            get
            {
                var count = 0;
                var localAveragePosition = UnityEngine.Vector3.zero;
                var allObjects = FindObjectsOfType<GameObject>();
                for (var i = 0; i < allObjects.Length; i++)
                {
                    var obj = allObjects[i];
                    if (CoordinateRemapper.IsInLocalCoordinates(obj))
                    {
                        var position = obj.transform.position;
                        if (position.IsFinite())
                        {
                            ++count;
                            localAveragePosition = localAveragePosition + position;   
                        }
                    }
                }
                localAveragePosition = count > 0 ? localAveragePosition / count : UnityEngine.Vector3.zero;
                return localAveragePosition.ToCoordinates();
            }
        }        
    }
}