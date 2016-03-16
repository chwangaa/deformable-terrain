using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.Snapshots
{
    public class SnapshotMenu : MonoBehaviour
    {
        private const string DEBUG_API_URL = "http://localhost:5005";
        private const string DEV_API_URL = "http://localhost:5000/api/v1";
        private const string CHECK_LOG_FOR_INFO = "Check log for more information";
        private const string WEB_EXCEPTION_MESSAGE_FORMAT = "Communication with server failed with status: {0}. {1}";
        private const string SERVER_RESPONDED_WITH_ERROR_FORMAT = "The service responded with the following error message: {0}";
        private const string ERROR_LOADING_SNAPSHOT = "Error loading snapshot";
        private static readonly string UNKNOWN_ERROR_MESSAGE = string.Format("An unknown error occured. {0}", CHECK_LOG_FOR_INFO);

        private static readonly SnapshotLoader SNAPSHOT_LOADER = new SnapshotLoader(DEBUG_API_URL, DEV_API_URL);

        private static String UserSnapshotName
        {
            get { return Environment.UserName; }
        }

        [MenuItem("Improbable/Take Snapshot %#&S")]
        private static void SaveSnapshot()
        {
            try
            {
                SnapshotResponse saveSnapshotResponse = SNAPSHOT_LOADER.SaveSnapshot(UserSnapshotName);
                Debug.Log("Snapshot saved response: " + saveSnapshotResponse.Content);
            }
            catch (WebException e)
            {
                HandleWebException(e);
            }
            catch (Exception e)
            {
                HandleUnknownException(e);
            }
        }

        [MenuItem("Improbable/Load Snapshot %#&R")]
        private static void LoadSnapshot()
        {
            try
            {
                SnapshotResponse response = SNAPSHOT_LOADER.LoadLocalSnapshot(UserSnapshotName);
                if (!response.Success)
                {
                    HandleSnapshotLoadFailure(response);
                }
                else
                {
                    Debug.Log("Snapshot loaded successfully");
                }
            }
            catch (DirectoryNotFoundException e)
            {
                HandleNoSnapshotFoundException(e);
            }
            catch (FileNotFoundException e)
            {
                HandleNoSnapshotFoundException(e);
            }
            catch (WebException e)
            {
                HandleWebException(e);
            }
            catch (Exception e)
            {
                HandleUnknownException(e);
            }
        }

        private static void HandleSnapshotLoadFailure(SnapshotResponse response)
        {
            var messageContent = string.Format(SERVER_RESPONDED_WITH_ERROR_FORMAT, response.Status);
            EditorUtility.DisplayDialog(ERROR_LOADING_SNAPSHOT, messageContent, "ok");
            Debug.LogError("Error loading snashot. Resoponse: " + response.Content);
        }

        private static String FormatDictionaryForLogging(Dictionary<String, String> content)
        {
            return string.Join(",", content.Select(keyValuePair => keyValuePair.Key + ":" + keyValuePair.Value).ToArray());
        }

        private static void HandleWebException(WebException webException)
        {
            Debug.LogException(webException);
            string errorMessage = string.Format(WEB_EXCEPTION_MESSAGE_FORMAT, webException.Status, CHECK_LOG_FOR_INFO);
            EditorUtility.DisplayDialog("Web request failed", errorMessage, "ok");
        }

        private static void HandleNoSnapshotFoundException(Exception e)
        {
            Debug.LogException(e);
            EditorUtility.DisplayDialog(ERROR_LOADING_SNAPSHOT, "No user snapshot found", "ok");
        }

        private static void HandleUnknownException(Exception e)
        {
            Debug.LogException(e);
            EditorUtility.DisplayDialog("An error occured", UNKNOWN_ERROR_MESSAGE, "ok");
        }
    }
}