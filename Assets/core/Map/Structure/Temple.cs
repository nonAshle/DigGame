public class Temple
{
    private  Block _block = new Block(typeBlock.TEMPLEBLOCK, 0);
    private  Block noneblock = new Block(typeBlock.NONEBLOCK, 0);
    private Block[][] temple;

    public Temple()
    {
        _block = new Block(typeBlock.TEMPLEBLOCK, 0);
        noneblock = new Block(typeBlock.NONEBLOCK, 0);

        temple = new Block[6][]
        {
            new Block[] { _block, _block, _block, _block, _block },
            new Block[] { noneblock, _block, _block, _block, noneblock },
            new Block[] { noneblock, noneblock, noneblock, noneblock, noneblock },
            new Block[] { noneblock, noneblock, noneblock, noneblock, noneblock },
            new Block[] { noneblock, _block, _block, _block, noneblock },
            new Block[] { _block, _block, _block, _block, _block }
        };
    }

    public Block[][] GetTemple()
    {
        return temple;
    }

    public int GetXsizeTemple()
    {
        return temple.Length;
    }

    public int GetYsizeTemple()
    {
        return temple[0].Length;
    }
}
