using System;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)] public int resolution = 80;

    [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;
    
    [HideInInspector] public bool isShapeSettingsFoldout;
    [HideInInspector] public bool isColorSettingsFoldout;

    public enum FaceRenderMask
    {
        All,
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    };

    public FaceRenderMask faceRenderMask;
    public bool autoUpdate = true; 
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    
    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    private ColorGenerator _colorGenerator = new ColorGenerator();
    
    private TerrainFace[] _terrainFaces;

    private const int VerticesSize = 6;
    

    private void Initialize()
    {
        this._shapeGenerator.UpdateSettings(this.shapeSettings);
        this._colorGenerator.UpdateSettings(this.colorSettings);
        
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
            if (!this._meshFilters[i])
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = this.transform;
                
                meshObject.AddComponent<MeshRenderer>();
                this._meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                this._meshFilters[i].sharedMesh = new Mesh();
            }

            this._meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = this.colorSettings.planetMaterial;
            this._terrainFaces[i] = new TerrainFace(
                this._shapeGenerator,
                this._meshFilters[i].sharedMesh, 
                this.resolution, 
                directions[i]
                );
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1  == i;
            this._meshFilters[i].gameObject.SetActive(renderFace);
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
        for (int i = 0; i < VerticesSize; i++)
        {
            if (this._meshFilters[i].gameObject.activeSelf)
            {
                this._terrainFaces[i].ConstructMesh();
            }   
        }
        
        this._colorGenerator.UpdateElevation(this._shapeGenerator.elevationMinMax);
    }

    private void GenerateColor()
    {
        this._colorGenerator.UpdateColors();
    }

}
