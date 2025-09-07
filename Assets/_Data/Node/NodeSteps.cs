using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NodeSteps
{
    public string nodeId;
    public NodeDirections direction = NodeDirections.Still;
    public string stepsString = "";
    public string directionString = "";
    public Node toNode;
    public Node fromNode;
    public int turns = 0;
    public NodeSteps parent;
    public NodeSteps(Node toNode, Node fromNode)
    {
        this.nodeId = toNode.PrintID();
        this.toNode = toNode;
        this.fromNode = fromNode;
        this.direction = this.getDirection(fromNode, toNode);
    }
    protected virtual NodeDirections getDirection(Node fromNode, Node toNode)
    {
        if (fromNode == null || toNode == null) return NodeDirections.Still;
        if (fromNode.x == toNode.x &&  fromNode.y < toNode.y) return NodeDirections.Up;
        if (fromNode.x == toNode.x && fromNode.y > toNode.y) return NodeDirections.Down;
        if (fromNode.x > toNode.x && fromNode.y == toNode.y) return NodeDirections.Left;
        if (fromNode.x < toNode.x && fromNode.y == toNode.y) return NodeDirections.Right;
        return NodeDirections.Still;
    }
}
