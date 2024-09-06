using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Network.HttpRequest
{
    public class HttpRequest
    {
        public static void Post(string url, string jsonBody, Action<string> onSuccess, Action<string> onFailure)
        {
            UnityWebRequest request = UnityWebRequest.PostWwwForm(url,jsonBody);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest().completed += asyncOperation =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    onFailure?.Invoke(request.error);
                }
                request.Dispose();
            };
        }
    }
}