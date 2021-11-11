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

    [Header("����")]
    [SerializeField] private Text tutorialText = null;
    [SerializeField] private Image skipImg = null;
    [SerializeField] private Text tipText = null;

    private string curText;

    private bool isText = false;

    
    private bool isTextEnd = false;
    private bool isFinished = false;
    public bool isObstacle = false;
    public bool isFever = false;
    public bool isTime = false;

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

        //SceneManager.sceneLoaded += (scene, mode) => InitTutorial(); //������ ����
    }

    private void Start()
    {
        //InitTutorial();
        StartCoroutine(InitTuto());
    }
    private void Update()
    {
        SkipText();
    }

    public bool IsTuto()
    {
        return (isObstacle || isFever || isTime);
    }

    IEnumerator InitTuto()
    {
        yield return new WaitForSeconds(0.1f);
        InitTutorial();
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
                    gameObject.SetActive(false);
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
                    gameObject.SetActive(false);
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
        HidePanel(false, 1f);
        yield return oneSecWait;

        //ShowText("�Ҹ�", 1f);
        //ShowExImg(true);
        //yield return new WaitUntil(() => isFinished);
        //ShowExImg(false);
        //isFinished = false;

        ShowText("����� �ù�ȸ�� �����Դϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("������ ���� ���ڸ� ������ ���� �����ϸ� �˴ϴ�", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ش� ������ �ش��ϴ� ��ư�� ������ Ȯ�� ��ư�� ������ ���ڰ� ����˴ϴ�", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�ù谡 �������� �ֽ��ϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�־��� �ð��ȿ� ���ڸ� �����ϼ���", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�׷� ������ ���ϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;

        EventManager.Invoke("OnPackageStart");
        gameObject.SetActive(false);
    }
    IEnumerator StagePackager()
    {
        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("�ù谡 �������� �ֽ��ϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�־��� �ð��ȿ� ���ڸ� �����ϼ���", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�׷� ������ ���ϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;

        EventManager.Invoke("OnPackageStart");
        gameObject.SetActive(false);
    }
    IEnumerator StageTutorial()
    {
        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("�־��� ���ڵ��� �Ǹ��ϰ� �����߱���!", 1.2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("���� ������ �����ߴ� ���ڵ��� �з��ؾ� �մϴ�", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("���ڸ� �巡���ϸ� �巡�� �� �������� ���ڰ� �̵��մϴ�", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("������ ������ �´� ���ο� ���ڸ� ������ �˴ϴ�", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;

        EventManager.Invoke("StageOrRank");
        yield return new WaitUntil(() => isObstacle);

        //EventManager.Invoke("StopBoxCoroutine");
        tutorialText.text = "";
        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("�� �̷�! �ٸ� ������ �Ǽ��� �����̾� ��Ʈ�� ������� �ö�Ա���", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("����ǿ� ���� ���ڴ� ����� �� �����ϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("����ǿ� ���ڰ� ���� �ʰ� �����ؼ� �з��ϼ���!", 1.3f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;

        isObstacle = false;
        EventManager.Invoke("PackagerEnd");
        gameObject.SetActive(false);
    }
    IEnumerator RankTutorial()
    {
        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("���⼱ �ٸ� ������� ������ �ܷ� �� �ֽ��ϴ�", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�־��� �ð��ȿ� �ٸ� ������� ���� �ù踦 �з��ϼ���!", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("������ ������ �ص帮�ڽ��ϴ�", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        EventManager.Invoke("RankInitSpawn");

        yield return new WaitUntil(() => isFever);

        ShowText("�� ���ڴ� ������ ������� ���ڸ� �з��� �� �ְ� ���ִ� �����Դϴ�!", 2.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;
        //PoolManager.instance.RemoveBox();
        isFever = false;

        yield return new WaitUntil(() => isTime);

        ShowText("�� ���ڴ� �з��� �� �ִ� �ð��� �÷��ִ� �����Դϴ�", 2.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�� ���ڵ��� ���ڸ� �������� �з��� ������ �����մϴ�!", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�� ���ڵ��� Ȱ���Ͽ� �� ���� ���ڸ� �з��� ������", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("�׷� ������ ���ϴ�", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        PoolManager.instance.RemoveBox();

        HidePanel(true, 1f);
        yield return oneSecWait;
        isTime = false;
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
