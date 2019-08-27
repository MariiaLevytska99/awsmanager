using System;
using System.Collections.Generic;
using System.Text;
using awsmanagerLib.Configuration;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace awsmanagerLib.Models
{
    public class ApiGatewayUrl
    {
        public string Url { get; set; }
        public Code Code { get; set; }


        public ApiGatewayUrl CheckUrl(string url)
        {

            Url = url;
            var validUrl = GetHeaders(url);
            Code = new Code(((int)validUrl).ToString(), validUrl.ToString());
            return this;

        }

        private HttpStatusCode GetHeaders(string url)
        {
            Uri outUri;
            HttpStatusCode result = default(HttpStatusCode);
            if (ValidHttpURL(url, out outUri) == true)
            {
                Url = outUri.AbsoluteUri;
                var request = HttpWebRequest.Create(outUri.AbsoluteUri);
                request.Method = "HEAD";
                try
                {
                    try
                    {
                        var response = request.GetResponse() as HttpWebResponse;
                        if (response != null)
                        {
                            result = response.StatusCode;
                            response.Close();
                        }


                    }
                    catch (WebException ex)
                    {
                        return HttpStatusCode.NotFound;
                    }

                   
                       
                    return result;
                }
                catch (WebException e) when (e.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    return HttpStatusCode.NotFound;
                }
            }
            else return HttpStatusCode.NotFound;
        }

        private bool ValidHttpURL(string s, out Uri resultURI)
        {
            if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
                s = "http://" + s;

            if (Uri.TryCreate(s, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }

        

    }
}
