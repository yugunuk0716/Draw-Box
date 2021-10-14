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
            //스테이지 씬으로 이동
        });
        stAndInBtns[1].onClick.AddListener(() =>
        {
            //무한모드로 이동
        });
    }

    private void Update()
    {
        //if(Input.touchCount > 0)
        //{
        //    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) //ui클릭시 true 아닐시 false
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
