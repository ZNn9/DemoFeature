using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerDataCloud playerDataCloud;

    [ContextMenu("Save")]
    public void save()
    {
        CloudSave.SaveData("PlayerData", playerDataCloud);
        Debug.Log("Saved Data");
    }
}
