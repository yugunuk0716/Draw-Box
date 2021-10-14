using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    public Transform touchPos;
    public Transform[] lineTrm;

    

    private List<Vector3> mouseTrms = new List<Vector3>();

    private Box box;

    void Start()
    {

    }

    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            mouseTrms.Add(Input.mousePosition);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);
            Collider2D coll = Physics2D.OverlapPoint(clickPos);

            if (coll != null&&coll.gameObject.CompareTag("Player")) 
            {
                print("상자 찾음");
                box = coll.gameObject.GetComponent<Box>();
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            mouseTrms.Add(Input.mousePosition);
            print("마우스 입력 끝");
            if (box != null)
            {
                if (Mathf.Abs(mouseTrms[0].x - mouseTrms[mouseTrms.Count - 1].x) > Mathf.Abs(mouseTrms[0].y - mouseTrms[mouseTrms.Count - 1].y))
                {
                    if (mouseTrms[0].x > mouseTrms[mouseTrms.Count - 1].x)
                    {

                        SetLineIndex(false);
                        Move(Vector2.left);




                    }
                    else if (mouseTrms[0].x < mouseTrms[mouseTrms.Count - 1].x)
                    {

                        SetLineIndex(true);
                        Move(Vector2.right);

                    }
                }

                else
                {
                    if (mouseTrms[0].y > mouseTrms[mouseTrms.Count - 1].y)
                    {

                        Move(Vector2.down);



                    }
                }
            }

        }

#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchPos.position = touch.position;
                    break;
                case TouchPhase.Ended:
                    if (Input.touchCount > 5)
                    {
                        if (Input.GetTouch(touch.tapCount - 1).position.x > touchPos.position.x)
                        {
                            Move(Vector2.right);
                        }
                        if (Input.GetTouch(touch.tapCount - 1).position.x < touchPos.position.x)
                        {
                            Move(Vector2.left);
                        }
                        if (Input.GetTouch(touch.tapCount - 1).position.y > touchPos.position.y)
                        {
                            Move(Vector2.up);
                        }
                        if (Input.GetTouch(touch.tapCount - 1).position.y > touchPos.position.y)
                        {
                            Move(Vector2.down);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
#endif
    }

    public void SetLineIndex(bool on)
    {
        box.lineIdx += on ? 1 : -1;
        if(box.lineIdx > lineTrm.Length - 1)
        {
            box.lineIdx = 0;
        }
        else if(box.lineIdx < 0)
        {
            box.lineIdx = lineTrm.Length - 1;
        }
    }

    public void Move(Vector2 vec)
    {

        if (box != null) 
        {

            Vector2 dest = new Vector2(lineTrm[box.lineIdx].localPosition.x, box.transform.position.y + vec.y * 0.53f);
            print(dest.x);
            //Vector2 dest = box.gameObject.transform.position + new Vector3(vec.x * 0.53f, vec.y * 0.53f);

            RaycastHit2D hit = Physics2D.BoxCast(dest, box.gameObject.transform.lossyScale * 0.2f, 0, new Vector2(0, 0));

            if (hit.collider == null || !hit.collider.CompareTag("Player")) 
            {
                box.gameObject.transform.position = dest;
                mouseTrms.Clear();
                box = null;
            }

          

            
        }
        else
        {
            print("Box가 비어있습니다");
        }
    }



}
