using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System;
using Systems.Account.Enum;
using Systems.Account.Model;
using Systems.SaveLoad.Manager;
using Systems.Account.Manager;
using Systems.SaveLoad.Service;

// Chưa sử lý liên kết
// Chưa sử lý đồng bộ sau khi liên kết
namespace Systems.Account.Service
{
    public class AccountService
    {
        // Action
        public void StartGame()
        {
            SetAttributeAccount();
            // Check Version data player could
        }
        // SignIn ZONE

        // Logout ZONE
        public void LogoutAccount()
        {
            //Not really
            if (SignInResult.AccountType == AccountType.NotSignIn) return;
            PlayerDataManager.Instance.hero = null;
            if (SignInResult.AccountType == AccountType.Player)
                AuthenticationService.Instance.SignOut();
            SignInResult.AccountType = AccountType.NotSignIn;
            SignInResult.IdPlayer = string.Empty;

        }
        // Delete ZONE
        public void DeleteAccount()
        {
            if (FindPlayerId() == string.Empty) return;
            SignInResult.AccountType = AccountType.NotSignIn;
            SignInResult.IdPlayer = string.Empty;
            if (FindPlayerId() == "AnonymousAccount")
            {
                SaveLoadManager.Instance.saveLoadLocalService.DeleteFolder("AnonymousAccount");
                return;
            }
            // Not done
            SaveLoadManager.Instance.saveLoadLocalService.DeleteFolder(FindPlayerId());
            //Delete account in cloud (Not done)
        }
        // Find Zone
        public string FindPlayerId(bool isManagerByFolder = true)
        {
            if (AuthenticationService.Instance.IsSignedIn)
                return AuthenticationService.Instance.PlayerId;
            if(isManagerByFolder == true && SaveLoadManager.Instance.saveLoadLocalService.CheckAnonymousFolderExists())
                return "AnonymousAccount";
            if (isManagerByFolder == false && SaveLoadManager.Instance.saveLoadLocalService.CheckAnonymousFileExists())
                return "AnonymousAccount";
            return string.Empty;
        }
        // Check Zone
        private AccountType CheckAccountType(bool isManagerByFolder = true)
        {
            if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                return AccountType.Player;
            if (isManagerByFolder == true && SaveLoadManager.Instance.saveLoadLocalService.CheckAnonymousFolderExists())
                return AccountType.Anonymous;
            if (isManagerByFolder == false && SaveLoadManager.Instance.saveLoadLocalService.CheckAnonymousFileExists())
                return AccountType.Anonymous;
            return AccountType.NotSignIn;
        }
        // Set ZONE
        public void SetAttributeAccount(bool isManagerByFolder = true)
        {
            SignInResult.AccountType = CheckAccountType(isManagerByFolder);
            SignInResult.IdPlayer = FindPlayerId();
        }
    }
}



