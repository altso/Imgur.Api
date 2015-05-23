using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Imgur.Api.v3.Http
{
    internal class UnderscoreMappingResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return Regex.Replace(propertyName, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4").ToLowerInvariant();
        }
    }
}