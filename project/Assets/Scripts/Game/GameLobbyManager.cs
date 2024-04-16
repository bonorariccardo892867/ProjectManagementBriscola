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
using UnityEngine.SceneManagement;

namespace Game{
    public class GameLobbyManager : Singleton<GameLobbyManager>{   
        private List<LobbyPlayerData> lobbyPlayerDatas = new List<LobbyPlayerData>();
        private LobbyPlayerData localLobbyPlayerData;
        private LobbyData lobbyData;
        private readonly int maxPlayers = 4;
        private bool inGame = false;

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

            bool succeeded = await LobbyManager.instance.CreateLobby(maxPlayers, localLobbyPlayerData.Serialize(), lobbyData.Serialize());
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
        private async void OnLobbyUpdated(Lobby lobby)
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

            if(lobbyData.RelayJoinCode != default && !inGame){
                await JoinRelayServer(lobbyData.RelayJoinCode);
                SceneManager.LoadSceneAsync("Game");
            }
        }

        // Method to get the list of lobby players
        public List<LobbyPlayerData> GetPlayers()
        {
            return lobbyPlayerDatas;
        }

        // Initiates the game start process. This method handles the allocation of necessary network resources for the game and updates lobby and player data accordingly
        public async Task StartGame()
        {
            string joinRelayCode = await RelayManager.instance.CreateRelay(maxPlayers);
            inGame = true;

            lobbyData.RelayJoinCode = joinRelayCode;
            await LobbyManager.instance.UpdateLobbyData(lobbyData.Serialize());

            string allocationId = RelayManager.instance.GetAllocationId();
            string connectionData = RelayManager.instance.GetConnectionData();
            await LobbyManager.instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize(), allocationId, connectionData);
        
            SceneManager.LoadSceneAsync("Game");
        }

        private async Task<bool> JoinRelayServer(string relayJoinCode)
        {
            inGame = true;
            string allocationId = RelayManager.instance.GetAllocationId();
            string connectionData = RelayManager.instance.GetConnectionData();
            await LobbyManager.instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize(), allocationId, connectionData);
            await RelayManager.instance.JoinRelay(relayJoinCode);
            return true;
        }

        public void GoBackToLobby()
        {
            inGame = false;
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}