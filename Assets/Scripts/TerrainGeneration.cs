using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public int depth = 20;

    public float scale;
    float xOffset = 0;
    float yOffset = 0;

    void Start()
    {
        xOffset = Random.Range(0, 9999);
        yOffset = Random.Range(0, 9999);
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + xOffset;
        float yCoord = (float)y / width * scale + yOffset;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
