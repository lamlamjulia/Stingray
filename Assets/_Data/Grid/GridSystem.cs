using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridSystem: PikaMonoBehaviour
{
    public List<Node> nodes;
    public int width = 20;
    public int height = 13;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.InitGridSystem();
    }
    protected virtual void InitGridSystem()
    {
        if (this.nodes.Count > 0) return;
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                Node node = new Node()
                {
                    x = x,
                    y = y
                };
                this.nodes.Add(node);
            }
        }
    }
}
