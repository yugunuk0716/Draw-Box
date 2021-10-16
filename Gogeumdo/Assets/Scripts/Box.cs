using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;

public class Box : MonoBehaviour, IResettable
{

    [Flags]
    public enum Line // 컨베이어 라인 줄 관리 enum
    {
        a = 0 << 0, //0
        b = 1 << 0, //1
        c = 1 << 1, //2
        d = b | c, //3
        e = 1 << 2, //4
        f = b | e, //5
        g = c | e, //6 
        h = b | c | e, //7
        i = 1 << 3, //8
        j = b | i //9
    }

    public Line line = Line.a; //이 상자의 목표 줄
    public event EventHandler Death; // 상자가 목표에 도달했을 때 실행되는 이벤트
    private float moveTime = 0.01f; 
    //박스 속도 : 벨트 속도 = 1/100 : 20
    private WaitForSeconds moveWS;

    
    public bool canMoveForward = true;

    public int lineIdx = 4; //현재 줄


    private void Start()
    {
        moveWS = new WaitForSeconds(moveTime);// 코루틴 대기시간 설정
    }

    
    private void Update()
    {
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(this.gameObject.transform.position.x, -2.5f, 2.5f), Mathf.Clamp(this.gameObject.transform.position.y, -4.75f, 4.75f));
        // 박스가 화면 밖으로 나가지 못하게 막기

    }


    private void OnEnable() //풀링을 해서 다시 켜졌을 때 실행해야 할 것들 추가
    {
        SetLine();
        StartCoroutine(BoxMove());
        lineIdx = 4;
    }

    public void SetLine() // 박스의 줄을 랜덤으로 설정
    {
        int idx = UnityEngine.Random.Range(0, 9);
        line = (Line)idx;
        gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //print(idx);
    }


    public IEnumerator BoxMove() // 박스가 앞으로 이동하게 하는 코루틴
    {
        while (true)
        {
            if (canMoveForward) 
            {
                this.gameObject.transform.position += new Vector3(0, 0.01f, 0);
            }
            
            yield return moveWS;
        }
    }


    private void OnCollisionEnter2D(Collision2D col) 
    {

        if (col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            canMoveForward = false;
        }
        if (col.gameObject.CompareTag("ConveyorBelt"))// 컨베이어 벨트 도착점에 닿았을 때
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//컨베이어 벨트라인 스크립트를 받아온다
            if (obj != null)//널 체크
            {
                if (obj.lineIndex == (int)line) //박스의 라인과 목표 라인이 동일하다면
                {
                    Death(this, null); // Death 이벤트를 실행
                }
            }
        }
       

    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            canMoveForward = true;
        }
    }





    public void Reset()//Death 이벤트 발동시 실행되는 함수
    {
        gameObject.SetActive(false);
    }

    
}
