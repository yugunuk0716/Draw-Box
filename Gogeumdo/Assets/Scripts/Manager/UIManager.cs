using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("�ټ��� UIManager�� �������Դϴ�");
        }
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
