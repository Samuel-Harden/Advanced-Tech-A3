using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] LayerMask unwalkable_mask;

    private Tile[,] grid;
    private int grid_size_x, grid_size_z;
    private float tile_radius;

    private List<GameObject> cubes;


    public void CreateGrid(int _map_width, int _map_height, float _tile_size)
    {
        cubes = new List<GameObject>();

        tile_radius = _tile_size / 2;

        grid_size_x = Mathf.RoundToInt(_map_width / _tile_size);
        grid_size_z = Mathf.RoundToInt(_map_height / _tile_size);

        grid = new Tile[grid_size_x, grid_size_z];

        Vector3 world_pos = new Vector3(tile_radius, 0, tile_radius);

        for (int x = 0; x < grid_size_x; x++)
        {
            for (int z = 0; z < grid_size_z; z++)
            {
                bool walkable = !(Physics.CheckSphere(world_pos, tile_radius / 2, unwalkable_mask));

                grid[x, z] = new Tile(walkable, world_pos);

                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube.transform.position = world_pos;
                cube.transform.localScale = new Vector3(_tile_size / 2, _tile_size / 2, _tile_size / 2);

                if (!walkable)
                {
                    cube.GetComponent<Renderer>().material.color = Color.red;
                }

                if (walkable)
                {
                    cube.GetComponent<Renderer>().material.color = Color.white;
                }

                cubes.Add(cube);

                world_pos.x += _tile_size;
            }

            world_pos.x =  tile_radius;
            world_pos.z += _tile_size;

        }
    }


    public void ResetGrid()
    {
        grid = new Tile[grid_size_x, grid_size_z];

        for (int i = (cubes.Count - 1); i >= 0; i--)
        {
            Destroy(cubes[i].gameObject);
        }
    }


    /*void OnDrawGizmos()
    {
        if(grid != null)
        {
            foreach (Tile tile in grid)
            {
                Gizmos.color = (tile.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(tile.world_pos, Vector3.one * (tile_radius - 0.1f));
            }
        }
    }*/
}
