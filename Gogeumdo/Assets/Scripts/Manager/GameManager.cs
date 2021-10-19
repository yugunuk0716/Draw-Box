using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱���

    //public int boxCount = 0;
    public bool isGameover = false; // ���ӿ��� üũ
    public bool isStage = false; //�������� ������� ���Ѹ������ üũ�ϱ� ���� ����
    public bool isFever = false; //�ǹ����� üũ�ϱ� ���� ����
    public int stageIndex = 0; //�������� �ε��� - �̿� ���� ����������� �����ؾ���
    public int boxCount = 0; //�ڽ� ī��Ʈ - �̰Ϳ� ���� �ǹ� ���ڰ� ����

    private void Awake()
    {
        if(instance != null)// �̱��� �ߺ� üũ
        {
            Debug.LogError("�ټ��� ���ӸŴ����� ������");
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            LoadManager.LoadScene("InGame");
        }
    }
}
