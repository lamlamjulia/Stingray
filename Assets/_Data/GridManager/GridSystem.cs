using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridSystem: GridAbstract
{
    [Header("GridSystem")]
    public List<Node> nodes;
    public int width = 18;
    public int height = 11;
    public BlocksProfile blocksProfile;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.InitGridSystem();
        this.Start();
        //this.LoadBlockProfile();
    }
    protected override void Start()
    {
        this.SpawnBlocks();
    }
    protected virtual void LoadBlockProfile()
    {
        if(this.blocksProfile != null) return;
        
    }
    protected virtual void InitGridSystem()
    {
        if (this.nodes.Count > 0) return;
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                Node node = new Node()
                {
                    x = x,
                    y = y
                };
                this.nodes.Add(node);
            }
        }
    }
    protected virtual void SpawnBlocks()
    {
        Vector3 pos = Vector3.zero;
        foreach (Node node in this.nodes)
        {
            pos.x = node.x;
            pos.y = node.y;
            Transform block = this.ctrl.blockSpawner.Spawn(BlockSpawner.BLOCK, pos, Quaternion.identity);
            block.gameObject.SetActive(true);
        }
    }
}
