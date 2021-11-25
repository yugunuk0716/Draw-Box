using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Button nextStageBtn;
    public Text stageIdxText;

    public Transform starParent;
    private Image[] star;

    CanvasGroup cv;
    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        cv = GetComponent<CanvasGroup>();
        star = starParent.GetComponentsInChildren<Image>();

        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Stage"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        nextStageBtn.onClick.AddListener(() => OnClickNextStageBtn());

        
        stageIdxText.text = $"Stage {GameManager.instance.stageIndex}";

    }

    public override void Open(object data = null, int closeCount = 1) // UI 활성화
    {
        base.Open(data, closeCount);
        for (int i = 0; i < star.Length; i++)
        {
            if (i == 0 && GameManager.instance.CompareCount(i))
            {
                GameManager.instance.isGameover = true;
                //게임 클리어 실패
            }
            star[i].sprite = UIManager.instance.ChangeStarSprite(i);
        }
        Time.timeScale = 0f;
    }

    public void OnClickNextStageBtn() // 다음 스테이지 이동 버튼
    {
        GameManager.instance.stageIndex++;
        LoadManager.LoadScene("InGamePackager");
        Time.timeScale = 1;

    }

    public override void SetAlpha(bool on) // UI 활성화 및 비활성화 함수
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }
}
