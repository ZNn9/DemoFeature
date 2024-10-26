using System;
using UnityEngine;
using Systems.SaveLoad.Interface;
using Systems.SaveLoad.Model;

namespace Systems.SaveLoad.Manager
{
    public class HeroManager : MonoBehaviour, IBind<PlayerData>
    {
        [field: SerializeField] public Guid Id { get; set; } = Guid.NewGuid();
        [SerializeField] public PlayerData playerData;
        public void Bind(PlayerData playerData)
        {
            this.playerData = playerData;
        }
    }
}


