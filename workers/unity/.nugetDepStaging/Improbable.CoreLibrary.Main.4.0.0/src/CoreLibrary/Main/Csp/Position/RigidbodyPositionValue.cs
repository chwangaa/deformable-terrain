using Improbable.Corelib.Util;
using Improbable.Math;
using Improbable.Unity;
using UnityEngine;

namespace Improbable.Corelib.Csp.Position
{
    public class RigidbodyPositionValue : IReadWriteValue<Coordinates>
    {
        private readonly GameObject gameObject;

        public RigidbodyPositionValue(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public Coordinates GetValue()
        {
            return gameObject.transform.position.ToCoordinates();
        }

        public void SetValue(Coordinates value)
        {
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                gameObject.GetComponent<Rigidbody>().MovePosition(value.ToUnityVector());
            }
        }
    }
}