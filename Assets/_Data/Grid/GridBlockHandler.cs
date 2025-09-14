using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class GridBlockHandler : GridAbstract
{
    //[Header("GridBlockHandler")]
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;
    public List<BlockCtrl> remainingBlocks;
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
            this.SetRemainingBlocks();
        }
        
        this.firstBlock = null;
        this.lastBlock = null;
        Debug.Log("Remaining block count: " + this.ctrl.gridSystem.blockSpawnCount);
        Debug.Log("Pathfinding done, reset blocks");
        if (!hasPairsLeft())
        {
            this.Shuffle();
        }
        this.ctrl.pathfinding.DataReset();
    }
    public virtual bool hasPairsLeft()
    {        
        for(int i = 0; i < remainingBlocks.Count(); i++)
        {
            for(int j = i + 1; j < remainingBlocks.Count(); j++)
            {
                BlockCtrl a = remainingBlocks[i];
                BlockCtrl b = remainingBlocks[j];
                if(a.blockID != b.blockID) continue;
                if(this.ctrl.pathfinding.FindPath(a,b)) return true;
            }
        }
        return false;
    }
    
    public virtual void Shuffle()
    {
        List<Node> occupiedNodes = new List<Node>();
        foreach(BlockCtrl block in remainingBlocks)
        {
            int x = block.blockData.node.x;
            int y = block.blockData.node.y;
            if(block.blockData.node != null && x > 0 && y > 0 && x < this.ctrl.gridSystem.width - 1 && y < this.ctrl.gridSystem.height-1)
                occupiedNodes.Add(block.blockData.node);
        }
        //clear old links
        foreach (BlockCtrl block in remainingBlocks)
        {
            Node oldNode = block.blockData.node;
            oldNode.blockCtrl = null;
            oldNode.occupied = false;
        }
        //shuffle
        for (int i = occupiedNodes.Count - 1; i > 0; i--)
        {
            int ran = Random.Range(0, i + 1);
            (occupiedNodes[i], occupiedNodes[ran]) = (occupiedNodes[ran], occupiedNodes[i]);
            Debug.Log("Shuffle called");
        }
        
        //reassign
        for(int i = 0; i < remainingBlocks.Count(); i++)
        {
            BlockCtrl block = remainingBlocks[i];
            Node oldNode = block.blockData.node;
            Node newNode = occupiedNodes[i];
            
            if(oldNode != null) oldNode = null;

            block.blockData.SetNode(newNode);
            newNode.blockCtrl = block;
            newNode.occupied = true;
            newNode.nodeObj.transform.position = new Vector3(newNode.x, newNode.y, 0f);
        }        
    }
    public virtual void SetRemainingBlocks()
    {
        remainingBlocks = this.ctrl.gridSystem.activeBlocks;
    }
    protected virtual void FreeBlock()
    {
        this.ctrl.gridSystem.FreeNode(this.firstBlock.blockData.node);
        this.ctrl.gridSystem.FreeNode(this.lastBlock.blockData.node);
    }
}
