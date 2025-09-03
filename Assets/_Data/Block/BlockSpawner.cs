using UnityEngine;


public class BlockSpawner: Spawner
{
    [Header("BlockSpawner")]
    private static BlockSpawner instance;
    public static BlockSpawner Instance => instance;
    public static string BLOCK = "Block";
    public static string LINKER = "Linker";
    public static string HOLDER = "NodeTransform";

    protected override void Awake()
    {
        base.Awake();
        if (BlockSpawner.instance != null) Debug.LogError("Only 1 Spawner should exist");
        BlockSpawner.instance = this;
    }
    
}
