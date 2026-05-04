using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    Dictionary<UnityEngine.Vector3, Block> blocks = new Dictionary<UnityEngine.Vector3, Block>();

    private int sizechunk;

    private Block[][] chunk;

    public ChunkData(int size)
    {
        sizechunk = size;

        chunk = new Block[sizechunk][];
    }

    public int GetSizeChunk()
    {
        return sizechunk;
    }

    public Block[][] GetDataChunk()
    {
        return chunk;
    }

    public bool AddBlock(Vector3 posBlock, Block block) 
    {
        return blocks.TryAdd(posBlock, block);
    }

    public bool RemoveBlock(Vector3 posBlock)
    {
        return blocks.Remove(posBlock);
    }

    public Block GetBlock(Vector3 posBlock)
    {
        return blocks.GetValueOrDefault(posBlock);
    }
}