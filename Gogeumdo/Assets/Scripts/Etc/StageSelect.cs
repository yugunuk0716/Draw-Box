using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageSelect : MonoBehaviour
{
    public Transform btnParent; //�������� ��ư���� �θ�
    private Button[] stageButton; // �������� �Ѿ�� ��ư

    private void Start()
    {
        stageButton = btnParent.GetComponentsInChildren<Button>(); //��ư�� ��������

        for (int i = 1; i < stageButton.Length + 1; i++)
        {
            int idx = i; //Ŭ���� ������ ���Ͽ� �̷��� ����Ѵ�.
            stageButton[idx - 1].onClick.AddListener(() =>
            {
                GameManager.instance.stageIndex = idx; //���ӸŴ����� �������� �ε����� �ٲ���
                LoadManager.LoadScene("InGamePackager");
            });
        }
    }
}