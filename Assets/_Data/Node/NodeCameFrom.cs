using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NodeCameFrom
{
    public string nodeId;
    public Node current;
    public Node cameFrom;
    public NodeCameFrom(Node currentNode, Node cameFromNode)
    {
        this.nodeId = currentNode.x.ToString() + 'x' + currentNode.y.ToString();
        this.current = currentNode;
        this.cameFrom = cameFromNode;
    }
    
}
