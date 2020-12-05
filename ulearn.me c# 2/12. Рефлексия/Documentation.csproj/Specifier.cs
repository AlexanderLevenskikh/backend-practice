using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            var type = typeof(T);
            return type.GetCustomAttributes<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
        }

        private IEnumerable<MethodInfo> GetApiMethodsInfo()
        {
            var type = typeof(T);

            return type.GetMethods()
                .Where(info => info.GetCustomAttributes<ApiMethodAttribute>().Any());
        }

        private MethodInfo GetApiMethodInfo(string methodName)
        {
            return GetApiMethodsInfo()
                .FirstOrDefault(info => info.Name == methodName);
        }

        private ParameterInfo GetApiMethodParameterInfo(string methodName, string paramName)
        {
            return GetApiMethodsInfo()
                .FirstOrDefault(info => info.Name == methodName)
                ?.GetParameters()
                .FirstOrDefault(info => info.Name == paramName);
        }

        public string[] GetApiMethodNames()
        {
            return GetApiMethodsInfo().Select(info => info.Name).ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            return GetApiMethodInfo(methodName)
                ?.GetCustomAttributes<ApiDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            return GetApiMethodInfo(methodName)
                .GetParameters()
                .Select(info => info.Name)
                .ToArray();
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            return GetApiMethodParameterInfo(methodName, paramName)
                ?.GetCustomAttributes<ApiDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var paramInfo = GetApiMethodParameterInfo(methodName, paramName);
            var description = new ApiParamDescription
            {
                ParamDescription = new CommonDescription
                {
                    Name = paramInfo == null ? paramName : paramInfo.Name
                }
            };

            if (paramInfo == null)
            {
                return description;
            }

            var attrs = paramInfo.GetCustomAttributes();

            foreach (var attr in attrs)
            {
                switch (attr)
                {
                    case ApiIntValidationAttribute attribute:
                        description.MinValue = attribute.MinValue;
                        description.MaxValue = attribute.MaxValue;
                        break;
                    case ApiRequiredAttribute attribute:
                        description.Required = attribute.Required;
                        break;
                    case ApiDescriptionAttribute attribute:
                        description.ParamDescription.Description = attribute.Description;
                        break;
                }
            }

            return description;
        }

        private ApiParamDescription GetApiMethodReturnParameter(string methodName)
        {
            var returnParameter = typeof(T)
                .GetMethod(methodName)?
                .ReturnParameter;

            if (returnParameter?.Name == null)
            {
                return null;
            }

            var atrIntValidation = returnParameter
                .GetCustomAttribute(typeof(ApiIntValidationAttribute)) as ApiIntValidationAttribute;
            var atrRequired = returnParameter
                .GetCustomAttribute(typeof(ApiRequiredAttribute)) as ApiRequiredAttribute;
            var parameterDescription = new ApiParamDescription
            {
                MaxValue = atrIntValidation?.MaxValue,
                MinValue = atrIntValidation?.MinValue,
                ParamDescription = new CommonDescription
                {
                    Description = GetApiMethodParamDescription(methodName, returnParameter.Name),
                    Name = returnParameter.Name
                }
            };
            if (atrRequired != null)
                parameterDescription.Required = atrRequired.Required;
            
            parameterDescription.ParamDescription.Name = parameterDescription.ParamDescription.Name == ""
                ? null
                : parameterDescription.ParamDescription.Name;
            
            return parameterDescription;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            return typeof(T)
                .GetMethod(methodName)?
                .GetCustomAttribute(typeof(ApiMethodAttribute)) == null
                ? null
                : new ApiMethodDescription
                {
                    ParamDescriptions = GetApiMethodParamNames(methodName)
                        .Select(x => GetApiMethodParamFullDescription(methodName, x))
                        .ToArray(),
                    MethodDescription = new CommonDescription
                    {
                        Description = GetApiMethodDescription(methodName),
                        Name = methodName
                    },
                    ReturnDescription = GetApiMethodReturnParameter(methodName)
                };
        }
    }
}