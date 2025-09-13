using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridBlockHandler : GridAbstract
{
    //[Header("GridBlockHandler")]
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;
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
        if (this.firstBlock.blockID == this.lastBlock.blockID)
        {
            bool pathFound = this.ctrl.pathfinding.FindPath(this.firstBlock, this.lastBlock);
            if (pathFound) this.FreeBlock();
        }
        
        this.firstBlock = null;
        this.lastBlock = null;
        Debug.Log("Pathfinding done, reset blocks");
        this.ctrl.pathfinding.DataReset();

      
    }
    protected virtual void FreeBlock()
    {
        this.ctrl.gridSystem.FreeNode(this.firstBlock.blockData.node);
        this.ctrl.gridSystem.FreeNode(this.lastBlock.blockData.node);
    }
}
