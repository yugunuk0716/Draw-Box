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
    public event EventHandler nAnswer;
    public float moveTime = 0.01f; 
    //�ڽ� �ӵ� : ��Ʈ �ӵ� = 1/100 : 20
    public WaitForSeconds moveWS;

    private BoxCollider2D col;


    public int lineIdx = 4; //���� ��


    public void Start()
    {
        moveWS = new WaitForSeconds(moveTime);// �ڷ�ƾ ���ð� ����

        col = GetComponent<BoxCollider2D>();
    }

    


    public void OnEnable() //Ǯ���� �ؼ� �ٽ� ������ �� �����ؾ� �� �͵� �߰�
    {
        SetLine();
        StartCoroutine(BoxMove());
        lineIdx = UnityEngine.Random.Range(0,9);
    }

    protected void SetLine() // �ڽ��� ���� �������� ����
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
            // transform.position += new Vector3(0, 0.01f, 0);
            Vector2 dest = new Vector2(0, 0.315f);
            RaycastHit2D hit = Physics2D.BoxCast(dest + (Vector2)gameObject.transform.position, gameObject.transform.lossyScale * 0.2f, 0, new Vector2(0, 0));

            if (hit.collider == null || !hit.collider.CompareTag("Player") || hit.collider == col || !hit.collider.CompareTag("Obstacle")) //�浹ü�� ���ų� �浹ü�� �ٸ� �ڽ��� �ƴ� ���� �̵��� �� �ִ� �����
            {

                dest = new Vector2(0, moveTime); //���� ��ġ�� �޾ƿ´�
                print(dest + " " + moveTime);
                gameObject.transform.position += (Vector3)dest;// ���� �̵�
            }

            yield return moveWS;
        }
    }


    protected void OnCollisionEnter2D(Collision2D col) 
    {

       
        if (col.gameObject.CompareTag("ConveyorBelt"))// �����̾� ��Ʈ �������� ����� ��
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//�����̾� ��Ʈ���� ��ũ��Ʈ�� �޾ƿ´�
            if (obj != null)//�� üũ
            {
                if (obj.lineIndex == (int)line || GameManager.instance.isFever) //�ڽ��� ���ΰ� ��ǥ ������ �����ϰų� �ǹ��� Ȱ��ȭ���̶��
                {
                    Death(this, null); // Death �̺�Ʈ�� ����
                }
                else
                {
                    nAnswer(this, null);
                }
            }
        }
       

    }
   





    public void Reset()//Death �̺�Ʈ �ߵ��� ����Ǵ� �Լ�
    {
        gameObject.SetActive(false);
    }

    
}
