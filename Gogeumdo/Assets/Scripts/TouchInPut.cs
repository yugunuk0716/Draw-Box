using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public Transform touchPos;

    void Start()
    {
        
    }

    void Update()
    {
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
    }

    public void Move(Vector2 vec) 
    {
        Debug.Log(vec.x + "  "  + vec.y );
    }
}
