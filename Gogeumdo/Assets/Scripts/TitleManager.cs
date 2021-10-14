using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public PanelScript stOrInPanel;
    public Button[] stAndInBtns; //0 = stage //1 = infinite

    private void Start()
    {
        stAndInBtns = stOrInPanel.GetComponentsInChildren<Button>();
        stAndInBtns[0].onClick.AddListener(() =>
        {
            //�������� ������ �̵�
        });
        stAndInBtns[1].onClick.AddListener(() =>
        {
            //���Ѹ��� �̵�
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
        if(Input.GetMouseButtonDown(0))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                stOrInPanel.Open();
            }
        }
    }
}
