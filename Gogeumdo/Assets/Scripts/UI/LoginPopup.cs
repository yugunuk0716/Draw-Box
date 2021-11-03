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
            //�α��� VO�� ���� �ش� ������ �����Ѵ�
            NetworkManager.instance.SendPostRequest("login", payload, result =>
            {
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
                //print(vo.result);

                if (vo.result)
                {
                    //ȸ������â�� ���� ������
                    PopupManager.instance.OpenPopup("alert", "�α��� ����", 2);
                    //print(vo.payload);
                    NetworkManager.instance.SetToken(vo.payload); //��ū ����

                }
                else
                {
                    //false��� �󷵸� ������
                    PopupManager.instance.OpenPopup("alert", vo.payload);
                }
            });
        });
    }
}
