using UnityEngine;

public class Generate
{
    private Block GenerateBlocks()
    {
        Block block = null;
        int angle = Random.Range(1, 5) * 90;

        switch (Random.Range(0, 4))
        {
            case 0:
                block = new Block(typeBlock.NONEBLOCK, 0);
                break;
            case 1:
                block = new Block(typeBlock.DEFAULTBLOCK, angle);
                break;
            case 2:
                block = new Block(typeBlock.CLEANBLOCK, angle);
                break;
            case 3:
                block = new Block(typeBlock.OLDBLOCK, angle);
                break;
        }

        return block;
    }

    //Первый слой, это камни обычные
    private void GenerateLayerOne(Block[][] chunk, int chunksize)
    {
        for (int i = 0; i < chunksize; i++)
        {
            chunk[i] = new Block[chunksize];

            for (int j = 0; j < chunksize; j++)
            {
                chunk[i][j] = GenerateBlocks();
            }
        }
    }

    //Второй слой, это голда
    private void GenerateLayerTwo(Block[][] chunk, int chunksize)
    {
        for (int i = 0; i < 3; i++)
        {
            int x = Random.Range(0, chunksize);
            int y = Random.Range(0, chunksize);

            chunk[x][y] = new Block(typeBlock.GOLDBLOCK, 0);

            int Depth = Random.Range(2, 6);

            for (int j = 0; j < Depth; j++)
            {
                int xDepth = x + Random.Range(-1, 2);
                int yDepth = y + Random.Range(-1, 2);

                xDepth = Mathf.Clamp(xDepth, 0, chunksize - 1);
                yDepth = Mathf.Clamp(yDepth, 0, chunksize - 1);

                chunk[xDepth][yDepth] = new Block(typeBlock.GOLDBLOCK, 0);

                x = xDepth;
                y = yDepth;
            }

        }
    }

    //Третий слой, это серебро
    private void GenerateLayerThree(Block[][] chunk, int chunksize)
    {
        for (int i = 0; i < 6; i++)
        {
            int x = Random.Range(0, chunksize);
            int y = Random.Range(0, chunksize);

            int attempts = 0;
            while (chunk[x][y].GetTypeBlock() == typeBlock.GOLDBLOCK && attempts < 10)
            {
                x = Random.Range(0, chunksize);
                y = Random.Range(0, chunksize);
                attempts++;
            }

            chunk[x][y] = new Block(typeBlock.SILVERBLOCK, 0);

            int Depth = Random.Range(2, 6);

            for (int j = 0; j < Depth; j++)
            {
                int moveAttempts = 0;
                while (moveAttempts < 5)
                {
                    int xDepth = Mathf.Clamp(x + Random.Range(-1, 2), 0, chunksize - 1);
                    int yDepth = Mathf.Clamp(y + Random.Range(-1, 2), 0, chunksize - 1);

                    if (chunk[xDepth][yDepth].GetTypeBlock() != typeBlock.GOLDBLOCK)
                    {
                        x = xDepth;
                        y = yDepth;
                        chunk[x][y] = new Block(typeBlock.SILVERBLOCK, 0);
                        break;
                    }
                    moveAttempts++;
                }
            }

        }
    }

    //Четвертый слой, это храмы
    private void GenerateLayerFour(Block[][] chunk, int chunksize, int quantitytemple)
    {
        Temple temple = new Temple();
        Block[][] templateTemple = temple.GetTemple();

        for (int i = 0; i < quantitytemple; i++)
        {
            int x = Random.Range(0, chunksize - temple.GetXsizeTemple());
            int y = Random.Range(0, chunksize - temple.GetYsizeTemple());

            for (int j = 0; j < 6; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    typeBlock type = templateTemple[j][k].GetTypeBlock();

                    chunk[x + k][y + j] = new Block(type, 0);
                }
            }
        }
    }

    public void GenerateChunk(Block[][] chunk, int chunksize, int quantitytemple)
    {
        GenerateLayerOne(chunk, chunksize);
        GenerateLayerTwo(chunk, chunksize);
        GenerateLayerThree(chunk, chunksize);
        GenerateLayerFour(chunk, chunksize, quantitytemple);
    }
}