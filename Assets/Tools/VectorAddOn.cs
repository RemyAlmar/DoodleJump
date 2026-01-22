using UnityEngine;

namespace VectorAddOn
{
    public static class VectorAddOn
    {
        public static Vector2 Abs(this Vector2 _vec) => new(Mathf.Abs(_vec.x), Mathf.Abs(_vec.y));
    }
}
