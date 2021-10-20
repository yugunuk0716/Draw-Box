using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelScript : MonoBehaviour
{
    private CanvasGroup canvasGroup; //�гο� �ٿ��� ĵ�����׷�
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>(); //ĵ���� �׷��� ������
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); //������� �������
        }
        //ĵ�����׷� �ʱ�ȭ
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void Open()
    {
        SetAlpha(true);
    }
    public virtual void Close()
    {
        SetAlpha(false);
    }

    public virtual void SetAlpha(bool on) //open�� close�� �� ������ �޾� ���ĸ� �ٲ���
    {
        DOTween.To(() => canvasGroup.alpha, value => canvasGroup.alpha = value, on ? 1f : 0f, 0.8f).OnComplete(() => {
            canvasGroup.interactable = on;
            canvasGroup.blocksRaycasts = on;
        });
    }

    
}