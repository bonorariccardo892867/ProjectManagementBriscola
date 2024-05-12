using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartAndLeave : NetworkBehaviour
{
    // Method called when the start button is clicked
    public void StartButton(){
        // Check if there are two players connected
        if(NetworkServer.connections.Count == 2){
            gameObject.SetActive(false);
            GameObject.Find("PCounter").SetActive(false);
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();

            // Call RpcSetName() on GameManager to set player names
            GameObject.Find("GameManager").GetComponent<GameManager>().RpcSetName();

            playerManager.RpcRemoveLeaveButton();

            // Shuffle the three times when the server starts
            for(int i=0; i<3; i++)
                playerManager.Shuffle();

            // Deal cards using the PlayerManager
            playerManager.DealCards(6);
            playerManager.SetScene(transform.position);
        }
    }

    // Method called when the leave button is clicked
    public void LeaveButton(){
        NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            // If hosting, stop hosting
            networkManager.StopHost();
        }
        if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            // If client, stop client
            networkManager.StopClient();
        }
    }
}