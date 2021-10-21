using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltManager : MonoBehaviour
{
    public static BeltManager instance; //싱글톤


    public  float beltSpeed = 120;

    //private int wave = 0;//웨이브 관리

    private Animator[] anim; 

    private void Awake()
    {
        if (instance != null) //싱글톤 중복 체크
        {
            Debug.LogError("다수의 BeltManager가 실행중");
        }
        instance = this;
    }

    void Start()
    {
        anim = GetComponentsInChildren<Animator>();// 애니메이터 받아오기
        SetBeltSpeed(beltSpeed);
    }

    void Update()
    {
        
    }

    private void SetBeltSpeed(float beltSpeed) 
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].speed = beltSpeed; // 애니메이션 속도 조절
        }
    }
}
