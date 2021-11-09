using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    #region CamEffect
    public CinemachineVirtualCamera vCam;

    CinemachineBasicMultiChannelPerlin cmPerlin;
    Tween camTween = null;
    #endregion

    #region BoxDieEffect
    public GameObject imageBase;
    private Sprite successImage;
    private Sprite failImage;

    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 캠이펙트 매니저가 실행중입니다");
        }
        instance = this;

        successImage = Resources.Load<Sprite>("success");
        failImage = Resources.Load<Sprite>("fail");

    }
    private void Start()
    {
        cmPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void SetCamShake(float duration, float power = 0.5f)
    {
        if (camTween != null && camTween.IsActive())
        {
            camTween.Kill();
        }

        cmPerlin.m_AmplitudeGain = power;
        camTween = DOTween.To(() => cmPerlin.m_AmplitudeGain, value => cmPerlin.m_AmplitudeGain = value, 0, duration);
    }


    public void BoxDieEffect(bool result, Vector2 position) 
    {
        if (result) 
        {
            imageBase.GetComponent<SpriteRenderer>().sprite = successImage;
        }
        else
        {
            imageBase.GetComponent<SpriteRenderer>().sprite = failImage;
        }
        imageBase.transform.position = position;
        imageBase.SetActive(true);
        Invoke(nameof(SetImageFalse), 2f);
    }

    private void SetImageFalse()
    {
        imageBase.SetActive(false);
    }

}
