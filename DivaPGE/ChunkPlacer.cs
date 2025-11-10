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
            //Chunk BeginingChunk = Instantiate(FirstChunk);
            //BeginingChunk.transform.position = gameObject.transform.position;
            //spawnedChunks.Add(FirstChunk);
            //for (int i = 0; i < TotalAmountOfElements; i++)
            //    SpawnChunk();

            ProcedureGenerator procedureGenerator = new ProcedureGenerator(width, height);

            procedureGenerator.GenerateMapWithStyle(mapStyle, TotalAmountOfElements);

            ProcedureMapGenerator.Chunk[,] virtualMap = procedureGenerator.GetMap();

            int rotateTimes;

            for (int x = 0; x < virtualMap.GetLength(0);  x++)
            {
                for (int z = 0;  z < virtualMap.GetLength(1); z++)
                {
                    Chunk printingChunk;
                    rotateTimes = 0;
                    Vector3 offset;
                    printingChunk = ChooseRightOneRandomChunk(virtualMap[x, z], out rotateTimes);
                    offset = new Vector3(x, origin.position.y, z);

                    printingChunk = Instantiate(printingChunk);
                    printingChunk.transform.position = offset;
                    printingChunk.transform.Rotate(0, 90*rotateTimes, 0);
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
            List<Chunk> rightChunks = new List<Chunk>();
            int targetPoints = chunk.DirectionsCount();
            ConnectionType[] targetDirections;

            int foundedSame;
            ProcedureMapGenerator.Chunk rotationChunk = chunk;
            prefRotationTimes = 0;

            foreach (Chunk chunkPrefab in ChunkPrefabs)
            {
                foreach (AttachPoint attachPoint in chunkPrefab.Points)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        targetDirections = FindTargetDirections(rotationChunk, targetPoints);
                        foundedSame = 0;

                        if (targetDirections.Any(dir => dir == attachPoint.connectionType))
                            foundedSame++;
                        if (foundedSame == targetPoints)
                        {
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

            return rightChunks[UnityEngine.Random.Range(0, rightChunks.Count)];
        }
        private ConnectionType[] FindTargetDirections(ProcedureMapGenerator.Chunk chunk, int targetPoints)
        {
            ConnectionType[] targetDirections = new ConnectionType[targetPoints];

            int n = 0;

            foreach (var key in chunk.directions.Keys.ToList())
            {
                if (chunk.directions[key].Item1)
                    targetDirections[n++] = key;
            }

            return targetDirections;
        }
        //private void SpawnChunk()
        //{
        //    Chunk newChunk;
        //    AttachPoint previousRandomizedPoint;
        //    AttachPoint newRandomizedPoint;

        //    if (spawnedChunks.Count == 1)
        //    {
        //        newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
        //        newRandomizedPoint = newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)];
        //        newChunk.transform.position = spawnedChunks[0].Begin.transform.position - newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)].transform.localPosition;
        //        if (Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position) != 180f)
        //        {
        //            float differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position);
        //            newChunk.transform.RotateAround(newRandomizedPoint.transform.position, Vector3.up, differenceAngle + 180f);
        //            differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position);
        //            if (differenceAngle + 180f != 180f)
        //                Debug.Log($"Поворот не удался, угол остался {Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position)} градусов");
        //        }
        //        spawnedChunks[0].Begin.attached = true;
        //        newRandomizedPoint.attached = true;
        //        spawnedChunks.Add(newChunk);
        //    }
        //    else
        //    {
        //        newChunk = Instantiate(ChunkPrefabs[UnityEngine.Random.Range(0, ChunkPrefabs.Length)]);
        //        previousRandomizedPoint = spawnedChunks[spawnedChunks.Count - 1].Points[UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count - 1].Points.Length)];
        //        while (previousRandomizedPoint.attached != false)
        //            previousRandomizedPoint = spawnedChunks[spawnedChunks.Count - 1].Points[UnityEngine.Random.Range(0, spawnedChunks[spawnedChunks.Count - 1].Points.Length)];
        //        newRandomizedPoint = newChunk.Points[UnityEngine.Random.Range(0, newChunk.Points.Length)];
        //        newChunk.transform.position = previousRandomizedPoint.transform.position - newRandomizedPoint.transform.localPosition;
        //        if (Vector3.Angle(newRandomizedPoint.transform.position, previousRandomizedPoint.transform.position) != 180f)
        //        {
        //            float differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, previousRandomizedPoint.transform.position);
        //            newChunk.transform.RotateAround(newChunk.transform.position, newRandomizedPoint.transform.up, differenceAngle + 180f);
        //            differenceAngle = Vector3.Angle(newRandomizedPoint.transform.position, spawnedChunks[0].Begin.transform.position);
        //            if (differenceAngle + 180f != 180f)
        //                Debug.Log($"Поворот не удался, угол остался {Vector3.Angle(newRandomizedPoint.transform.position, previousRandomizedPoint.transform.position)} градусов");
        //        }
        //        previousRandomizedPoint.attached = true;
        //        newRandomizedPoint.attached = true;
        //        spawnedChunks.Add(newChunk);
        //    }
        //}
    }
}