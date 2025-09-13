using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class BlockCtrl: PikaMonoBehaviour
{
    [Header("Block Ctrl")]
    //public SpriteRenderer spriteRender;
    //public Transform model;
    public SpriteRenderer sprite;
    public string blockID = "#";
    public BlockData blockData;
    public List<BlockCtrl> neighbors = new List<BlockCtrl>();
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadBlockData();
    }
    protected virtual void LoadModel()
    {
        if (this.sprite != null) return;
        Transform model = transform.Find("Model");
        this.sprite = model.GetComponent<SpriteRenderer>();
        //Debug.Log(transform.name + " LoadModel", gameObject);
    }
    //public virtual void ReloadModel()
    //{
    //    this.model = transform.Find("Model");
    //    this.spriteRender = this.model.GetComponent<SpriteRenderer>();
    //}
    protected virtual void LoadBlockData()
    {
        if (this.blockData != null) return;
        this.blockData = transform.Find("BlockData").GetComponent<BlockData>();
        //Debug.Log(transform.name + " LoadBlockData", gameObject);
    }
    //public virtual void SetSprite(Sprite sprite)
    //{
    //    this.sprite = sprite;
    //    //this.spriteRender.sprite = sprite;
    //}
}
