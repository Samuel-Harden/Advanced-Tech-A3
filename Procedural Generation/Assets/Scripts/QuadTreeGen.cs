using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeGen : MonoBehaviour
{
    [Header("Area Size (x, z)")]
    [SerializeField] int map_width;
    [SerializeField] int map_height;

    [Space]
    [Header("Max no. of divisions")]
    [SerializeField] int max_depth;

    [Space]
    [Header("Show/Hide Gizmos")]
    [SerializeField] bool show_positions;
    [SerializeField] bool show_nodes;

    [Space]
    [Header("Perlin Noise (Range 5-30)")]
    [SerializeField] int perlin_noise;

    [Space]
    [Header("Density of area types")]
    [SerializeField] float jumpable_density;
    [SerializeField] float walkable_density;
    [SerializeField] float impassable_density;

    [Space]
    [Header("References")]
    [SerializeField] GameObject node;
    [SerializeField] GameObject node_parent;

    [SerializeField] bool show_pf;

    private List<Vector3> positions;
    private List<Node> nodes;
    private Perlin perlin;
    private int divide_count = 1;

    Pathfinding pathfinding;


    void Start()
    {
        positions = new List<Vector3>();
        nodes = new List<Node>();
        perlin = gameObject.GetComponent<Perlin>();

        pathfinding = gameObject.GetComponent<Pathfinding>();

        CheckBounds();

        GeneratePositions();

        GenerateInitialNode();
    }


    void GeneratePositions()
    {
        perlin.GeneratePerlinData(map_width, map_height, perlin_noise, positions,
            jumpable_density, walkable_density, impassable_density);
    }


    private void GenerateInitialNode()
    {
        Vector3 pos = Vector3.zero;
        float size_x = map_width;
        float size_z = map_height;

        var node_obj = Instantiate(node, pos, node.transform.rotation);

        nodes.Add(node_obj.GetComponent<Node>());

        node_obj.GetComponent<Node>().SetDivideCount(divide_count);

        node_obj.transform.parent = node_parent.transform;

        node_obj.GetComponent<Node>().Initialise(Vector3.zero,
            size_x, size_z, positions, show_nodes, max_depth, node, nodes, perlin, 0, 0,
            node_parent.transform);

        Debug.Log("No of Nodes PRE CleanUp: " + nodes.Count);

        for (int i = (nodes.Count - 1); i >= 0; i--)
        {
            if (nodes[i].HasDivided())
            {
                Destroy(nodes[i].gameObject, 0.2f);
                nodes.RemoveAt(i);
                Debug.Log("Removed Node");
            }
        }

        Debug.Log("No of Nodes CleanedUp: " + nodes.Count);

        if(show_pf)
        pathfinding.CreateGrid(map_width, map_height, GetSmallestNode());
    }


    public void Rebuild()
    {
        for (int i = (nodes.Count - 1); i >= 0; i--)
        {
            Destroy(nodes[i].gameObject);
        }

        positions.Clear();
        nodes.Clear();

        CheckBounds();
        GeneratePositions();
        GenerateInitialNode();

        //pathfinding.ResetGrid();
        //pathfinding.CreateGrid();
    }


    private float GetSmallestNode()
    {
        float smallest_node = (map_width / 2);

        for (int i = 0; i < max_depth - 1; i++)
        {
            smallest_node = smallest_node / 2;
        }

        return smallest_node;
    }


    private void CheckBounds()
    {
        if (perlin_noise < 5)
            perlin_noise = 5;

        if (perlin_noise > 30)
            perlin_noise = 30;
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (!show_positions)
            return;

        Gizmos.color = Color.white;

        foreach(Vector3 pos in positions)
        {
            Gizmos.DrawWireSphere(pos, 1);
        }
    }
}
