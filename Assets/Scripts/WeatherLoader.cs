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
    public Material night;
    public Material rainy;
    public Material snowy;
    public Light sun;
    WeatherManager weatherManager;
    bool isRaining = false;
    DateTime sunrise;
    DateTime sunset;

    // Start is called before the first frame update
    void Start()
    {
        weatherManager = new WeatherManager(apiKey);
        UpdateWeather(location);
    }

    public void UpdateWeather(string new_location)
    {
        location = new_location;
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

        //Check for sunrise/sunset
        foreach (var sun in doc.Descendants("sun"))
        {
            var rise = sun.Attribute("rise");
            if (rise != null)
            {
                sunrise = DateTime.Parse(rise.Value);
            }
            var set = sun.Attribute("set");
            if (set != null)
            {
                sunset = DateTime.Parse(set.Value);
            }
        }

        // Change skybox for weather
        if (mode == "no")
        {
            // If weather is clear, check day/night
            if (DateTime.Now < sunrise || DateTime.Now > sunset)
            {
                RenderSettings.skybox = night;
            } else
            {
                RenderSettings.skybox = sunny;
            }
        }
        if (mode == "rain")
        {
            RenderSettings.skybox = rainy;
        }
        if (mode == "snow")
        {
            RenderSettings.skybox = snowy;
        }

        //Change sun intensity
        if (DateTime.Now < sunrise || DateTime.Now > sunset)
        {
            sun.intensity = 0.25f;
        }
        else
        {
            sun.intensity = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
