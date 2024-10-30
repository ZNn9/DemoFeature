using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

namespace Systems.Multiplayer.Manager
{
    public class RelayManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI joinCodeText;
        [SerializeField] private TMP_InputField joinCodeInputField;

        private void Start()
        {
            // await UnityServices.InitializeAsync();
            // if (!AuthenticationService.Instance.IsSignedIn)
            // {
            //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
            // }
        }
        public async void StartRelay()
        {

            string joinCode = await StartHostWithRelay();
            Debug.Log($"JoinCode: {joinCode}");
            // joinCodeText.text = joinCode;
        }
        public async void JoinRelay()
        {
            if (joinCodeInputField.text == "")
            {
                Debug.LogError("Join code is empty");
                return;
            }
            bool joined = await StartClientWithRelay(joinCodeInputField.text);
            if (joined)
            {
                joinCodeInputField.text = "";
            }
        }
        private async Task<string> StartHostWithRelay(int maxConnections = 4)
        {
            if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                await UnityServices.InitializeAsync();
            }
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return NetworkManager.Singleton.StartHost() ? joinCode : null;
        }

        private async Task<bool> StartClientWithRelay(string joinCode)
        {
            if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                await UnityServices.InitializeAsync();
            }
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
        }
    }

}
