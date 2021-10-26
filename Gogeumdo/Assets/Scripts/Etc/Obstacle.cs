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
        SetMoveSpeed(UnityEngine.Random.Range(0.01f, 0.05f)); //�ӵ� ���� ����
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ConveyorBelt"))// �����̾� ��Ʈ �������� ����� ��
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//�����̾� ��Ʈ���� ��ũ��Ʈ�� �޾ƿ´�
            if (obj != null)//�� üũ
            {
                Death(this, null); // Death �̺�Ʈ�� ����
            }
        }
        else if(col.gameObject.CompareTag("Player"))
        {
            Box box = col.gameObject.GetComponent<Box>();
            if(box != null)
            {
                //����Ʈ�� �ʿ��ϸ� ���⼭ �ؾ��ҵ�?
                box.DeathEvent();
            }
        }
    }
}
