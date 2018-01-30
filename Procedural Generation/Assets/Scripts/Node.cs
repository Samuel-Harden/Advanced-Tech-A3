using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private float size_x;
    private float size_z;

    private bool divided;
    private bool gizmos_enabled;

    private List<GameObject> child_nodes;

    private int no_divisions = 4;
    private int divide_count;

    Vector3 bottom_left_pos;
    Vector3 bottom_right_pos;
    Vector3 top_left_pos;
    Vector3 top_right_pos;

    private float average_density;

    bool cheese = false;


    public void Initialise(Vector3 _position, float _size_x, float _size_z,
        List<Vector3> _positions, bool _show_nodes, int _depth, GameObject _node,
        List<Node> _nodes, Perlin _perlin, int _division, float _avg_density)
    {
        child_nodes = new List<GameObject>();

        gizmos_enabled = _show_nodes;

        average_density = 0;

        size_x = _size_x;
        size_z = _size_z;

        transform.position = _position;

        // Line up positions to size of node
        bottom_left_pos = _position;
        bottom_right_pos = new Vector3(_position.x + size_x, 0, _position.z);
        top_left_pos = new Vector3(_position.x, 0, _position.z + size_z);
        top_right_pos = new Vector3(_position.x + size_x, 0, _position.z + size_z);

        //transform.parent = _parent_node.transform;

        if (_depth > 0)
        {
            // Check if this node needs spliting
            if (DivideCheck(_positions, _perlin, _avg_density))
            {
                divided = true;
                _division++;
                Divide(_positions, _depth, _node, _nodes, _perlin, _division);
            }

            if (!divided)
            {
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

                plane.transform.position = new Vector3(transform.position.x + size_x / 2, 0.5f, transform.position.z + size_z / 2);

                plane.transform.localScale = new Vector3(size_x / 10, 1.0f, size_z / 10);

                plane.transform.parent = transform;

                plane.GetComponent<Renderer>().material.color = Color.gray;
            }
        }

        if (_depth == 0)
        {
            cheese = true;
            if (average_density == 0)
                average_density = _avg_density;

            GenerateArea(_perlin);
        }
    }


    private void GenerateArea(Perlin _perlin)
    {
        // 
        if (average_density < _perlin.GetDensityPercentages()[1])
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(transform.position.x + size_x / 2, -0.5f, transform.position.z + size_z / 2);

            cube.transform.localScale = new Vector3(size_x, 1.0f, size_z);

            cube.GetComponent<Renderer>().material.color = Color.blue;

            cube.transform.parent = transform;
        }

        // Anything in between is walkable

        else if(average_density >= _perlin.GetDensityPercentages()[1])
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(transform.position.x + size_x / 2, 1.5f, transform.position.z + size_z / 2);

            cube.transform.localScale = new Vector3(size_x, 3.0f, size_z);

            cube.GetComponent<Renderer>().material.color = Color.black;

            cube.transform.parent = transform;
        }
    }


    private bool DivideCheck(List<Vector3> _positions, Perlin _perlin, float _avg_density)
    {
        int count = 0;

        foreach (Vector3 pos in _positions)
        {
            // is this position within bounds of node
            if (pos.x >= transform.position.x && pos.x < (transform.position.x + size_x) &&
                pos.z >= transform.position.z && pos.z < (transform.position.z + size_z))
            {
                // What values are inthis area, so we can produce the correct density type
                average_density += _perlin.GetDensityType((int)pos.x, (int)pos.z);
                count++;
            }
        }

        average_density = average_density / count;

        if (average_density == 0)
            average_density = _avg_density;

        if (count >= divide_count)
            return true;

        return false;
    }


    void Divide(List<Vector3> _positions, int _depth, GameObject _node, List<Node> _nodes, Perlin _perlin, int _division)
    {
        // Each recursion this should go down one
        _depth -= 1;

        Vector3 new_position = transform.position;

        float new_size_x = size_x / 2;
        float new_size_z = size_z / 2;

        int count = 0;

        // Order (Bottom left, Bottom right, Top left, Top right)
        for (int i = 0; i < no_divisions; i++)
        {
            var node_obj = Instantiate(_node, new_position, _node.transform.rotation);

            _nodes.Add(node_obj.GetComponent<Node>());

            child_nodes.Add(node_obj);

            node_obj.GetComponent<Node>().SetDivideCount(divide_count);

            node_obj.GetComponent<Node>().Initialise(new_position, new_size_x, new_size_z, _positions, gizmos_enabled, _depth, _node, _nodes, _perlin, _division, average_density);

            new_position.x += size_x / 2;

            node_obj.transform.parent = transform;

            count++;

            if (count > 1)
            {
                new_position.x = transform.position.x;
                new_position.z += size_z / 2;
                count = 0;
            }
        }
    }


    public void SetDivideCount(int _count)
    {
        divide_count = _count;
    }


    public float GetAverageDensity()
    {
        return average_density;
    }


    private void OnDrawGizmos()
    {
        if(gizmos_enabled)
        {
            if(!divided)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(bottom_left_pos, bottom_right_pos);
                Gizmos.DrawLine(bottom_right_pos, top_right_pos);
                Gizmos.DrawLine(top_right_pos, top_left_pos);
                Gizmos.DrawLine(top_left_pos, bottom_left_pos);
            }
        }
    }
}
