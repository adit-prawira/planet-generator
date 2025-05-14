using UnityEngine;

public class ShapeGenerator 
{
   ShapeSettings shapeSettings;


   public ShapeGenerator(ShapeSettings shapeSettings)
   {
      this.shapeSettings = shapeSettings;
   }

   public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere) =>
      pointOnUnitSphere * this.shapeSettings.planetRadius;

}
