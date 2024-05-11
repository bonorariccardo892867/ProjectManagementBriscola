using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
	public TextMeshProUGUI hostScore;
	public TextMeshProUGUI clientScore;
	public TextMeshProUGUI hostName;
	public TextMeshProUGUI clientName;
	private GameManager gm;
	
    private int hostCount=0;
	private int clientCount=0;
	private bool end;
	
	// Method to set up the scoreboard
	public void SetScoreBoard(string player)
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hostName.text = (player == "P1") ? "YOU" : gm.clientName;
		clientName.text = (player == "P2") ? "YOU" : gm.clientName;
    }

	// Method to set the text color for the current turn player
	public void SetGreenText(string player){
		if(player == "P1"){
			hostName.color = HexToColor("#18781C");
			hostName.fontStyle = FontStyles.Bold;
			hostName.fontStyle |= FontStyles.Underline;
			clientName.color = Color.black;
			clientName.fontStyle = FontStyles.Bold;
		}else if(player == "P2"){
			clientName.color = HexToColor("#18781C");
			clientName.fontStyle = FontStyles.Bold;
			clientName.fontStyle |= FontStyles.Underline;
			hostName.color = Color.black;
			hostName.fontStyle = FontStyles.Bold;
		}else if(player == "P"){
			hostName.color = Color.black;
			hostName.fontStyle = FontStyles.Bold;
			clientName.color = Color.black;
			clientName.fontStyle = FontStyles.Bold;
		}
	}

	// Convert hex color string to Color
	private Color HexToColor(string hex)
    {
        Color color = Color.white; 
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

	// Method to update host player score
	public void HostUpdate(int n){
		hostCount+=n;
		hostScore.text = hostCount.ToString();
		CheckEnd();
	}

	// Method to update client player score
	public void ClientUpdate(int n){
		clientCount+=n;
		clientScore.text = clientCount.ToString();
		CheckEnd();
	}

	// Method to check if the game is ended
	private void CheckEnd(){
		if(hostCount + clientCount == 120 && end){
			MenuScript menu = GameObject.Find("NetworkManager").GetComponent<MenuScript>();
			if(hostCount > clientCount)
				menu.FinalResults(hostName.text, hostScore.text + " - " + clientScore.text);
			else if(hostCount < clientCount)
				menu.FinalResults(clientName.text, clientScore.text + " - " + hostScore.text);
			else
				menu.FinalResults("", hostScore.text + " - " + clientScore.text); 
		}
	}

	// Method to update the end variable
	public void UpdateEnd(){
		end = true;
	}
}
