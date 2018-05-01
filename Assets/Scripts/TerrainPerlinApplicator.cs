using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainPerlinApplicator : MonoBehaviour {

    [Range(0,0.05f)]
    public float perlinScale = 0.05f;
    [Range(0,0.2f)]
    public float perlinHeightModifier = 0.05f;

    private Terrain _terrain;

    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
    }

    private void Update()
    {
        ApplyPerlin();
    }

    private void ApplyPerlin()
    {
        TerrainData terrainData = _terrain.terrainData;
        int width = terrainData.heightmapWidth;
        int height = terrainData.heightmapHeight;
        float[,] perlinHeights = new float[width, height];

        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                perlinHeights[x, y] = Mathf.PerlinNoise(x * perlinScale, y * perlinScale) * perlinHeightModifier;
            }
        }

        terrainData.SetHeights(0, 0, perlinHeights);
    }
}
