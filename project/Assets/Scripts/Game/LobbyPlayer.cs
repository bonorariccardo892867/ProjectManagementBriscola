using System.Collections;
using System.Collections.Generic;
using GameFramework.Core.Data;
using TMPro;
using UnityEngine;

namespace Game{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        private LobbyPlayerData data;

        public void SetData(LobbyPlayerData playerData){
            data = playerData;
            playerName.text = data.Gamertag;
            gameObject.SetActive(true);
        }

        public void SetData(){
            gameObject.SetActive(false);
        }
    }
}
