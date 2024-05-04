using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class TurnCounter : NetworkBehaviour
{
    // Method to update the turn counter text
    public void SetTurn(string player){
        transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Turn " + player;
    }
}
