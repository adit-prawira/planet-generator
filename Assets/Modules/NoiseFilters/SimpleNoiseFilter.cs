using UnityEngine;

public class SimpleNoiseFilter: INoiseFilter
{
    private Noise noise = new Noise();
    private NoiseSettings.SimpleNoiseSettings _settings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this._settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = this._settings.baseRoughness;
        float amplitude = 1;
        for (int i = 0; i < this._settings.numberOfLayers; i++)
        {
            float v =this.noise.Evaluate(point*frequency + this._settings.centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= this._settings.roughness;
            amplitude *= this._settings.persistence;
        }

        noiseValue = noiseValue - this._settings.minimumValue;
        return noiseValue * this._settings.strength;
    }

}
