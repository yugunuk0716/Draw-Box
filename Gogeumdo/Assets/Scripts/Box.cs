using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;

public class Box : MonoBehaviour, IResettable
{

    [Flags]
    public enum Line // �����̾� ���� �� ���� enum
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

    public Line line = Line.a; //�� ������ ��ǥ ��
    public event EventHandler Death; // ���ڰ� ��ǥ�� �������� �� ����Ǵ� �̺�Ʈ
    private float moveTime = 0.01f; 
    //�ڽ� �ӵ� : ��Ʈ �ӵ� = 1/100 : 20
    private WaitForSeconds moveWS;

    
    public bool canMoveForward = true;

    public int lineIdx = 4; //���� ��


    private void Start()
    {
        moveWS = new WaitForSeconds(moveTime);// �ڷ�ƾ ���ð� ����
    }

    
    private void Update()
    {
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(this.gameObject.transform.position.x, -2.5f, 2.5f), Mathf.Clamp(this.gameObject.transform.position.y, -4.75f, 4.75f));
        // �ڽ��� ȭ�� ������ ������ ���ϰ� ����

    }


    private void OnEnable() //Ǯ���� �ؼ� �ٽ� ������ �� �����ؾ� �� �͵� �߰�
    {
        SetLine();
        StartCoroutine(BoxMove());
        lineIdx = 4;
    }

    public void SetLine() // �ڽ��� ���� �������� ����
    {
        int idx = UnityEngine.Random.Range(0, 9);
        line = (Line)idx;
        gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //print(idx);
    }


    public IEnumerator BoxMove() // �ڽ��� ������ �̵��ϰ� �ϴ� �ڷ�ƾ
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
        if (col.gameObject.CompareTag("ConveyorBelt"))// �����̾� ��Ʈ �������� ����� ��
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//�����̾� ��Ʈ���� ��ũ��Ʈ�� �޾ƿ´�
            if (obj != null)//�� üũ
            {
                if (obj.lineIndex == (int)line) //�ڽ��� ���ΰ� ��ǥ ������ �����ϴٸ�
                {
                    Death(this, null); // Death �̺�Ʈ�� ����
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





    public void Reset()//Death �̺�Ʈ �ߵ��� ����Ǵ� �Լ�
    {
        gameObject.SetActive(false);
    }

    
}
