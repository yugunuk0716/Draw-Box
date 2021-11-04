using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private string baseUrl = "http://localhost:54000";

    private string token = "";

    

    public void SetToken(string token)
    {
        this.token = token;
        PlayerPrefs.SetString("token", token);
        print(token);
        PopupManager.instance.ShowBtn(false);
    }
    

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        token = PlayerPrefs.GetString("token", ""); //¾øÀ¸¸é null
        //print(token);
    }
    private void Start()
    {
        if (!token.Equals(""))
        {
            PopupManager.instance.ShowBtn(false);
        }
        
    }

    public void Logout()
    {
        token = null;
        PlayerPrefs.DeleteKey("token");
        PopupManager.instance.ShowBtn(true);
    }

    public void SendGetRequest(string url, string queryString, Action<string> callBack)
    {
        StartCoroutine(SendGet($"{baseUrl}/{url}{queryString}", callBack));
    }

    public void SendPostRequest(string url, string payload, Action<string> callBack)
    {
        StartCoroutine(SendPost($"{baseUrl}/{url}", payload, callBack));
    }
    IEnumerator SendGet(string url, Action<string> callBack)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);

        req.SetRequestHeader("Authorization", "Bearer " + token);
        //print(token);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            callBack(data);
        }
        else
        {
            callBack("{\"result\":false,\"msg\": \"error in communicaion\"}");
        }
    }
    IEnumerator SendPost(string url, string payload, Action<string> callBack)
    {
        UnityWebRequest req = UnityWebRequest.Post(url, payload);
        req.SetRequestHeader("Content-Type", "application/json");
        print(token);
        req.SetRequestHeader("Authorization", $"Bearer {token}");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(payload);
        req.uploadHandler = new UploadHandlerRaw(jsonToSend);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            callBack(data);
        }
        else
        {
            callBack("{\"result\":false,\"msg\": \"error in communicaion\"}");
        }
    }
}
