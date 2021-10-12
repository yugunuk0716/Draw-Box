using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public CanvasGroup stOrInPanel;
    public CanvasGroup stagePanel;
    public Button[] stAndInBtns; //0 = stage //1 = infinite

    private void Start()
    {
        stAndInBtns = stOrInPanel.GetComponentsInChildren<Button>();
    }

    private void Update()
    {
        
    }
}
