
using Systems.Hero.Model;
using Systems.SaveLoad.Manager;
using Systems.Scriptable.Events;
using UnityEngine;
namespace Systems.Hero.Manager
{
    public class PlayerDataManager : PersistentSingleton<PlayerDataManager>
    {
        public PlayerData playerData;
        public bool isModified = false;
        private Vector3 lastPosition;

        public GameObject playerHero; // Tham chiếu tới Hero của người chơi

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {

        }

        private void Update()
        {
            if (playerData != null && playerHero != null)
            {
                Vector3 currentPosition = playerHero.transform.position;
                Quaternion currentRotation = playerHero.transform.rotation;
                if (currentPosition != lastPosition)
                {
                    lastPosition = currentPosition;
                    UpdatePlayerPositionRotation(currentPosition, currentRotation);
                }
            }
        }

        private void UpdatePlayerPositionRotation(Vector3 position, Quaternion rotation)
        {
            playerData.position = position;
            playerData.rotation = rotation;
            if (isModified == true) return;
            isModified = true;
            SaveLoadManager.Instance.saveLoadLocalService.trackableService.UpdateChange("playerData", true);
        }
        private void OnEnable()
        {
            Observer.Instance.AddObserver("onPlayGame", OnPlayGame);
            Observer.Instance.AddObserver("onLeaveGame", OnLeaveGame);
        }
        private void OnDisable()
        {
            Observer.Instance.RemoveListener("onPlayGame", OnPlayGame);
            Observer.Instance.RemoveListener("onLeaveGame", OnLeaveGame);
        }
        private void OnPlayGame(object[] obj)
        {
            // playerHero = GameObject.FindWithTag("PlayerHero");
            Debug.Log($"PlayerHero: {playerHero}");
            playerHero.transform.position = playerData.position;
            playerHero.transform.rotation = playerData.rotation;
            if (playerHero != null)
            {
                playerHero.SetActive(true); // Bật Hero khi bắt đầu trò chơi
            }
        }

        private void OnLeaveGame(object[] obj)
        {
            if (playerHero != null)
            {
                playerHero.SetActive(false); // Tắt Hero khi rời khỏi trò chơi
            }
        }

    }
}
