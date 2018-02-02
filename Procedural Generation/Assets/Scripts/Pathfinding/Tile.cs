using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable;
    public Vector3 world_pos;

    public Tile(bool _walkable, Vector3 _world_pos)
    {
        walkable = _walkable;
        world_pos = _world_pos;
    }
}
