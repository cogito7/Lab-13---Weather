using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager
{
    private string apiKey;
    private const string xmlApi = "https://api.openweathermap.org/data/2.5/weather?q=Orlando,us&mode=xml&appid=";

    public WeatherManager(string apiKey)
    {
        this.apiKey = apiKey;
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        Debug.Log("Calling API");
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

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        Debug.Log("Getting Weather XML");
        return CallAPI(xmlApi + apiKey, callback);
    }

}