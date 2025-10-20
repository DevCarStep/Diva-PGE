// MyUnityPlugin/MathUtils.cs
using System.Numerics;
using System;
using UnityEngine;

namespace DivaPGE
{
    public static class Gen
    {
        public class Tunnel
        {
            public GameObject tunnel_start;
            public GameObject tunnel_end;
            public List<GameObject> possible_objects;
            static int GetNumberOfSnapPoints()
            {
                int count = 0;
                List<GameObject> listOfObjects = new List<GameObject>();
                listOfObjects.Insert(0, tunnel_start);
                listOfObjects.Add(tunnel_end);
                foreach (var parentObject in listOfObjects)
                {
                    Transform parentTransform = parentObject.transform;
                    foreach (Transform child in parentTransform)
                    {
                        if (child.gameObject.name == "snap_point")
                            count++;
                    }
                }
                return count;
            }
            UnityEngine.Vector3[] snapPoints = new UnityEngine.Vector3[GetNumberOfSnapPoints()];
            public static void Generate(float length, float widthX, float heightY, UnityEngine.Vector3[] points)
            {
                foreach (UnityEngine.Vector3 i in points)
                {

                }
            }
        }
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