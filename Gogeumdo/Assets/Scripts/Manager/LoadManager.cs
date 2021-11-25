using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    Image progressBar;
    Vector2 loadingBarDefaultSize;
    float timer = 0f;


    static string nextScene;

    public static bool isLoaded = false;

    private void Awake()
    {
        loadingBarDefaultSize = progressBar.GetComponent<RectTransform>().sizeDelta;  //�̹����� �⺻ ������ ����
        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, loadingBarDefaultSize.y); // �̹����� ������ 0���� 

    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    //�ε� ���� ���� ���������� �̵��ϴ� �Լ�
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Load");
    }


    //�ε� ó�� �Լ�
    IEnumerator LoadSceneProgress()
    {
        isLoaded = false;
        yield return new WaitForSeconds(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        if (async == null) yield break;
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
        
           
            if (async.progress < 0.9f) 
            {
                yield return null;
            }
            else
            {

                timer += 0.01f;
                progressBar.rectTransform.sizeDelta = new Vector2(loadingBarDefaultSize.x * timer, loadingBarDefaultSize.y);
                if (progressBar.rectTransform.sizeDelta.x >= 835f) 
                {
                    yield return new WaitForSeconds(1);
                    isLoaded = true;
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
