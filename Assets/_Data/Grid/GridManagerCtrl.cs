using UnityEngine;

public class GridManagerCtrl : PikaMonoBehaviour
{
    [Header("GridManagerCtrl ")]
    private static GridManagerCtrl instance;
    public static GridManagerCtrl Instance => instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BlockSpawner blockSpawner;
    public GridBlockHandler blockHandler;
    public IPathfinding pathfinding;
    public GridSystem gridSystem;

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
    protected virtual void LoadGridSystem()
    {
        if(this.gridSystem != null) return;
        this.gridSystem = transform.Find("GridSystem").GetComponent<GridSystem>();
        Debug.Log(transform.name + " LoadGridsystem", gameObject);
    }
    protected virtual void LoadPathFinding()
    {
        if (this.pathfinding != null) return;
        this.pathfinding = transform.GetComponentInChildren<IPathfinding>();
        Debug.LogWarning(transform.name + " LoadPathFinding", gameObject);
    }
}
