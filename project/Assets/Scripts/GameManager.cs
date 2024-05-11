using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour{

    // String to keep track of turns played
    public string turnsPlayed;
    
    // Index to keep track of the current turn state
    private int index = 0;

    // SyncVars to store player names
    [SyncVar]
    public string hostName = "Host";

    [SyncVar]
    public string clientName = "Client";

    // Method to set player names
    [ClientRpc]
    public void RpcSetName(){
        if(PlayerPrefs.HasKey("user_name")){
            if(isServer){
                CmdSetHostName(PlayerPrefs.GetString("user_name"));
            }else{
                CmdSetClientName(PlayerPrefs.GetString("user_name"));
            }
        }
    }

    // Command to set client name
    [Command(requiresAuthority = false)]
    private void CmdSetClientName(string name){
        clientName = name;
    }

    // Command to set host name
    [Command(requiresAuthority = false)]
    private void CmdSetHostName(string name){
        hostName = name;
    }

    // Method to update the turns played
    public void UpdateTurnsPlayed(string player, bool empty, bool isWinner = false){
        switch (index)
        {
            case 0:
                turnsPlayed = (turnsPlayed == "P1") ? "P2" : "P1";
                ++index;
            break;
            case 1:
                turnsPlayed = "P";
                ++index;
            break;
            case 2:
                turnsPlayed = (player == "P1" && isWinner) || (player == "P2" && !isWinner) ? "P1" : "P2";
                index = 0;
            break;
        }
        ScoreManager sm = GameObject.Find("ScoreBoard(Clone)").GetComponent<ScoreManager>();
        sm.SetGreenText(turnsPlayed);
        if(empty) 
            sm.UpdateEnd();
    }
}
