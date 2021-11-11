using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public Transform touchPos; // 터치했을 때의 위치를 저장할 변수
    public Transform[] lineTrm;// 라인 별 위치를 저장하는 배열

    private float feverTime = 5f;
    private WaitForSeconds feverWs;

    public bool canRightMove = true; // 지금 이동해야할 방향이 오른쪽인지 아닌지를 체크하는 변수

    private List<Vector3> mouseTrms = new List<Vector3>();// 클릭시 마우스의 위치를 계속해서 저장하는 List

    private Box box; // 박스를 받아올 변수

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 BoxManager가 실행중");
            return;
        }
        instance = this;

        feverWs = new WaitForSeconds(feverTime);
    }

    void Update()
    {

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseTrms.Add(Input.mousePosition);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);
                Collider2D coll = Physics2D.OverlapPoint(clickPos);

                if (coll != null && coll.gameObject.CompareTag("Player"))
                {
                    box = coll.gameObject.GetComponent<Box>();
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                mouseTrms.Add(Input.mousePosition);
                if (box != null)
                {
                    if (Mathf.Abs(mouseTrms[0].x - mouseTrms[mouseTrms.Count - 1].x) > Mathf.Abs(mouseTrms[0].y - mouseTrms[mouseTrms.Count - 1].y))
                    {
                        if (mouseTrms[0].x > mouseTrms[mouseTrms.Count - 1].x)
                        {
                            //SetLineIndex(false);
                            Move(Vector2.left);
                        }
                        else if (mouseTrms[0].x < mouseTrms[mouseTrms.Count - 1].x)
                        {
                            //SetLineIndex(true);
                            Move(Vector2.right);
                        }
                    }

                    else
                    {
                        if (mouseTrms[0].y > mouseTrms[mouseTrms.Count - 1].y)
                        {
                            Move(Vector2.down);
                        }
                        else if (mouseTrms[0].y < mouseTrms[mouseTrms.Count - 1].y)
                        {
                            if (!box.isCollisionBox && !box.isCollisionBelt)
                            {
                                Move(Vector2.up);
                            }
                        }
                    }
                }

            }
        }


        #region 모바일용
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (Input.touchCount > 0)
        //    {
        //        Touch touch = Input.GetTouch(0);
        //        switch (touch.phase)
        //        {
        //            case TouchPhase.Began:
        //                touchPos.position = touch.position;
        //                break;
        //            case TouchPhase.Ended:
        //                if (Input.touchCount > 5)
        //                {
        //                    if (Input.GetTouch(touch.tapCount - 1).position.x > touchPos.position.x)
        //                    {
        //                        Move(Vector2.right);
        //                    }
        //                    if (Input.GetTouch(touch.tapCount - 1).position.x < touchPos.position.x)
        //                    {
        //                        Move(Vector2.left);
        //                    }
        //                    if (Input.GetTouch(touch.tapCount - 1).position.y > touchPos.position.y)
        //                    {
        //                        Move(Vector2.up);
        //                    }
        //                    if (Input.GetTouch(touch.tapCount - 1).position.y > touchPos.position.y)
        //                    {
        //                        Move(Vector2.down);
        //                    }
        //                }
        //                break;

        //            default:
        //                break;
        //        }
        //    }
        //}
        //#endif
        #endregion

    }

    public IEnumerator Fever()
    {
        if (GameManager.instance.isFever) yield break;
        GameManager.instance.isFever = true; //피버 시작

        yield return feverWs;

        GameManager.instance.isFever = false; //피버 종료
    }

    public void SetLineIndex(bool on) // 라인 인덱스를 변경하는 함수
    {
        box.lineIdx += on ? 1 : -1;
        if (box.lineIdx > lineTrm.Length - 1)
        {
            box.lineIdx = 0;
        }
        else if (box.lineIdx < 0)
        {
            box.lineIdx = lineTrm.Length - 1;
        }
    }
    public int GetNextLineIndex(bool canRightMove, int idx) // 이동할 곳이 갈수 있는 곳인지 확인하기 위해 테스트하는 함수
    {
        idx += canRightMove ? 1 : -1;
        if (idx > lineTrm.Length - 1)
        {
            idx = 0;
        }
        else if (idx < 0)
        {
            idx = lineTrm.Length - 1;
        }
        return idx;
    }

    public void Move(Vector2 vec) // 상자를 이동시키는 함수
    {

        if (box != null) //널 체크
        {
            if (vec.x == 1)//갈 방향이 오른쪽인지 아닌지 체크
            {
                canRightMove = true;
            }
            else if (vec.x == -1)
            {
                canRightMove = false;
            }
          

            Vector2 dest = new Vector2(lineTrm[vec.y == -1 ? box.lineIdx : GetNextLineIndex(canRightMove, box.lineIdx)].localPosition.x, box.transform.position.y + vec.y);//갈 곳에 위치를 담아놓는 변수
            //Vector2 dest = box.gameObject.transform.position + new Vector3(vec.x * 0.53f, vec.y * 0.53f);

            RaycastHit2D hit = Physics2D.BoxCast(dest, box.gameObject.transform.lossyScale * 0.2f, 0, new Vector2(0, 0)); // 위의 dest 변수를 가져와 Boxcast로 갈 위치에 충돌체를 저장하는 변수

            if ((hit.collider == null) || (!hit.collider.CompareTag("Player")) && hit.collider.CompareTag("Obstacle") ) //충돌체가 없거나 충돌체가 다른 박스가 아닐 경우는 이동할 수 있는 경우임
            {
                if (vec.y != -1 && vec.y != 1)//아래쪽으로 이동시엔 라인인덱스를 바꾸지 않기 위한 조건문
                {
                    SetLineIndex(canRightMove);
                }
              

                dest = new Vector2(lineTrm[box.lineIdx].localPosition.x, box.transform.position.y + vec.y * 0.7f); //실제 위치를 받아온다
                box.gameObject.transform.position = dest;// 실제 이동
            }
            mouseTrms.Clear();// 한번 이동 했으므로 저장해 두었던 마우스 위치를 지움
            box = null; // 한번 이동 했으므로 다른 박스를 받아오기 위해 비움
        }
        else
        {
            print("Box가 비어있습니다");
        }
    }



}
