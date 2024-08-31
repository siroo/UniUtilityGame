using System.Linq;
using UnityEngine;

namespace Game
{
    public static class ExtentionCapsuleCollider
    {
        public static void GetLocalPoint(this CapsuleCollider collider, out Vector3 point1, out Vector3 point2)
        {
            var direction = new Vector3 { [collider.direction] = 1 };
            var offset = collider.height / 2 - collider.radius;
            point1 = collider.center - direction * offset;
            point2 = collider.center + direction * offset;
        }
    }

    public static class ExtentionCharacterController
    {
        public static void GetLocalPoint(this CharacterController collider, out Vector3 point1, out Vector3 point2)
        {
            var direction = Vector3.up;
            var offset = collider.height / 2 - collider.radius;
            point1 = collider.center - direction * offset;
            point2 = collider.center + direction * offset;
        }

        public static void GetWorldPoint(this CharacterController collider, out Vector3 point1, out Vector3 point2)
        {
            GetLocalPoint(collider, out point1, out point2);
            point1 = collider.transform.TransformPoint(point1);
            point2 = collider.transform.TransformPoint(point2);
        }

        public static float GetMaxRaidus(this CharacterController collider)
        {
            float radius = collider.radius;
            var vec3 = collider.transform.TransformVector(radius, radius, radius);
            radius = Enumerable.Range(0, 3).Select(xyz => vec3[xyz]).Select(Mathf.Abs).Max();
            return radius;
        }
    }

    public static class ExtentionVector3
    {
        public static bool IsZero(this Vector3 v, float epsilon = 0.000001f)
        {
            return (Game.Utils.IsZero(v.x, epsilon) && Game.Utils.IsZero(v.y, epsilon) && Game.Utils.IsZero(v.z, epsilon));
        }
    }

    public static class ExtentionFloat
    {
        public static bool IsZero(this float a, float epsilon = 0.000001f)
        {
            return Game.Utils.FloatEquals(a, 0.0f, epsilon);
        }
    }
}
