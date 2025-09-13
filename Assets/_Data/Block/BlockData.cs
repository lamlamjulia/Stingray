using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
        this.ctrl.sprite.sprite = sprite;
        this.ctrl.blockID = sprite.name;
    }
    
}
