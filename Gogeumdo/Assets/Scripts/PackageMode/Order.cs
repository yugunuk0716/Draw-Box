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
    public Text totalBoxCountText; // �������� �� ���ڼ� �ؽ�Ʈ
    public Text leftBoxCountText; // ���� ���ڼ� �ؽ�Ʈ
    public Button confirmButton; // ���ڿ� �ε��� ������ Ȯ���ϴ� ��ư
    public List<GameObject> boxObjs; // Ʈ�������� �ִϸ��̼� ���� ���� ������Ʈ
    private int leftBoxCount; // ���� ���ڼ�


    private void Start()
    {
        
        //��ư�� �̺�Ʈ �߰�
        substituteZeroButton.onClick.AddListener(() => SubstituteNumberButton(0));
        substituteOneButton.onClick.AddListener(() => SubstituteNumberButton(1));
        substituteTwoButton.onClick.AddListener(() => SubstituteNumberButton(2));
        substituteThreeButton.onClick.AddListener(() => SubstituteNumberButton(3));
        substituteFourButton.onClick.AddListener(() => SubstituteNumberButton(4));
        confirmButton.onClick.AddListener(() => Confirm());

        leftBoxCount = 50;//�ӽ�
        totalBoxCountText.text = $"{leftBoxCount}";
        //totalBoxCount = GameManager.instance.stageBox[GameManager.instance.stageIndex];//GameManager���� ���������� ���ڸ� �����ͼ� �� ���ڼ��� ����
        SetBox();//���� 
    }



    private void SetBox() //�� ���ڸ� �������� �� ����� �� �͵�
    {
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
        leftBoxCountText.text = $"{leftBoxCount}";
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
            SetBox(); //�� ���ڸ� ����
            
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
