using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour{
    public string turnsPlayed;
    public void UpdateTurnsPlayed(){
        if (turnsPlayed == "P1")
            turnsPlayed = "P2";
        else
            turnsPlayed = "P1";
    }
}
