using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Node
{
    public int x = 0;
    public int y = 0;
    public float posX = 0;
    public int weight = 1;
    public bool occupied = false;
    public Node up;
    public Node left;
    public Node right;
    public Node down;
    public BlockCtrl blockCtrl;
    public int nodeId;
}
