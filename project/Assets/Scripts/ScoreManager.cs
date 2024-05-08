using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public GameObject HostScore;
	public GameObject ClientScore;
    public int hostCount=0;
	public int clientCount=0;
	private Text txt;
	
	public void OnStartClient(){
        
        HostScore = GameObject.Find("HostScore");
        ClientScore = GameObject.Find("ClientScore");
		hostCount=0;
		clientCount=0;
    }
	
	public void hostUpdate(int n){
		hostCount+=n;
		txt = HostScore.GetComponent<UnityEngine.UI.Text>();
		txt.text = hostCount.ToString();
	}
	public void clientUpdate(int n){
		clientCount+=n;
		txt = ClientScore.GetComponent<UnityEngine.UI.Text>();
		txt.text = clientCount.ToString();
	}
}
