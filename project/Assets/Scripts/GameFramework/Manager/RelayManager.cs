using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace GameFramework.Core.GameFramework.Manager{
    public class RelayManager : Singleton<RelayManager>
    {
        private string joinCode;
        private string ip;
        private int port;
        private byte[] connectionData;
        private System.Guid allocationId;

        // Creates a relay for network communication with a maximum number of connections
        public async Task<string> CreateRelay(int maxConnection){
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            ip = dtlsEnpoint.Host;
            port = dtlsEnpoint.Port;

            allocationId = allocation.AllocationId;
            connectionData = allocation.ConnectionData;
            
            return joinCode;
        }

        // Joins an existing relay using a join code
        public async Task<bool> JoinRelay(string _joinCode){
            joinCode = _joinCode;
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);

            RelayServerEndpoint dtlsEnpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            ip = dtlsEnpoint.Host;
            port = dtlsEnpoint.Port;

            allocationId = allocation.AllocationId;
            connectionData = allocation.ConnectionData;

            return true;
        }

        // Retrieves the allocation ID of the current relay allocation
        public string GetAllocationId()
        {
            return allocationId.ToString();
        }

        // Retrieves the connection data of the current relay allocation
        public string GetConnectionData()
        {
            return connectionData.ToString();
        }
    }
}
