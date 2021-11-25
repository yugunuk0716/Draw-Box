using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltAnim : MonoBehaviour
{


    public  float beltSpeed = 120;


    private Animator anim; 

  

    void Start()
    {
        anim = GetComponentInChildren<Animator>();// 애니메이터 받아오기
        SetBeltSpeed(beltSpeed);
    }

    void Update()
    {
        
    }

    private void SetBeltSpeed(float beltSpeed) //애니메이션 속도 조정
    {
        anim.speed = beltSpeed;
    }
}
