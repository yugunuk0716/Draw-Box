using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlertPopup : PanelScript
{
    public Button confirmBtn;
    public Text msgText;

    public int closeCount = 1;
    public int buildIdx;
    CanvasGroup cv;

    protected override void Awake()
    {
        base.Awake();
        confirmBtn.onClick.AddListener(() =>
        {
            //팝업을 닫을 때
            if (buildIdx == 0)
                PopupManager.instance.ClosePopup();
            else
                UIManager.instance.ClosePanel();
        });
    }

    private void Start()
    {
        buildIdx = SceneManager.GetActiveScene().buildIndex;
    }

    public override void Open(object data, int closeCount)
    {
        cv = GetComponent<CanvasGroup>();
        base.Open(closeCount);
        this.closeCount = closeCount;
        msgText.text = (string)data;
    }
    public override void Close() // UI 닫는 함수
    {
        base.Close();
        this.closeCount--;
        if (this.closeCount > 0)
        {
            if (buildIdx == 0)
                PopupManager.instance.ClosePopup();
            else
                UIManager.instance.ClosePanel();
        }
    }

    public override void SetAlpha(bool on) //UI 활성화 및 비활성화 함수
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }
}
