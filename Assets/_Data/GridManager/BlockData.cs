using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockData: BlockAbstract
{
    [Header("Block Data")]
    public Node node;
    public virtual void SetNode(Node node)
    {
        this.node = node;
    }
    public virtual void SetSprite(Sprite sprite)
    {
        this.ctrl.spriteRender.sprite = sprite;
    }
}
