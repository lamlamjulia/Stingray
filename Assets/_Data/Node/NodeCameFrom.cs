using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NodeCameFrom
{
    public string nodeId;
    public NodeDirections direction = NodeDirections.Still;
    
    public Node toNode;
    public Node fromNode;
    public NodeCameFrom(Node fromNode, Node toNode)
    {
        this.nodeId = fromNode.PrintID();
        this.toNode = toNode;
        this.fromNode = fromNode;
        this.direction = this.getDirection(fromNode, toNode);
    }
    protected virtual NodeDirections getDirection(Node fromNode, Node toNode)
    {
        if(fromNode.x == toNode.x &&  fromNode.y < toNode.y) return NodeDirections.Up;
        if (fromNode.x == toNode.x && fromNode.y > toNode.y) return NodeDirections.Down;
        if (fromNode.x > toNode.x && fromNode.y == toNode.y) return NodeDirections.Left;
        if (fromNode.x < toNode.x && fromNode.y == toNode.y) return NodeDirections.Right;
        return NodeDirections.Still;
    }
}
