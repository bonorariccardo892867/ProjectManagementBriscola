using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameFramework.Core.Data{
    // Class to handle player data
    public class LobbyPlayerData
    {
        private string id;
        private string gamerTag;

        // Property to access the player ID
        public string Id => id;

        // Property to access the player's gamertag
        public string Gamertag => gamerTag;

        // Method to initialize player data
        public void Inizialize(string idPlayer, string gamerTagPlayer){
            id = idPlayer;
            gamerTag = gamerTagPlayer;
        }

        // Method to serialize player data into a dictionary
        public Dictionary<string, string> Serialize(){
            return new Dictionary<string, string>(){
                {"Id", id},
                {"GamerTag", gamerTag}
            };
        }

        // Method to initialize player data from a dictionary
        public void Inizialize(Dictionary<string, PlayerDataObject> playerData){
            UpdateState(playerData);
        }

        // Method to update player state based on player data
        public void UpdateState(Dictionary<string, PlayerDataObject> playerData){
            if(playerData.ContainsKey("Id"))
                id = playerData["Id"].Value;
            if(playerData.ContainsKey("GamerTag"))
                gamerTag = playerData["GamerTag"].Value;
        }
    }
}
