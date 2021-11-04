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
    public Image timeProgress;
    public Text totalPerLeftBoxCountText; // ���� ���ڼ� �ؽ�Ʈ
    public Button confirmButton; // ���ڿ� �ε��� ������ Ȯ���ϴ� ��ư
    public float currentTime = 0f;//���� �ð�
    public List<GameObject> boxObjs; // Ʈ�������� �ִϸ��̼� ���� ���� ������Ʈ
    public Sprite[] openBoxSprites;// �ε����� ���� ���ڸ� �����س��� �迭
    public Sprite[] closedBoxSprites;// �ε����� ���� ���ڸ� �����س��� �迭
    public GameObject targetBox; // ��ǥ ������ �̹���
    private bool isSetTimerProgress = false;
    private int leftBoxCount; // ���� ���ڼ�
    private int totalBoxCount;// �з� ������������ �з��ؾ��� ���� ��


    IEnumerator progressCoroutine;// StopCoroutine�� �����ϱ� ���� �ڷ�ƾ�� ������ �����س��´�









    private void Start()
    {
        progressCoroutine = SetProgress();
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
       
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(SetProgress());
        }
    }


    private void SetBox() //�� ���ڸ� �������� �� ����� �� �͵�
    {
        if (leftBoxCount <= 0) //���� ���� ���ڰ� 0���϶��
        {
            LoadManager.LoadScene("InGame");//����

        }
        orderIdx = Random.Range(0, 5);
        orderText.text = $"{areas[orderIdx]}";
        totalPerLeftBoxCountText.text = $"{leftBoxCount} / {totalBoxCount}";
        targetBox.GetComponent<SpriteRenderer>().sprite = closedBoxSprites[orderIdx];
        BoxTween();

        progressCoroutine = SetProgress();
        StartCoroutine(progressCoroutine);

        
        

    }

    private void SubstituteNumberButton(int substituteNum) // ���ڿ� �ε��� ����
    {
        
        GameObject box = boxObjs.Find(x => Vector3.Distance(x.transform.position, Vector3.zero) < 3f);
        box.GetComponent<SpriteRenderer>().sprite = closedBoxSprites[substituteNum];
        boxIdx = substituteNum;
       
    }

    private void Confirm() //Ȯ�� ��ư
    {
        StopCoroutine(progressCoroutine);

        if (boxIdx == orderIdx) //boxIdx �� orderIdx�� ���ٸ� �ǰ� �Էµ� ���̹Ƿ�
        {
            GameManager.instance.boxIdxQueue.Enqueue(orderIdx);//���� ������������ ���� ���� queue�� �ִ´�
            print(orderIdx);
            leftBoxCount--; //���� ���ڼ��� ���δ�
            SetBox(); //�� ���ڸ� ����
        }
        else
        {
            leftBoxCount--;//�ƴ϶�� �� ���� ���� ���� �ٿ��ش�
            totalBoxCount--;//
            SetBox();
        }



    }


    private void BoxTween() 
    {
        //���� Ʈ����
        Sequence seq = DOTween.Sequence();
        GameObject box = boxObjs.Find(x => Vector3.Distance(x.transform.position, Vector3.zero) < 3f);
        GameObject box2 = boxObjs.Find(x => Vector3.Distance(x.transform.position, new Vector3(-4.5f, 0, 0)) < 3f);
        box.GetComponent<SpriteRenderer>().sprite = closedBoxSprites[boxIdx];
        if (box == null || box2 == null)
            return;
        box.SetActive(true);
        box2.SetActive(true);
        box2.GetComponent<SpriteRenderer>().sprite = openBoxSprites[boxIdx];
        if (totalBoxCount - leftBoxCount == 0) //ù �ڽ�
        {
            box.SetActive(false);
            box.transform.DOLocalMoveX(-4.5f,0.1f);
            seq.Append(box2.transform.DOLocalMoveX(0f, 1f));
        }
        else if(totalBoxCount - leftBoxCount == totalBoxCount)// ������ �ڽ�
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

    IEnumerator SetProgress()// �ð��� ��Ÿ���� slider�� filamount ���� �����ϴ� �ڷ�ƾ
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
        
        leftBoxCount--;//fillamount�� 1�� �Ǹ� �� ���ڴ� �з� ����
        totalBoxCount--;
        isSetTimerProgress = false;
        SetBox();
        
    }




}
