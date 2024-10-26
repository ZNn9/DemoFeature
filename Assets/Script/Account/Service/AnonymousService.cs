using System.Collections.Generic;
using System.Threading.Tasks;
using Systems.Account.Enum;
using Systems.Account.Manager;
using Systems.Account.Model;
using Systems.SaveLoad.Manager;
using Systems.SaveLoad.Model;
using Systems.SaveLoad.Service;

namespace Systems.Account.Service
{
    public class AnonymousAccount
    {
        public async Task SignInAnonymous()
        {
            if (!SaveLoadManager.Instance.saveLoadLocalService.CheckAnonymousFolderExists())
            {
                AccountData playerData = new AccountData();
                playerData.idPlayer = "AnonymousAccount";
                await SaveLoadManager.Instance.saveLoadLocalService.SaveAsync<AccountData>(playerData.idPlayer, "AccountData" ,playerData);
            }
            SignInResult.AccountType = AccountType.Anonymous;
            SignInResult.IdPlayer = "AnonymousAccount";
        }
    }
}