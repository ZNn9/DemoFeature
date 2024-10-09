using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSingleMode : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add this method to instantiate the playerPrefab
    public void OnButtonClick()
    {
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); // Adjust position and rotation as needed
    }
}
