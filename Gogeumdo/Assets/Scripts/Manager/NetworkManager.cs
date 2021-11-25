using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    ///url
    //private string baseUrl = "http://localhost:54000";
    private string baseUrl = "http://son.gondr.net:80"; 

    private string token = ""; //토큰

    

    public void SetToken(string token) //토큰 넣어주기
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

        token = PlayerPrefs.GetString("token", ""); //없으면 null
        //print(token);
    }
    private void Start()
    {
        if (!token.Equals("")) //토큰이 있다면
        {
            PopupManager.instance.ShowBtn(false);  //로그아웃버튼을 보여주기
        }
        SceneManager.sceneLoaded += OnSceneLoad; //씬 로드시 해줘야 할 일 추가
    }
    public bool TokenConfirm() //토큰 확인 
    {
        return (token != "" && token != null);
    }
    public void Logout() //로그아웃 버튼
    {
        token = null;
        PlayerPrefs.DeleteKey("token");
        PopupManager.instance.ShowBtn(true);
    }
    public void OnSceneLoad(Scene scene, LoadSceneMode mode) //씬 로드 시 해줘야 할 일
    {
        if (scene.buildIndex == 0) //만약 타이틀이라면
        {
            if (!token.Equals("")) //토큰이 있다면
            {
                PopupManager.instance.ShowBtn(false); //로그아웃버튼을 보여주기
            }
        }
    }
    public void SendGetRequest(string url, string queryString, Action<string> callBack)
    {
        StartCoroutine(SendGet($"{baseUrl}/{url}{queryString}", callBack));
    }

    public void SendPostRequest(string url, string payload, Action<string> callBack)
    {
        StartCoroutine(SendPost($"{baseUrl}/{url}", payload, callBack));
    }
    IEnumerator SendGet(string url, Action<string> callBack) //get방식
    {
        UnityWebRequest req = UnityWebRequest.Get(url);

        req.SetRequestHeader("Authorization", "Bearer " + token); //헤더에 토큰을 실어서 보내준다.
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
    IEnumerator SendPost(string url, string payload, Action<string> callBack) //post방식
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
