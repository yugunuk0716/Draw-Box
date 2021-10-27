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
        //�ӵ� ���� ������ �߰��ؾ� �Ѵ�.
        SetMoveSpeed(UnityEngine.Random.Range(0.03f, 0.07f)); //�ӵ� ���� ����
    }

    public override void InitBox()
    {
        int idx = UnityEngine.Random.Range(0, 5);
        line = (Line)idx;
        gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.lineColorDic[line];
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("ConveyorBelt"))// �����̾� ��Ʈ �������� ����� ��
        {
            ConveyorBeltLine obj = col.GetComponent<ConveyorBeltLine>();//�����̾� ��Ʈ���� ��ũ��Ʈ�� �޾ƿ´�
            if (obj != null)//�� üũ
            {
                Death(this, null); // Death �̺�Ʈ�� ����
            }
        }
        else if (col.CompareTag("Player"))
        {
            Box box = col.GetComponent<Box>();
            if (box != null)
            {
                //����Ʈ�� �ʿ��ϸ� ���⼭ �ؾ��ҵ�?
                box.DeathEvent();
            }
        }
    }
}
