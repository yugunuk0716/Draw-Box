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

    //튜토리얼에 쓰이는 UI 
    [Header("설명")]
    [SerializeField] private Text tutorialText = null;
    [SerializeField] private Image skipImg = null;
    [SerializeField] private Text tipText = null;

    private string curText; //현재텍스트

    private bool isText = false; //텍스트를 쓰고있는가

    public bool isTuto = false;
    
    //튜토리얼 관련 bool변수들
    private bool isTextEnd = false;
    private bool isFinished = false;
    public bool isObstacle = false;
    public bool isFever = false;
    public bool isTime = false;

    private Tweener textTween = null; //텍스트용 트위너

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
        skipImg.transform.DOLocalMoveY(skipImg.transform.localPosition.y + 10f, 0.5f).SetLoops(-1, LoopType.Yoyo); //위 아래 움직임 반복

        //SceneManager.sceneLoaded += (scene, mode) => InitTutorial(); //나중을 위해
    }

    private void Start()
    {
        //InitTutorial();
        StartCoroutine(InitTuto()); //튜토리얼 초기화
    }
    private void Update()
    {
        SkipText();//텍스트 스킵용
    }

    public bool IsTuto() //튜토리얼중인지 검사
    {
        return (isObstacle || isFever || isTime);
    }

    IEnumerator InitTuto()
    {
        yield return new WaitForSeconds(0.05f);
        InitTutorial();
    }

    public void InitTutorial() //씬에 따른 튜토리얼 등
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

    IEnumerator StagePackagerTutorial() //스테이지 패키저에서 쓰는 튜토리얼
    {
        isTuto = true;
        HidePanel(false, 1f);
        yield return oneSecWait;

        //ShowText("할말", 1f);
        //ShowExImg(true);
        //yield return new WaitUntil(() => isFinished);
        //ShowExImg(false);
        //isFinished = false;

        ShowText("당신은 택배회사 직원입니다.", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("옆에서 오는 상자를 지역에 따라 포장하면 됩니다.", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("해당 지역에 해당하는 버튼을 누르고 확인 버튼을 누르면 상자가 포장됩니다.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("택배가 몰려오고 있습니다!", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("주어진 시간안에 상자를 포장하세요!", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("그럼 건투를 빕니다.", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;
        isTuto = false;

        EventManager.Invoke("OnPackageStart");
        gameObject.SetActive(false);
    }
    IEnumerator StagePackager() //이건 그냥 스테이지 패키저에서 무조건 적으로 나오는 코루틴
    {
        isTuto = true;
        SoundManager.instance.ChangeBgmSound(0.04f);

        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("택배가 몰려오고 있습니다!", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("주어진 시간안에 상자를 포장하세요!", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("그럼 건투를 빕니다.", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;
        isTuto = false;

        EventManager.Invoke("OnPackageStart");
        gameObject.SetActive(false);
    }
    IEnumerator StageTutorial() //스테이지모드 튜토리얼
    {
        SoundManager.instance.ChangeBgmSound(0.04f);
        isTuto = true;
        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("주어진 상자들을 훌륭하게 포장했군요!", 1.2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("이제 이전에 포장했던 상자들을 분류해야 합니다.", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("상자를 드래그하면 드래그 한 방향으로 상자가 이동합니다.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("상자의 지역에 맞는 라인에 상자를 넣으면 됩니다.", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("그럼 건투를 빕니다.", 1.5f);
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

        ShowText("오 이런! 다른 직원의 실수로 컨베이어 벨트에 경고판이 올라왔군요.", 2f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("경고판에 닿은 상자는 사용할 수 없습니다.", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("경고판에 상자가 닿지 않게 조심해서 분류하세요!", 1.3f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        HidePanel(true, 1f);
        yield return oneSecWait;
        isTuto = false;

        isObstacle = false;
        EventManager.Invoke("PackagerEnd");
        gameObject.SetActive(false);
    }
    IEnumerator RankTutorial() //랭킹모드 튜토리얼
    {
        SoundManager.instance.ChangeBgmSound(0.04f);
        isTuto = true;

        HidePanel(false, 1f);
        yield return oneSecWait;

        ShowText("여기선 다른 직원들과 실적을 겨룰 수 있습니다.", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("주어진 시간안에 다른 사람보다 많은 택배를 분류하세요!", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("아이템 설명을 해드리겠습니다.", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        EventManager.Invoke("RankInitSpawn");

        yield return new WaitUntil(() => isFever);

        ShowText("이 상자는 일정 시간동안 지역에 상관없이 상자를 분류할 수 있게 해주는 상자입니다!", 3f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;
        //PoolManager.instance.RemoveBox();
        isFever = false;

        yield return new WaitUntil(() => isTime);

        ShowText("이 상자는 상자를 분류할 수 있는 시간을 늘려주는 상자입니다.", 2.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("이 상자들은 상자를 일정개수 분류할 때마다 등장합니다!", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("이 상자들을 활용하여 더 많은 상자를 분류해 보세요.", 1.5f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        ShowText("그럼 건투를 빕니다.", 1f);
        yield return new WaitUntil(() => isFinished);
        isFinished = false;

        PoolManager.instance.RemoveBox();

        HidePanel(true, 1f);
        yield return oneSecWait;
        isTuto = false;
        isTime = false;
        EventManager.Invoke("StageOrRank");
        gameObject.SetActive(false);
    }

    private void ShowText(string text, float dur = 1f) //텍스트 트윈
    {
        skipImg.enabled = false;
        tipText.DOKill();
        tipText.color = new Color(1, 1, 1, 0);

        isText = true;
        isTextEnd = false;

        curText = text;

        tutorialText.text = "";
        SoundManager.instance.PlayTextSfx();
        textTween = tutorialText.DOText(text, dur)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        isTextEnd = true;
                        skipImg.enabled = true;
                        SoundManager.instance.StopTextSfx();
                        tipText.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
                    });
    }

    private void HidePanel(bool isHide, float dur = 1f) //튜토리얼 패널 껏다 키는 함수
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

    private void SkipText() //튜토리얼 텍스트 스킵관련 함수
    {
        if (!isText) return;

        if (!isTextEnd && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            isTextEnd = true;
            skipImg.enabled = true;

            textTween.Kill();
            tutorialText.text = curText;
            tipText.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
        }
        else if (isTextEnd && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            isText = false;
            isFinished = true;
        }
    }
}
