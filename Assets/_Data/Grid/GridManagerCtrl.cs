using UnityEngine;

public class GridManagerCtrl : PikaMonoBehaviour
{
    [Header("GridManagerCtrl ")]
    private static GridManagerCtrl instance;
    public static GridManagerCtrl Instance => instance;

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
        Vector3 pos;
        Transform chooseObj;
        if (this.firstBlock == null)
        {
            this.firstBlock = blockCtrl;
            pos = blockCtrl.transform.position;
            chooseObj = this.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
            chooseObj.gameObject.SetActive(true);
            return;
        }

        // When selecting the second block
        this.lastBlock = blockCtrl;
        pos = blockCtrl.transform.position;
        chooseObj = this.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
        chooseObj.gameObject.SetActive(true);

        // ✅ Run pathfinding immediately
        this.pathfinding.FindPath(this.firstBlock, this.lastBlock);
        this.firstBlock = null;
        this.lastBlock = null;
        Debug.Log("Pathfinding done, reset blocks");

        //if (this.firstBlock != null && this.lastBlock != null)
        //{
        //    this.pathfinding.FindPath(this.firstBlock, this.lastBlock);
        //    this.firstBlock = null;
        //    this.lastBlock = null;
        //    Debug.Log("Reset blocks");
        //    return;
        //}

        //Vector3 pos;
        //Transform chooseObj;
        //if (this.firstBlock == null)
        //{
        //    this.firstBlock = blockCtrl;
        //    pos = blockCtrl.transform.position;
        //    chooseObj = this.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
        //    chooseObj.gameObject.SetActive(true);
        //    return;
        //}

        //this.lastBlock = blockCtrl;
        //pos = blockCtrl.transform.position;
        //chooseObj = this.blockSpawner.Spawn(BlockSpawner.CHOOSE, pos, Quaternion.identity);
        //chooseObj.gameObject.SetActive(true);
    }
    protected virtual void LoadPathFinding()
    {
        if (this.pathfinding != null) return;
        this.pathfinding = transform.GetComponentInChildren<IPathfinding>();
        Debug.LogWarning(transform.name + " LoadPathFinding", gameObject);
    }
}
