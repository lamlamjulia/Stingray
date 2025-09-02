using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS: GridAbstract, IPathfinding
{
    [Header("BFS")]
    public List<Node> queue = new List<Node> ();
    public List<Node> path = new List<Node>();
    public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node> ();

    public virtual void FindPath(BlockCtrl startBlock, BlockCtrl endBlock)
    {
        Node start = startBlock.blockData.node;
        Node end = endBlock.blockData.node;

        this.Enqueue(start);
        this.cameFrom[start] = start;

        while (this.queue.Count > 0)
        {
            Node current = this.Dequeue();
            current.blockCtrl.blockData.SetColor(Color.black);

            if(current == end)
            {
                this.ConstructPath(startBlock, endBlock);
                break;
            }
            foreach(Node neighbor in current.neighbors())
            {
                if(neighbor == null) continue;
                if(this.IsValidPath(neighbor) && !cameFrom.ContainsKey(neighbor))
                {
                    neighbor.blockCtrl.blockData.SetColor(Color.black);
                    this.Enqueue(neighbor);
                    this.cameFrom[neighbor] = current;
                }
            }


        }

    }
    protected virtual void Enqueue(Node node)
    {
        this.queue.Add(node);
    }
    protected virtual Node Dequeue()
    {
        Node node = this.queue[0];
        this.queue.RemoveAt(0);
        return node;
    }
    //protected virtual void ShowPath()
    //{
    //    //Vector3 pos;
    //    foreach(Node node in this.path)
    //    {
    //        //

    //    }
    //}
    protected virtual void ConstructPath(BlockCtrl start, BlockCtrl end)
    {
        BlockCtrl currentBlock = end;
        while (currentBlock != start) 
        {
            Node current = currentBlock.blockData.node;
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(start.blockData.node);
        path.Reverse();
    }
    protected virtual bool IsValidPath(Node node)
    {
        return !node.occupied;
    }
}
