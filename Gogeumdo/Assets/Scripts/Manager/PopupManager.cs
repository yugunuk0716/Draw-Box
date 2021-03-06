using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    public Button registerPopupBtn;
    public Button loginPopupBtn;
    public Button logoutBtn;
    public Transform popupParent;

    private CanvasGroup popupCanvasGroup;

    public RegisterPopup registerPopupPrefab;
    public LoginPopup loginPopupPrefab;
    public AlertPopup alertPopupPrefab;

    public Dictionary<string, PanelScript> popupDic = new Dictionary<string, PanelScript>();
    private Stack<PanelScript> popupStack = new Stack<PanelScript>();

    private void Awake()
    {
        if(instance!= null)
        {
            Debug.LogError("다수의 PopupManager가 실행중");
            return;
        }
        instance = this;

    }
    private void Start()
    {
        popupCanvasGroup = popupParent.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
        {
            popupCanvasGroup = popupParent.gameObject.AddComponent<CanvasGroup>();
        }
        //켄버스 그룹 초기화
        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;
        //딕셔너리에 UI 프리펩 넣어놓기
        popupDic.Add("register", Instantiate(registerPopupPrefab, popupParent));
        popupDic.Add("login", Instantiate(loginPopupPrefab, popupParent));
        popupDic.Add("alert", Instantiate(alertPopupPrefab, popupParent));

        //버튼에 이벤트 추가
        registerPopupBtn.onClick.AddListener(() => OpenPopup("register"));
        loginPopupBtn.onClick.AddListener(() => OpenPopup("login"));
        logoutBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.Logout();
        });
    }
    public void ShowBtn(bool on)// 버튼 활성화
    {
        loginPopupBtn.gameObject.SetActive(on);
        logoutBtn.gameObject.SetActive(!on);
    }
    public void OpenPopup(string name, object data = null,int closeCount = 1) // 딕셔너리에 있는 UI 프리펩을 활성화
    {
        if(popupStack.Count == 0)
        {
            DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 1, 0.8f).OnComplete(() =>
            {
                popupCanvasGroup.interactable = true;
                popupCanvasGroup.blocksRaycasts = true;
            });
        }
        popupStack.Push(popupDic[name]);
        popupDic[name].Open(data, closeCount);
    }
    public void ClosePopup() //UI 비활성화
    {
        popupStack.Pop().Close();

        if (popupStack.Count == 0)
        {
            DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 0, 0.8f).OnComplete(() =>
            {
                popupCanvasGroup.interactable = false;
                popupCanvasGroup.blocksRaycasts = false;
            });
        }
    }
}
