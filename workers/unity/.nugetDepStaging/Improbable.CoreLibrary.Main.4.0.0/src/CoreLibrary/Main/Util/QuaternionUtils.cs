using UnityEngine;

namespace Improbable.Corelib.Util
{
    public static class QuaternionUtils
    {
        public static Quaternion ToUnityQuaternion(this Math.Quaternion improbableQuaternion)
        {
            return new Quaternion(improbableQuaternion.X, improbableQuaternion.Y, improbableQuaternion.Z, improbableQuaternion.W);
        }

        public static Math.Quaternion ToNativeQuaternion(this Quaternion unityQuaternion)
        {
            return new Math.Quaternion(unityQuaternion.w, unityQuaternion.x, unityQuaternion.y, unityQuaternion.z);
        }

        public static Math.Quaternion FromEuler(Vector3 euler)
        {
            return Quaternion.Euler(euler).ToNativeQuaternion();
        }
    }
}