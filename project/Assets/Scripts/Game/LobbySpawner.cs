using System;
using System.Collections;
using System.Collections.Generic;
using Game.Events;
using GameFramework.Core.Data;
using UnityEngine;

namespace Game{
    public class LobbySpawner : MonoBehaviour
    {
         [SerializeField] private List<LobbyPlayer> players;

        private void OnEnable(){
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable() {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerDatas = GameLobbyManager.instance.GetPlayers();
            for(int i=0; i<4; i++){
                if(i<playerDatas.Count){
                    LobbyPlayerData data = playerDatas[i];
                    players[i].SetData(data);
                }else{
                    players[i].SetData();
                }
            }
        }
    }
}

