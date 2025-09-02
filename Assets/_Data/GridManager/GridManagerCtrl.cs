using UnityEngine;

public class GridManagerCtrl : PikaMonoBehaviour
{
    [Header("GridManagerCtrl ")]
    private static GridManagerCtrl instance;
    public static GridManagerCtrl Instance => instance;
    public static string BLOCK = "Block";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BlockSpawner blockSpawner;
    public IPathfinding pathfinding;
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
        this.LoadPathFinding();
    }
    protected virtual void LoadSpawner()
    {
        if (this.blockSpawner != null) return;
        this.blockSpawner = transform.Find("BlockSpawner").GetComponent<BlockSpawner>();
        Debug.Log(transform.name + " LoadSpawner", gameObject); 
    }
    public virtual void SetNode(BlockCtrl blockCtrl)
    {
        if (this.firstBlock != null && this.lastBlock != null) 
        {            
            this.pathfinding.FindPath(this.firstBlock, this.lastBlock);
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
    protected virtual void LoadPathFinding()
    {
        if (this.pathfinding != null) return;
        this.pathfinding = transform.GetComponentInChildren<BFS>();
        Debug.LogWarning(transform.name + " LoadPathFinding", gameObject);
    }
}
