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
        private LobbyData lobbyData;

        // Property that returns if the player is the host
        public bool IsHost => localLobbyPlayerData.Id == LobbyManager.instance.GetHostId(); 


        private void OnEnable() {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable() {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        // Method to create the lobby by calling the CreateLobby method of LobbyManager.cs
        public async Task<bool> CreateLobby(){
            localLobbyPlayerData = new LobbyPlayerData();
            localLobbyPlayerData.Inizialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
            lobbyData = new LobbyData();
            lobbyData.Inizialize(); // Update when the LobbyData class is updated

            bool succeeded = await LobbyManager.instance.CreateLobby(4, localLobbyPlayerData.Serialize(), lobbyData.Serialize());
            return succeeded;
        }

        // Method to get the lobby code
        public string GetLobbyCode(){
            return LobbyManager.instance.GetLobbyCode();
        }

        // Method to join a lobby with code
        public async Task<bool> JoinLobby(string code){
            localLobbyPlayerData = new LobbyPlayerData();
            localLobbyPlayerData.Inizialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");
            bool succeeded = await LobbyManager.instance.JoinLobby(code, localLobbyPlayerData.Serialize());
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

            lobbyData = new LobbyData();
            lobbyData.Inizialize(lobby.Data);

            Events.LobbyEvents.OnLobbyUpdated?.Invoke();
        }

        // Method to get the list of lobby players
        public List<LobbyPlayerData> GetPlayers()
        {
            return lobbyPlayerDatas;
        }
    }
}