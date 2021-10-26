using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class Obstacle : Box
{
    public override event EventHandler Death;

    public override void OnEnable()
    {
        base.OnEnable();
        //속도 랜덤 조정을 추가해야 한다.
        SetMoveSpeed(UnityEngine.Random.Range(0.01f, 0.05f)); //속도 랜덤 조정
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ConveyorBelt"))// 컨베이어 벨트 도착점에 닿았을 때
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//컨베이어 벨트라인 스크립트를 받아온다
            if (obj != null)//널 체크
            {
                Death(this, null); // Death 이벤트를 실행
            }
        }
        else if(col.gameObject.CompareTag("Player"))
        {
            Box box = col.gameObject.GetComponent<Box>();
            if(box != null)
            {
                //이펙트가 필요하면 여기서 해야할듯?
                box.DeathEvent();
            }
        }
    }
}
