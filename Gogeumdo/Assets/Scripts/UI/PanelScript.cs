using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelScript : MonoBehaviour
{
    private CanvasGroup canvasGroup; //패널에 붙여둔 캔버스그룹


    
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>(); //캔버스 그룹을 가져옴
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); //없을경우 만들어줌
        }
        //캔버스그룹 초기화
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void Open(object data = null, int closeCount = 1) // UI 활성화 함수
    {
        SetAlpha(true);
    }
    public virtual void Close() // UI 비활성화 함수
    {
        SetAlpha(false);
    }
    public virtual void OnClickHomeBtn(string str) // 홈버튼 함수
    {
       
        Close();
        Time.timeScale = 1f;
        GameManager.instance.Init();
        LoadManager.LoadScene(str);
    }

    public virtual void OnClickRetryBtn() // 다시시작 함수
    {
        GameManager.instance.Init();
        Time.timeScale = 1f;
        Close();
        if (GameManager.instance.isStage) 
        {
            LoadManager.LoadScene("InGamePackager");
        }
        else
        {
            LoadManager.LoadScene("InGame");
        }
    }

   
    public virtual void SetAlpha(bool on) //open과 close를 불 변수로 받아 알파를 바꿔줌
    {
       // DG.Tweening.Core.Debugger.LogSafeModeReport(this);
        DOTween.To(() => canvasGroup.alpha, value => canvasGroup.alpha = value, on ? 1f : 0f, 0.8f).OnComplete(() => {
            canvasGroup.interactable = on;
            canvasGroup.blocksRaycasts = on;
        });
    }

    
}
