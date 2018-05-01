using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshTerrain : MonoBehaviour {

    public Vector2 squareSize = Vector2.one;
    public float heightStretchFactor = 5f;
    [Range(0, 2)]
    public float perlinScaleFactor = 1f;
    [Range(1, 100)]
    public int meshLength;
    [Range(1, 100)]
    public int meshWidth;
    [Range(0, 1)]
    public float volcanoHeight = 0.9f;
    public float volcanoDipFactor = 10f;

    private MeshFilter _meshFilter;
    private MeshCreator _meshCreator;

	// Use this for initialization
	private void Awake () {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCreator = new MeshCreator();
	}

    private void Update()
    {
        _meshCreator.Clear();
        GenerateTerrainMesh();
    }

    private void GenerateTerrainMesh()
    {
        // Generate Squares
        for (int x = 0; x < meshWidth; x++)
        {
            for (int y = 0; y < meshLength; y++)
            {
                // Create Vertices
                Vector3 t0 = new Vector3(squareSize.x / 2, 0, -squareSize.y / 2); // top left
                Vector3 t1 = new Vector3(-squareSize.x / 2, 0, -squareSize.y / 2); // bottom left
                Vector3 t2 = new Vector3(-squareSize.x / 2, 0, squareSize.y / 2); // bottom right
                Vector3 t3 = new Vector3(squareSize.x / 2, 0, squareSize.y / 2); // top right

                // Shift Vertices to match point on mesh
                Vector3 offset = new Vector3(x * squareSize.x, 0, y * squareSize.y);
                t0 += offset;
                t1 += offset;
                t2 += offset;
                t3 += offset;

                // Add perlin variable to heights
                t0.y = Mathf.PerlinNoise(t0.x * perlinScaleFactor, t0.z * perlinScaleFactor) * heightStretchFactor;
                t1.y = Mathf.PerlinNoise(t1.x * perlinScaleFactor, t1.z * perlinScaleFactor) * heightStretchFactor;
                t2.y = Mathf.PerlinNoise(t2.x * perlinScaleFactor, t2.z * perlinScaleFactor) * heightStretchFactor;
                t3.y = Mathf.PerlinNoise(t3.x * perlinScaleFactor, t3.z * perlinScaleFactor) * heightStretchFactor;

                // Todo - Make volcanos
                float volcanoModifiedHeight = volcanoHeight * heightStretchFactor;
                t0.y = t0.y > volcanoModifiedHeight ? t0.y - (t0.y - volcanoModifiedHeight) * volcanoDipFactor : t0.y;
                t1.y = t1.y > volcanoModifiedHeight ? t1.y - (t1.y - volcanoModifiedHeight) * volcanoDipFactor : t1.y;
                t2.y = t2.y > volcanoModifiedHeight ? t2.y - (t2.y - volcanoModifiedHeight) * volcanoDipFactor : t2.y;
                t3.y = t3.y > volcanoModifiedHeight ? t3.y - (t3.y - volcanoModifiedHeight) * volcanoDipFactor : t3.y;

                // Builds Triangle
                _meshCreator.BuildTriangle(t0, t1, t2);
                _meshCreator.BuildTriangle(t0, t2, t3);
            }
        }

        // Build Mesh
        _meshFilter.mesh = _meshCreator.CreateMesh();
    }
}
