using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    public PanelScript stOrInPanel; //stage나 infinite 모드를 고르기 위한 패널
    public Button[] stAndInBtns; //0 = stage //1 = infinite

    private void Awake()
    {
        
    }

    private void Start()
    {
        stAndInBtns = stOrInPanel.GetComponentsInChildren<Button>(); // 패널밑에서 버튼들을 가져와준다
        stAndInBtns[0].onClick.AddListener(() =>
        {
            //스테이지 씬으로 이동
            GameManager.instance.isStage = true; 
            LoadManager.LoadScene("Stage");
        });
        stAndInBtns[1].onClick.AddListener(() =>
        {
            GameManager.instance.isStage = false;
            //랭크모드로 이동
            LoadManager.LoadScene("InGame");
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
        //테스트용 코드
        if(Input.GetMouseButtonDown(0)) //좌클릭시
        {
            if(!EventSystem.current.IsPointerOverGameObject()) //ui가 아닌 다른곳을 클릭했을때  
            {
                stOrInPanel.Open(); //패널을 켜주기
            }
        }
    }
}
