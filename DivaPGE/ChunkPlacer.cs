using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DivaPGE
{
    public class ChunkPlacer : MonoBehaviour
    {
        public Chunk[] ChunkPrefabs;
        private List<Chunk> spawnedChunks = new List<Chunk>();
        public Chunk FirstChunk;
        private void Start()
        {
            spawnedChunks.Add(FirstChunk);
            for (int i = 0; i < 3; i++)
                SpawnChunk();
        }
        private void Update()
        {
            
        }
        private void SpawnChunk()
        {
            Chunk newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
            newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position - newChunk.Begin.localPosition;
            spawnedChunks.Add(newChunk);
        }
    }
}