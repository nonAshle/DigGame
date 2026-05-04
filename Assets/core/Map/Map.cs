using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Dictionary<Vector3, ChunkData> fullmap = new Dictionary<Vector3, ChunkData>();

    private Dictionary<Vector3, Chunk> activechunk = new Dictionary<Vector3, Chunk>();

    public bool AddChunkData(Vector3 position, ChunkData chunkData)
    {
        return fullmap.TryAdd(position, chunkData);
    }

    public bool AddActiveChunk(Vector3 position, Chunk chunk)
    {
        return activechunk.TryAdd(position, chunk);
    }

    public bool RemoveActiveChunk(Vector3 position)
    {
        return activechunk.Remove(position);
    }

    public List<Vector3> GetActiveChunkKey()
    {
        List<Vector3> keys = new List<Vector3>();

        foreach (var key in activechunk.Keys)
        {
            keys.Add(key);
        }

        return keys;
    }

    public ChunkData GetChunkData(Vector3 position)
    {
        return fullmap.GetValueOrDefault(position);
    }

    public Chunk GetChunk(Vector3 position)
    {
        return activechunk.GetValueOrDefault(position);
    }
}
