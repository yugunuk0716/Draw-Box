using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageManager : MonoBehaviour
{
    public Transform btnParent; //�������� ��ư���� �θ�
    private Button[] stageButton; // �������� �Ѿ�� ��ư

    private void Start()
    {
        stageButton = btnParent.GetComponentsInChildren<Button>(); //��ư�� ��������

        for (int i = 0; i < stageButton.Length; i++)
        {
            int idx = i; //Ŭ���� ������ ���Ͽ� �̷��� ����Ѵ�.
            stageButton[idx].onClick.AddListener(() =>
            {
                GameManager.instance.stageIndex = idx; //���ӸŴ����� �������� �ε����� �ٲ���
                LoadManager.LoadScene("InGame");
            });
        }
    }
}
