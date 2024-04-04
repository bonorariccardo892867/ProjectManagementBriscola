using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

namespace Game{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        // Start is called before the first frame update
        void Start()
        {
            // Add the lobby code
            lobbyCodeText.text = GameLobbyManager.instance.GetLobbyCode();
        }
    }
}
