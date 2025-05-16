public class MinMax
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMax()
    {
        this.Min = float.MaxValue;
        this.Max = float.MinValue;
    }

    public void AddValue(float value)
    {
        if(value > this.Max) this.Max = value;
        if(value < this.Min) this.Min = value;
    }
}