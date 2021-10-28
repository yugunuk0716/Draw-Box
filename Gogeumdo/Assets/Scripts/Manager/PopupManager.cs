using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;

        popupDic.Add("register", Instantiate(registerPopupPrefab, popupParent));
        popupDic.Add("login", Instantiate(loginPopupPrefab, popupParent));
        popupDic.Add("alert", Instantiate(alertPopupPrefab, popupParent));

        registerPopupBtn.onClick.AddListener(() => OpenPopup("register"));
        loginPopupBtn.onClick.AddListener(() => OpenPopup("login"));
        logoutBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.Logout();
        });
    }
    public void ShowBtn(bool on)
    {
        loginPopupBtn.gameObject.SetActive(on);
        logoutBtn.gameObject.SetActive(!on);
    }
    public void OpenPopup(string name, object data = null,int closeCount = 1)
    {
        if(popupStack.Count == 0)
        {
            popupCanvasGroup.interactable = true;
            DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 1, 0.8f).OnComplete(() =>
            {
                popupCanvasGroup.interactable = true;
                popupCanvasGroup.blocksRaycasts = true;
            });
        }
        popupStack.Push(popupDic[name]);
        popupDic[name].Open(data, closeCount);
    }
    public void ClosePopup()
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
