using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NodeCameFrom
{
    public Node current;
    public Node cameFrom;
    public NodeCameFrom(Node currentNode, Node cameFromNode)
    {
        this.current = currentNode;
        this.cameFrom = cameFromNode;
    }
    
}
