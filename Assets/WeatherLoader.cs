using System;
using System.Collections;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeatherLoader : MonoBehaviour
{
    public string apiKey = "";
    public string location = WeatherManager.locations[0];
    public Material sunny;
    public Material rainy;
    public Material snowy;
    WeatherManager weatherManager;
    bool isRaining = false;

    // Start is called before the first frame update
    void Start()
    {
        weatherManager = new WeatherManager(apiKey);
        Action<string> weatherUpdateAction = (string xml) => parseWeather(xml);
        StartCoroutine(weatherManager.GetWeatherXML(location, weatherUpdateAction));
    }

    void parseWeather(string xml)
    {
        Debug.Log(xml);
        XDocument doc = XDocument.Parse(xml);

        //Check for precipitation
        string mode = "no";
        foreach (var precipitation in doc.Descendants("precipitation"))
        {
            var precipitationMode = precipitation.Attribute("mode");
            if (precipitationMode != null)
            {
                mode = precipitationMode.Value;
            }
        }

        //Change skybox
        if (mode == "no")
        {
            RenderSettings.skybox = sunny;
        }
        if (mode == "rain")
        {
            RenderSettings.skybox = rainy;
        }
        if (mode == "snow")
        {
            RenderSettings.skybox = snowy;
        }

        //Check time
        int hour = System.DateTime.Now.Hour;
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
