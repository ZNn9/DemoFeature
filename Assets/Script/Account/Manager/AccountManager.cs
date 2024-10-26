using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Security.Authentication;
using Systems.SaveLoad.Model;
using Systems.SaveLoad.Manager;
using Systems.Account.Service;
using Systems.Account.Enum;
using Systems.Account.Model;

namespace Systems.Account.Manager
{
    // Chưa sử lý liên kết
    // Chưa sử lý đồng bộ sau khi liên kết
    public class AccountManager : PersistentSingleton<AccountManager>
    {
        public AccountService accountService = new AccountService();
        public AnonymousAccount anonymousAccount = new AnonymousAccount();
        public bool IsModified = false;
        public event Action OnPlayerLogin;
        public event Action OnPlayerLoggedOut;
        public event Action OnPlayerDeleteAccount;
        protected override void Awake()
        {
            base.Awake();
        }
        public void Start()
        {
            SaveLoadManager.Instance.saveLoadLocalService.trackableService.AddChange("account");
            accountService.StartGame();
            if (!(SignInResult.AccountType == AccountType.NotSignIn))
                OnPlayerLogin?.Invoke();
            // Action OnPlayerLogin ? <skip login feature UI>
        }
        public async Task LoginAccountAnonymous()
        {
            await anonymousAccount.SignInAnonymous();
            OnPlayerLogin?.Invoke();
        }
        public void LogoutAccount()
        {
            accountService.LogoutAccount();
            OnPlayerLoggedOut?.Invoke();
        }
        public void DeleteAccount()
        {
            accountService.DeleteAccount();
            OnPlayerDeleteAccount?.Invoke();
        }
        async void SetupEvents()
        {
            if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                try
                {
                    await UnityServices.InitializeAsync();
                    Debug.Log("Unity Services initialized successfully.");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
                    return;  // Avoid further execution if initialization fails
                }
            }
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Sign in anonymously succeeded!");
                // Shows how to get a playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            };

            AuthenticationService.Instance.SignInFailed += (err) => { Debug.LogError(err); };

            AuthenticationService.Instance.SignedOut += () => { Debug.Log("Player signed out."); };

            AuthenticationService.Instance.Expired += () => { Debug.Log("Player session could not be refreshed and expired."); };
        }
    }
}
