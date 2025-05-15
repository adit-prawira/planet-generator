using UnityEngine;

public class ShapeGenerator 
{
   ShapeSettings shapeSettings;
   INoiseFilter[] noiseFilters;
   

   public ShapeGenerator(ShapeSettings shapeSettings)
   {
      this.shapeSettings = shapeSettings;
      this.noiseFilters = new INoiseFilter[this.shapeSettings.noiseLayers.Length];
      for (int i = 0; i < this.shapeSettings.noiseLayers.Length; i++)
      {
         this.noiseFilters[i]  = NoiseFilterFactory.CreateNoiseFilter(this.shapeSettings.noiseLayers[i].noiseSettings);
      }
   }

   public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
   {
      float firstLayerValue = 0;
      float elevation = 0;

      if (this.noiseFilters.Length > 0)
      {
         firstLayerValue = this.noiseFilters[0].Evaluate(pointOnUnitSphere);
         if (this.shapeSettings.noiseLayers[0].enabled) elevation = firstLayerValue;
      }

      for (int i = 1; i < this.noiseFilters.Length; i++)
      {
         if(!this.shapeSettings.noiseLayers[i].enabled) continue;
         float mask = this.shapeSettings.noiseLayers[i].isFirstLayerAsMask ? firstLayerValue : 1;
         elevation += this.noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
      }
      return pointOnUnitSphere * this.shapeSettings.planetRadius * (1 + elevation);
   }

}
