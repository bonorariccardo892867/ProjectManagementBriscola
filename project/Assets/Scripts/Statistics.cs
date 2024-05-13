using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public TextMeshProUGUI win;
    public TextMeshProUGUI draw;
    public TextMeshProUGUI lose;
    

    void Start()
    {
        if(PlayerPrefs.HasKey("win") && PlayerPrefs.HasKey("draw") && PlayerPrefs.HasKey("lose")){
            int w = PlayerPrefs.GetInt("win");
            int d = PlayerPrefs.GetInt("draw");
            int l = PlayerPrefs.GetInt("lose"); 
            win.text = w.ToString() + " WIN";
            draw.text = d.ToString() + " DRAW";
            lose.text = l.ToString() + " LOSE";
        }else{
            Debug.Log("Aggiunte");
            win.text = "0 WIN";
            draw.text = "0 DRAW";
            lose.text = "0 LOSE";
            PlayerPrefs.SetInt("win", 0);
            PlayerPrefs.SetInt("draw", 0);
            PlayerPrefs.SetInt("lose", 0);
            PlayerPrefs.Save();
        }
    }
}
