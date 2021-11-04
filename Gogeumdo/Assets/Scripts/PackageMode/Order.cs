using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Order : MonoBehaviour
{
    [Header("OrderSheet")]
    public Text orderText; // 상자 안에 들어가야 할 것 텍스트
    public readonly string[] areas = new string[5] {"경기도","강원도","경상도","전라도","충청도" };// 텍스트 저장
    private int orderIdx;// 랜덤으로 들어가야 할 것 설정

    [Header("PackableBox")] // 상자 안에 인덱스를 넣는 버튼들
    public Button substituteZeroButton;
    public Button substituteOneButton;
    public Button substituteTwoButton;
    public Button substituteThreeButton;
    public Button substituteFourButton;
    private int boxIdx; // 상자에 들어가 있는 인덱스

    [Header("Total")]
    public Image timeProgress;
    public Text totalPerLeftBoxCountText; // 남은 상자수 텍스트
    public Button confirmButton; // 상자에 인덱스 넣을걸 확정하는 버튼
    public float currentTime = 0f;//제한 시간
    public List<GameObject> boxObjs; // 트위닝으로 애니메이션 만들 상자 오브젝트
    public Sprite[] openBoxSprites;// 인덱스별 열린 상자를 저장해놓은 배열
    public Sprite[] closedBoxSprites;// 인덱스별 닫힌 상자를 저장해놓은 배열
    public GameObject targetBox; // 목표 상자의 이미지
    private bool isSetTimerProgress = false;
    private int leftBoxCount; // 남은 상자수
    private int totalBoxCount;// 분류 스테이지에서 분류해야할 상자 수


    IEnumerator progressCoroutine;// StopCoroutine을 실행하기 위해 코루틴을 변수로 선언해놓는다









    private void Start()
    {
        progressCoroutine = SetProgress();
        //버튼에 이벤트 추가
        substituteZeroButton.onClick.AddListener(() => SubstituteNumberButton(0));
        substituteOneButton.onClick.AddListener(() => SubstituteNumberButton(1));
        substituteTwoButton.onClick.AddListener(() => SubstituteNumberButton(2));
        substituteThreeButton.onClick.AddListener(() => SubstituteNumberButton(3));
        substituteFourButton.onClick.AddListener(() => SubstituteNumberButton(4));
        confirmButton.onClick.AddListener(() => Confirm());

        totalBoxCount = GameManager.instance.stageBox[GameManager.instance.stageIndex];//GameManager에서 스테이지별 상자를 가져와서 총 상자수로 설정
        //totalBoxCount = 50;//임시
        leftBoxCount = totalBoxCount;
        SetBox();//다음 

        currentTime = totalBoxCount * 3f;
        //상자 하나당 3초 드립니다

      
       
        
    }


    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(SetProgress());
        }
    }


    private void SetBox() //한 상자를 포장했을 때 해줘야 할 것들
    {
        if (leftBoxCount <= 0) //만약 남은 상자가 0이하라면
        {
            LoadManager.LoadScene("InGame");//상자

        }
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
        totalPerLeftBoxCountText.text = $"{leftBoxCount} / {totalBoxCount}";
        targetBox.GetComponent<SpriteRenderer>().sprite = closedBoxSprites[orderIdx];
        BoxTween();

        progressCoroutine = SetProgress();
        StartCoroutine(progressCoroutine);

        
        

    }

    private void SubstituteNumberButton(int substituteNum) // 상자에 인덱스 대입
    {
        
        GameObject box = boxObjs.Find(x => Vector3.Distance(x.transform.position, Vector3.zero) < 3f);
        box.GetComponent<SpriteRenderer>().sprite = closedBoxSprites[substituteNum];
        boxIdx = substituteNum;
       
    }

    private void Confirm() //확인 버튼
    {
        StopCoroutine(progressCoroutine);

        if (boxIdx == orderIdx) //boxIdx 와 orderIdx가 같다면 옳게 입력된 것이므로
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);//다음 스테이지에서 쓰기 위해 queue에 넣는다
            print(orderIdx);
            leftBoxCount--; //남은 상자수를 줄인다
            SetBox(); //새 상자를 내요
        }
        else
        {
            leftBoxCount--;//아니라면 걍 남은 상자 수만 줄여준다
            totalBoxCount--;//
            SetBox();
        }



    }


    private void BoxTween() 
    {
        //상자 트위닝
        Sequence seq = DOTween.Sequence();
        GameObject box = boxObjs.Find(x => Vector3.Distance(x.transform.position, Vector3.zero) < 3f);
        GameObject box2 = boxObjs.Find(x => Vector3.Distance(x.transform.position, new Vector3(-4.5f, 0, 0)) < 3f);
        box.GetComponent<SpriteRenderer>().sprite = closedBoxSprites[boxIdx];
        if (box == null || box2 == null)
            return;
        box.SetActive(true);
        box2.SetActive(true);
        box2.GetComponent<SpriteRenderer>().sprite = openBoxSprites[boxIdx];
        if (totalBoxCount - leftBoxCount == 0) //첫 박스
        {
            box.SetActive(false);
            box.transform.DOLocalMoveX(-4.5f,0.1f);
            seq.Append(box2.transform.DOLocalMoveX(0f, 1f));
        }
        else if(totalBoxCount - leftBoxCount == totalBoxCount)// 마지막 박스
        {
            seq.Append(box.transform.DOLocalMoveX(4.5f, 1f).OnComplete(() =>
            {
                box.transform.localPosition = new Vector3(-4.5f, 0);

            }));
        }
        else
        {
            seq.Append(box.transform.DOLocalMoveX(4.5f, 1f).OnComplete(() =>
            {
                box.transform.localPosition = new Vector3(-4.5f, 0);

            })).Join(box2.transform.DOLocalMoveX(0f, 1f));
        }

    }

    IEnumerator SetProgress()// 시간을 나타내는 slider의 filamount 값을 조절하는 코루틴
    {
        isSetTimerProgress = true;
        timeProgress.fillAmount = 1f;
        float time = 3f;
        float t = 0f;
        while (true)
        {
            yield return null;
            t += Time.deltaTime;
            if (t >= time)
            {
                break;
            }
            timeProgress.fillAmount = Mathf.Lerp(1f, 0f, t / time);
        }
        
        leftBoxCount--;//fillamount가 1이 되면 그 상자는 분류 실패
        totalBoxCount--;
        isSetTimerProgress = false;
        SetBox();
        
    }




}
