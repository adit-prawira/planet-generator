using System;
using UnityEngine;

[Serializable]
public class NoiseSettings
{
    public enum FilterType {Simple, Rigid}
    public FilterType filterType;
    
    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;
    
    [Serializable]
    public class SimpleNoiseSettings {
        
        public float strength = 1;
        public Vector3 centre;
    
        [Range(1, 8)] public int numberOfLayers = 1;
        public float persistence = 0.5f;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float minimumValue;
    }

    [Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }
}
