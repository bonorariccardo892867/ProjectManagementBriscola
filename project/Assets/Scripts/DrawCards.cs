using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DrawCards : NetworkBehaviour
{
    public PlayerManager playerManager;

    // Method called when the button is clicked
    public void OnCLick(){
        gameObject.SetActive(false);
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();

        // Shuffle the deck twice when the server starts
        for(int i=0; i<5; i++)
            playerManager.Shuffle();

        // Deal cards using the PlayerManager
        playerManager.DealCards(6);
        playerManager.SetBriscola(transform.position);
    }
}