using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameFramework.Core.Data{
    // Class to handle player data
    public class LobbyPlayerData
    {
        private string id;
        private string gamerTag;

        public string Id => id;
        public string Gamertag => gamerTag;

        public void Inizialize(string idPlayer, string gamerTagPlayer){
            id = idPlayer;
            gamerTag = gamerTagPlayer;
        }

        public Dictionary<string, string> Serialize(){
            return new Dictionary<string, string>(){
                {"Id", id},
                {"GamerTag", gamerTag}
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
        }
    }
}
