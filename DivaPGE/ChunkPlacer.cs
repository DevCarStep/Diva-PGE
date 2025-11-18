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

            origin = this.gameObject.transform;

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

            chunk.directions[ConnectionType.Up] = new Tuple<bool, bool?>(false, false);
            chunk.directions[ConnectionType.Down] = new Tuple<bool, bool?>(false, false);

            List<Chunk> rightChunks = new List<Chunk>();
            int targetPoints = chunk.DirectionsCount();
            List<ConnectionType> targetDirections;

            int foundedSame;
            ProcedureMapGenerator.Chunk rotationChunk;
            prefRotationTimes = 0;

            foreach (Chunk chunkPrefab in ChunkPrefabs)
            {
                List<ConnectionType> basicPrefabDirections = FindTargetDirections(chunkPrefab);
                rotationChunk = chunk;
                for (int i = 0; i < 4; i++)
                {
                    prefRotationTimes = 0;
                    targetDirections = FindTargetDirections(rotationChunk);
                    //foundedSame = 0;

                    //if (targetDirections.Any(dir => dir == attachPoint.connectionType))
                    //    foundedSame++;
                    if (CustomListEquals(targetDirections, basicPrefabDirections))
                    {
                        Debug.Log($"Выбрано с {prefRotationTimes} раза");
                        rightChunks.Add(chunkPrefab);
                        break;
                    }
                    //if (foundedSame == targetPoints)
                    //{
                    //    Debug.Log($"Выбрано с {prefRotationTimes} раза");
                    //    rightChunks.Add(chunkPrefab);
                    //    break;
                    //}
                    else
                    {
                        rotationChunk.RotateChunk();
                        prefRotationTimes++;
                    }
                }
            }
            int rand = UnityEngine.Random.Range(0, rightChunks.Count - 1);

            Chunk result = rightChunks[rand];

            if (result != null)
                if (rightChunks.Count > 1)
                    return result;
                else
                { Debug.Log($"Лист с выбранными чанками длиной в {rightChunks.Count} элементов."); return rightChunks[0]; }
            else
            {
                Debug.Log($"Ничего не выбрано ({result.GetType()})");
                return ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length - 1)];
            }
        }
        private List<ConnectionType> FindTargetDirections(ProcedureMapGenerator.Chunk chunk)
        {
            List<ConnectionType> targetDirections = new List<ConnectionType>();

            foreach (var key in chunk.directions.Keys.ToList())
            {
                if (key == ConnectionType.Down || key == ConnectionType.Up)
                    continue;
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
        private List<ConnectionType> FindTargetDirections(DivaPGE.Chunk chunk)
        {
            List<ConnectionType> targetDirections = new List<ConnectionType>();

            foreach (var attachPoint in chunk.Points)
            {
                if (attachPoint.connectionType != ConnectionType.Up || attachPoint.connectionType != ConnectionType.Down)
                    targetDirections.Add(attachPoint.connectionType);
            }

            return targetDirections;
        }
        bool CustomListEquals(List<ConnectionType> list1, List<ConnectionType> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                    return false;
            }

            return true;
        }
    }
}