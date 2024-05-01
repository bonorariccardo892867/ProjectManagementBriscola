using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class DrawCards : NetworkBehaviour
{
    public PlayerManager playerManager;
    public GameObject PlayerArea;
    // Start is called before the first frame update
    public void OnCLick(){
        PlayerArea = GameObject.Find("PlayerArea");
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();

        int itemCount = PlayerArea.GetComponent<GridLayoutGroup>().transform.childCount;
        if(itemCount == 0){
            playerManager.CmdDealCards(3);
        } else if(itemCount == 2){
            playerManager.CmdDealCards(1);
        }
    }
}