using System.Numerics;
using System;
using UnityEngine;
using UnityEditor;

namespace DivaPGE
{
    public class ChunkPlacer : MonoBehaviour
    {
        public Chunk[] ChunkPrefabs;
        List<Chunk> spawnedChunks = new List<Chunk>();
        private void Start()
        {

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