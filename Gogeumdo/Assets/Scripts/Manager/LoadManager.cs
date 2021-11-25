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
        loadingBarDefaultSize = progressBar.GetComponent<RectTransform>().sizeDelta;  //이미지의 기본 사이즈 정의
        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, loadingBarDefaultSize.y); // 이미지의 사이즈 0으로 

    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    //로딩 씬을 거쳐 다음씬으로 이동하는 함수
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Load");
    }


    //로딩 처리 함수
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
