using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public int boxCount = 0;
    public bool isGameover = false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 게임매니저가 실행중");
        }
        instance = this;
    }
}
