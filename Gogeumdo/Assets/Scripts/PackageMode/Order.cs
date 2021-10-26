using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Order : MonoBehaviour
{
    [Header("OrderSheet")]
    public Text orderText; // ���� �ȿ� ���� �� �� �ؽ�Ʈ
    public readonly string[] areas = new string[5] {"��⵵","������","���","����","��û��" };// �ؽ�Ʈ ����
    private int orderIdx;// �������� ���� �� �� ����

    [Header("PackableBox")] // ���� �ȿ� �ε����� �ִ� ��ư��
    public Button substituteZeroButton;
    public Button substituteOneButton;
    public Button substituteTwoButton;
    public Button substituteThreeButton;
    public Button substituteFourButton;
    private int boxIdx; // ���ڿ� �� �ִ� �ε���

    [Header("Total")]
    public Text totalPerLeftBoxCountText; // ���� ���ڼ� �ؽ�Ʈ
    public Button confirmButton; // ���ڿ� �ε��� ������ Ȯ���ϴ� ��ư
    public Text timerText = null;//���� �ð� �ؽ�Ʈ
    public float currentTime = 0f;//���� �ð�
    public List<GameObject> boxObjs; // Ʈ�������� �ִϸ��̼� ���� ���� ������Ʈ
    private int leftBoxCount; // ���� ���ڼ�
    private int totalBoxCount;
   


  


    private void Start()
    {
        
        //��ư�� �̺�Ʈ �߰�
        substituteZeroButton.onClick.AddListener(() => SubstituteNumberButton(0));
        substituteOneButton.onClick.AddListener(() => SubstituteNumberButton(1));
        substituteTwoButton.onClick.AddListener(() => SubstituteNumberButton(2));
        substituteThreeButton.onClick.AddListener(() => SubstituteNumberButton(3));
        substituteFourButton.onClick.AddListener(() => SubstituteNumberButton(4));
        confirmButton.onClick.AddListener(() => Confirm());

        totalBoxCount = 50;//�ӽ�
        leftBoxCount = totalBoxCount;
       //totalBoxCount = GameManager.instance.stageBox[GameManager.instance.stageIndex];//GameManager���� ���������� ���ڸ� �����ͼ� �� ���ڼ��� ����
        SetBox();//���� 

        currentTime = totalBoxCount * 3f;
        //���� �ϳ��� 3�� �帳�ϴ�
    }


    private void Update()
    {
        currentTime -= Time.deltaTime;

        int min = (int)currentTime / 60;
        int hour = (int)currentTime / (60 * 60);

        timerText.text = $"{hour % 60}:{(min % 60).ToString("00")}:{(currentTime % 60).ToString("00.00")}";

        if (currentTime <= 0) 
        {
            //���� ���� �г� ����ߴ�
            print("���� ����");
        }
    }


    private void SetBox() //�� ���ڸ� �������� �� ����� �� �͵�
    {
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
        totalPerLeftBoxCountText.text = $"{leftBoxCount} / {totalBoxCount}";
    }

    private void SubstituteNumberButton(int substituteNum) // ���ڿ� �ε��� ����
    {
        boxIdx = substituteNum;
    }

    private void Confirm() //Ȯ�� ��ư
    {
        if (boxIdx == orderIdx) //boxIdx �� orderIdx�� ���ٸ� �ǰ� �ԷµȰ��̹Ƿ�
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);//���� ������������ ���� ���� queue�� �־��
            leftBoxCount--; //���� ���ڼ� �ٿ���
            if (leftBoxCount <= 0) 
            {
                LoadManager.LoadScene("InGame");
                return;

            }
            else
            {
                SetBox(); //�� ���ڸ� ����
            }
            
        }
        else
        {
            leftBoxCount--;//�ƴ϶�� �� ���� ���� ���� �ٿ��ش�
        }
        //���� Ʈ����
        //Sequence seq = DOTween.Sequence();
        //GameObject box = boxObjs.Find(x => !x.activeSelf);
        //box.SetActive(true);
        //seq.Append(box.transform.DOLocalMoveX(4.5f, 1f).OnComplete(() =>
        //{
        //    box.transform.localPosition = new Vector3(-4.5f, 0);

        //})).Join(boxObj2.transform.DOLocalMoveX(0f, 1f));




    }






}
