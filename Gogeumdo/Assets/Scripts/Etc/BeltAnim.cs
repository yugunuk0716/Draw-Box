using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltAnim : MonoBehaviour
{


    public  float beltSpeed = 120;


    private Animator anim; 

  

    void Start()
    {
        anim = GetComponentInChildren<Animator>();// �ִϸ����� �޾ƿ���
        SetBeltSpeed(beltSpeed);
    }

    void Update()
    {
        
    }

    private void SetBeltSpeed(float beltSpeed) //�ִϸ��̼� �ӵ� ����
    {
        anim.speed = beltSpeed;
    }
}
