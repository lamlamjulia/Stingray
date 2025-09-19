using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GridBlockHandler : GridAbstract
{
    //[Header("GridBlockHandler")]
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;
    public List<BlockCtrl> remainingBlocks;
    protected override void Start()
    {
        SetRemainingBlocks();
    }
    public virtual void SetNode(BlockCtrl blockCtrl)
    {
        Vector3 pos;
        Transform chooseObj;
        
        if (this.firstBlock == null)
        {
            this.firstBlock = blockCtrl;
            pos = blockCtrl.transform.position;
            chooseObj = this.ctrl.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
            chooseObj.gameObject.SetActive(true);
            return;
        }

        // When selecting the second block
        this.lastBlock = blockCtrl;
        pos = blockCtrl.transform.position;
        chooseObj = this.ctrl.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
        chooseObj.gameObject.SetActive(true);

        // Run pathfinding immediately
        if (this.firstBlock.blockID == this.lastBlock.blockID && this.firstBlock.blockData.ctrl != this.lastBlock.blockData.ctrl)
        {
            bool pathFound = this.ctrl.pathfinding.FindPath(this.firstBlock, this.lastBlock);
            if (pathFound) this.FreeBlocks();
        }
        
        this.firstBlock = null;
        this.lastBlock = null;
        Debug.Log("Pathfinding done, reset blocks");
        if (!hasValidMoves())
        {
            Debug.Log("No more moves, shuffle");
            this.Shuffle();
        }
        this.ctrl.pathfinding.DataReset();
    }
    public virtual bool hasValidMoves()
    {
        var remainingBlocks = this.ctrl.gridSystem.nodes
            .Where(n => n.blockCtrl != null && n.blockCtrl.gameObject.activeInHierarchy)
            .Select(n => n.blockCtrl)
            .ToList();
        if(remainingBlocks.Count <= 1) return false;
        Debug.LogWarning("Check hasValidMoves");
        for(int i = 0; i < remainingBlocks.Count(); i++)
        {
            for(int j = i + 1; j < remainingBlocks.Count(); j++)
            {
                BlockCtrl a = remainingBlocks[i];
                BlockCtrl b = remainingBlocks[j];
                Debug.LogWarning("for loop hasValidMoves");
                if (a == null|| b == null) continue;
                if (a.blockID != b.blockID) continue;

                if (this.ctrl.pathfinding.FindPath(a, b))
                {
                    Debug.LogWarning("hasValidMoves: true");
                    return true; 
                }
            }
        }
        Debug.LogWarning("hasValidMoves: false");
        return false;
    }
    public virtual void SetRemainingBlocks()
    {
        remainingBlocks = this.ctrl.gridSystem.activeBlocks;
    }
    public virtual void Shuffle()
    {
        Debug.LogWarning("Shuffle() called, remaining count = " + remainingBlocks.Count);
        List<BlockCtrl> activeBlocks = remainingBlocks
            .Where(n => n!= null && n.blockData != null && n.blockData.node != null)
            .ToList();
        if(activeBlocks.Count <= 1)
        {
            Debug.LogWarning("No active blocks!");
            return;
        }
        List<Sprite> sprites = activeBlocks
            .Where(b => b.blockData.ctrl.sprite.sprite != null)
            .Select(b => b.blockData.ctrl.sprite.sprite)
            .ToList();
        //shuffle
        for (int i = sprites.Count - 1; i > 0; i--)
        {
            int ran = Random.Range(0, i + 1);
            (sprites[i], sprites[ran]) = (sprites[ran], sprites[i]);
            Debug.Log("Shuffling");
        }
        for (int i = 0; i < activeBlocks.Count; i++)
        {
            if(sprites[i] != null)
                activeBlocks[i].blockData.SetSprite(sprites[i]);
            else
                Debug.LogWarning("Skipped null blocks");
            Debug.LogWarning("Reassign blocks");
        }
        Debug.LogWarning("Shuffle done!");
    }
    protected virtual void FreeBlocks()
    {
        this.ctrl.gridSystem.FreeBlock(this.firstBlock);
        this.ctrl.gridSystem.FreeBlock(this.lastBlock);
        if (!hasValidMoves())
        {
            Debug.Log("No more moves, shuffle");
            this.Shuffle();
        }
    }
}
