using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/BlocksProfile", order = 1)]
public class BlocksProfile : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
}
