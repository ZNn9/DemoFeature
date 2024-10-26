using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Systems.SaveLoad.Service;
using Unity.Services.Core;
using System;
using Systems.Account.Manager;
using Systems.Account.Model;
using Systems.Account.Enum;
using Systems.SaveLoad.Manager;
public class GameManager : PersistentSingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start() 
    {
        
    }

    private void PlayGame()
    {
        // Load Scene Map Hero Playing
    }
    public async void LeaveGame()
    {
        await SaveLoadManager.Instance.SavePlayerData();
        // Leave Scene Map Hero Playing
    }
}
