using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Systems.Account.Enum;
using Systems.Account.Manager;
using Systems.Account.Model;
using Systems.SaveLoad.Manager;
using Systems.SaveLoad.Model;
using Systems.SaveLoad.Service;
using Unity.VisualScripting;
using UnityEngine;
namespace Systems.SaveLoad.Manager
{
    public class PlayerDataManager : PersistentSingleton<PlayerDataManager>
    {
        public HeroManager hero;
        public bool IsModified = false;
        public void OnEnable()
        {
            AccountManager.Instance.OnPlayerLoggedOut += ResetHeroData;
            AccountManager.Instance.OnPlayerDeleteAccount += ResetHeroData;
            AccountManager.Instance.OnPlayerLogin += HandlePlayerLogin;
        }

        public void OnDisable()
        {
            AccountManager.Instance.OnPlayerLoggedOut -= ResetHeroData;
            AccountManager.Instance.OnPlayerDeleteAccount -= ResetHeroData;
            AccountManager.Instance.OnPlayerLogin -= HandlePlayerLogin;
        }
        protected override void Awake()
        {
            base.Awake();
        }
        private async void Start()
        {
            SaveLoadManager.Instance.saveLoadLocalService.trackableService.AddChange("hero");
            await LoadHeroData();
        }

        private void Update()
        {
            if (hero != null && hero.playerData != null)
            {
                // Cập nhật vị trí người chơi
                UpdatePlayerPosition();
            }
        }
        private async void HandlePlayerLogin()
        {
            await LoadHeroData();
        }
        public async Task LoadHeroData()
        {
            if (!(SignInResult.AccountType == AccountType.NotSignIn))
            {
                hero = await SaveLoadManager.Instance.saveLoadLocalService.LoadAsync<HeroManager>(SignInResult.IdPlayer, "hero");
            }
        }
        public void ResetHeroData()
        {
            hero = null;
        }
        private void UpdatePlayerPosition()
        {
            hero.playerData.position = hero.transform.position;
            hero.playerData.rotation = hero.transform.rotation;
            if (IsModified)
                return;
            SaveLoadManager.Instance.saveLoadLocalService.trackableService.UpdateChange("hero", true);
        }
    }
}
