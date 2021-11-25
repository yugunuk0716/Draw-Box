using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public Transform touchPos; // 터치했을 때의 위치를 저장할 변수
    public Transform[] lineTrm;// 라인 별 위치를 저장하는 배열
    public Image fillImg;

    private float feverTime = 5f;
    private WaitForSeconds feverWs;

    public bool canRightMove = true; // 지금 이동해야할 방향이 오른쪽인지 아닌지를 체크하는 변수

    private List<Vector3> mouseTrms = new List<Vector3>();// 클릭시 마우스의 위치를 계속해서 저장하는 List

    private Box box; // 박스를 받아올 변수

    private Camera cam;

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

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {

        if (!EventSystem.current.IsPointerOverGameObject()) // 마우스 포인터가 UI 위에 있지 않고
        {
            if (Input.GetMouseButtonDown(0)) // 좌클릭을 했을 때
            {
                mouseTrms.Add(Input.mousePosition); // mouserTrm 리스트에 지금 마우스 포지션을 넣는다
                Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition); // 마우스 포지션을 월드 좌표로 바꾸고
                Collider2D coll = Physics2D.OverlapPoint(worldPos); // 마우스 위치에서 충돌 확인

                if (coll != null && coll.gameObject.CompareTag("Player")) // 만약 그 위치에서 box와 충돌하였다면
                {
                    box = coll.gameObject.GetComponent<Box>(); 
                }
            }

            else if (Input.GetMouseButtonUp(0)) // 좌클릭을 땠을 때
            {
                mouseTrms.Add(Input.mousePosition); // mouseTrm 리스트에 지금 마우스 포지션을 넣는다
                if (box != null)// 상자가 널이 아니면
                {
                    if (Mathf.Abs(mouseTrms[0].x - mouseTrms[mouseTrms.Count - 1].x) > Mathf.Abs(mouseTrms[0].y - mouseTrms[mouseTrms.Count - 1].y)) // x의 변위의 절댓값이 y의 변위의 절댓값을 보다 크다면 
                    {
                        if (mouseTrms[0].x > mouseTrms[mouseTrms.Count - 1].x) //첫 위치와 마지막 위치를 비교하여 첫 위치가 크다면
                        {
                            //SetLineIndex(false);
                            Move(Vector2.left);
                        }
                        else if (mouseTrms[0].x < mouseTrms[mouseTrms.Count - 1].x)// 첫위치와 마지막 위치를 비교하여 마지막 위치가 크다면
                        {
                            //SetLineIndex(true);
                            Move(Vector2.right);
                        }
                    }

                    else
                    {
                        if (mouseTrms[0].y > mouseTrms[mouseTrms.Count - 1].y)//첫 위치와 마지막 위치를 비교하여 첫 위치가 크다면
                        {
                            Move(Vector2.down);
                        }
                        else if (mouseTrms[0].y < mouseTrms[mouseTrms.Count - 1].y)// 첫위치와 마지막 위치를 비교하여 마지막 위치가 크다면
                        {
                            if (!box.isCollisionBox && !box.isCollisionBelt) // box의 상태가 다른 박스와 충돌하지 않았고 벨트랑 출돌하지 않았다면
                            {
                                Move(Vector2.up);
                            }
                        }
                    }
                }

            }
        }


    }

    public IEnumerator Fever() // 피버 상태 코루틴
    {
        if (GameManager.instance.isFever) yield break;

        fillImg.fillAmount = 1f;
        GameManager.instance.isFever = true; //피버 시작
        float time = 5f;
        float t = 0f;

        while (true) // 피버 남은 시간 표시
        {
            yield return null;
            t += Time.deltaTime;
            if (t >= time) break;
            fillImg.fillAmount = Mathf.Lerp(1f, 0f, t / time);
        }

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
        if (TutorialManager.instance.IsTuto()) return;
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
