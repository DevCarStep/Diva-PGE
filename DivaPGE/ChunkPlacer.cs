using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using ProcedureMapGenerator;
using System.Linq;
using System.Data;

namespace DivaPGE
{
    public class ChunkPlacer : MonoBehaviour
    {
        public Chunk FirstChunk;
        public Chunk[] ChunkPrefabs;
        private Transform origin;
        private List<Chunk> spawnedChunks = new List<Chunk>();
        public Chunk[] LastChunks;
        public bool RandomLastPosition;
        public int height;
        public int width;
        public int TotalAmountOfElements;
        public ProcedureMapGenerator.ProcedureGenerator.MapStyle mapStyle;
        private void Start()
        {
            ProcedureGenerator procedureGenerator = new ProcedureGenerator(width, height);

            procedureGenerator.GenerateMapWithStyle(mapStyle, TotalAmountOfElements);

            ProcedureMapGenerator.Chunk[,] virtualMap = procedureGenerator.GetMap();

            int rotateTimes;

            for (int x = 0; x < virtualMap.GetLength(0); x++)
            {
                for (int z = 0; z < virtualMap.GetLength(1); z++)
                {
                    Chunk printingChunk;
                    rotateTimes = 0;
                    Vector3 offset;

                    if (virtualMap[x, z] == null) { continue; }

                    printingChunk = ChooseRightOneRandomChunk(virtualMap[x, z], out rotateTimes);
                    offset = new Vector3(x, origin.position.y, z);

                    printingChunk = Instantiate(printingChunk);
                    printingChunk.transform.position = offset;
                    printingChunk.transform.Rotate(0, 90 * rotateTimes, 0);

                    Debug.Log(printingChunk.GetType());
                }
            }


        }
        private void Update()
        {

        }
        private void BuildToTheMap(ProcedureMapGenerator.Chunk[,] map)
        {

        }
        private Chunk ChooseRightOneRandomChunk(ProcedureMapGenerator.Chunk chunk, out int prefRotationTimes)
        {
            if (chunk == null) { prefRotationTimes = 0; Debug.Log("Null reference"); return new Chunk(); }

            List<Chunk> rightChunks = new List<Chunk>();
            int targetPoints = chunk.DirectionsCount();
            List<ConnectionType> targetDirections;

            int foundedSame;
            ProcedureMapGenerator.Chunk rotationChunk;
            prefRotationTimes = 0;

            foreach (Chunk chunkPrefab in ChunkPrefabs)
            {
                rotationChunk = chunk;
                foreach (AttachPoint attachPoint in chunkPrefab.Points)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        targetDirections = FindTargetDirections(rotationChunk);
                        foundedSame = 0;

                        if (targetDirections.Any(dir => dir == attachPoint.connectionType))
                            foundedSame++;
                        if (foundedSame == targetPoints)
                        {
                            Debug.Log($"Выбрано с {prefRotationTimes} раза");
                            rightChunks.Add(chunkPrefab);
                            break;
                        }
                        else
                        {
                            rotationChunk.RotateChunk();
                            prefRotationTimes++;
                        }
                    }
                }
            }

            Chunk result = rightChunks[UnityEngine.Random.Range(0, rightChunks.Count - 1)];

            if (result != null)
                if (rightChunks.Count > 1)
                    return result;
                else
                { Debug.Log($"Лист с выбранными чанками длиной в {rightChunks.Count} элементов."); return rightChunks[0]; }
            else
            {
                Debug.Log($"Ничего не выбрано ({result.GetType()})");
                return new Chunk();
            }
        }
        private List<ConnectionType> FindTargetDirections(ProcedureMapGenerator.Chunk chunk)
        {
            List<ConnectionType> targetDirections = new List<ConnectionType>();

            foreach (var key in chunk.directions.Keys.ToList())
            {
                if (chunk.directions[key].Item1)
                    targetDirections.Add(key);
            }

            string debugOutput = "";

            foreach (var dir in targetDirections)
            {
                debugOutput += dir;
                debugOutput += " ";
            }
            debugOutput += "(направлений ";
            debugOutput += Convert.ToString(targetDirections.Count);
            debugOutput += ").";
            return targetDirections;
        }
    }
}