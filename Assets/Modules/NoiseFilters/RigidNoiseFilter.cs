using UnityEngine;

public class RigidNoiseFilter :INoiseFilter
{
    private Noise noise = new Noise();
    private NoiseSettings.RigidNoiseSettings _settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this._settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = this._settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;
        
        for (int i = 0; i < this._settings.numberOfLayers; i++)
        {
            float v = 1 - Mathf.Abs(this.noise.Evaluate(point * frequency + this._settings.centre));
            v *= v;
            v *= weight;
            
            // ensure that weight value to be within 0 <= weight <= 1
            weight = Mathf.Clamp01(v * this._settings.weightMultiplier);
            noiseValue += v * 0.5f * amplitude;
            frequency *= this._settings.roughness;
            amplitude *= this._settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - this._settings.minimumValue);
        return noiseValue * this._settings.strength;
    }
}
