using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace Improbable.Unity.EditorTools.Snapshots
{
    public class SnapshotApi
    {
        private const string SNAPSHOT_SAVE_API_PATH = "/debug/takeSnapshot";
        private const string SNAPSHOT_LOAD_API_PATH = "/debug/loadSnapshot";
        private const string DEV_API_ENTITIES_PATH = "/entities/";
        private readonly string devapiEntitiesRoute;
        private readonly string snapshotLoadRoute;
        private readonly string snapsotSaveRoute;

        public SnapshotApi(string debugApiUrl, string devApiUrl)
        {
            snapsotSaveRoute = debugApiUrl + SNAPSHOT_SAVE_API_PATH;
            snapshotLoadRoute = debugApiUrl + SNAPSHOT_LOAD_API_PATH;
            devapiEntitiesRoute = devApiUrl + DEV_API_ENTITIES_PATH;
        }

        public SnapshotResponse SaveSnapshot(String snapshotFileName)
        {
            var request = PostSnapshotForm(snapshotFileName, snapsotSaveRoute);
            return ParseSnapshotResponse(request);
        }

        public SnapshotResponse LoadSnapshot(String snapshotFileName)
        {
            DeleteAllWorldEntities();
            var request = PostSnapshotForm(snapshotFileName, snapshotLoadRoute);
            return ParseSnapshotResponse(request);
        }

        private void DeleteAllWorldEntities()
        {
            var request = (HttpWebRequest) WebRequest.Create(devapiEntitiesRoute);
            request.Method = "DELETE";
            SnapshotResponse snapshotResponse = ParseSnapshotResponse(request);
            Debug.Log("Deleting all world entities");
            Debug.Log(snapshotResponse.Content);
        }

        private HttpWebRequest PostSnapshotForm(string fileName, string requestUriString)
        {
            var fullPath = CreateFullPath(fileName);
            var request = CreateRequest(requestUriString);
            PostFile(fullPath, request);
            return request;
        }

        private static void PostFile(string fullPath, HttpWebRequest request)
        {
            var postBytes = CreateFormData(fullPath);
            request.ContentLength = postBytes.Length;
            WriteFormDataToStream(request, postBytes);
        }

        private static void WriteFormDataToStream(HttpWebRequest request, byte[] postBytes)
        {
            using (var postStream = request.GetRequestStream())
            {
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Close();
            }
        }

        private static byte[] CreateFormData(string fullPath)
        {
            string urlEncode = "filename" + "=file://" + fullPath;
            byte[] postBytes = Encoding.ASCII.GetBytes(urlEncode);
            return postBytes;
        }

        private static HttpWebRequest CreateRequest(string requestUriString)
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUriString);
            request.Method = WebRequestMethods.Http.Post;
            request.Accept = "text/plain";
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }

        private static string CreateFullPath(string fileName)
        {
            return Path.GetFullPath(fileName)
                       .Replace("\\", "/");
        }

        private static SnapshotResponse ParseSnapshotResponse(HttpWebRequest request)
        {
            try
            {
                var webResponse = request.GetResponse();
                return SnapshotResponse.FromHttpResponse((HttpWebResponse) webResponse);
            }
            catch (WebException ex)
            {
                return HandleLoadException(ex);
            }
        }

        private static SnapshotResponse HandleLoadException(WebException ex)
        {
            Debug.LogError(ex);
            using (Stream data = ex.Response.GetResponseStream())
            {
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    Debug.LogError(text);
                }
            }
            throw ex;
        }
    }
}