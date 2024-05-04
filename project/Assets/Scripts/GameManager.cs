using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour{
    public string turnsPlayed;
    private int index = 0;

    public void UpdateTurnsPlayed(string player = "", bool isWinner = false){
        TurnCounter turnCounter = GameObject.Find("TurnDisplay(Clone)").GetComponent<TurnCounter>();
        switch (index)
        {
            case 0:
                if(turnsPlayed == "P1")
                    turnsPlayed = "P2";
                else
                    turnsPlayed = "P1";
                
                if(turnsPlayed == "P1")
                    turnCounter.SetTurn("Host");
                else
                    turnCounter.SetTurn("Client");
                ++index;
            break;
            case 1:
                turnCounter.SetTurn("");
                turnsPlayed = "P";
                ++index;
            break;
            case 2:
                turnsPlayed = (player == "P1" && isWinner) || (player == "P2" && !isWinner) ? "P1" : "P2";
                if(turnsPlayed == "P1")
                    turnCounter.SetTurn("Host");
                else
                    turnCounter.SetTurn("Client");
                index = 0;
            break;
        }
    }
}
