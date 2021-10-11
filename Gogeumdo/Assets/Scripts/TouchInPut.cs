using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public Transform touchPos;

    
    private List<Vector3> mouseTrms = new List<Vector3>();

    public Box box;

    void Start()
    {
        
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) 
        {
            //print("마우스 입력");
            mouseTrms.Add(Input.mousePosition);
        }
        
        else if (Input.GetMouseButtonUp(0))
        {
            print("마우스 입력 끝");
            if (Mathf.Abs(mouseTrms[0].x - mouseTrms[mouseTrms.Count - 1].x) > Mathf.Abs(mouseTrms[0].y - mouseTrms[mouseTrms.Count - 1].y)) 
            {
                if (mouseTrms[0].x > mouseTrms[mouseTrms.Count - 1].x)
                {
                    
                    Move(Vector2.left);

                }
                else if (mouseTrms[0].x < mouseTrms[mouseTrms.Count - 1].x)
                {
                    Move(Vector2.right);

                }
            }

            else
            {
                if (mouseTrms[0].y > mouseTrms[mouseTrms.Count - 1].y)
                {
                    Move(Vector2.down);
                }
                else //if(mouseTrms[0].y < mouseTrms[mouseTrms.Count - 1].y)
                {
                    Move(Vector2.up);
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

    public void Move(Vector2 vec) 
    {
        box.gameObject.transform.position += new Vector3(vec.x, vec.y);
        mouseTrms.Clear();
        print(vec.x + "  " + vec.y);
    }
}
