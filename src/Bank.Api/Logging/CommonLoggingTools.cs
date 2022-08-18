using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Api.Logging
{
    public static class CommonLoggingTools
    {
        public static async Task<RequestResponseData> FormatRequestBody(HttpRequest request)
        {
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            var obj = new RequestResponseData();
            obj.Path = request.Path.HasValue ? request.Path.Value : null;
            obj.QueryString = request.QueryString.HasValue ? request.QueryString.Value : null;
            obj.RequestBody = bodyAsText;

            return obj;
        }

        public static async Task<string> FormatResponseBody(HttpResponse response)
        {
            // We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            response.Body.Position = 0;

            return text;
        }

        public static string SerializeHeaders(IHeaderDictionary headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                //if (item.Value != null)
                //{
                var header = string.Empty;
                foreach (var value in item.Value)
                {
                    header += value + " ";
                }

                // Trim the trailing space and add item to the dictionary
                header = header.TrimEnd(" ".ToCharArray());
                dict.Add(item.Key, header);
                //}
            }

            return JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
