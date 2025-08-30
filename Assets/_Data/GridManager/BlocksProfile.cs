using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlocksProfile", menuName = "Scriptable Objects/BlocksProfile")]
public class BlocksProfile : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
}
