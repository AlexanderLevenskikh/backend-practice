using System.Linq;
using System.Reflection;

namespace MyPhotoshop
{
    public class StaticParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        private static PropertyInfo[] properties;
        private static ParameterInfo[] descriptions;
        
        
        static StaticParametersHandler()
        {
            properties = typeof(TParameters)
                .GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();
            
            descriptions = typeof(TParameters)
                .GetProperties()
                .Select(info => info.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(x => x.Length > 0)
                .Select(x => x[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }
        public ParameterInfo[] GetDescription()
        {
            return descriptions;
        }

        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();

            for (var i = 0; i < values.Length; i++)
            {
                properties[i].SetValue(parameters, values[i], new object[0]);
            };

            return parameters;
        }
    }
}