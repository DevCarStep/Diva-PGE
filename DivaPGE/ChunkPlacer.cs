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

            if (spawnedChunks.Count == 1)
            {
                newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
                newChunk.transform.position = spawnedChunks[0].Begin.position - newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)].localPosition;
                spawnedChunks.Add(newChunk);
            }
            newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
            int randomizedpoint = UnityEngine.Random.Range(0, newChunk.Points.Length);
            newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].Points[UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count-1].Points.Length)].position - newChunk.Points[randomizedpoint].localPosition;
            //if ()      дописать!!!!
            newChunk.transform.RotateAround(newChunk.Points[randomizedpoint].transform.position, Vector3.up, 180);
            spawnedChunks.Add(newChunk);
        }
    }
}