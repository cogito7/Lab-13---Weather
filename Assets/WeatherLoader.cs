using System;
using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class WeatherLoader : MonoBehaviour
{
    public string apiKey = "";
    public Material sunny;
    public Material rainy;
    WeatherManager weatherManager;
    bool isRaining = false;

    // Start is called before the first frame update
    void Start()
    {
        weatherManager = new WeatherManager(apiKey);
        Action<string> weatherUpdateAction = (string xml) => parseWeather(xml);
        StartCoroutine(weatherManager.GetWeatherXML(weatherUpdateAction));
    }

    void parseWeather(string xml)
    {
        Debug.Log(xml);
        XDocument doc = XDocument.Parse(xml);
        foreach (var precipitation in doc.Descendants("precipitation"))
        {
            var precipitationMode = precipitation.Attribute("mode");
            if (precipitationMode != null && precipitationMode.Value == "yes")
            {
                isRaining = true;
            }
            else
            {
                isRaining = false;
            }
        }
        if (isRaining)
        {
            RenderSettings.skybox = rainy;
        } else
        {
            RenderSettings.skybox = sunny;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
