using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace OpenMagic.AspNet.WebApi.Specifications.Helpers
{
    public static class HttpContentExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetModelErrors(this HttpContent content)
        {
            var objectContent = content as ObjectContent<HttpError>;
            var value = objectContent?.Value as HttpError;
            var errors = value?["ModelState"] as HttpError;

            if (errors == null)
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }

            return
                from error in errors
                from errorMessage in (IEnumerable<string>)error.Value
                select new KeyValuePair<string, string>(error.Key, errorMessage);
        }
    }
}