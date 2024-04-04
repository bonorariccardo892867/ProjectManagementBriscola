using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.Events;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameFramework.Core.GameFramework.Manager{
    public class LobbyManager: Singleton<LobbyManager>{

        private Lobby lobby;
        private Coroutine heartbeatCoroutine;
        private Coroutine refreshLobbyCoroutine;

        public string Id => lobby.Id;

        // method for lobby creation
        public async Task<bool> CreateLobby(int maxPlayers, Dictionary<string, string> data, bool isPrivate=true){

            // player data
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

            // lobby options
            CreateLobbyOptions options = new CreateLobbyOptions(){
                IsPrivate = isPrivate,
                Player = player
            };  

            try{
                lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", maxPlayers, options);
            }catch(System.Exception){
                return false;
            }

            heartbeatCoroutine = StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 6f));
            refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));
            return true;
        }

        // Coroutine for sending periodic heartbeat pings to the lobby
        private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
        {
            while(true){
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return new WaitForSecondsRealtime(waitTimeSeconds);
            }
        }

        // Coroutine for periodically refreshing lobby information
        private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTimeSeconds)
        {
            while(true){
                Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
                yield return new WaitUntil(() => task.IsCompleted); 
                Lobby newLobby = task.Result;
                if(newLobby.LastUpdated > lobby.LastUpdated){
                    lobby = newLobby;
                    LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
                }
                yield return new WaitForSecondsRealtime(waitTimeSeconds);
            }
        }

        private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
            foreach (var(key, value) in data){
                playerData.Add(key, new PlayerDataObject(
                    visibility: PlayerDataObject.VisibilityOptions.Member,
                    value: value));
            }
            return playerData;
        }

        // Method to delete the lobby when the application is quitting
        public void OnApplicationQuit() {
            if(lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
                LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
        }

        // Method to get the lobby code
        internal string GetLobbyCode()
        {
            return lobby?.LobbyCode;
        }

        // Method to join a lobby with code
        public async Task<bool> JoinLobby(string code, Dictionary<string, string> data)
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(data));

            options.Player = player;
            try{
                lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
            }catch(System.Exception){
                return false;
            }

            refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));
            return true;
        }

        public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
        {
            List<Dictionary<string, PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();

            foreach(Player player in lobby.Players){
                data.Add(player.Data);
            }
            return data;
        }
    }
}