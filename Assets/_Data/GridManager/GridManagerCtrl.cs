using UnityEngine;

public class GridManagerCtrl : PikaMonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BlockSpawner blockSpawner;

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
}
