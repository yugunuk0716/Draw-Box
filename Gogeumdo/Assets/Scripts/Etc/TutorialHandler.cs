using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{
    private CanvasGroup cg = null;

    [Header("설명")]
    [SerializeField] private Text tutorialText = null;
    [SerializeField] private Image skipImg = null;
    [SerializeField] private Text tipText = null;

    private string curText;

    private bool isText = false;

    private bool isTextEnd = false;
    private bool isFinished = false;

    private Tweener textTween = null;

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        tutorialText.text = " ";

        skipImg.enabled = false;
        skipImg.transform.DOLocalMoveY(skipImg.transform.localPosition.y + 10f, 0.5f).SetLoops(-1, LoopType.Yoyo);

        if(GameManager.instance.isStage)
        {
            if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (GameManager.instance.stageIndex == 1)
                {
                    StartCoroutine(StagePackagerTutorial());
                }
                else
                {
                    EventManager.Invoke("OnPackageStart");
                }
            }
            else
            {
                if (GameManager.instance.stageIndex == 1)
                {
                    StartCoroutine(StageTutorial());
                }
                else
                {
                    EventManager.Invoke("StageOrRank");
                }
            }
        }
        else
        {
            NetworkManager.instance.SendGetRequest("rankconfirm", "", res =>
             {
                 ResponseVO vo = JsonUtility.FromJson<ResponseVO>(res);

                 if(vo.result)
                 {
                     EventManager.Invoke("StageOrRank");
                 }
                 else
                 {
                     StartCoroutine(RankTutorial());
                 }
             });
        }
    }
    private void Update()
    {
        SkipText();
    }

    IEnumerator StagePackagerTutorial()
    {
        HidePanel(false, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        ShowText("할말", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("할말", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("할말", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("할말", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        EventManager.Invoke("OnPackageStart");
        gameObject.SetActive(false);
    }
    IEnumerator StageTutorial()
    {
        HidePanel(false, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        HidePanel(true, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        EventManager.Invoke("StageOrRank");
        gameObject.SetActive(false);
    }
    IEnumerator RankTutorial()
    {
        HidePanel(false, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        HidePanel(true, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        EventManager.Invoke("StageOrRank");
        gameObject.SetActive(false);
    }

    private void ShowText(string text, float dur = 1f)
    {
        skipImg.enabled = false;
        tipText.DOKill();
        tipText.color = new Color(1, 1, 1, 0);

        isText = true;
        isTextEnd = false;

        curText = text;

        tutorialText.text = "";
        textTween = tutorialText.DOText(text, dur)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        isTextEnd = true;
                        skipImg.enabled = true;
                        tipText.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
                    });
    }

    private void HidePanel(bool isHide, float dur = 1f)
    {
        if (isHide)
        {
            cg.DOFade(0f, dur);
        }
        else
        {
            cg.DOFade(1f, dur);
        }
    }

    private void SkipText()
    {
        if (!isText) return;

        if (!isTextEnd && Input.GetKeyDown(KeyCode.Return))
        {
            isTextEnd = true;
            skipImg.enabled = true;

            textTween.Kill();
            tutorialText.text = curText;
            tipText.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
        }
        else if (isTextEnd && Input.GetKeyDown(KeyCode.Return))
        {
            isText = false;
            isFinished = true;
        }
    }
}
