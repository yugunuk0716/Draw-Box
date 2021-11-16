using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;

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

public class Box : MonoBehaviour, IResettable
{

    public Line line = Line.a; //�� ������ ��ǥ ��
    public virtual event EventHandler Death; // ���ڰ� ��ǥ�� �������� �� ����Ǵ� �̺�Ʈ

    public float moveTime = 0.01f; 
    //�ڽ� �ӵ� : ��Ʈ �ӵ� = 1/100 : 20
    public bool isCollisionBox = false;
    public bool isCollisionBelt = false;

    public WaitForSeconds moveWS;
    private BoxCollider2D col;
    public SpriteRenderer spriteRenderer;


    public int lineIdx = 2; //���� ��

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        moveWS = new WaitForSeconds(moveTime);// �ڷ�ƾ ���ð� ����


        
    }

    public virtual void OnEnable() //Ǯ���� �ؼ� �ٽ� ������ �� �����ؾ� �� �͵� �߰�
    {
        //InitBox();
        StartCoroutine(BoxMove());
        lineIdx = UnityEngine.Random.Range(0,5); //0 ~ 4��° ����
    }

    public virtual void InitBox() // �ڽ��� ���� �������� ����
    {
        int idx = 0;
        //int idx = UnityEngine.Random.Range(0, 5); //0 ~ 4��° ����
        if (GameManager.instance.boxIdxQueue.Count > 0)
        {
            idx = GameManager.instance.boxIdxQueue.Dequeue();
        }
        else
        {
            idx = UnityEngine.Random.Range(0, 5);
            //��ũ����϶� idx�� �������� ����� ��
        }
        lineIdx = UnityEngine.Random.Range(0, 5);
        line = (Line)idx;
        spriteRenderer.sprite = GameManager.instance.lineSpriteDic[line];
        //print(idx);
    }

    public void SetMoveSpeed(float speed)
    {
        moveTime = speed;
        moveWS = new WaitForSeconds(moveTime);
    }


    public virtual IEnumerator BoxMove() // �ڽ��� ������ �̵��ϰ� �ϴ� �ڷ�ƾ
    {
        while (true)
        {
            // transform.position += new Vector3(0, 0.01f, 0);
            Vector2 dest = new Vector2(0, moveTime);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, transform.position.y + moveTime);
            if (((hit.collider == null || hit.collider == col || !hit.collider.CompareTag("Player") ||this.gameObject.CompareTag("Obstacle")) && !isCollisionBelt && !isCollisionBox) && !TutorialManager.instance.IsTuto()) 
                //�浹ü�� ���ų� �浹ü�� �ٸ� �ڽ��� �ƴ� ��� �Ǵ� �� �Լ��� ���� ������Ʈ�� �±װ� ��ֹ��̶�� �̵��� �� �ִ� ����̰� �ٸ� ��ü�� ����ִ� ���¶�� �̵� �Ұ�
            {
                dest = new Vector2(0, moveTime); //���� ��ġ�� �޾ƿ´�

                gameObject.transform.position += (Vector3)dest;// ���� �̵�
                  
            }



            yield return moveWS;
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D col) 
    {

       
        if (col.gameObject.CompareTag("ConveyorBelt"))// �����̾� ��Ʈ �������� ����� ��
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//�����̾� ��Ʈ���� ��ũ��Ʈ�� �޾ƿ´�
            if (obj != null)//�� üũ
            {
                if (obj.lineIndex == (int)line || GameManager.instance.isFever) //�ڽ��� ���ΰ� ��ǥ ������ �����ϰų� �ǹ��� Ȱ��ȭ���̶��
                {
                    if (this.gameObject.CompareTag("Player"))
                    {
                        EffectManager.instance.SetCamShake(0.5f);
                        EffectManager.instance.BoxDieEffect(true, this.gameObject.transform.position);
                        SoundManager.instance.PlaySfxSound(SoundManager.instance.endLineSfx, 0.13f);
                    }
                    Death(this, null); // Death �̺�Ʈ�� ����
                }
                else
                {
                    isCollisionBelt = true;
                }
            }
        }

        else if (col.gameObject.CompareTag("Player")) //�ٸ� ���ڿ� ����� ��
        {
            isCollisionBox = true;
        }
       

    }
    protected virtual void OnCollisionExit2D(Collision2D col) 
    {
        if (col.gameObject.CompareTag("ConveyorBelt"))// �����̾� ��Ʈ ���������� �������� ��
        {
            isCollisionBelt = false;
        }
        else if (col.gameObject.CompareTag("Player"))// �ٸ� ���ڿ��� �������� ��
        {
            isCollisionBox = false;
        }
    }
   
    public void DeathEvent() //��ֹ��� ������� �ʿ��� �����̺�Ʈ
    {
        PoolManager.instance.AddBoxCount(false);
        GameManager.instance.boxCount--;
        if (this.gameObject.CompareTag("Player")) 
        {
            EffectManager.instance.SetCamShake(0.5f, 4f);
            EffectManager.instance.BoxDieEffect(false, this.gameObject.transform.position);
            SoundManager.instance.PlaySfxSound(SoundManager.instance.deathBoxSfx,0.13f);
        }
        Death(this, null);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(new Vector2(BoxManager.instance.lineTrm[lineIdx].position.x, PoolManager.instance.spawnPoint.position.y), transform.lossyScale);
    //}


    public void Reset()//Death �̺�Ʈ �ߵ��� ����Ǵ� �Լ�
    {
        
        gameObject.SetActive(false);
    }








}

