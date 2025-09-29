using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS: GridAbstract, IPathfinding
{
    [Header("BFS")]
    public List<Node> path = new List<Node>();
    Queue<NodeSteps> nodeQueue = new Queue<NodeSteps>();
    Dictionary<(Node,NodeDirections), int> visitedDic = new Dictionary<(Node, NodeDirections), int>();
    public virtual void DataReset()
    {
        this.path = new List<Node>();
        this.nodeQueue = new Queue<NodeSteps>();
        this.visitedDic = new Dictionary<(Node, NodeDirections), int>();
    }
    public virtual bool FindPath(BlockCtrl startBlock, BlockCtrl targetBlock)
    {
        Node start = startBlock.blockData.node;
        Node target = targetBlock.blockData.node;
        
        nodeQueue.Clear();
        visitedDic.Clear();

        var startStep = new NodeSteps(start, null); //start has no parent

        startStep.turns = 0;

        //Debug.LogWarning($"FindPath called: start={(start == null ? "NULL" : start.PrintID())}, target={(target == null ? "NULL" : target.PrintID())}");

        nodeQueue.Enqueue(startStep);

        visitedDic[(start, NodeDirections.Still)] = 0;

        List<NodeSteps> candidates = new List<NodeSteps>();

        while (this.nodeQueue.Count > 0)
        {
            var current = nodeQueue.Dequeue();
                        
            foreach(Node neighbor in current.toNode.Neighbors())
            {
                if (neighbor == null) continue;
                if(!this.IsValidPath(neighbor, target)) continue;

                var step = new NodeSteps(neighbor, current.toNode);

                step.parent = current;

                int newTurns = current.turns;

                if (current.direction != NodeDirections.Still && step.direction != current.direction)
                    newTurns++;
                
                step.turns = newTurns;

                if (newTurns > 2) continue;

                if (neighbor == target)
                {
                    if (current.turns <= 2)
                    {
                        candidates.Add(step);
                    }
                    continue;
                }

                var key = (neighbor, step.direction);

                if (visitedDic.TryGetValue(key, out int bestTurns) && bestTurns <= newTurns) continue;

                visitedDic[key] = newTurns;

                nodeQueue.Enqueue(step);

            }
        }
        if (candidates.Count > 0)
        {
            var best = candidates.OrderBy(s => s.turns).ThenBy(s => GetPathLength(s)).First();
            this.path = ConstructFinalPath(best);
            //Debug.LogWarning($"Best path chosen with {best.turns} turns, length={GetPathLength(best)}");
            return true;
        }
        //Debug.LogWarning("No valid path found");
        return false;

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

    protected virtual bool IsValidPath(Node node, Node end)
    {
        if (node == end) return true;
        return !node.occupied;
    }
    //protected virtual void ShowScanStep(NodeSteps step)
    //{
    //    Vector3 pos = step.toNode.nodeObj.transform.position;
    //    Transform obj = BlockSpawner.Instance.Spawn(BlockSpawner.SCANSTEP, pos, Quaternion.identity);
    //    obj.gameObject.SetActive(true);
    //    if (step == null)
    //    {
    //       // Debug.LogWarning("ShowScanStep: step is null");
    //        return;
    //    }
    //    if (step.toNode == null)
    //    {
    //        //Debug.LogWarning("ShowScanStep: toNode is null");
    //        return;
    //    }

    //    //Debug.Log($"[SCAN] {step.toNode.PrintID()} dir={step.direction} turns={step.turns}");

    //    if (step.parent?.toNode != null)
    //    {
    //        Vector3 from = new Vector3(step.parent.toNode.posX, step.parent.toNode.y, 0);
    //        Vector3 to = new Vector3(step.toNode.posX, step.toNode.y, 0);
    //        Debug.DrawLine(from, to, Color.black, 1f);
    //    }
    //}
}
