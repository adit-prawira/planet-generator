using System;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    
    
    [Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;
        public NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrength;

        [Range(0,1)]
        public float blendAmount;
        
        [Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            
            [Range(0,1)]
            public float tintPercent;
            
            [Range(0,1)]
            public float startHeight;
            
        }
    }
}
