using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelScript : MonoBehaviour
{
    private CanvasGroup canvasGroup; //패널에 붙여둔 캔버스그룹

    public CanvasGroup rankListPanel;
    public Transform content;
    public ScoreRank rankPrefab;

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

    public virtual void Open(object data = null, int closeCount = 1)
    {
        SetAlpha(true);
        Time.timeScale = 0f;
    }
    public virtual void Close()
    {
        SetAlpha(false);
        Time.timeScale = 1f;
    }
    public virtual void OnClickHomeBtn()
    {
        Close();
        Time.timeScale = 1f;
        LoadManager.LoadScene("Stage");
        GameManager.instance.Init();
    }

    public virtual void OnClickRetryBtn()
    {
        GameManager.instance.Init();
        Time.timeScale = 1f;
        Close();
        LoadManager.LoadScene("InGame");
    }

    public virtual void OnClickRankBtn()
    {
        //여기서 하시면 댐
        NetworkManager.instance.SendGetRequest("ranklist", "", json =>
        {
            Transform[] childs = content.GetComponentsInChildren<Transform>();
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(json);

            if (res.result)
            {
                ScoreListVO vo = JsonUtility.FromJson<ScoreListVO>(res.payload);
                for (int i = 1; i < childs.Length; i++)
                {
                    Destroy(childs[i].gameObject);
                }

                for (int i = 0; i < vo.list.Count; i++)
                {
                    ScoreRank obj = Instantiate(rankPrefab, content);
                    obj.SetData(i + 1, vo.list[i]);
                }
                RankListPanel(true);
            }
            else
            {
                //오류 발생 시
            }
        });

    }


    public virtual void RankListPanel(bool on) 
    {
        rankListPanel.alpha = on ? 1 : 0;
        rankListPanel.blocksRaycasts = on;
        rankListPanel.interactable = on;
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
