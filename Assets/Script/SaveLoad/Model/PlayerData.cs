using System;
using Systems.SaveLoad.Interface;

using UnityEngine;

namespace Systems.SaveLoad.Model
{
    [System.Serializable]
    public class PlayerData : ISaveable
    {
        [field: SerializeField] public Guid Id { get; set; }
        public Vector3 position;
        public Quaternion rotation;
        public long coin = 0;
        public int level = 0;
        public int experience = 0;
        public int baseHealth = 200;
        public int maxShield = 200;
        public int attack = 20;
        public int defense = 20;
        public int accuracy = 10;
        public int luck = 10;
        public int evasion = 5;
    }
}
