using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DivaPGE
{
    public class ChunkPlacer : MonoBehaviour
    {
        public Chunk FirstChunk;
        public Chunk[] ChunkPrefabs;
        private List<Chunk> spawnedChunks = new List<Chunk>();
        public Chunk[] LastChunks;
        public bool RandomLastPosition;
        public float height;
        public float width;
        public int TotalAmountOfElements;
        private void Start()
        {
            Chunk BeginingChunk = Instantiate(FirstChunk);
            BeginingChunk.transform.position = gameObject.transform.position;
            spawnedChunks.Add(FirstChunk);
            for (int i = 0; i < TotalAmountOfElements; i++)
                SpawnChunk();
        }
        private void Update()
        {
            
        }
        private void SpawnChunk()
        {
            Chunk newChunk;
            AttachPoint previousRandomizedPoint;
            AttachPoint newRandomizedPoint;

            if (spawnedChunks.Count == 1)
            {
                newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
                newRandomizedPoint = newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)];
                newChunk.transform.position = spawnedChunks[0].Begin.transform.position - newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)].transform.localPosition;
                if (Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position) != 180f)
                {
                    float differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position);
                    newChunk.transform.RotateAround(newRandomizedPoint.transform.position, Vector3.up, differenceAngle + 180f);
                    differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position);
                    if (differenceAngle + 180f != 180f)
                        Debug.Log($"Поворот не удался, угол остался {Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position)} градусов");
                }
                spawnedChunks[0].Begin.attached = true;
                newRandomizedPoint.attached = true;
                spawnedChunks.Add(newChunk);
            }
            else
            {
                newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
                previousRandomizedPoint = spawnedChunks[spawnedChunks.Count - 1].Points[UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count - 1].Points.Length)];
                while (previousRandomizedPoint.attached != false)
                    previousRandomizedPoint = spawnedChunks[spawnedChunks.Count - 1].Points[UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count - 1].Points.Length)];
                newRandomizedPoint = newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)];
                newChunk.transform.position = previousRandomizedPoint.transform.position - newRandomizedPoint.transform.localPosition;
                if (Vector3.Angle(newRandomizedPoint.transform.position, previousRandomizedPoint.transform.position) != 180f)
                {
                    float differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, previousRandomizedPoint.transform.position);
                    newChunk.transform.RotateAround(newChunk.transform.position, newRandomizedPoint.transform.up, differenceAngle + 180f);
                    differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position);
                    if (differenceAngle + 180f != 180f)
                        Debug.Log($"Поворот не удался, угол остался {Vector3.Angle(newRandomizedPoint.transform.position, previousRandomizedPoint.transform.position)} градусов");
                }
                previousRandomizedPoint.attached = true;
                newRandomizedPoint.attached = true;
                spawnedChunks.Add(newChunk);
            }
        }
    }
}