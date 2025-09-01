using UnityEngine;

public class GridManagerCtrl : PikaMonoBehaviour
{
    [Header("GridManagerCtrl ")]
    private static GridManagerCtrl instance;
    public static GridManagerCtrl Instance => instance;
    public static string BLOCK = "Block";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BlockSpawner blockSpawner;
    public BlockCtrl firstBlock;
    public BlockCtrl lastBlock;

    protected override void Awake()
    {
        base.Awake();
        if (GridManagerCtrl.instance != null) Debug.LogError("Only 1 GridManagerCtrl  should exist");
        GridManagerCtrl.instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawner();
    }
    protected virtual void LoadSpawner()
    {
        if (this.blockSpawner != null) return;
        this.blockSpawner = transform.Find("BlockSpawner").GetComponent<BlockSpawner>();
        Debug.Log(transform.name + " LoadSpawner", gameObject); 
    }
    protected virtual void SetBlock(BlockCtrl blockCtrl)
    {
        if (this.firstBlock != null && this.lastBlock != null) 
        {
            this.firstBlock = null;
            this.lastBlock = null;
            Debug.LogWarning("Reset blocks");
            return;
        }
        if (this.firstBlock == null)
        {
            this.firstBlock = blockCtrl;
            return;
        }

        this.lastBlock = blockCtrl;
    }
}
