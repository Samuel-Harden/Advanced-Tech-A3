using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perlin : MonoBehaviour
{
    private List<float> perlin_value;

    private float perlin_noise;

    private float water_density = 33.3f;
    private float walkable_density = 33.3f;
    private float impassable_density = 33.3f;

    private List<int> density_percentages;

    private float[,] density_types;


    public void GeneratePerlinData(int _map_width, int _map_height, int _noise, List<Vector3> _positions, float _water, float _walkable, float _impassable)
    {
        perlin_value = new List<float>();
        perlin_noise = _noise;

        density_types = new float[_map_width, _map_height];

        density_percentages = new List<int>();

        water_density = _water;
        walkable_density = _walkable;
        impassable_density = _impassable;

        SetPercentages();

        float seed = Random.Range(0, 100);

        /*for (int h = 0; h < _map_height; h++)
        {
            for (int w = 0; w < _map_width; w++)
            {
                int result = (int)(Mathf.PerlinNoise(w / perlin_noise + seed, h / perlin_noise + seed) * 100);

                perlin_value.Add(result);

                if (result >= 50)
                {
                    Vector3 pos = new Vector3(w, 0, h);
                    _positions.Add(pos);
                }
            }
        }*/

        for (int h = 0; h < _map_height; h++)
        {
            for (int w = 0; w < _map_width; w++)
            {
                density_types[w, h] = (int)(Mathf.PerlinNoise(w / perlin_noise + seed, h / perlin_noise + seed) * 100);

                perlin_value.Add(density_types[w, h]);

                // ONLY WANT TO ADD A POS IF ITS NOT WALKABLE
                if (density_types[w, h] < density_percentages[0] || density_types[w, h] >= density_percentages[1])
                {
                    Vector3 pos = new Vector3(w, 0, h);
                    _positions.Add(pos);
                }
            }
        }
    }


    public void SetPerlinNoise(int _noise)
    {
        perlin_noise = _noise;
    }


    public float GetPerlinData(int _pos)
    {
        return perlin_value[_pos];
    }


    public float GetDensityType(int _posX, int _posZ)
    {
        return density_types[_posX, _posZ];
    }


    public List<int> GetDensityPercentages()
    {
        return density_percentages;
    }


    private void SetPercentages()
    {
        float total = walkable_density + water_density + impassable_density;

        int water = Mathf.RoundToInt(water_density / total * 100);
        int walkable = Mathf.RoundToInt(walkable_density / total * 100);
        int impassable = Mathf.RoundToInt(impassable_density / total * 100);

        density_percentages.Add(water);
        density_percentages.Add(water + walkable);
        density_percentages.Add(water + walkable + impassable);
    }
}
