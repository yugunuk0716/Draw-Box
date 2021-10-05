using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltManager : MonoBehaviour
{
    public static BeltManager instance;


    private float beltSpeed;
    private bool isWaveUp = false;
    private int boxCount = 0;
    private Animator[] anim;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("다수의 BeltManager가 실행중");
        }
        instance = this;
    }

    void Start()
    {
        anim = GetComponentsInChildren<Animator>();
        SetBeltSpeed(beltSpeed);
    }

    void Update()
    {
        
    }

    private void SetBeltSpeed(float beltSpeed) 
    {
        for (int i = 0; i < anim.Length - 1; i++)
        {
            anim[i + 1].speed = beltSpeed;
        }
    }
}
