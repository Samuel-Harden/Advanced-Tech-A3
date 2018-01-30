using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeGen : MonoBehaviour
{
    [Header("Area Size (x, z)")]
    [SerializeField] int map_width;
    [SerializeField] int map_height;

    [Space]
    [Header("Number of Positions")]
    [SerializeField] int no_positions;

    [Space]
    [Header("Max no. of divisions")]
    [SerializeField] int max_depth;

    [Space]
    [Header("No of positions needed to divide")]
    [SerializeField] int divide_count = 2;

    [Space]
    [Header("Show/Hide Gizmos")]
    [SerializeField] bool show_positions;
    [SerializeField] bool show_nodes;

    [Space]
    [Header("Use Perlin for Generation?")]
    [SerializeField] bool use_perlin;

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

    private List<Vector3> positions;

    private List<Node> nodes;

    private Perlin perlin;

    void Start()
    {
        positions = new List<Vector3>();
        nodes = new List<Node>();
        perlin = gameObject.GetComponent<Perlin>();

        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //plane.transform.position = new Vector3(map_width / 2, 0.0f, map_height / 2);
        //plane.transform.localScale = new Vector3(map_width / 10, 1.0f, map_height / 10);

        if (perlin_noise < 5)
            perlin_noise = 5;

        if (perlin_noise > 30)
            perlin_noise = 30;

        GeneratePositions();

        GenerateInitialNode();
    }


    void GeneratePositions()
    {
        //Generate Random Set
        if (!use_perlin)
        {
            for (int i = 0; i < no_positions; i++)
            {
                Vector3 pos = new Vector3(Random.Range(0, map_width), 0, Random.Range(0, map_height));

                positions.Add(pos);
            }
        }

        else if(use_perlin)
        {
            perlin.GeneratePerlinData(map_width, map_height, perlin_noise, positions, jumpable_density, walkable_density, impassable_density);
        }
    }


    private void GenerateInitialNode()
    {
        Vector3 pos = Vector3.zero;
        float size_x = map_width;
        float size_z = map_height;

        var node_obj = Instantiate(node, pos, node.transform.rotation);

        nodes.Add(node_obj.GetComponent<Node>());

        node_obj.GetComponent<Node>().SetDivideCount(divide_count);

        node_obj.transform.parent = transform;

        node_obj.GetComponent<Node>().Initialise(Vector3.zero,
            size_x, size_z, positions, show_nodes, max_depth, node, nodes, perlin, 0, 0);
    }


    public void Rebuild()
    {
        for (int i = (nodes.Count - 1); i >= 0; i--)
        {
            Destroy(nodes[i].gameObject);
        }

        positions.Clear();
        nodes.Clear();

        GeneratePositions();
        GenerateInitialNode();
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
