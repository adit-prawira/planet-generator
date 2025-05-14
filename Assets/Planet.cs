using System;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)] public int resolution = 80;

    [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;
    
    [HideInInspector] public bool isShapeSettingsFoldout;
    [HideInInspector] public bool isColorSettingsFoldout;

    public bool autoUpdate = true; 
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    
    private ShapeGenerator shapeGenerator;
    
    private TerrainFace[] _terrainFaces;

    private const int VerticesSize = 6;
    

    private void Initialize()
    {
        this.shapeGenerator = new ShapeGenerator(this.shapeSettings);
        
        int meshFiltersCount = this._meshFilters?.Length ?? 0;
        bool hasMeshFilters = meshFiltersCount > 0;

        // Only create mesh filters objects if it does not exist or empty
        // size 6 vertices that made up of 2 triangles
        if (!hasMeshFilters) this._meshFilters = new MeshFilter[VerticesSize];

        this._terrainFaces = new TerrainFace[VerticesSize];
        Vector3[] directions =
        {

            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };
        for (int i = 0; i < VerticesSize; i++)
        {

            // Only create mesh objects if it does not exist, otherwise skip the loop step
            if (this._meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = this.transform;
                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                this._meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                this._meshFilters[i].sharedMesh = new Mesh();
            }

            this._terrainFaces[i] = new TerrainFace(
                this.shapeGenerator,
                this._meshFilters[i].sharedMesh, 
                this.resolution, 
                directions[i]
                );

        }
    }

    public void GeneratePlanet()
    {
        this.Initialize();
        this.GenerateMesh();
        this.GenerateColor();
    }

    public void OnColorSettingsUpdated()
    {
        if (!this.autoUpdate) return;
        this.Initialize();
        this.GenerateColor();
    }

    public void OnShapeSettingsUpdated()
    {
        if(!this.autoUpdate) return;
        this.Initialize();
        this.GenerateMesh();
    }
    
    private void GenerateMesh()
    {
        foreach (TerrainFace face in this._terrainFaces)
        {
            face?.ConstructMesh();
        }
    }

    private void GenerateColor()
    {
        foreach (MeshFilter meshFilter in this._meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = this.colorSettings.planetColor;
        }
    }

}
