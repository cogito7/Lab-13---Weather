using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager
{
    public static readonly string[] locations = {
        "Orlando,us",
        "Seattle,us",
        "London,uk"
    };
    private string apiKey;
    private const string xmlApi = "https://api.openweathermap.org/data/2.5/weather?mode=xml";

    public WeatherManager(string apiKey)
    {
        this.apiKey = apiKey;
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            Debug.Log($"Sending web request {url}");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"network problem: {request.error}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"response error: {request.responseCode}");
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherXML(string location, Action<string> callback)
    {
        return CallAPI($"{xmlApi}&q={location}&appid={apiKey}", callback);
    }

}