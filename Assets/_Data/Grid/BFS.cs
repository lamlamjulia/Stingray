using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS: GridAbstract, IPathfinding
{
    [Header("BFS")]
    public List<Node> queue = new List<Node> ();
    public List<Node> path = new List<Node>();
    public List<NodeCameFrom> cameFromNodes = new List<NodeCameFrom> ();
    public List<Node> visited = new List<Node> ();

    public virtual void FindPath(BlockCtrl startBlock, BlockCtrl endBlock)
    {
        Node start = startBlock.blockData.node;
        Node end = endBlock.blockData.node;

        this.Enqueue(start);
        this.cameFromNodes.Add(new NodeCameFrom(start, end));
        this.visited.Add(start);


        while (this.queue.Count > 0)
        {
            Node current = this.Dequeue();

            if(current == end)
            {
                ConstructPath(start, end);
                break;
            }
            foreach(Node neighbor in current.Neighbors())
            {
                if(neighbor == null) continue;
                if(this.IsValidPath(neighbor, end) && !this.visited.Contains(neighbor))
                {
                    this.Enqueue(neighbor);
                    this.visited.Add(neighbor);
                    this.cameFromNodes.Add(new NodeCameFrom(current, neighbor));
                }
            }
        }

        this.ShowVisited();
        this.ShowPath();

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
    protected virtual void ShowVisited()
    {
        foreach (Node node in this.visited)
        {
            Vector3 pos = node.nodeObj.transform.position;
            Transform obj = this.ctrl.blockSpawner.Spawn(BlockSpawner.SCAN, pos, Quaternion.identity);
            obj.gameObject.SetActive(true);
        }
    }
    protected virtual void ShowPath()
    {
        Vector3 pos;
        foreach (Node node in this.path)
        {
            pos = node.nodeObj.transform.position;
            Transform linker = this.ctrl.blockSpawner.Spawn(BlockSpawner.LINKER, pos, Quaternion.identity);
            linker.gameObject.SetActive(true);
        }
    }
    protected virtual void ConstructPath(Node start, Node end)
    {
        Node current = end;
        while (current != start) 
        {
            Debug.Log("Added into path");
            path.Add(current);
            current = this.GetCameFrom(current);
        }
        path.Add(start);
        path.Reverse();
    }
    protected virtual Node GetCameFrom(Node toNode)
    {
        return this.cameFromNodes.First(item => item.toNode == toNode).fromNode;
    }
    protected virtual bool IsValidPath(Node node, Node end)
    {
        return !node.occupied || node == end;
    }
}
