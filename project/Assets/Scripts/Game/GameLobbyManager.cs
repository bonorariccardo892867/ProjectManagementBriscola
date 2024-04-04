using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GameFramework.Core;
using GameFramework.Core.Data;
using GameFramework.Core.GameFramework.Manager;
using GameFramework.Events;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

namespace Game{
    public class GameLobbyManager : Singleton<GameLobbyManager>{   
        private List<LobbyPlayerData> lobbyPlayerDatas = new List<LobbyPlayerData>();
        private LobbyPlayerData localLobbyPlayerData;

        private void OnEnable() {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable() {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        // Method to create the lobby by calling the CreateLobby method of LobbyManager.cs
        public async Task<bool> CreateLobby(){
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Inizialize(AuthenticationService.Instance.PlayerId, "HostPlayer", true);
            bool succeeded = await LobbyManager.instance.CreateLobby(4, playerData.Serialize());
            return succeeded;
        }

        // Method to get the lobby code
        public string GetLobbyCode(){
            return LobbyManager.instance.GetLobbyCode();
        }

        // Method to join a lobby with code
        public async Task<bool> JoinLobby(string code){
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Inizialize(AuthenticationService.Instance.PlayerId, "JoinPlayer", false);
            bool succeeded = await LobbyManager.instance.JoinLobby(code, playerData.Serialize());
            return succeeded;
        }

        // Event handler for lobby update events
        private void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.instance.GetPlayersData();
            lobbyPlayerDatas.Clear();

            foreach(Dictionary<string, PlayerDataObject> data in playerData){
                LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Inizialize(data);

                if(lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                    localLobbyPlayerData = lobbyPlayerData;

                lobbyPlayerDatas.Add(lobbyPlayerData);
            }
            Events.LobbyEvents.OnLobbyUpdated?.Invoke();
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return lobbyPlayerDatas;
        }

        public string GetHostId(){
            string hostId = "";
            for(int i=0; i<lobbyPlayerDatas.Count; i++){
                LobbyPlayerData playerData = lobbyPlayerDatas[i];
                if(playerData.Host == true){
                    hostId = playerData.Id;
                }
            }
            return hostId;
        }
    }
}