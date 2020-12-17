using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    class FromDistribution : Attribute
    {
        public Type DistributionType { get; set; }
        public double[] Params { get; set; }

        public FromDistribution(Type distributionType, params double[] pDoubles)
        {
            DistributionType = distributionType;
            Params = pDoubles;
        }
    }

    class Generator<T> where T : new()
    {
        private List<(PropertyInfo, IContinuousDistribution)> distributionsWithProperies;
        
        public Generator()
        {
            distributionsWithProperies = GetDistributionsWithProperties();
        }

        static List<(PropertyInfo, IContinuousDistribution)> GetDistributionsWithProperties()
        {
            var propsWithDistributionAttr = typeof(T)
                .GetProperties()
                .Where(prop => prop.GetCustomAttributes<FromDistribution>().Any());
            var result = new List<(PropertyInfo, IContinuousDistribution)>();
            
            foreach (var property in propsWithDistributionAttr)
            {
                var val = property
                    .GetCustomAttributes<FromDistribution>()
                    .First();
                IContinuousDistribution distribution;
                
                if (val.DistributionType == typeof(NormalDistribution))
                {
                    if (val.Params.Length > 2 || val.Params.Length == 1)
                    {
                        throw new ArgumentException(val.DistributionType.Name);
                    }
                    
                    distribution = val.Params.Length == 0 
                        ? new NormalDistribution()
                        : new NormalDistribution(val.Params[0], val.Params[1]);
                } else if (val.DistributionType == typeof(ExponentialDistribution))
                {
                    if (val.Params.Length != 1)
                    {
                        throw new ArgumentException(val.DistributionType.Name);
                    }
                    
                    distribution = new ExponentialDistribution(val.Params[0]);
                }
                else
                {
                    throw new ArgumentException(val.DistributionType.Name);
                }
                
                result.Add((property, distribution));
            }

            return result;
        }
        
        public T Generate(Random rnd)
        {
            var obj = new T();

            foreach (var (property, distribution) in distributionsWithProperies)
            {
                property.SetValue(obj, distribution.Generate(rnd));
            }

            return obj;
        }
    }
}
