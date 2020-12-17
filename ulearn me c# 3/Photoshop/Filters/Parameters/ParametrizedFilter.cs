namespace MyPhotoshop
{
    public abstract class ParametrizedFilter<TParameters> : IFilter
        where TParameters : IParameters, new()
    {
        IParametersHandler<TParameters> handler = new StaticParametersHandler<TParameters>();
        public ParameterInfo[] GetParameters()
        {
            return handler.GetDescription();
        }

        public Photo Process(Photo original, double[] values)
        {
            var parameters = handler.CreateParameters(values);
            return Process(original, parameters);
        }

        public abstract Photo Process(Photo original, TParameters parameters);
    }
}