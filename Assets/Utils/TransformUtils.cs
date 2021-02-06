using UnityEngine;

namespace Utils
{
    public static class TransformUtils
    {
        public static Vector2 Direction(this Transform transform) => transform.up;
    }
}