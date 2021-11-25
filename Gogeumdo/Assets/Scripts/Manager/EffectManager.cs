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
    public GameObject fullImageBase;
    private Sprite successImage;
    private Sprite failImage;
    private Sprite bloodScreenImage;

    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 캠이펙트 매니저가 실행중입니다");
        }
        instance = this;

        //동적 로딩
        successImage = Resources.Load<Sprite>("success");
        failImage = Resources.Load<Sprite>("fail");
        bloodScreenImage = Resources.Load<Sprite>("blood");
        

    }
    private void Start()
    {
        //시네머신 컴포넌트 가져오기
        cmPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void SetCamShake(float duration, float power = 0.5f) //캐메라 흔드는 함수
    {
        if (camTween != null && camTween.IsActive())
        {
            camTween.Kill();
        }

        cmPerlin.m_AmplitudeGain = power;
        camTween = DOTween.To(() => cmPerlin.m_AmplitudeGain, value => cmPerlin.m_AmplitudeGain = value, 0, duration);
    }


    public void BoxDieEffect(bool result, Vector2 position, float duration = 1.5f) //박스 사망시 이펙트
    {
        if (result) //박스가 분류된 것이라면
        {
            imageBase.GetComponent<SpriteRenderer>().sprite = successImage;
        }
        else // 박스가 장애물에 맞은거라면
        {
            imageBase.GetComponent<SpriteRenderer>().sprite = failImage;
            fullImageBase.GetComponent<SpriteRenderer>().sprite = bloodScreenImage;
            fullImageBase.SetActive(true);
        }
        imageBase.transform.position = position;
        imageBase.SetActive(true);
        Invoke(nameof(SetImageFalse), duration);
    }

    private void SetImageFalse()// 일정 시간 후에 sprite가 들어있는 게임 오브젝트를 비활성화
    {
        imageBase.SetActive(false);
        fullImageBase.SetActive(false);
    }

}
