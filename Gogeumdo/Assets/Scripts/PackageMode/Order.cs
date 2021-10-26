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
    public Text totalPerLeftBoxCountText; // 남은 상자수 텍스트
    public Button confirmButton; // 상자에 인덱스 넣을걸 확정하는 버튼
    public Text timerText = null;//제한 시간 텍스트
    public float currentTime = 0f;//제한 시간
    public List<GameObject> boxObjs; // 트위닝으로 애니메이션 만들 상자 오브젝트
    public Sprite[] boxSprites;
    public GameObject targetBox;
    private int leftBoxCount; // 남은 상자수
    private int totalBoxCount;
    private bool isTweening = false;
   


  


    private void Start()
    {
        
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
        currentTime -= Time.deltaTime;

        int min = (int)currentTime / 60;
        int hour = (int)currentTime / (60 * 60);

        timerText.text = $"{hour % 60}:{(min % 60).ToString("00")}:{(currentTime % 60).ToString("00.00")}";

        if (currentTime <= 0) 
        {
            //게임 오버 패널 띄워야댐
            print("게임 오버");
        }
    }


    private void SetBox() //한 상자를 포장했을 때 해줘야 할 것들
    {
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
        totalPerLeftBoxCountText.text = $"{leftBoxCount} / {totalBoxCount}";
        targetBox.GetComponent<SpriteRenderer>().sprite = boxSprites[orderIdx];
        BoxTween(orderIdx);
    }

    private void SubstituteNumberButton(int substituteNum) // 상자에 인덱스 대입
    {
        
        GameObject box = boxObjs.Find(x => x.transform.position.x == 0f );
        box.GetComponent<SpriteRenderer>().sprite = boxSprites[substituteNum];
        boxIdx = substituteNum;
       
    }

    private void Confirm() //확인 버튼
    {
        if (boxIdx == orderIdx && !isTweening) //boxIdx 와 orderIdx가 같다면 옳게 입력된 것이므로
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);//다음 스테이지에서 쓰기 위해 queue에 넣는다
            leftBoxCount--; //남은 상자수를 줄인다
            if (leftBoxCount <= 0) //만약 남은 상자가 0이하라면
            {
                LoadManager.LoadScene("InGame");//상자

            }
            else
            {
                SetBox(); //새 상자를 내요
            }
            
        }
        else
        {
            leftBoxCount--;//아니라면 걍 남은 상자 수만 줄여준다
            totalBoxCount--;
            SetBox();
        }
        




    }


    private void BoxTween(int curIdx) 
    {
        isTweening = true;
        //상자 트위닝
        Sequence seq = DOTween.Sequence();
        GameObject box = boxObjs.Find(x => x.transform.position.x == 0);
        GameObject box2 = boxObjs.Find(x => x.transform.position.x == -4.5f);
        if (box == null || box2 == null)
            return;
        box.SetActive(true);
        box2.SetActive(true);
        box2.GetComponent<SpriteRenderer>().sprite = boxSprites[boxIdx];
        if (totalBoxCount - leftBoxCount == 0) //첫 박스
        {
            box.SetActive(false);
            box.transform.DOLocalMoveX(-4.5f,0.1f);
            seq.Append(box2.transform.DOLocalMoveX(0f, 1f).OnComplete(() => isTweening = false));
        }
        else if(totalBoxCount - leftBoxCount == totalBoxCount)// 마지막 박스
        {
            seq.Append(box.transform.DOLocalMoveX(4.5f, 1f).OnComplete(() =>
            {
                box.transform.localPosition = new Vector3(-4.5f, 0);

            }).OnComplete(() => isTweening = false));
        }
        else
        {
            seq.Append(box.transform.DOLocalMoveX(4.5f, 1f).OnComplete(() =>
            {
                box.transform.localPosition = new Vector3(-4.5f, 0);

            })).Join(box2.transform.DOLocalMoveX(0f, 1f).OnComplete(() => isTweening = false));
        }
      
    }




}
