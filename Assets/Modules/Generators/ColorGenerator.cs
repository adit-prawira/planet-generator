using UnityEngine;

public class ColorGenerator
{
   private ColorSettings _settings;
   private Texture2D _texture;
   private const int TextureResolution = 50;
   private const string VectorName = "_elevationMinMax";
   private const string TextureName = "_texture";
   private INoiseFilter _biomeNoiseFilter;
   
   public void UpdateSettings(ColorSettings settings)
   {
      this._settings = settings;
      int numberOfBiomes = this._settings.biomeColorSettings.biomes.Length;
      if(!this._texture || this._texture.height != numberOfBiomes) 
         this._texture = new Texture2D(TextureResolution*2, this._settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
      this._biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(this._settings.biomeColorSettings.noiseSettings);
   }

   public void UpdateElevation(MinMax elevationMinMax)
   {
      this._settings.planetMaterial.SetVector(VectorName, new Vector4(elevationMinMax.Min, elevationMinMax.Max));
   }

   public float BiomePercentFromPoint(Vector3 pointOfUnitSphere)
   {
      float heightPercent = (pointOfUnitSphere.y + 1) / 2f;
      heightPercent += (this._biomeNoiseFilter.Evaluate(pointOfUnitSphere) - this._settings.biomeColorSettings.noiseOffset) 
                       * this._settings.biomeColorSettings.noiseStrength;
      float biomeIndex = 0;
      int numberOfBiomes = this._settings.biomeColorSettings.biomes.Length;
      float blendRange = this._settings.biomeColorSettings.blendAmount / 2f + .001f;
      
      for (int i = 0; i < numberOfBiomes; i++)
      {
         float distance = heightPercent - this._settings.biomeColorSettings.biomes[i].startHeight;
         float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);

         // avoid biome index from getting to large
         biomeIndex *= (1 - weight);
         biomeIndex += i * weight;
      }
      
      // ensure value is between [-1, 1]
      return biomeIndex/Mathf.Max(1, (numberOfBiomes - 1));
   }

   public void UpdateColors()
   {
      Color[] colors = new Color[this._texture.width * this._texture.height];

      int colorIndex = 0;
      
      foreach (var biome in this._settings.biomeColorSettings.biomes){
         for (int i = 0; i < TextureResolution * 2; i++)
         {
            // if i < texture resolution sample from ocean gradient otherwise from biome gradient
            Color gradientColor = i < TextureResolution 
               ? this._settings.oceanColor.Evaluate(i / (TextureResolution - 1f))
               : biome.gradient.Evaluate((i - TextureResolution) / (TextureResolution - 1f));
            Color tintColor = biome.tint;
            colors[colorIndex] = gradientColor * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
            colorIndex++;
         }
      }
      
      this._texture.SetPixels(colors);
      this._texture.Apply();
      this._settings.planetMaterial.SetTexture(TextureName, this._texture);
   }
}