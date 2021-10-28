using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageSelect : MonoBehaviour
{
    public Transform btnParent; //스테이지 버튼들의 부모
    private Button[] stageButton; // 스테이지 넘어가는 버튼
    public Button HomeBtn;

    private void Start()
    {
        stageButton = btnParent.GetComponentsInChildren<Button>(); //버튼들 가져오기

        for (int i = 1; i < stageButton.Length + 1; i++)
        {
            int idx = i; //클로저 문제로 인하여 이렇게 써야한다.
            stageButton[idx - 1].onClick.AddListener(() =>
            {
                GameManager.instance.stageIndex = idx; //게임매니저의 스테이지 인덱스를 바꿔줌
                LoadManager.LoadScene("InGamePackager");
            });
        }
        HomeBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0); //메인씬
        });
    }
}
