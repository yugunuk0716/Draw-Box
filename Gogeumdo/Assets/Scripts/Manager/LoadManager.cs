using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    Image progressBar; // 로딩 진행 상황을 표시할 이미지
    Vector2 loadingBarDefaultSize;// 위의 이미지의 기본 사이즈



    static string nextScene;// 다음 씬의 이름을 저장할 변수

    public static bool isLoaded = false;// 씬 로딩 진행이 다 되었는지 체크할 변수


    private void Awake()
    {

        loadingBarDefaultSize = progressBar.GetComponent<RectTransform>().sizeDelta;//로딩 이미지의 기본사이즈 설정
        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, loadingBarDefaultSize.y);// 로딩 이미지의 RectTransform을 받아와서 사이즈를 설정함

    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());// 로딩씬에 들어온다는 것은 로딩을 한다는 것이 때문에 로딩 코루틴을 시작함
    }


    public static void LoadScene(string sceneName) // 다른 씬에서 씬 이동을 하기위해 부르는 스태틱 함수
    {
        nextScene = sceneName;
        //PoolManager.ResetPool();
        SceneManager.LoadScene("Load");
    }




    IEnumerator LoadSceneProgress()//로딩 코루틴
    {
        isLoaded = false;
        yield return new WaitForSeconds(1); //로딩씬에 오자마자 다른 씬으로 너머가면 부자연스러울 수도 있으므로 대기
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene); //씬 로딩 상태를 받아온다
        if (async == null) yield break;//씬 로딩 상태가 비어있으면 씬 로딩 상태가 아니므로 코루틴 탈출
        async.allowSceneActivation = false;// 씬이 다 로딩 되었을 때 자동으로 이동하는 기능을 꺼준다

        while (!async.isDone)//씬 로딩이 다 되지 않았을 때 반복
        {
            progressBar.rectTransform.sizeDelta = new Vector2(loadingBarDefaultSize.x * async.progress, loadingBarDefaultSize.y); //로딩 이미지의 크기에 씬 로딩 진행도를 곱해서 진행도를 표시
            if (async.progress >= 0.9f) // 로딩이 다 되었다면
            {
                yield return new WaitForSeconds(1);// 1초 기다리고
                isLoaded = true;//로딩 다됬다고 불 변수 바꿔주고
                async.allowSceneActivation = true;//씬 자동 이동 기능을켜준다
            }
            yield return null;
        }
    }
}
