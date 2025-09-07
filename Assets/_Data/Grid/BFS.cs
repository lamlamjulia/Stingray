using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS: GridAbstract, IPathfinding
{
    [Header("BFS")]
    //public List<Node> queue = new List<Node> ();
    public List<Node> path = new List<Node>();
    //public List<NodeSteps> cameFromNodes = new List<NodeSteps> ();
    //public List<Node> visited = new List<Node> ();
    Queue<NodeSteps> nodeQueue = new Queue<NodeSteps>();
    Dictionary<(Node,NodeDirections), int> visitedDic = new Dictionary<(Node, NodeDirections), int>();
    public virtual void FindPath(BlockCtrl startBlock, BlockCtrl targetBlock)
    {
        Node start = startBlock.blockData.node;
        Node target = targetBlock.blockData.node;
        
        nodeQueue.Clear();
        visitedDic.Clear();

        var startStep = new NodeSteps(start, null); //start has no parent
        startStep.turns = 0;

        Debug.LogWarning($"FindPath called: start={(start == null ? "NULL" : start.PrintID())}, target={(target == null ? "NULL" : target.PrintID())}");

        nodeQueue.Enqueue(startStep);

        Debug.LogWarning($"Queue count after enqueue = {this.nodeQueue.Count}");
        //this.Enqueue(start);
        visitedDic[(start, NodeDirections.Still)] = 0;
        //this.cameFromNodes.Add(new NodeSteps(start, start));
        //this.visited.Add(start);
        List<NodeSteps> candidates = new List<NodeSteps>();
        //NodeSteps nodeStep;
        //List<NodeSteps> steps;
        while (this.nodeQueue.Count > 0)
        {
            var current = nodeQueue.Dequeue();
            //Debug.LogWarning($"Dequeued: {current.nodeId}");

            if (current.toNode == target && current.turns <= 2)
            {
                Debug.LogWarning("Pair found");
                //this.path = ConstructFinalPath(current);
                candidates.Add(current);
                continue;
                //break;
            }
            foreach(Node neighbor in current.toNode.Neighbors())
            {
                Debug.LogWarning("Testing top!");
                if (neighbor == null) continue;
                //if (this.visitedDic.Contains(neighbor)) continue;
                if(!this.IsValidPath(neighbor, target)) continue;

                var step = new NodeSteps(neighbor, current.toNode);

                step.parent = current;

                int newTurns = current.turns;

                if (current.direction != NodeDirections.Still && step.direction != current.direction)
                    newTurns++;
                
                step.turns = newTurns;

                if (newTurns > 2) continue;

                var key = (neighbor, step.direction);

                if (visitedDic.TryGetValue(key, out int bestTurns) && bestTurns <= newTurns) continue;

                visitedDic[key] = newTurns;

                //ShowScanStep(current);

                nodeQueue.Enqueue(step);

                //this.visited.Add(neighbor);
                //this.cameFromNodes.Add(nodeStep);

                //steps = this.BuildTmpSteps(neighbor, start);
                //nodeStep.stepsString = this.GetStringFromSteps(steps);
                //nodeStep.directionString = this.GetDirectionsFromSteps(steps);
                //nodeStep.turns = this.CountTurnNodes(neighbor, start);    
                
                //if(nodeStep.turns > 3) continue;
                //this.Enqueue(neighbor);
            }
            if (candidates.Count > 0)
            {
                var best = candidates.OrderBy(s => s.turns).ThenBy(s => GetPathLength(s)).First();
                this.path = ConstructFinalPath(best);
                Debug.LogWarning($"Best path chosen with {best.turns} turns, length={GetPathLength(best)}");
                ShowScanStep(current);
            }
            else
            {
                Debug.LogWarning("No valid path found ❌");
            }
        }

        //this.ShowVisited();
        this.ShowPath();

    }
    protected virtual int GetPathLength(NodeSteps step)
    {
        int length = 0;
        while (step != null)
        {
            length++;
            step = step.parent;
        }
        return length;
    }
    //protected virtual void Enqueue(Node node)
    //{
    //    if (node == null)
    //    {
    //        Debug.LogWarning("⚠️ Tried to enqueue NULL node");
    //        return;
    //    }
    //    this.nodeQueue.Add(node);
    //    Debug.LogWarning($"Enqueued {node.PrintID()}, queue size now {this.nodeQueue.Count}");
    //}
    //protected virtual Node Dequeue()
    //{
    //    Node node = this.nodeQueue[0];
    //    this.nodeQueue.RemoveAt(0);
    //    return node;
    //}
    //protected virtual void ShowVisited()
    //{
    //    foreach (Node node in this.visitedDic)
    //    {
    //        Vector3 pos = node.nodeObj.transform.position;
    //        Transform obj = this.ctrl.blockSpawner.Spawn(BlockSpawner.SCAN, pos, Quaternion.identity);
    //        obj.gameObject.SetActive(true);
    //    }
    //}
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
    protected virtual List<Node> ConstructFinalPath(NodeSteps end)
    {
        List<Node> finalPath = new List<Node>();
        NodeSteps current = end;
        while (current != null) 
        {
            finalPath.Add(current.toNode);
            current = current.parent;
        }
        finalPath.Reverse();
        return finalPath;
    }
    //protected virtual Node GetFromNode(Node toNode)
    //{
    //    return this.GetNodeSteps(toNode).fromNode;
    //}
    //protected virtual NodeSteps GetNodeSteps(Node toNode)
    //{
    //    return this.cameFromNodes.First(item => item.toNode == toNode);
    //}

    protected virtual bool IsValidPath(Node node, Node end)
    {
        if (node == end) return true;
        return !node.occupied;
    }
    //protected virtual string GetStringFromSteps(List<NodeSteps> steps)
    //{
    //    string stepsString = "";
    //    foreach (NodeSteps nodeStep in steps)
    //    {
    //        stepsString += nodeStep.toNode.PrintID() + "=>";
    //    }
    //    return stepsString;
    //}

    //protected virtual string GetDirectionsFromSteps(List<NodeSteps> steps)
    //{
    //    string stepsString = "";
    //    foreach (NodeSteps nodeStep in steps)
    //    {
    //        stepsString += nodeStep.direction + "=>";
    //    }
    //    return stepsString;
    //}

    //protected virtual int CountTurnNodes (Node current, Node start)
    //{
    //    int count = 0;
    //    List<NodeSteps> steps = BuildTmpSteps(current, start);
    //    count = this.CountTurnSteps(steps);
    //    return count;
    //}
    //protected virtual int CountTurnSteps(List<NodeSteps> nodeSteps)
    //{
    //    List<NodeDirections> directions = new List<NodeDirections>();
    //    NodeDirections dir;
    //    foreach(NodeSteps step in nodeSteps)
    //    {
    //        dir = step.direction;
    //        if(directions.Contains(dir)) continue;
    //        directions.Add(dir);
    //    }
    //    return directions.Count;
    //}
    //protected virtual List<NodeSteps> BuildTmpSteps(Node current, Node start)
    //{
    //    List<NodeSteps> tmpPath = new List<NodeSteps>();
    //    Node checkNode = current;
    //    for(int i = 0; i < this.cameFromNodes.Count; i++)
    //    {
    //        NodeSteps step = this.GetNodeSteps(checkNode);
    //        tmpPath.Add(step);
    //        current = step.fromNode;
    //        if (step.fromNode == start) break;
    //    }
    //    //this.ShowScanStep(current);
    //    return tmpPath;
    //}
    protected virtual void ShowScanStep(NodeSteps step)
    {
        Vector3 pos = step.toNode.nodeObj.transform.position;
        Transform obj = BlockSpawner.Instance.Spawn(BlockSpawner.SCANSTEP, pos, Quaternion.identity);
        obj.gameObject.SetActive(true);
        if (step == null)
        {
            Debug.LogWarning("ShowScanStep: step is null");
            return;
        }
        if (step.toNode == null)
        {
            Debug.LogWarning("ShowScanStep: toNode is null");
            return;
        }

        Debug.Log($"[SCAN] {step.toNode.PrintID()} dir={step.direction} turns={step.turns}");

        if (step.parent?.toNode != null)
        {
            Vector3 from = new Vector3(step.parent.toNode.posX, step.parent.toNode.y, 0);
            Vector3 to = new Vector3(step.toNode.posX, step.toNode.y, 0);
            Debug.DrawLine(from, to, Color.black, 1f);
        }
    }
}
