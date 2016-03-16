using System;
using System.Net;
using System.Text;
using Improbable.Util.IO;

namespace Improbable.Unity.EditorTools.Snapshots
{
    public class SnapshotResponse
    {
        private const int HTTP_SUCCESS_START = 2;
        private static readonly Encoding RESPONSE_ENCODING = Encoding.UTF8;

        public SnapshotResponse(string content, String status, bool success)
        {
            Content = content;
            Status = status;
            Success = success;
        }

        public String Content { get; private set; }
        public string Status { get; private set; }
        public bool Success { get; private set; }

        public static SnapshotResponse FromHttpResponse(HttpWebResponse webResponse)
        {
            String responseString = ExtractResponseAsString(webResponse);
            return new SnapshotResponse(responseString, webResponse.StatusCode.ToString(), IsSuccess(webResponse));
        }

        private static String ExtractResponseAsString(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                return StreamExtensions.StreamToString(stream, RESPONSE_ENCODING);
            }
        }

        private static bool IsSuccess(HttpWebResponse webResponse)
        {
            return (((int) webResponse.StatusCode) / 100) == HTTP_SUCCESS_START;
        }
    }
}