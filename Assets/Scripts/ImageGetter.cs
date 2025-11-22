using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class ImageGetter : MonoBehaviour
{
    private const string testWebImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Cat_August_2010-4.jpg/2560px-Cat_August_2010-4.jpg";

    public string image = testWebImage;

    private static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();

    void Start()
    {
        Action<Texture2D> action = (Texture2D texture) => {
            cache[image] = texture;
            setTexture(texture);
        };
        StartCoroutine(GetWebImage(image, action));
    }
    private void setTexture(Texture2D texture)
    {
        Debug.Log("Setting texture");
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
    }

    public IEnumerator GetWebImage(string webImage, Action<Texture2D> callback)
    {
        if (cache.ContainsKey(webImage))
        {
            callback(cache.GetValueOrDefault(webImage));
        }
        else
        {
            Debug.Log("Starting");
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
            yield return request.SendWebRequest();
            callback(DownloadHandlerTexture.GetContent(request));
        }
    }

    void Update()
    {

    }
}
