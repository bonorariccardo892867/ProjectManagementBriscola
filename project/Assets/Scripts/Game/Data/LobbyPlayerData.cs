using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameFramework.Core.Data{
    // Class to handle player data
    public class LobbyPlayerData
    {
        private string id;
        private string gamerTag;
        private bool host;

        public string Id => id;
        public string Gamertag => gamerTag;
        public bool Host => host;

        public void Inizialize(string idPlayer, string gamerTagPlayer, bool isHost){
            id = idPlayer;
            gamerTag = gamerTagPlayer;
            host = isHost;
        }

        public Dictionary<string, string> Serialize(){
            return new Dictionary<string, string>(){
                {"Id", id},
                {"GamerTag", gamerTag},
                {"Host", host.ToString()}
            };
        }

        public void Inizialize(Dictionary<string, PlayerDataObject> playerData){
            UpdateState(playerData);
        }

        public void UpdateState(Dictionary<string, PlayerDataObject> playerData){
            if(playerData.ContainsKey("Id"))
                id = playerData["Id"].Value;
            if(playerData.ContainsKey("GamerTag"))
                gamerTag = playerData["GamerTag"].Value;
            if(playerData.ContainsKey("Host"))
                host = playerData["Host"].Value == "True";
        }
    }
}
