using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;

public class Box : MonoBehaviour, IResettable
{

    [Flags]
    public enum Line
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

    public Line line = Line.a;
    public event EventHandler Death;
    private float moveTime = 0.01f;
    private WaitForSeconds moveWS;

    
    public bool canMoveFoword = true;

    public int lineIdx = 4;


    private void Start()
    {
        moveWS = new WaitForSeconds(moveTime);

    }
    private void Update()
    {
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(this.gameObject.transform.position.x, -2.5f, 2.5f), Mathf.Clamp(this.gameObject.transform.position.y, -4.75f, 4.75f));

    }
    private void OnEnable()
    {
        SetLine();
        StartCoroutine(BoxMove());
    }

    public void SetLine()
    {
        int idx = UnityEngine.Random.Range(0, 9);
        line = (Line)idx;
        gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //print(idx);
    }


    public IEnumerator BoxMove()
    {
        while (true)
        {
            if (canMoveFoword) 
            {
                this.gameObject.transform.position += new Vector3(0, 0.01f, 0);
            }
            
            yield return moveWS;
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ConveyorBelt"))
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();
            if (obj != null)
            {
                if (obj.lineIndex == (int)line) //¸ÂÀ½
                {
                    Death(this, null);
                }
            }
        }
        if (col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            canMoveFoword = false;
        }

    }
    private void OnCollisionExit2D(Collision2D col)
    {
        canMoveFoword = true;
    }



    

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    
}
