using System.Collections;
using System.Collections.Generic;
using Game;
using GameFramework.Core.GameFramework.Manager;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        // Start is called before the first frame update
        void Start()
        {
            // Add the lobby code
            lobbyCodeText.text = GameLobbyManager.instance.GetLobbyCode();
            /*
            backButton.onClick.AddListener(OnBackClick);
            */
        }

        /*
        private async void OnBackClick(){
            if(AuthenticationService.Instance.PlayerId == GameLobbyManager.instance.GetHostId()){
                SceneManager.LoadScene("MainMenu");
                await LobbyService.Instance.DeleteLobbyAsync(LobbyManager.instance.Id); 
            }else{
                SceneManager.LoadScene("MainMenu");
                await LobbyService.Instance.RemovePlayerAsync(LobbyManager.instance.Id, AuthenticationService.Instance.PlayerId);
            }
        }
        */
    }
}
