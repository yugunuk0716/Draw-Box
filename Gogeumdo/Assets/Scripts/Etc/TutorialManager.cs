using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    private CanvasGroup cg = null;

    [Header("설명")]
    [SerializeField] private Text tutorialText = null;
    [SerializeField] private Image skipImg = null;
    [SerializeField] private Text tipText = null;
    [SerializeField] private Image exImg = null;

    private string curText;

    private bool isText = false;

    private bool isTextEnd = false;
    private bool isFinished = false;
    public bool isObstacle = false;

    private Tweener textTween = null;

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        tutorialText.text = " ";

        skipImg.enabled = false;
        skipImg.transform.DOLocalMoveY(skipImg.transform.localPosition.y + 10f, 0.5f).SetLoops(-1, LoopType.Yoyo);

        InitTutorial();
        //SceneManager.sceneLoaded += (scene, mode) => InitTutorial(); //나중을 위해
    }
    private void Update()
    {
        SkipText();
    }

    public void InitTutorial()
    {
        if (GameManager.instance.isStage)
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (GameManager.instance.stageIndex == 1)
                {
                    StartCoroutine(StagePackagerTutorial());
                }
                else
                {
                    StartCoroutine(StagePackager());
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

                if (vo.result)
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

    IEnumerator StagePackagerTutorial()
    {
        HidePanel(false, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        //ShowText("할말", 1f);
        //ShowExImg(true);
        //yield return new WaitUntil(() => isFinished);
        //ShowExImg(false);
        //isFinished = false;

        ShowText("당신은 택배회사 직원입니다", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("옆에서 오는 상자를 지역에 따라 포장하면 됩니다", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("해당 지역에 해당하는 버튼을 누르고 확인 버튼을 누르면 상자가 포장됩니다", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        StartCoroutine(StagePackager());
    }
    IEnumerator StagePackager()
    {
        ShowText("택배가 몰려오고 있습니다", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("주어진 시간안에 상자를 포장하세요", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("그럼 건투를 빕니다", 1f);
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

        ShowText("주어진 상자들을 훌륭하게 포장했군요!", 1.2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("이제 이전에 포장했던 상자들을 분류해야 합니다", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("상자를 드래그하면 드래그 한 방향으로 상자가 이동합니다", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("상자의 지역에 맞는 라인에 상자를 넣으면 됩니다", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        skipImg.enabled = false;
        HidePanel(true, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        EventManager.Invoke("StageOrRank");
        yield return new WaitUntil(() => isObstacle);

        EventManager.Invoke("StopBoxCoroutine");
        HidePanel(false, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        ShowText("오 이런! 다른 직원의 실수로 컨베이어 벨트에 경고판이 올라왔군요", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("경고판에 닿은 상자는 사용할 수 없습니다", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("경고판에 상자가 닿지 않게 조심해서 분류하세요!", 1.3f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        skipImg.enabled = false;
        HidePanel(true, 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        EventManager.Invoke("PackagerEnd");
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
    private void ShowExImg(bool isShow,Sprite sprite = null)
    {
        exImg.sprite = sprite;
        exImg.enabled = isShow;
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
