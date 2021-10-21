using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [Header("orderSheet")]
    [Range(0,5)]
    private int orderIdx;
    public Text orderText;
    public readonly string[] areas = new string[5] {"경기도","강원도","경상도","전라도","충청도" };


    [Header("PackableBox")]
    private int boxIdx;
    public Button substituteZeroButton;
    public Button substituteOneButton;
    public Button substituteTwoButton;
    public Button substituteThreeButton;
    public Button substituteFourButton;

    [Header("Total")]
    public Text totalBoxCountText;
    public Button confirmButton;
    private int totalBoxCount;


    private void Start()
    {
        SetNext();

        substituteZeroButton.onClick.AddListener(() => SubstituteNumberButton(0));
        substituteOneButton.onClick.AddListener(() => SubstituteNumberButton(1));
        substituteTwoButton.onClick.AddListener(() => SubstituteNumberButton(2));
        substituteThreeButton.onClick.AddListener(() => SubstituteNumberButton(3));
        substituteFourButton.onClick.AddListener(() => SubstituteNumberButton(4));
        confirmButton.onClick.AddListener(() => Confirm());

        // totalBoxCount = ;
    }



    private void SetNext() //한 상자를 포장했을 때 해줘야 할 것들
    {
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
    }

    private void SubstituteNumberButton(int substituteNum)
    {
        boxIdx = substituteNum;
    }

    private void Confirm() 
    {
        if (boxIdx == orderIdx) 
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);
            
        }
        else
        {
            totalBoxCount--;
        }
        //상자 왼쪽으로 트위닝
    }






}
