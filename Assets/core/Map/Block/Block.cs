using UnityEngine;

public class Block
{
    private typeBlock typeblock;
    private destructionState stateBlock;
    private int angle;
    private GameObject currentVisualEffect;

    public void RemoveCurrentVisualEffect()
    {
        if (currentVisualEffect != null)
        {
            UnityEngine.Object.Destroy(currentVisualEffect);
        }
    }

    public void SetCurrentVisualEffects(GameObject visualEffect)
    {
        currentVisualEffect = visualEffect;
    }

    public GameObject GetCurrentVisualEffect()
    {
        return currentVisualEffect;
    }

    public Block(typeBlock typeblock, int angle)
    {
        this.typeblock = typeblock;
        this.angle = angle;
        stateBlock = destructionState.NONE;
    }

    public void SetDestructionState(destructionState state)
    {
        stateBlock = state;
    }

    public destructionState GetDestructionState()
    {
        return stateBlock;
    }

    public typeBlock GetTypeBlock()
    {
        return this.typeblock;
    }

    public int GetAngle()
    {
        return this.angle;
    }
}