using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridBlockHandler : GridAbstract
{
    //[Header("GridBlockHandler")]
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;
    public List<BlockCtrl> blocksRemaining;
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
            if (pathFound) this.FreeBlock();
        }
        
        this.firstBlock = null;
        this.lastBlock = null;
        Debug.Log("Remaining block count: " + this.ctrl.gridSystem.blockSpawnCount);
        Debug.Log("Pathfinding done, reset blocks");
        Debug.Log("is stuck? " + this.isStuck());
        this.ctrl.pathfinding.DataReset();

      
    }
    public virtual bool hasPairsLeft()
    {
        for(int i = 0; i < blocksRemaining.Count(); i++)
        {
            for(int j = i + 1; j < blocksRemaining.Count(); j++)
            {
                BlockCtrl a = blocksRemaining[i];
                BlockCtrl b = blocksRemaining[j];
                if(a.blockID != b.blockID) continue;
                if(this.ctrl.pathfinding.FindPath(a,b)) return true;
            }
        }
        return true;
    }
    public virtual bool isStuck()
    {
        if(!this.hasPairsLeft())
        {
            return true;
        }
        return false;
    }
    public virtual void GetAllRemainingBlocks()
    {
        blocksRemaining = this.ctrl.gridSystem.activeBlocks;
    }
    protected virtual void FreeBlock()
    {
        this.ctrl.gridSystem.FreeNode(this.firstBlock.blockData.node);
        this.ctrl.gridSystem.FreeNode(this.lastBlock.blockData.node);
    }
}
