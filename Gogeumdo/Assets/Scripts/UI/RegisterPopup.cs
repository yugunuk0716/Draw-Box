using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class RegisterPopup : PanelScript
{
    public Button registerBtn;
    public Button closeBtn;

    public InputField nameInput;
    public InputField idInput;
    public InputField passInput;
    public InputField passConfirmInput;

    protected override void Awake()
    {
        base.Awake();
        registerBtn.onClick.AddListener(() =>
        {
            {
                Regex reg = new Regex(@"^[°¡-ÆRa-zA-Z]{2,8}$");
                if (!reg.IsMatch(nameInput.text))
                {
                    Debug.Log("ÀÌ¸§Àº ¹Ýµå½Ã ÇÑ±Û ¶Ç´Â ¿µ¹®À¸·Î 2-8±ÛÀÚ¿©¾ß ÇÕ´Ï´Ù");
                    PopupManager.instance.OpenPopup("alert", "ÀÌ¸§Àº ¹Ýµå½Ã ÇÑ±Û ¶Ç´Â ¿µ¹®À¸·Î 2-3±ÛÀÚ¿©¾ß ÇÕ´Ï´Ù");
                    return;
                }
                reg = new Regex(@"^[a-zA-z0-9]{8}$");
            }

            RegisterVO vo = new RegisterVO(nameInput.text, idInput.text, passInput.text);
            string json = JsonUtility.ToJson(vo);

            NetworkManager.instance.SendPostRequest("register", json, result =>
            {
                Debug.Log(result);
                ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);

                if (vo.result)
                {
                    //È¸¿ø°¡ÀÔÃ¢µµ °°ÀÌ ´ÝÈ÷°Ô
                    PopupManager.instance.OpenPopup("alert", vo.payload, 2);
                }
                else
                {
                    //false¶ó¸é ¾ó·µ¸¸ ´ÝÈ÷°Ô
                    PopupManager.instance.OpenPopup("alert", vo.payload);
                }
            });
        });
        closeBtn.onClick.AddListener(() => { PopupManager.instance.ClosePopup(); });
    }
}
