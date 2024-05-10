using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : MonoBehaviour
{
    public Text obj_text;
    public InputField display;

    void Start()
    {
        if(PlayerPrefs.HasKey("user_name") && PlayerPrefs.GetString("user_name") != "")
            obj_text.text = PlayerPrefs.GetString("user_name");
        else{
            obj_text.text = "Guest";
            PlayerPrefs.SetString("user_name", obj_text.text);
            PlayerPrefs.Save();
        }
    }

    public void Create()
    {
        if(!string.IsNullOrWhiteSpace(display.text)){
            obj_text.text = display.text;
            PlayerPrefs.SetString("user_name", obj_text.text);
            PlayerPrefs.Save();
        }
    }
}
