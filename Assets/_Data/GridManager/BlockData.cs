using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockData: BlockAbstract
{
    [Header("Block Data")]
    public Sprite sprite;
    public int blockID;
    public Node node;
    public virtual void SetNode(Node node)
    {
        this.node = node;
    }
}
