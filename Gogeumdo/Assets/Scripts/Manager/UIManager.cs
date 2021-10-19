using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Transform panelParent;
    public MenuPanel menuPanel;
    public Button menuBtn;

    private CanvasGroup cv;

    Dictionary<string, PanelScript> panelDic = new Dictionary<string, PanelScript>();
    Stack<PanelScript> panelStack = new Stack<PanelScript>();


    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("다수의 UIManager가 실행중입니다");
        }
        instance = this;
    }

    void Start()
    {
        panelDic.Add("menu", Instantiate(menuPanel, panelParent));
        menuBtn.onClick.AddListener(() => 
        {
            Time.timeScale = 0;
            OpenPanel("menu");
        });
    }

    public void OpenPanel(string name) 
    {
        cv = panelDic[name].GetComponent<CanvasGroup>();
        cv.interactable = true;
        cv.alpha = 1f; 
        cv.blocksRaycasts = true;
        

        panelStack.Push(panelDic[name]);
        panelDic[name].Open();
    }

    public void ClosePanel() 
    {
        cv = panelStack.Peek().GetComponent<CanvasGroup>();
        panelStack.Pop().Close();

        cv.alpha = 0f;
        cv.interactable = false;
        cv.blocksRaycasts = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
