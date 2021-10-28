using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageSelect : MonoBehaviour
{
    public Transform btnParent; //�������� ��ư���� �θ�
    private Button[] stageButton; // �������� �Ѿ�� ��ư
    public Button HomeBtn;

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
        HomeBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0); //���ξ�
        });
    }
}
