using UnityEngine;


public class BlockSpawner: Spawner
{
    [Header("BlockSpawner")]
    private static BlockSpawner instance;
    public static BlockSpawner Instance => instance;
    public static string BLOCK = "Block";
    public BlocksProfile blocksProfile;

    protected override void Awake()
    {
        base.Awake();
        if (BlockSpawner.instance != null) Debug.LogError("Only 1 Spawner should exist");
        BlockSpawner.instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBlockProfile();
    }
    protected virtual void LoadBlockProfile()
    {
        if (this.blocksProfile != null) return;
        this.blocksProfile = Resources.Load<BlocksProfile>("Pikachu");
        Debug.Log(transform.name + " LoadBlocksProfile", gameObject);
    }
}
