using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS: GridAbstract, IPathfinding
{
    [Header("BFS")]
    public List<Node> queue = new List<Node> ();
    public List<Node> path = new List<Node>();
    public List<NodeSteps> cameFromNodes = new List<NodeSteps> ();
    public List<Node> visited = new List<Node> ();

    public virtual void FindPath(BlockCtrl startBlock, BlockCtrl targetBlock)
    {
        Node start = startBlock.blockData.node;
        Node target = targetBlock.blockData.node;
        

        this.Enqueue(start);
        this.cameFromNodes.Add(new NodeSteps(start, start));
        this.visited.Add(start);

        NodeSteps nodeStep;
        List<NodeSteps> steps;
        while (this.queue.Count > 0)
        {
            Node current = this.Dequeue();

            if(current == target)
            {
                this.path = ConstructFinalPath(start, target);
                break;
            }
            foreach(Node neighbor in current.Neighbors())
            {
                if(neighbor == null) continue;
                if (this.visited.Contains(neighbor)) continue;
                if(!this.IsValidPath(neighbor, target)) continue;

                nodeStep = new NodeSteps(neighbor, current);                
                this.visited.Add(neighbor);
                this.cameFromNodes.Add(nodeStep);

                steps = this.BuildTmpSteps(neighbor, start);
                nodeStep.stepsString = this.GetStringFromSteps(steps);
                nodeStep.directionString = this.GetDirectionsFromSteps(steps);
                nodeStep.turns = this.CountTurnNodes(neighbor, start);                
                if(nodeStep.turns > 4) continue;
                this.Enqueue(neighbor);

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
    protected virtual List<Node> ConstructFinalPath(Node start, Node end)
    {
        List<Node> finalPath = new List<Node>();
        Node current = end;
        while (current != start) 
        {
            finalPath.Add(current);
            current = this.GetFromNode(current);
        }
        finalPath.Add(start);
        finalPath.Reverse();
        return finalPath;
    }
    protected virtual Node GetFromNode(Node toNode)
    {
        return this.GetNodeSteps(toNode).fromNode;
    }
    protected virtual NodeSteps GetNodeSteps(Node toNode)
    {
        return this.cameFromNodes.Find(item => item.toNode == toNode);
    }

    protected virtual bool IsValidPath(Node node, Node end)
    {
        return !node.occupied || node == end;
    }
    protected virtual string GetStringFromSteps(List<NodeSteps> steps)
    {
        string stepsString = "";
        foreach (NodeSteps nodeStep in steps)
        {
            stepsString += nodeStep.toNode.PrintID() + "=>";
        }
        return stepsString;
    }

    protected virtual string GetDirectionsFromSteps(List<NodeSteps> steps)
    {
        string stepsString = "";
        foreach (NodeSteps nodeStep in steps)
        {
            stepsString += nodeStep.direction + "=>";
        }
        return stepsString;
    }

    protected virtual int CountTurnNodes (Node current, Node start)
    {
        int count = 0;
        List<NodeSteps> steps = BuildTmpSteps(current, start);
        count = this.CountTurnSteps(steps);
        return count;
    }
    protected virtual int CountTurnSteps(List<NodeSteps> nodeSteps)
    {
        List<NodeDirections> directions = new List<NodeDirections>();
        NodeDirections dir;
        foreach(NodeSteps step in nodeSteps)
        {
            dir = step.direction;
            if(directions.Contains(dir)) continue;
            directions.Add(dir);
        }
        return directions.Count;
    }
    protected virtual List<NodeSteps> BuildTmpSteps(Node current, Node start)
    {
        List<NodeSteps> tmpPath = new List<NodeSteps>();
        Node checkNode = current;
        for(int i = 0; i < this.cameFromNodes.Count; i++)
        {
            NodeSteps step = GetNodeSteps(checkNode);
            tmpPath.Add(step);
            current = step.fromNode;
            if (step.fromNode == start) break;
        }
        this.ShowScanStep(current);
        return tmpPath;
    }
    protected virtual void ShowScanStep(Node currentNode)
    {
        Vector3 pos = currentNode.nodeObj.transform.position;
        Transform obj = BlockSpawner.Instance.Spawn(BlockSpawner.SCANSTEP, pos, Quaternion.identity);
        obj.gameObject.SetActive(true);
    }
}
