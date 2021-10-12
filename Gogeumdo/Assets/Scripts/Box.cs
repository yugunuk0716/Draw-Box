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

    Line line = Line.a;
    public event EventHandler Death;

    private void Start()
    {
        
    }

    public void SetLine()
    {
        int idx = UnityEngine.Random.Range(0, 9);
        line = (Line)idx;
        print(idx);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("ConveyorBelt"))
        {
            ConveyorBeltLine obj = col.gameObject.GetComponent<ConveyorBeltLine>();
            if(obj != null)
            {
                if (line.HasFlag((Line)obj.lineIndex)) //¸ÂÀ½
                {
                    Death(this, null);
                }
            }
        }
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        SetLine();
    }
}
