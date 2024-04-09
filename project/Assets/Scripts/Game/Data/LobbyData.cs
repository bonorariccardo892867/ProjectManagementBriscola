using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace GameFramework.Core.Data{
    // Future class for LobbyData
    public class LobbyData
    {
        // Method to initialize lobby data
        public void Inizialize(){

        }

        // Method to initialize lobby data from a dictionary
        public void Inizialize(Dictionary<string, DataObject> lobbyData){
            UpdateState(lobbyData);
        }

        // Method to update player state based on player data
        public void UpdateState(Dictionary<string, DataObject> lobbyData){
            
        }

        // Method to serialize lobby data into a dictionary
        public Dictionary<string, string> Serialize(){
            return new Dictionary<string, string>(){

            };
        }
    }
}
