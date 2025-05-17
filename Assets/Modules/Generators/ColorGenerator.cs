using UnityEngine;

public class ColorGenerator
{
   private ColorSettings _settings;
   private Texture2D _texture;
   private const int TextureResolution = 50;
   private const string VectorName = "_elevationMinMax";
   private const string TextureName = "_texture";
   
   public void UpdateSettings(ColorSettings settings)
   {
      this._settings = settings;
      if(!this._texture) this._texture = new Texture2D(TextureResolution, 1);
   }

   public void UpdateElevation(MinMax elevationMinMax)
   {
      this._settings.planetMaterial.SetVector(VectorName, new Vector4(elevationMinMax.Min, elevationMinMax.Max));
   }

   public void UpdateColors()
   {
      Color[] colors = new Color[TextureResolution];

      for (int i = 0; i < TextureResolution; i++)
      {
         colors[i] = this._settings.gradient.Evaluate(i / (TextureResolution - 1f));
      }
      
      this._texture.SetPixels(colors);
      this._texture.Apply();
      this._settings.planetMaterial.SetTexture(TextureName, this._texture);
   }
}