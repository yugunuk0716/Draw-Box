using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageManager : MonoBehaviour
{
    public Transform btnParent;
    private Button[] stageButton;

    private void Start()
    {
        stageButton = btnParent.GetComponentsInChildren<Button>();

        for (int i = 0; i < stageButton.Length; i++)
        {
            int idx = i;
            stageButton[idx].onClick.AddListener(() =>
            {
                GameManager.instance.stageIndex = idx;
                LoadManager.LoadScene("InGame");
            });
        }
    }
}
