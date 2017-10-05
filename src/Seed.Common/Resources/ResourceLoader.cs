using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Common.Resources
{
    public class ResourceLoader
    {
        public async Task<string> Load<TInAssembly>(string resourceName)
        {
            var assembly = typeof(TInAssembly).GetTypeInfo().Assembly;

            var fullyQualifiedResourceName = GetFullyQualifiedResourceName(assembly, resourceName);

            var resourceStream = assembly.GetManifestResourceStream(fullyQualifiedResourceName);

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private string GetFullyQualifiedResourceName(Assembly assembly, string resourceName)
        {
            var availableResources = assembly.GetManifestResourceNames();

            var fullyQualifiedResourceName =
                availableResources.SingleOrDefault(x => x.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (fullyQualifiedResourceName != null)
                return fullyQualifiedResourceName;

            var errorMessage = $"Resource '{resourceName}' not found in assembly '{assembly.GetName().Name}'\r\n" +
                               $"Available resources:\r\n" +
                               new string('-', 40) +
                               $"\r\n{string.Join("\r\n", assembly.GetManifestResourceNames())}\r\n" +
                               new string('-', 40);

            throw new Exception(errorMessage);
        }
    }
}