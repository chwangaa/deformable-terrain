using Improbable.Math;

namespace Improbable.Corelib.Util
{
    public static class Vector3Utils
    {
        public static double SquareDistance(Vector3d pointA, Vector3d pointB)
        {
            return SquareDistance(pointA.X, pointA.Y, pointA.Z, pointB.X, pointB.Y, pointB.Z);
        }

        public static double SquareDistance(UnityEngine.Vector3 pointA, UnityEngine.Vector3 pointB)
        {
            return SquareDistance(pointA.x, pointA.y, pointA.z, pointB.x, pointB.y, pointB.z);
        }

        public static double SquareDistance(UnityEngine.Vector3 pointA, Vector3d pointB)
        {
            return SquareDistance(pointA.x, pointA.y, pointA.z, pointB.X, pointB.Y, pointB.Z);
        }

        public static double SquareDistance(Vector3d pointA, UnityEngine.Vector3 pointB)
        {
            return SquareDistance(pointA.X, pointA.Y, pointA.Z, pointB.x, pointB.y, pointB.z);
        }

        private static double SquareDistance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double deltaX = x1 - x2;
            double deltaY = y1 - y2;
            double deltaZ = z1 - z2;
            return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
        }
    }
}