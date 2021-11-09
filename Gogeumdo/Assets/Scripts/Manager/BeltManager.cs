using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltManager : MonoBehaviour
{
    public static BeltManager instance; //�̱���


    public  float beltSpeed = 120;

    //private int wave = 0;//���̺� ����

    private Animator anim; 

    private void Awake()
    {
        if (instance != null) //�̱��� �ߺ� üũ
        {
            Debug.LogError("�ټ��� BeltManager�� ������");
        }
        instance = this;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();// �ִϸ����� �޾ƿ���
        SetBeltSpeed(beltSpeed);
    }

    void Update()
    {
        
    }

    private void SetBeltSpeed(float beltSpeed) 
    {
        anim.speed = beltSpeed;
    }
}
