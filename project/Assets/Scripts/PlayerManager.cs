using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PlayerManager : NetworkBehaviour {
    public GameObject card1;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;

    List <GameObject> cards = new List<GameObject>();

    public override void OnStartClient(){
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("OtherArea");
        DropZone = GameObject.Find("DropZone");
    }
    [Server]
    public override void OnStartServer(){
        cards.Add(card1);
    }
    [Command]
    public void CmdDealCards(){
        for (int i=0;i<5;i++){
            GameObject card = Instantiate(cards[Random.Range(0,cards.Count)],new Vector2(0,0),Quaternion.identity);
            NetworkServer.Spawn(card,connectionToClient);
            RpcShowCard(card,"dealt");
        }
    }
    [ClientRpc]
    private void RpcShowCard(GameObject card, string type){
        if (type == "dealt"){
            card.transform.SetParent(PlayerArea.transform,false);
        }else if (type == "played"){
            card.transform.SetParent(EnemyArea.transform,false);
        }
    }
}
