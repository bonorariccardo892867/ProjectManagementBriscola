using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.Core.Data;
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

        // Property that returns the ID of the lobby
        public string Id => lobby.Id;

        // Property that returns the number of players in the lobby
        public int NumberOfPlayers => lobby.Players.Count;

        // method for lobby creation
        public async Task<bool> CreateLobby(int maxPlayers, Dictionary<string, string> data, Dictionary<string, string> lobbyData, bool isPrivate=true){

            // player data
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

            // lobby options
            CreateLobbyOptions options = new CreateLobbyOptions(){
                Data = SerializeLobbyData(lobbyData),
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

        // Method to serialize player data.
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

        // Method to serialize lobby data.
        private Dictionary<string, DataObject> SerializeLobbyData(Dictionary<string, string> data)
        {
            Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
            foreach (var(key, value) in data){
                lobbyData.Add(key, new DataObject(
                    visibility: DataObject.VisibilityOptions.Member,
                    value: value));
            }
            return lobbyData;
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

        // Method to get player data in the lobby
        public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
        {
            List<Dictionary<string, PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();

            foreach(Player player in lobby.Players){
                data.Add(player.Data);
            }
            return data;
        }

        // Method to update playerData
        public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> data, string allocationId = default, string connectionData = default){
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
            UpdatePlayerOptions options = new UpdatePlayerOptions(){
                Data = playerData,
                AllocationId = allocationId,
                ConnectionInfo = connectionData
            };
            try{
                lobby = await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, id, options);
            }catch(System.Exception){
                return false;
            }

            LobbyEvents.OnLobbyUpdated(lobby);

            return true;
        }

        // Method to update lobbyData
        public async Task<bool> UpdateLobbyData(Dictionary<string, string> data){
            Dictionary<string, DataObject> lobbyData = SerializeLobbyData(data);
            UpdateLobbyOptions options = new UpdateLobbyOptions(){
                Data = lobbyData
            };
            try{
                lobby = await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, options);
            }catch(System.Exception){
                return false;
            }

            LobbyEvents.OnLobbyUpdated(lobby);

            return true;
        }

        // Method to get the ID of the lobby host
        public string GetHostId()
        {
            return lobby.HostId;
        }

        // Method to delete the lobby when the application is quitting
        public void OnApplicationQuit() {
            Disconnection();
        }

        public void Disconnection()
        {
            if (lobby != null)
            {
                if (lobby.HostId == AuthenticationService.Instance.PlayerId)
                {
                    StopCoroutine(heartbeatCoroutine);
                    StopCoroutine(refreshLobbyCoroutine);
                    LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
                }
                else
                {
                    StopCoroutine(refreshLobbyCoroutine);
                    LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
                }
            }
        }
    }
}