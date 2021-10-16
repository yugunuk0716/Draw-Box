using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    Image progressBar; // �ε� ���� ��Ȳ�� ǥ���� �̹���
    Vector2 loadingBarDefaultSize;// ���� �̹����� �⺻ ������



    static string nextScene;// ���� ���� �̸��� ������ ����

    public static bool isLoaded = false;// �� �ε� ������ �� �Ǿ����� üũ�� ����


    private void Awake()
    {

        loadingBarDefaultSize = progressBar.GetComponent<RectTransform>().sizeDelta;//�ε� �̹����� �⺻������ ����
        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, loadingBarDefaultSize.y);// �ε� �̹����� RectTransform�� �޾ƿͼ� ����� ������

    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());// �ε����� ���´ٴ� ���� �ε��� �Ѵٴ� ���� ������ �ε� �ڷ�ƾ�� ������
    }


    public static void LoadScene(string sceneName) // �ٸ� ������ �� �̵��� �ϱ����� �θ��� ����ƽ �Լ�
    {
        nextScene = sceneName;
        //PoolManager.ResetPool();
        SceneManager.LoadScene("Load");
    }




    IEnumerator LoadSceneProgress()//�ε� �ڷ�ƾ
    {
        isLoaded = false;
        yield return new WaitForSeconds(1); //�ε����� ���ڸ��� �ٸ� ������ �ʸӰ��� ���ڿ������� ���� �����Ƿ� ���
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene); //�� �ε� ���¸� �޾ƿ´�
        if (async == null) yield break;//�� �ε� ���°� ��������� �� �ε� ���°� �ƴϹǷ� �ڷ�ƾ Ż��
        async.allowSceneActivation = false;// ���� �� �ε� �Ǿ��� �� �ڵ����� �̵��ϴ� ����� ���ش�

        while (!async.isDone)//�� �ε��� �� ���� �ʾ��� �� �ݺ�
        {
            progressBar.rectTransform.sizeDelta = new Vector2(loadingBarDefaultSize.x * async.progress, loadingBarDefaultSize.y); //�ε� �̹����� ũ�⿡ �� �ε� ���൵�� ���ؼ� ���൵�� ǥ��
            if (async.progress >= 0.9f) // �ε��� �� �Ǿ��ٸ�
            {
                yield return new WaitForSeconds(1);// 1�� ��ٸ���
                isLoaded = true;//�ε� �ى�ٰ� �� ���� �ٲ��ְ�
                async.allowSceneActivation = true;//�� �ڵ� �̵� ��������ش�
            }
            yield return null;
        }
    }
}
