using System.Collections.Generic;

namespace OpenMagic.AspNet.WebApi.Specifications.Helpers
{
    public class Given
    {
        public Given()
        {
            ModelErrors = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ModelErrors { get; set; }
    }
}