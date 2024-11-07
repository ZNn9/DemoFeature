
using System.Collections;
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

        [SerializeField] public GameObject playerHero; // Tham chiếu tới Hero của người chơi

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {

        }

        private void Update()
        {
            // if (playerData != null && playerHero != null && GameManager.Instance.isPlayingInMap)
            // {
            //     Vector3 currentPosition = playerHero.transform.position;
            //     Quaternion currentRotation = playerHero.transform.rotation;
            //     if (currentPosition != lastPosition)
            //     {
            //         lastPosition = currentPosition;
            //         UpdatePlayerPositionRotation(currentPosition, currentRotation);
            //     }
            // }
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
            // Set position and rotation first
            StartCoroutine(ActivatePlayerHeroWithDelay());
        }

        private IEnumerator ActivatePlayerHeroWithDelay()
        {
            // Đợi 3 giây
            yield return new WaitForSeconds(2f);

            // // Đặt vị trí và xoay của playerHero
            // Debug.Log($"PlayerDataManager: {playerData.position}");

            // playerHero.transform.position = playerData.position;
            // playerHero.transform.rotation = playerData.rotation;

            // // Kích hoạt GameObject sau khi đã đặt vị trí và xoay
            // if (playerHero != null)
            // {
            //     playerHero.SetActive(true);
            // }
            // Giả sử bạn đã load dữ liệu vào `playerData`
            if (playerData != null && playerHero != null)
            {
                // Cập nhật vị trí từ dữ liệu đã lưu
                playerHero.transform.position = playerData.position;
                playerHero.transform.rotation = playerData.rotation;

                // Bật GameObject sau khi cập nhật vị trí
                playerHero.SetActive(true);
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
