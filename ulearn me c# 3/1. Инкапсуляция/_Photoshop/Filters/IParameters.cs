namespace MyPhotoshop
{
    public interface IParameters
    {
        ParameterInfo[] GetDescription();
        void SetValues(double[] values);
    }
}