using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    public PanelScript stOrInPanel; //stage�� infinite ��带 ���� ���� �г�
    public Button[] stAndInBtns; //0 = stage //1 = infinite

    private void Awake()
    {
        
    }

    private void Start()
    {
        stAndInBtns = stOrInPanel.GetComponentsInChildren<Button>(); // �гιؿ��� ��ư���� �������ش�
        stAndInBtns[0].onClick.AddListener(() =>
        {
            //�������� ������ �̵�
            GameManager.instance.isStage = true; 
            LoadManager.LoadScene("Stage");
        });
        stAndInBtns[1].onClick.AddListener(() =>
        {
            GameManager.instance.isStage = false;
            //��ũ���� �̵�
            LoadManager.LoadScene("InGame");
        });
    }

    private void Update()
    {
        //if(Input.touchCount > 0)
        //{
        //    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) //uiŬ���� true �ƴҽ� false
        //    {
        //        stOrInPanel.Open();
        //    }
        //}
        //�׽�Ʈ�� �ڵ�
        if(Input.GetMouseButtonDown(0)) //��Ŭ����
        {
            if(!EventSystem.current.IsPointerOverGameObject()) //ui�� �ƴ� �ٸ����� Ŭ��������  
            {
                stOrInPanel.Open(); //�г��� ���ֱ�
            }
        }
    }
}
