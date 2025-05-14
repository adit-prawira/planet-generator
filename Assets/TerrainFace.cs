using UnityEngine;

public class TerrainFace
{
    private Mesh _mesh;
    private int _resolution;
    private Vector3 _localUp;
    private Vector3 _axisA;
    private Vector3 _axisB;
    
    private ShapeGenerator _shapeGenerator;

    public TerrainFace(
        ShapeGenerator shapeGenerator,
        Mesh mesh, 
        int resolution, 
        Vector3 localUp)
    {
        this._shapeGenerator = shapeGenerator;
        this._mesh = mesh;
        this._resolution = resolution;
        
        this._localUp = localUp;
        this._axisA = new Vector3(this._localUp.y, this._localUp.z, this._localUp.x);
        
        // perpendicular to localUp and axisA
        this._axisB = Vector3.Cross(this._localUp, this._axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[this._resolution * this._resolution];
        int[] triangles = new int[(this._resolution - 1) * (this._resolution - 1) * 2 * 3];
        int triangleIndex = 0;
        
        for (int y = 0; y < this._resolution; y++)
        {
            for (int x = 0; x < this._resolution; x++)
            {
                int i = x + y * this._resolution;
                Vector2 percent = new Vector2(x, y) / (this._resolution - 1);
                Vector3 pointOnUnitCube = this._localUp 
                                          + ((percent.x - .5f) * 2 * this._axisA)
                                          + ((percent.y - .5f) * 2 * this._axisB);
                // ensure all vertices to be the same distance to center
                // thus converting the shapes to be as a sphere
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;
                vertices[i] = this._shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);
                
                bool isXEdge = x == this._resolution - 1;
                bool isYEdge = y == this._resolution - 1;
                bool isEdge =isXEdge || isYEdge;
                
                if (isEdge) continue;
                
                triangles[triangleIndex] = i;
                triangles[triangleIndex + 1] = i + this._resolution + 1;
                triangles[triangleIndex + 2] = i + this._resolution;

                triangles[triangleIndex + 3] = i;
                triangles[triangleIndex + 4] = i + 1;
                triangles[triangleIndex + 5] = i + this._resolution + 1;
                    
                // index added by total of 6 vertices added
                triangleIndex += 6; 
            }
        }
        
        this._mesh.Clear();
        this._mesh.vertices = vertices;
        this._mesh.triangles = triangles;
        this._mesh.RecalculateNormals();
    }
}
