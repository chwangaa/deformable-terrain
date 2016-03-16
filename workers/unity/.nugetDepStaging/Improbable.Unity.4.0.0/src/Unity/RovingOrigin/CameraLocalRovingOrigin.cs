using System;
using Improbable.Math;

namespace Improbable.Unity.RovingOrigin
{
    /// <summary>
    /// A Roving Origin for use when the game has a main camera that needs to be tracked 
    /// </summary>
    public class CameraLocalRovingOrigin : DefaultRovingOriginBase
    {
        protected override Coordinates NewOrigin
        {
            get
            {
                if (UnityEngine.Camera.main != null)
                {
                    var parent = UnityEngine.Camera.main.transform.parent;
                    if (parent != null)
                    {
                        return parent.position.ToCoordinates();
                    }
                    return UnityEngine.Camera.main.transform.position.ToCoordinates();
                }
                throw new InvalidOperationException("No main camera");
            }
        }        
    }
}