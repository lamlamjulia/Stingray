using UnityEngine;


public class BlockSpawner: Spawner
{
    [Header("BlockSpawner")]
    private static BlockSpawner instance;
    public static BlockSpawner Instance => instance;
    public static string BLOCK = "Block";
    public static string LINKER = "Linker";
    public static string NODEOBJ = "NodeObj";
    public static string CHOOSE = "Choose";
    public static string SCAN = "Scan";
    public static string SCANSTEP = "ScanStep";

    protected override void Awake()
    {
        base.Awake();
        if (BlockSpawner.instance != null) Debug.LogError("Only 1 Spawner should exist");
        BlockSpawner.instance = this;
    }
    
}
