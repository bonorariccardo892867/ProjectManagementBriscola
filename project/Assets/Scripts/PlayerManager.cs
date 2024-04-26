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
    public void PlayCard(GameObject card){
        CmdPlayCard(card);
    }
    [Command]
    void CmdPlayCard(GameObject card){
        RpcShowCard(card,"played");
    }
    [ClientRpc]
    private void RpcShowCard(GameObject card, string type){
        if (type == "dealt"){
            if (isOwned){
                card.transform.SetParent(PlayerArea.transform,false);
            }else{
                card.transform.SetParent(EnemyArea.transform,false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }else if (type == "played"){
           card.transform.SetParent(DropZone.transform,false);
        }
    }


    [Command]
    public void CmdTargetSelfCard()
    {
        TargetSelfCard();
    }

    [Command]
    public void CmdTargetOtherCard(GameObject target) {
        NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
        TargetOtherCard(opponentIdentity.connectionToClient);
    }

    [TargetRpc]

    private void TargetSelfCard()
    {
        Debug.Log("target self Card");
    } 

    [TargetRpc]

    private void TargetOtherCard(NetworkConnection target)
    {
        Debug.Log("targeted other card");
    }
}
