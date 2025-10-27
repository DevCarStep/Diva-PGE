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
            int previousRandomizedPoint = UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count - 1].Points.Length);
            int newRandomizedPoint;

            if (spawnedChunks.Count == 1)
            {
                newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
                newRandomizedPoint = UnityEngine.Random.Range(0, newChunk.Points.Length);
                newChunk.transform.position = spawnedChunks[0].Begin.position - newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)].localPosition;
                if (Vector3.Angle(newChunk.Points[newRandomizedPoint].transform.position, spawnedChunks[0].Begin.transform.position) != 180f)
                {
                    float differenceAngle = Vector3.Angle(newChunk.Points[newRandomizedPoint].transform.position, spawnedChunks[0].Begin.transform.position);
                    newChunk.transform.RotateAround(newChunk.Points[newRandomizedPoint].transform.position, Vector3.up, (differenceAngle) + 180f);
                    if ((differenceAngle) + 180f != 180f)
                        Debug.Log($"Поворот не удался, угол остался {Vector3.Angle(newChunk.Points[newRandomizedPoint].transform.position, spawnedChunks[0].Begin.transform.position)} градусов");
                }
                spawnedChunks.Add(newChunk);
            }
            newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
            newRandomizedPoint = UnityEngine.Random.Range(0, newChunk.Points.Length);
            newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].Points[UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count-1].Points.Length)].position - newChunk.Points[newRandomizedPoint].localPosition;
            if (Vector3.Angle(newChunk.Points[newRandomizedPoint].transform.position, spawnedChunks[spawnedChunks.Count - 1].Points[previousRandomizedPoint].transform.position) != 180f)
            {
                float differenceAngle = Vector3.Angle(newChunk.Points[newRandomizedPoint].transform.position, spawnedChunks[spawnedChunks.Count - 1].Points[previousRandomizedPoint].transform.position);
                newChunk.transform.RotateAround(newChunk.Points[newRandomizedPoint].transform.position, Vector3.up, (differenceAngle)+180f);
                if ((differenceAngle) + 180f != 180f)
                    Debug.Log($"Поворот не удался, угол остался {Vector3.Angle(newChunk.Points[newRandomizedPoint].transform.position, spawnedChunks[spawnedChunks.Count - 1].Points[previousRandomizedPoint].transform.position)} градусов");
            }    
            spawnedChunks.Add(newChunk);
        }
    }
}