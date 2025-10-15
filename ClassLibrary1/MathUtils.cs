// MyUnityPlugin/MathUtils.cs
using System.Numerics;
using System;
using UnityEngine;

namespace MyUnityPlugin
{
    public static class MathUtils
    {
        public static float Clamp01(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static UnityEngine.Vector3 RandomPointInCircle(float radius)
        {
            float angle = UnityEngine.Random.Range(0f, 360f);
            float dist = UnityEngine.Random.Range(0f, radius);
            return new UnityEngine.Vector3(Mathf.Cos(angle) * dist, 0, Mathf.Sin(angle) * dist);
        }
    }
}