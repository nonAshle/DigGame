using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Chunk : MonoBehaviour
{
    private UnityEngine.Vector3 coordinate;

    public void SetCoordinateChunk(UnityEngine.Vector3 position)
    {
        coordinate = position;
    }

    private ChunkData data;

    public void Initialize(ChunkData chunkData)
    {
        this.data = chunkData;
        chunk = data.GetDataChunk();
    }

    private Block[][] chunk;

    public GameObject defaultblockPrefab;
    public GameObject cleanblockPrefab;
    public GameObject oldblockPrefab;
    public GameObject templeblockPrefab;
    public GameObject silverblockPrefab;
    public GameObject goldblockPrefab;

    private bool isFilling = false;

    public void GenerateChunk()
    {
        Generate generate = new Generate();

        int quantitytemple = 1;
        int chunksize = data.GetSizeChunk();

        generate.GenerateChunk(chunk, chunksize, quantitytemple);
    }

    public IEnumerator FillChunk(bool immediate = false)
    {
        if (this == null)
        { 
            yield break;
        }

        if (isFilling)
        {
            yield break;
        }

        isFilling = true;

        int blocksCreated = 0;

        UnityEngine.Vector3 coordinate;

        for (int i = 0; i < data.GetSizeChunk(); i++)
        {
            for (int j = 0;  j < data.GetSizeChunk(); j++)
            {
                coordinate = new UnityEngine.Vector3(transform.position.x + i + 0.5f, transform.position.y + j + 0.5f, 0);

                if (chunk[i][j].GetTypeBlock() == typeBlock.DEFAULTBLOCK)
                {
                    Instantiate(defaultblockPrefab, coordinate, UnityEngine.Quaternion.Euler(0, 0, chunk[i][j].GetAngle()), transform);
                    data.AddBlock(coordinate, chunk[i][j]);
                }
                else if (chunk[i][j].GetTypeBlock() == typeBlock.CLEANBLOCK)
                {
                    Instantiate(cleanblockPrefab, coordinate, UnityEngine.Quaternion.Euler(0, 0, chunk[i][j].GetAngle()), transform);
                    data.AddBlock(coordinate, chunk[i][j]);
                }
                else if (chunk[i][j].GetTypeBlock() == typeBlock.OLDBLOCK)
                {
                    Instantiate(oldblockPrefab, coordinate, UnityEngine.Quaternion.Euler(0, 0, chunk[i][j].GetAngle()), transform);
                    data.AddBlock(coordinate, chunk[i][j]);
                }
                else if (chunk[i][j].GetTypeBlock() == typeBlock.TEMPLEBLOCK)
                {
                    Instantiate(templeblockPrefab, coordinate, UnityEngine.Quaternion.Euler(0, 0, chunk[i][j].GetAngle()), transform);
                    data.AddBlock(coordinate, chunk[i][j]);
                }
                else if (chunk[i][j].GetTypeBlock() == typeBlock.SILVERBLOCK)
                {
                    Instantiate(silverblockPrefab, coordinate, UnityEngine.Quaternion.Euler(0, 0, chunk[i][j].GetAngle()), transform);
                    data.AddBlock(coordinate, chunk[i][j]);
                }
                else if (chunk[i][j].GetTypeBlock() == typeBlock.GOLDBLOCK)
                {
                    Instantiate(goldblockPrefab, coordinate, UnityEngine.Quaternion.Euler(0, 0, chunk[i][j].GetAngle()), transform);
                    data.AddBlock(coordinate, chunk[i][j]);
                }

                blocksCreated++;

                if (!immediate)
                {
                    if (blocksCreated >= 4)
                    {
                        blocksCreated = 0;
                        yield return null;
                    }
                }
            }
        }
    }

    public void DestroyandSetblock(UnityEngine.Vector3 coordinate, typeBlock type, int angle)
    {
        Collider2D hit = Physics2D.OverlapPoint(coordinate);

        if (!(hit == null))
            Destroy(hit.gameObject);

        chunk[((Mathf.FloorToInt(coordinate.y) % 64) + 64) % 64][((Mathf.FloorToInt(coordinate.x) % 64) + 64) % 64] = new Block(type, angle);
    }
    
    public UnityEngine.Vector3 GetCoordinateChunk()
    {
        return coordinate;
    }

    public IEnumerator DestroyChunkGradually()
    {
        int childCount = transform.childCount;
        int deletedchild = 0;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);

            deletedchild++;

            if (deletedchild >= 4)
            {
                deletedchild = 0;
                yield return null;
            }
        }

        Destroy(transform.gameObject);
    }
}