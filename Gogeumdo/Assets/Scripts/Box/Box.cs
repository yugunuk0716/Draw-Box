using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;

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

public class Box : MonoBehaviour, IResettable
{

    public Line line = Line.a; //이 상자의 목표 줄
    public virtual event EventHandler Death; // 상자가 목표에 도달했을 때 실행되는 이벤트

    public float moveTime = 0.01f; 
    //박스 속도 : 벨트 속도 = 1/100 : 20
    public bool isCollisionBox = false;
    public bool isCollisionBelt = false;

    public WaitForSeconds moveWS;
    private BoxCollider2D col;
    public SpriteRenderer spriteRenderer;


    public int lineIdx = 2; //현재 줄

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        moveWS = new WaitForSeconds(moveTime);// 코루틴 대기시간 설정


        
    }

    public virtual void OnEnable() //풀링을 해서 다시 켜졌을 때 실행해야 할 것들 추가
    {
        //InitBox();
        StartCoroutine(BoxMove());
        lineIdx = UnityEngine.Random.Range(0,5); //0 ~ 4번째 라인
    }

    public virtual void InitBox() // 박스의 줄을 랜덤으로 설정
    {
        int idx = 0;
        //int idx = UnityEngine.Random.Range(0, 5); //0 ~ 4번째 라인
        if (GameManager.instance.boxIdxQueue.Count > 0)
        {
            idx = GameManager.instance.boxIdxQueue.Dequeue();
        }
        else
        {
            idx = UnityEngine.Random.Range(0, 5);
            //랭크모드일때 idx를 랜덤으로 해줘야 함
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


    public virtual IEnumerator BoxMove() // 박스가 앞으로 이동하게 하는 코루틴
    {
        while (true)
        {
            // transform.position += new Vector3(0, 0.01f, 0);
            Vector2 dest = new Vector2(0, moveTime);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, transform.position.y + moveTime);
            if (((hit.collider == null || hit.collider == col || !hit.collider.CompareTag("Player") ||this.gameObject.CompareTag("Obstacle")) && !isCollisionBelt && !isCollisionBox) && !TutorialManager.instance.IsTuto()) 
                //충돌체가 없거나 충돌체가 다른 박스가 아닐 경우 또는 이 함수를 쓰는 오브젝트의 태그가 장애물이라면 이동할 수 있는 경우이고 다른 물체에 닿아있는 상태라면 이동 불가
            {
                dest = new Vector2(0, moveTime); //실제 위치를 받아온다

                gameObject.transform.position += (Vector3)dest;// 실제 이동
                  
            }



            yield return moveWS;
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D col) 
    {

       
        if (col.gameObject.CompareTag("ConveyorBelt"))// 컨베이어 벨트 도착점에 닿았을 때
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();//컨베이어 벨트라인 스크립트를 받아온다
            if (obj != null)//널 체크
            {
                if (obj.lineIndex == (int)line || GameManager.instance.isFever) //박스의 라인과 목표 라인이 동일하거나 피버가 활성화중이라면
                {
                    if (this.gameObject.CompareTag("Player"))
                    {
                        EffectManager.instance.SetCamShake(0.5f);
                        EffectManager.instance.BoxDieEffect(true, this.gameObject.transform.position);
                        SoundManager.instance.PlaySfxSound(SoundManager.instance.endLineSfx, 0.13f);
                    }
                    Death(this, null); // Death 이벤트를 실행
                }
                else
                {
                    isCollisionBelt = true;
                }
            }
        }

        else if (col.gameObject.CompareTag("Player")) //다른 상자에 닿았을 때
        {
            isCollisionBox = true;
        }
       

    }
    protected virtual void OnCollisionExit2D(Collision2D col) 
    {
        if (col.gameObject.CompareTag("ConveyorBelt"))// 컨베이어 벨트 도착점에서 떨어졌을 때
        {
            isCollisionBelt = false;
        }
        else if (col.gameObject.CompareTag("Player"))// 다른 상자에서 떨어졌을 때
        {
            isCollisionBox = false;
        }
    }
   
    public void DeathEvent() //장애물에 닿았을때 필요한 죽음이벤트
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


    public void Reset()//Death 이벤트 발동시 실행되는 함수
    {
        
        gameObject.SetActive(false);
    }








}

