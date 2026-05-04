using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject defaultmap;
    public GameObject chunkPrefab;
    public GameObject player;
    public GameObject blockDestroyedStageOne;
    public GameObject blockDestroyedStageTwo;
    public TextMeshProUGUI directionText;
    public TextMeshProUGUI coordinateText;

    private int currentx;
    private int currenty;

    private float nextDigTime = 0f;

    Vector2Int oldplayerchunk = new Vector2Int(1, 1);
    Vector2Int currentplayerchunk;

    Vector3 coordinate;

    ChunkData currentChunkData;
    Chunk currentchunk;
    GameObject newChunk;

    Map map = new Map();

    void Start()
    {
        Chunk defaultchunk = defaultmap.GetComponent<Chunk>();

        ChunkData defaultChunkData = new ChunkData(64);

        defaultchunk.Initialize(defaultChunkData);
        defaultchunk.SetCoordinateChunk(new UnityEngine.Vector3(defaultmap.transform.position.x, defaultmap.transform.position.y, defaultmap.transform.position.z));

        map.AddChunkData(defaultchunk.GetCoordinateChunk(), defaultChunkData);
        map.AddActiveChunk(defaultchunk.GetCoordinateChunk(), defaultchunk);

        defaultchunk.GenerateChunk();
        StartCoroutine(defaultchunk.FillChunk(true));

        defaultChunkData.RemoveBlock(new Vector3(31.5f, 31.5f, 0));
        defaultChunkData.AddBlock(new Vector3(31.5f, 31.5f, 0), new Block(typeBlock.NONEBLOCK, 0));

        defaultchunk.DestroyandSetblock(new Vector3(31.5f, 31.5f, 0), typeBlock.NONEBLOCK, 0);
        player.transform.position = new Vector3(31.5f, 31.5f, 0);
    }

    void Update()
    {
        currentx = Mathf.FloorToInt(player.transform.position.x / 64);
        currenty = Mathf.FloorToInt(player.transform.position.y / 64);

        currentplayerchunk = new Vector2Int(currentx, currenty);

        if (coordinateText.text != $"{Mathf.FloorToInt(player.transform.position.x)}.{Mathf.FloorToInt(player.transform.position.y)}:{currentx}.{currenty}")
        {
            coordinateText.text = $"{Mathf.FloorToInt(player.transform.position.x)}.{Mathf.FloorToInt(player.transform.position.y)}:{currentx}.{currenty}";
        }

        if (Input.GetKey(KeyCode.Q) && Time.time >= nextDigTime)
        {
            Dig();

            nextDigTime = Time.time + 0.3f;
        }

        if (currentplayerchunk == oldplayerchunk)
        {
            return;
        }

        oldplayerchunk = currentplayerchunk;

        CreateorFillChunk();
        StartCoroutine(DistantRemoveChunk());
    }

    private void CreateorFillChunk()
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                coordinate = new Vector3(((currentx + x) * 64), ((currenty + y) * 64), 0);

                currentChunkData = map.GetChunkData(coordinate);

                if (currentChunkData == null)
                {
                    newChunk = Instantiate(chunkPrefab, coordinate, UnityEngine.Quaternion.identity);
                    currentchunk = newChunk.GetComponent<Chunk>();

                    currentChunkData = new ChunkData(64);

                    currentchunk.Initialize(currentChunkData);
                    currentchunk.SetCoordinateChunk(coordinate);

                    map.AddChunkData(currentchunk.GetCoordinateChunk(), currentChunkData);
                    map.AddActiveChunk(currentchunk.GetCoordinateChunk(), currentchunk);

                    currentchunk.GenerateChunk();
                    StartCoroutine(currentchunk.FillChunk());
                }
                else
                {
                    currentchunk = map.GetChunk(coordinate);

                    if (currentchunk == null)
                    {
                        newChunk = Instantiate(chunkPrefab, coordinate, UnityEngine.Quaternion.identity);
                        currentchunk = newChunk.GetComponent<Chunk>();

                        currentchunk.Initialize(currentChunkData);
                        currentchunk.SetCoordinateChunk(coordinate);
                    }

                    if (map.AddActiveChunk(currentchunk.GetCoordinateChunk(), currentchunk))
                    {
                        StartCoroutine(currentchunk.FillChunk());
                    }
                }
            }
        }
    }

    private IEnumerator DistantRemoveChunk()
    {
        yield return new WaitForSeconds(1f);

        List<Vector3> activechunk;
        List<Vector3> toRemove = new List<Vector3>();

        Vector3 posPlayer = new Vector3(currentx * 64, currenty * 64, 0);

        activechunk = map.GetActiveChunkKey();

        foreach (Vector3 chunkcoord in activechunk)
        {
            if (Mathf.Abs(chunkcoord.x - posPlayer.x) > 192 || Mathf.Abs(chunkcoord.y - posPlayer.y) > 192)
            {
                toRemove.Add(chunkcoord);
            }
        }

        foreach (Vector3 chunkcoord in toRemove)
        {
            Chunk deletedChunk = map.GetChunk(chunkcoord);
            if (deletedChunk != null)
            {
                map.RemoveActiveChunk(chunkcoord);

                StartCoroutine(deletedChunk.DestroyChunkGradually());
            }
        }
    }

    private void Dig()
    {
        Vector3 posDig;

        switch (directionText.text)
        {
            case ">":
                posDig = new Vector3((Mathf.FloorToInt(player.transform.position.x) + 1) + 0.5f, Mathf.FloorToInt(player.transform.position.y) + 0.5f, 0);
                break;
            case "<":
                posDig = new Vector3((Mathf.FloorToInt(player.transform.position.x) - 1) + 0.5f, Mathf.FloorToInt(player.transform.position.y) + 0.5f, 0);
                break;
            case "^":
                posDig = new Vector3(Mathf.FloorToInt(player.transform.position.x) + 0.5f, (Mathf.FloorToInt(player.transform.position.y) + 1) + 0.5f, 0);
                break;
            case "v":
                posDig = new Vector3(Mathf.FloorToInt(player.transform.position.x) + 0.5f, (Mathf.FloorToInt(player.transform.position.y) - 1) + 0.5f, 0);
                break;
            default:
                return;
        }

        Vector3 posChunk = new Vector3(Mathf.FloorToInt(posDig.x / 64) * 64, Mathf.FloorToInt(posDig.y / 64) * 64, 0);

        ChunkData digData = map.GetChunkData(posChunk);
        Chunk chunkDig = map.GetChunk(posChunk);

        if (chunkDig == null || digData == null)
        {
            return;
        }

        Block digBlock = digData.GetBlock(posDig);

        if (digBlock == null || digBlock.GetTypeBlock() == typeBlock.NONEBLOCK)
        {
            return;
        }

        Vector3 localPos = new Vector3(((posDig.x % 64) + 64) % 64, ((posDig.y % 64) + 64) % 64, 0);

        switch (digBlock.GetDestructionState())
        {
            case destructionState.NONE:
                digBlock.SetDestructionState(destructionState.DAMAGED);

                GameObject stage1 = Instantiate(blockDestroyedStageOne, localPos, Quaternion.identity, chunkDig.transform);
                stage1.transform.localPosition = localPos;
                digBlock.SetCurrentVisualEffects(stage1);

                break;
            case destructionState.DAMAGED:
                digBlock.RemoveCurrentVisualEffect();

                digBlock.SetDestructionState(destructionState.VERYDAMAGED);

                GameObject stage2 = Instantiate(blockDestroyedStageTwo, localPos, Quaternion.identity, chunkDig.transform);
                stage2.transform.localPosition = localPos;
                digBlock.SetCurrentVisualEffects(stage2);


                break;
            case destructionState.VERYDAMAGED:
                digBlock.RemoveCurrentVisualEffect();

                digBlock.SetDestructionState(destructionState.DESTROYED);

                digData.RemoveBlock(posDig);
                digData.AddBlock(posDig, new Block(typeBlock.NONEBLOCK, 0));

                chunkDig.DestroyandSetblock(posDig, typeBlock.NONEBLOCK, 0);

                break;
            case destructionState.DESTROYED:
                return;
        }
    }
}