using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public Transform touchPos; // ��ġ���� ���� ��ġ�� ������ ����
    public Transform[] lineTrm;// ���� �� ��ġ�� �����ϴ� �迭

    private float feverTime = 5f;
    private WaitForSeconds feverWs;

    public bool canRightMove = true; // ���� �̵��ؾ��� ������ ���������� �ƴ����� üũ�ϴ� ����

    private List<Vector3> mouseTrms = new List<Vector3>();// Ŭ���� ���콺�� ��ġ�� ����ؼ� �����ϴ� List

    private Box box; // �ڽ��� �޾ƿ� ����

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� BoxManager�� ������");
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


        #region ����Ͽ�
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
        GameManager.instance.isFever = true; //�ǹ� ����

        yield return feverWs;

        GameManager.instance.isFever = false; //�ǹ� ����
    }

    public void SetLineIndex(bool on) // ���� �ε����� �����ϴ� �Լ�
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
    public int GetNextLineIndex(bool canRightMove, int idx) // �̵��� ���� ���� �ִ� ������ Ȯ���ϱ� ���� �׽�Ʈ�ϴ� �Լ�
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

    public void Move(Vector2 vec) // ���ڸ� �̵���Ű�� �Լ�
    {

        if (box != null) //�� üũ
        {
            if (vec.x == 1)//�� ������ ���������� �ƴ��� üũ
            {
                canRightMove = true;
            }
            else if (vec.x == -1)
            {
                canRightMove = false;
            }
          

            Vector2 dest = new Vector2(lineTrm[vec.y == -1 ? box.lineIdx : GetNextLineIndex(canRightMove, box.lineIdx)].localPosition.x, box.transform.position.y + vec.y);//�� ���� ��ġ�� ��Ƴ��� ����
            //Vector2 dest = box.gameObject.transform.position + new Vector3(vec.x * 0.53f, vec.y * 0.53f);

            RaycastHit2D hit = Physics2D.BoxCast(dest, box.gameObject.transform.lossyScale * 0.2f, 0, new Vector2(0, 0)); // ���� dest ������ ������ Boxcast�� �� ��ġ�� �浹ü�� �����ϴ� ����

            if ((hit.collider == null) || (!hit.collider.CompareTag("Player")) && hit.collider.CompareTag("Obstacle") ) //�浹ü�� ���ų� �浹ü�� �ٸ� �ڽ��� �ƴ� ���� �̵��� �� �ִ� �����
            {
                if (vec.y != -1 && vec.y != 1)//�Ʒ������� �̵��ÿ� �����ε����� �ٲ��� �ʱ� ���� ���ǹ�
                {
                    SetLineIndex(canRightMove);
                }
              

                dest = new Vector2(lineTrm[box.lineIdx].localPosition.x, box.transform.position.y + vec.y * 0.7f); //���� ��ġ�� �޾ƿ´�
                box.gameObject.transform.position = dest;// ���� �̵�
            }
            mouseTrms.Clear();// �ѹ� �̵� �����Ƿ� ������ �ξ��� ���콺 ��ġ�� ����
            box = null; // �ѹ� �̵� �����Ƿ� �ٸ� �ڽ��� �޾ƿ��� ���� ���
        }
        else
        {
            print("Box�� ����ֽ��ϴ�");
        }
    }



}
