using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GameFramework.Core.GameFramework.Manager;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private void OnEnable() {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;   

        NetworkManager.Singleton.LogLevel = LogLevel.Developer;
        NetworkManager.Singleton.NetworkConfig.EnableNetworkLogs = true;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if(NetworkManager.Singleton.LocalClientId == clientId){
            LobbyManager.instance.Disconnection();
            NetworkManager.Singleton.Shutdown();
        }
    }

    private void OnClientConnected(ulong obj)
    {
        Debug.Log($"Player connected: {obj}");
    }

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
        if(RelayManager.instance.IsHost){
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;
            (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) = RelayManager.instance.GetHostConnectionInfo();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ip, (ushort)port, allocationId, key, connectionData, true);
            NetworkManager.Singleton.StartHost();
        }else{
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = RelayManager.instance.GetClientConnectionInfo();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
            NetworkManager.Singleton.StartClient();
        }
    }

    private void Update(){
        if(NetworkManager.Singleton.ShutdownInProgress){
            GameLobbyManager.instance.GoBackToLobby();
        }
    }

    private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;
    }
}
