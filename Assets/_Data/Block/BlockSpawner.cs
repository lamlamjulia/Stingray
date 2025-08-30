using UnityEngine;

public class BlockSpawner: Spawner
{
    private static BlockSpawner instance;
    public static BlockSpawner Instance => instance;
    public static string BLOCK = "Block";

    protected override void Awake()
    {
        base.Awake();
        if (BlockSpawner.instance != null) Debug.LogError("Only 1 Spawner should exist");
        BlockSpawner.instance = this;
    }
}
