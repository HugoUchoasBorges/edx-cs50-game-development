using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    public static class Constants
    {
        public static Vector2 ScreenBounds;
        public static Vector2 PropScreenBounds => ScreenBounds * 1.5f;
        public static Camera Camera;
    }
}