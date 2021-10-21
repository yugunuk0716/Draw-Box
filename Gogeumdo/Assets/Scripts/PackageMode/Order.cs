using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [Header("OrderSheet")]
    public Text orderText;
    public readonly string[] areas = new string[5] {"경기도","강원도","경상도","전라도","충청도" };
    private int orderIdx;

    [Header("PackableBox")]
    public Button substituteZeroButton;
    public Button substituteOneButton;
    public Button substituteTwoButton;
    public Button substituteThreeButton;
    public Button substituteFourButton;
    private int boxIdx;

    [Header("Total")]
    public Text totalBoxCountText;
    public Text leftBoxCountText;
    public Button confirmButton;
    private int leftBoxCount;


    private void Start()
    {
        

        substituteZeroButton.onClick.AddListener(() => SubstituteNumberButton(0));
        substituteOneButton.onClick.AddListener(() => SubstituteNumberButton(1));
        substituteTwoButton.onClick.AddListener(() => SubstituteNumberButton(2));
        substituteThreeButton.onClick.AddListener(() => SubstituteNumberButton(3));
        substituteFourButton.onClick.AddListener(() => SubstituteNumberButton(4));
        confirmButton.onClick.AddListener(() => Confirm());

        leftBoxCount = 50;//임시
        totalBoxCountText.text = $"{leftBoxCount}";
        //totalBoxCount = GameManager.instance.stageBox[GameManager.instance.stageIndex];//원래 이거임
        SetNext();
    }



    private void SetNext() //한 상자를 포장했을 때 해줘야 할 것들
    {
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
        leftBoxCountText.text = $"{leftBoxCount}";
    }

    private void SubstituteNumberButton(int substituteNum)
    {
        boxIdx = substituteNum;
        print(substituteNum);
    }

    private void Confirm() 
    {
        if (boxIdx == orderIdx) 
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);
            leftBoxCount--;
            SetNext();
            
        }
        else
        {
            leftBoxCount--;
        }
        //상자 왼쪽으로 트위닝
    }






}
