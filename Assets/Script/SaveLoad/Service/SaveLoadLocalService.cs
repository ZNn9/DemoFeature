using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Systems.Account.Enum;
using Systems.Account.Model;
using Systems.Multiplay.Manager;
using Systems.SaveLoad.Interface;
using Systems.SaveLoad.Manager;
using Systems.SaveLoad.Model;

namespace Systems.SaveLoad.Service
{
    public class SaveLoadLocalService
    {
        public Dictionary<string, Func<Task>> saveActions;

        public List<string> listNameFlailedSaves = new List<string>();
        public TrackableService trackableService = new TrackableService();
        ISerializer serializer = new JsonSerializerService();
        private string dataPath;
        private string fileExtension = "json";
        public SaveLoadLocalService(string fileLocation)
        {
            this.dataPath = fileLocation;
        }
        public string GetDataPath()
        {
            return dataPath;
        }
        public void SetDataPath(string fileLocation)
        {
            this.dataPath = fileLocation;
        }
        public string GetPathFolder(string folderName)
        {
            return Path.Combine(dataPath, string.Concat(folderName));
        }
        public string GetPathToFile(string fileName)
        {
            return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
        }
        public string GetPathToFile(string folderName, string fileName)
        {
            return Path.Combine(dataPath, folderName, string.Concat(fileName, ".", fileExtension));
        }
        public void CreateFolder(string folderName)
        {
            if (!Directory.Exists(string.Concat(dataPath, folderName)))
            {
                Directory.CreateDirectory(string.Concat(dataPath, folderName));
                return;
            }
        }
        //Save ZONE
        public async Task SaveAsync<T>(string fileName, T data, bool overwrite = true)
        {
            string fileLocation = FindFileSave(fileName, overwrite);
            if (fileLocation == string.Empty) return;
            string jsonData = serializer.Serialize(data);
            await File.WriteAllTextAsync(fileLocation, jsonData);
        }
        public async Task SaveAsync<T>(string folderName, string fileName, T data, bool overwrite = true)
        {
            string fileLocation = FindFileSave(folderName, fileName, overwrite);
            if (fileLocation == string.Empty) return;
            string jsonData = serializer.Serialize(data);
            await File.WriteAllTextAsync(fileLocation, jsonData);
        }
        private string FindFileSave(string fileName, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(fileName);
            if (overwrite && File.Exists(fileLocation)) return fileLocation;
            if (!overwrite && !File.Exists(fileLocation)) return fileLocation;
            return string.Empty;
        }
        private string FindFileSave(string folderName, string fileName, bool overwrite = true)
        {
            CreateFolder(folderName);
            string fileLocation = GetPathToFile(folderName, fileName);
            if (overwrite && File.Exists(fileLocation)) return fileLocation;
            if (!overwrite && !File.Exists(fileLocation)) return fileLocation;
            return string.Empty;
        }
        // Save Action ZONE
        public async Task SavePlayerData()
        {
            saveActions = new Dictionary<string, Func<Task>>
            {
                { "hero", async () => await SaveHeroData() },
                // Thêm các hành động khác nếu cần
                // { "setting", async () => await SaveSettingData() }
            };
            var changes = trackableService.GetAllChanges();

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
                    }
                    catch (Exception)
                    {

                    }

                    if (!saveSuccess)
                    {
                        listNameFlailedSaves.Add(change.Key); // Thêm vào danh sách thất bại
                    }
                }
            }
            trackableService.ResetChanges();
            // Thử lưu lại các đối tượng không lưu được
            await RetryFailedSaves();
        }

        private async Task SaveHeroData()
        {
            bool isLoggedIn = SignInResult.AccountType == AccountType.Player;
            await SaveAsync<HeroManager>(SignInResult.IdPlayer, "hero", PlayerDataManager.Instance.hero);
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
            foreach (var key in listNameFlailedSaves.ToArray())
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
                    listNameFlailedSaves.Remove(key);
                }
            }
        }
        // Load ZONE
        public async Task<T> LoadAsync<T>(string fileName)
        {
            string fileLocation = GetPathToFile(fileName);

            if (!File.Exists(fileLocation))
            {
                return default;
            }
            string jsonData = await File.ReadAllTextAsync(fileLocation);
            return serializer.Deserialize<T>(File.ReadAllText(jsonData));
        }
        public async Task<T> LoadAsync<T>(string folderName, string fileName)
        {
            string fileLocation = GetPathToFile(folderName, fileName);

            if (!File.Exists(fileLocation))
            {
                return default;
            }
            string jsonData = await File.ReadAllTextAsync(fileLocation);
            return serializer.Deserialize<T>(File.ReadAllText(jsonData));
        }
        public void DeleteFile(string fileName)
        {
            string fileLocation = GetPathToFile(fileName);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
        }
        public void DeleteFile(string folderName, string fileName)
        {
            string fileLocation = GetPathToFile(folderName, fileName);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
        }
        public void DeleteFolder(string folderName)
        {
            string fileLocation = GetPathToFile(folderName);
            if (Directory.Exists(fileLocation))
            {
                Directory.Delete(fileLocation);
            }
        }
        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(dataPath))
            {
                File.Delete(filePath);
            }
        }
        public void DeleteAll(string folderName)
        {
            foreach (string filePath in Directory.GetFiles(dataPath, folderName))
            {
                File.Delete(filePath);
            }
        }
        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(dataPath))
            {
                if (Path.GetExtension(path) == fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
        public IEnumerable<string> ListSaves(string folderName)
        {
            foreach (string path in Directory.EnumerateFiles(dataPath, folderName))
            {
                if (Path.GetExtension(path) == fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        // Check ZONE
        public bool CheckFileExists(string nameFile)
        {
            return File.Exists(GetPathToFile(nameFile));
        }
        public bool CheckFolderExists(string nameFolder)
        {
            return Directory.Exists(GetPathFolder(nameFolder));
        }
        public bool CheckFolderToFileExists(string nameFolder, string nameFile)
        {
            return File.Exists(GetPathToFile(nameFolder, nameFile));
        }

        public bool CheckAnonymousFileExists()
        {
            return File.Exists(GetPathToFile("AnonymousAccount"));
        }
        public bool CheckAnonymousFolderExists()
        {
            return Directory.Exists(GetPathFolder("AnonymousAccount"));
        }
        public bool CheckAnonymousFolderToFileExists(string nameFile)
        {
            return File.Exists(GetPathToFile("AnonymousAccount", nameFile));
        }

        public bool CheckPlayerFileExists(string nameFilePlayer)
        {
            return File.Exists(GetPathToFile(nameFilePlayer));
        }
        public bool CheckPlayerFolderExists(string nameFolderPlayer)
        {
            return Directory.Exists(GetPathFolder(nameFolderPlayer));
        }
        public bool CheckPlayerFolderToFileExists(string nameFolderPlayer, string nameFile)
        {
            return File.Exists(GetPathToFile(nameFolderPlayer, nameFile));
        }
    }
}