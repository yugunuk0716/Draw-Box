using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : PanelScript
{
    public Button loginButton;
    public Button closeButton;

    public InputField idInput;
    public InputField passInput;
    protected override void Awake()
    {
        base.Awake();

        closeButton.onClick.AddListener(() =>
        {
            PopupManager.instance.ClosePopup();
        });
        loginButton.onClick.AddListener(() =>
        {
            LoginVO vo = new LoginVO(idInput.text, passInput.text);
            string payload = JsonUtility.ToJson(vo);
            //로그인 VO를 만들어서 해당 내용을 전달한다
            NetworkManager.instance.SendPostRequest("login", payload, result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                //print(vo.result);

                if (vo.result)
                {
                    //회원가입창도 같이 닫히게
                    PopupManager.instance.OpenPopup("alert", "로그인 성공", 2);
                    //print(vo.payload);
                    NetworkManager.instance.SetToken(vo.payload); //토큰 저장

                }
                else
                {
                    //false라면 얼럿만 닫히게
                    PopupManager.instance.OpenPopup("alert", vo.payload);
                }
            });
        });
    }
}
