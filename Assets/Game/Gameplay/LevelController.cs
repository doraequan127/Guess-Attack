using DG.Tweening; //
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //

namespace Funzilla
{
    internal class LevelController : MonoBehaviour
    {
        [SerializeField] private Material whiteMaterial;
        [SerializeField] internal GameController gameController;
        [SerializeField] internal Text enemyCountText;
        [SerializeField] internal Text attackLineLengthText;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject cameraZoom;
        private Vector3 firstMainCameraPosition;
        private Quaternion firstMainCameraRotation;
        internal bool isMouseDown = false;
        internal int enemyCount;
        internal List<Square> attackLine = new List<Square>();
        internal LevelScriptableObject levelScriptableObject;

        private void Awake()
        {
            firstMainCameraPosition = mainCamera.gameObject.transform.position;
            firstMainCameraRotation = mainCamera.gameObject.transform.rotation;
        }

        internal void Awake_()
        {
            levelScriptableObject = Resources.Load<LevelScriptableObject>("Level/Level" + gameController.level);
            if (levelScriptableObject == null)
            {
                levelScriptableObject = Resources.Load<LevelScriptableObject>("Level/Level1");
                gameController.level = 1;
                PlayerPrefs.SetInt("Level", 1);
                //gameController.saveGameScriptableObject.level = 1;
            }
            name = "Level" + gameController.level;
            enemyCount = levelScriptableObject.enemyPositionList.Count;
            enemyCountText.text = enemyCount + "";
            foreach (int i in levelScriptableObject.enemyPositionList)
                transform.GetChild(i).GetComponent<Square>().hasEnemy = true;
            attackLineLengthText.text = levelScriptableObject.attackLineLength + "";
        }

        private void Update()
        {
            if (!gameController.isPlayerTurn || gameController.gameState == GameState.PAUSE) return;
            mainCamera.transform.DOMove(cameraZoom.transform.position, 1);
            mainCamera.transform.DORotateQuaternion(cameraZoom.transform.rotation, 1);
            if (Input.GetMouseButtonDown(0)) isMouseDown = true;
            else if (Input.GetMouseButtonUp(0))
                StartCoroutine(MouseButtonUpIEnumerator(attackLine));
        }

        internal IEnumerator MouseButtonUpIEnumerator(List<Square> attackLine_)
        {
            isMouseDown = false;
            if (attackLine_.Count > 0)
            {
                gameController.humanGunExplode.Play();
                gameController.audioSource.PlayOneShot(gameController.fireGunSound);
                gameController.audioSource.PlayOneShot(gameController.flyBulletSound);
                mainCamera.transform.DOMove(firstMainCameraPosition, 1);
                mainCamera.transform.DORotateQuaternion(firstMainCameraRotation, 1);
                attackLineLengthText.text = levelScriptableObject.attackLineLength + "";
                gameController.isPlayerTurn = false;
                gameController.imagePlayer.DOPause();
                gameController.imagePlayer.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(0.1f);  //Âm thanh bắn pháo bị chậm nên phải thêm dòng này
                attackLine_.ForEach(i => i.Attack());
                attackLine_.Clear();
                yield return new WaitForSeconds(1);
                gameController.audioSource.PlayOneShot(gameController.explodeSound);
                if (enemyCount == 0) gameController.WinPopup();
                else gameController.AttackPlayer();
            }
        }    
    }
}