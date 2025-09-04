using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : GridAbstract
{
    [Header("Grid System")]
    public int width = 18;
    public int height = 11;
    private float offsetX = 0.2f;
    public BlocksProfile blocksProfile;
    public List<Node> nodes;
    public List<int> nodeIds;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.InitGridSystem();
        this.LoadBlockProflie();
    }

    protected virtual void LoadBlockProflie()
    {
        if (this.blocksProfile != null) return;
        this.blocksProfile = Resources.Load<BlocksProfile>("Pikachu");
        Debug.LogWarning(transform.name + " LoadBlockProflie", gameObject);
    }

    protected override void Start()
    {
        this.SpawnNodeObj();
        this.SpawnBlocks();
        this.FindNodesNeighbors();
        this.FindBlocksNeighbors();
    }

    protected virtual void FindNodesNeighbors()
    {
        int x, y;
        foreach (Node node in this.nodes)
        {
            x = node.x;
            y = node.y;
            node.up = this.GetNodeByXY(x, y + 1);
            node.right = this.GetNodeByXY(x + 1, y);
            node.down = this.GetNodeByXY(x, y - 1);
            node.left = this.GetNodeByXY(x - 1, y);
        }
    }

    protected virtual Node GetNodeByXY(int x, int y)
    {
        foreach (Node node in this.nodes)
        {
            if (node.x == x && node.y == y) return node;
        }

        return null;
    }

    protected virtual void FindBlocksNeighbors()
    {
        foreach (Node node in this.nodes)
        {
            if (node.blockCtrl == null) continue;
            node.blockCtrl.neighbors.Add(node.up.blockCtrl);
            node.blockCtrl.neighbors.Add(node.right.blockCtrl);
            node.blockCtrl.neighbors.Add(node.down.blockCtrl);
            node.blockCtrl.neighbors.Add(node.left.blockCtrl);
        }
    }

    protected virtual void InitGridSystem()
    {
        if (this.nodes.Count > 0) return;

        int nodeId = 0;
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                Node node = new Node
                {
                    x = x,
                    y = y,
                    posX = x - (this.offsetX * x),
                    nodeId = nodeId,
                };
                this.nodes.Add(node);
                this.nodeIds.Add(nodeId);
                nodeId++;
            }
        }
    }

    protected virtual void SpawnNodeObj()
    {
        Vector3 pos = Vector3.zero;
        foreach (Node node in this.nodes)
        {
            pos.x = node.posX;
            pos.y = node.y;
            //node: node in matrix
            //nodeObj: node in game
            //obj that has just been spawned
            Transform obj = this.ctrl.blockSpawner.Spawn(BlockSpawner.NODEOBJ, pos, Quaternion.identity);
            obj.name = "Holder_" + node.x.ToString() + "_" + node.y.ToString();
            obj.gameObject.SetActive(true);

            NodeObj nodeObj = obj.GetComponent<NodeObj>();
            nodeObj.gameObject.SetActive(true);
            nodeObj.SetText(node.y.ToString() + "\n" + node.x.ToString());
            Color color = node.y%2==0? Color.red : Color.green;
            nodeObj.SetColor(color);

            node.nodeObj = nodeObj;
            
        }
    }

    protected virtual void SpawnBlocks()
    {
        Vector3 pos = Vector3.zero;
        int blockCount = 4;
        foreach (Sprite sprite in this.blocksProfile.sprites)
        {
            for (int i = 0; i < blockCount; i++)
            {
                Node node = this.GetRandomNode();
                pos.x = node.posX;
                pos.y = node.y;

                Transform block = this.ctrl.blockSpawner.Spawn(BlockSpawner.BLOCK, pos, Quaternion.identity);
                BlockCtrl blockCtrl = block.GetComponent<BlockCtrl>();
                blockCtrl.blockData.SetSprite(sprite);

                this.LinkNodeBlock(node, blockCtrl);
                block.name = "Block_" + node.x.ToString() + "_" + node.y.ToString();

                block.gameObject.SetActive(true);
                node.occupied = true;
            }
        }
    }

    protected virtual Node GetRandomNode()
    {
        Node node;
        int randId;
        int nodeCount = this.nodes.Count;
        for (int i = 0; i < nodeCount; i++)
        {
            randId = Random.Range(0, this.nodeIds.Count);
            node = this.nodes[this.nodeIds[randId]];
            this.nodeIds.RemoveAt(randId);

            if (node.x == 0) continue;
            if (node.y == 0) continue;
            if (node.x == this.width - 1) continue;
            if (node.y == this.height - 1) continue;

            if (node.blockCtrl == null) return node;
        }

        Debug.LogError("Node can't found, this should not happen");
        return null;
    }

    protected virtual void LinkNodeBlock(Node node, BlockCtrl blockCtrl)
    {
        blockCtrl.blockData.SetNode(node);
        node.blockCtrl = blockCtrl;
    }
}
