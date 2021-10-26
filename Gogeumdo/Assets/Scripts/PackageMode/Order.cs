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
    public Sprite[] boxSprites;
    public GameObject targetBox;
    private int leftBoxCount; // ���� ���ڼ�
    private int totalBoxCount;
    private bool isTweening = false;
   


  


    private void Start()
    {
        
        //��ư�� �̺�Ʈ �߰�
        substituteZeroButton.onClick.AddListener(() => SubstituteNumberButton(0));
        substituteOneButton.onClick.AddListener(() => SubstituteNumberButton(1));
        substituteTwoButton.onClick.AddListener(() => SubstituteNumberButton(2));
        substituteThreeButton.onClick.AddListener(() => SubstituteNumberButton(3));
        substituteFourButton.onClick.AddListener(() => SubstituteNumberButton(4));
        confirmButton.onClick.AddListener(() => Confirm());

        totalBoxCount = GameManager.instance.stageBox[GameManager.instance.stageIndex];//GameManager���� ���������� ���ڸ� �����ͼ� �� ���ڼ��� ����
        //totalBoxCount = 50;//�ӽ�
        leftBoxCount = totalBoxCount;
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
        targetBox.GetComponent<SpriteRenderer>().sprite = boxSprites[orderIdx];
        BoxTween(orderIdx);
    }

    private void SubstituteNumberButton(int substituteNum) // ���ڿ� �ε��� ����
    {
        
        GameObject box = boxObjs.Find(x => x.transform.position.x == 0f );
        box.GetComponent<SpriteRenderer>().sprite = boxSprites[substituteNum];
        boxIdx = substituteNum;
       
    }

    private void Confirm() //Ȯ�� ��ư
    {
        if (boxIdx == orderIdx && !isTweening) //boxIdx �� orderIdx�� ���ٸ� �ǰ� �Էµ� ���̹Ƿ�
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);//���� ������������ ���� ���� queue�� �ִ´�
            leftBoxCount--; //���� ���ڼ��� ���δ�
            if (leftBoxCount <= 0) //���� ���� ���ڰ� 0���϶��
            {
                LoadManager.LoadScene("InGame");//����

            }
            else
            {
                SetBox(); //�� ���ڸ� ����
            }
            
        }
        else
        {
            leftBoxCount--;//�ƴ϶�� �� ���� ���� ���� �ٿ��ش�
            totalBoxCount--;
            SetBox();
        }
        




    }


    private void BoxTween(int curIdx) 
    {
        isTweening = true;
        //���� Ʈ����
        Sequence seq = DOTween.Sequence();
        GameObject box = boxObjs.Find(x => x.transform.position.x == 0);
        GameObject box2 = boxObjs.Find(x => x.transform.position.x == -4.5f);
        if (box == null || box2 == null)
            return;
        box.SetActive(true);
        box2.SetActive(true);
        box2.GetComponent<SpriteRenderer>().sprite = boxSprites[boxIdx];
        if (totalBoxCount - leftBoxCount == 0) //ù �ڽ�
        {
            box.SetActive(false);
            box.transform.DOLocalMoveX(-4.5f,0.1f);
            seq.Append(box2.transform.DOLocalMoveX(0f, 1f).OnComplete(() => isTweening = false));
        }
        else if(totalBoxCount - leftBoxCount == totalBoxCount)// ������ �ڽ�
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
