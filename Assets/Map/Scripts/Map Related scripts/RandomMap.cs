using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : Map
{
    public GameObject mapChunkPrefab;

    public List<Chunk> mapChunks = new List<Chunk>();

    public override void SetUp()
    {
        GenerateMapTiles();

        base.SetUp();
    }

    public void GenerateMapTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject holder = Instantiate(mapChunkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                holder.transform.parent = transform;
                holder.transform.localPosition = new Vector3(x * 5, 0, z * 5);

                Chunk chunk = holder.GetComponent<Chunk>();

                chunk.chunkType = (eChunkTypes)Random.Range(0, 4);
                chunk.GenerateRandomChunkVariant();


                mapChunks.Add(chunk);

                for (int i = 0; i < chunk.myChunk.chunkTiles.Length; i++)
                {
                    mapTiles.Add(chunk.myChunk.chunkTiles[i]);
                }
            }
        }
    }
}
