using UnityEngine;
using System.IO;
using Systems.Account.Manager;
using Systems.Account.Model;
using Systems.Account.Enum;
using System;
using Systems.SaveLoad.Service;
using Systems.SaveLoad.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Systems.Multiplay.Manager;
using Systems.Account.Service;

namespace Systems.SaveLoad.Manager
{
    public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    {
        public Dictionary<string, Func<Task>> saveActions;
        public List<string> listNameFailedSaves = new List<string>();
        public string dataPath;
        public SaveLoadLocalService saveLoadLocalService;
        private void OnEnable()
        {
            MultiplayerManager.Instance.OnNetworkRestored += OnNetworkRestored;
            AccountManager.Instance.OnPlayerLoggedOut += ResetListNameFailedSave;
        }

        private void OnDisable()
        {
            MultiplayerManager.Instance.OnNetworkRestored -= OnNetworkRestored;
        }
        protected override void Awake()
        {
            base.Awake();
            dataPath = Application.persistentDataPath;
            saveLoadLocalService = new SaveLoadLocalService(dataPath);

        }
        public void Start()
        {
            saveActions = new Dictionary<string, Func<Task>>
            {
                { "hero", async () => await SaveHeroData() },
                // Thêm các hành động khác nếu cần
                // { "setting", async () => await SaveSettingData() }
            };
        }
        private async void OnApplicationQuit()
        {
            await SavePlayerData();
        }
        private async void OnNetworkRestored()
        {
            await SavePlayerData();
        }
        
        // private async Task SaveSettingData()
        // {
        //     SettingManager.Instance.IsModified = false;
        //     await saveLoadLocalService.SaveAsync<SettingManager>(SignInResult.IdPlayer, "setting", SettingManager.Instance.setting);
        // }
        public async Task SavePlayerData()
        {
            await RetryFailedSaves();
            var changes = saveLoadLocalService.trackableService.GetAllChanges();

            foreach (var change in changes)
            {
                if (change.Value == true && saveActions.TryGetValue(change.Key, out var saveAction))
                {
                    bool saveSuccess = false;

                    // Thử lưu và ghi lại trạng thái thất bại nếu lưu không thành công
                    try
                    {
                        await saveAction();
                        saveSuccess = true; // Lưu thành công
                        if (listNameFailedSaves.Contains(change.Key))
                            listNameFailedSaves.Remove(change.Key);
                    }
                    catch (Exception)
                    {

                    }

                    // Nếu lưu không thành công, thêm vào danh sách thất bại
                    if (!saveSuccess && !listNameFailedSaves.Contains(change.Key))
                    {
                        listNameFailedSaves.Add(change.Key);
                    }
                }
            }
            saveLoadLocalService.trackableService.ResetChanges();
            // Thử lưu lại các đối tượng không lưu được
            await RetryFailedSaves();
        }

        private async Task SaveHeroData()
        {
            bool isLoggedIn = SignInResult.AccountType == AccountType.Player;
            await saveLoadLocalService.SaveAsync<HeroManager>(SignInResult.IdPlayer, "hero", PlayerDataManager.Instance.hero);
            PlayerDataManager.Instance.IsModified = false;
            try
            {
                if (isLoggedIn && MultiplayerManager.Instance.isNetworkAvailable)
                {
                    // Xử lý lưu dữ liệu lên cloud
                }
            }
            catch (Exception)
            {

            }
        }
        private async Task RetryFailedSaves()
        {
            foreach (var key in listNameFailedSaves.ToArray())
            {
                bool saveSuccess = false;

                try
                {
                    var saveAction = saveActions[key];
                    await saveAction();
                    saveSuccess = true;
                }
                catch (Exception)
                {

                }

                if (saveSuccess)
                {
                    listNameFailedSaves.Remove(key);
                }
            }
        }
        private void ResetListNameFailedSave()
        {
            listNameFailedSaves.Clear();
        }
    }
}
