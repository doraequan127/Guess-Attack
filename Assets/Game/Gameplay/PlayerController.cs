using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Funzilla
{
    internal class PlayerController : MonoBehaviour
    {
        [SerializeField] private Material greenMaterial;
        [SerializeField] internal GameController gameController;
        [SerializeField] internal Text humanCountText;
        [SerializeField] internal Text attackLineLengthText;
        [SerializeField] internal GameObject AButton;
        [SerializeField] internal GameObject BButton;
        [SerializeField] internal GameObject CButton;
        internal int humanCount;
        internal LevelScriptableObject levelScriptableObject;
        private List<PlayerSquare> attackLine = new List<PlayerSquare>();

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
            humanCount = levelScriptableObject.playerPositionList1.Count;
            humanCountText.text = humanCount + "";
            attackLineLengthText.text = levelScriptableObject.attackLineLength + "";
            if(levelScriptableObject.playerPositionList2.Count == 0)
                BButton.SetActive(false);
            if (levelScriptableObject.playerPositionList3.Count == 0)
                CButton.SetActive(false);
        }

        private void Start()
        {
            AButtonClick();
            if (PlayerPrefs.GetInt("Level") == 1) gameController.handImage.SetActive(true);
            //if (gameController.saveGameScriptableObject.level == 1) gameController.handImage.SetActive(true);
            gameController.handImageSelectButtonSortPanel.SetActive(false);
        }

        public void AButtonClick()
        {
            gameController.handImage.SetActive(false);
            if (PlayerPrefs.GetInt("Level") == 1) gameController.handImageSelectButtonSortPanel.SetActive(true);
            //if (gameController.saveGameScriptableObject.level == 1) gameController.handImageSelectButtonSortPanel.SetActive(true);
            AButton.GetComponent<Image>().color = Color.yellow;
            BButton.GetComponent<Image>().color = Color.white;
            CButton.GetComponent<Image>().color = Color.white;
            for (int i = 0; i < 36; i++)
            {
                PlayerSquare u = transform.GetChild(i).GetComponent<PlayerSquare>();
                u.hasHuman = false;
                u.stickman.SetActive(false);
            }
            foreach (int i in levelScriptableObject.playerPositionList1)
            {
                PlayerSquare u = transform.GetChild(i).GetComponent<PlayerSquare>();
                u.hasHuman = true;
                u.stickman.SetActive(true);
            }
        }

        public void BButtonClick()
        {
            gameController.handImage.SetActive(false);
            if (PlayerPrefs.GetInt("Level") == 1) gameController.handImageSelectButtonSortPanel.SetActive(true);
            //if (gameController.saveGameScriptableObject.level == 1) gameController.handImageSelectButtonSortPanel.SetActive(true);
            AButton.GetComponent<Image>().color = Color.white;
            BButton.GetComponent<Image>().color = Color.yellow;
            CButton.GetComponent<Image>().color = Color.white;
            for (int i = 0; i < 36; i++)
            {
                PlayerSquare u = transform.GetChild(i).GetComponent<PlayerSquare>();
                u.hasHuman = false;
                u.stickman.SetActive(false);
            }
            foreach (int i in levelScriptableObject.playerPositionList2)
            {
                PlayerSquare u = transform.GetChild(i).GetComponent<PlayerSquare>();
                u.hasHuman = true;
                u.stickman.SetActive(true);
            }
        }

        public void CButtonClick()
        {
            gameController.handImage.SetActive(false);
            if (PlayerPrefs.GetInt("Level") == 1) gameController.handImageSelectButtonSortPanel.SetActive(true);
            //if (gameController.saveGameScriptableObject.level == 1) gameController.handImageSelectButtonSortPanel.SetActive(true);
            AButton.GetComponent<Image>().color = Color.white;
            BButton.GetComponent<Image>().color = Color.white;
            CButton.GetComponent<Image>().color = Color.yellow;
            for (int i = 0; i < 36; i++)
            {
                PlayerSquare u = transform.GetChild(i).GetComponent<PlayerSquare>();
                u.hasHuman = false;
                u.stickman.SetActive(false);
            }
            foreach (int i in levelScriptableObject.playerPositionList3)
            {
                PlayerSquare u = transform.GetChild(i).GetComponent<PlayerSquare>();
                u.hasHuman = true;
                u.stickman.SetActive(true);
            }
        }

        internal void Attack()
        {
            StartCoroutine("AttackIEnumerator");
        }

        IEnumerator AttackIEnumerator()
        {
            attackLine.Clear();
            GenerateFirstAttack:
            int x = Mathf.RoundToInt(Random.Range(-0.49f, 5.49f));
            int y = Mathf.RoundToInt(Random.Range(-0.49f, 5.49f));
            PlayerSquare playerSquare = transform.GetChild(x + 6 * y).GetComponent<PlayerSquare>();
            if (playerSquare.isAttacked || !playerSquare.isOpened) goto GenerateFirstAttack;
            attackLine.Add(playerSquare);
            for (int i = 0; i < levelScriptableObject.attackLineLength - 1; i++)
            {
                GenerateNextAttack:
                int x_ = x + Mathf.RoundToInt(Random.Range(-1.49f, 1.49f));
                int y_ = y + Mathf.RoundToInt(Random.Range(-1.49f, 1.49f));
                if (x_ > 5 || x_ < 0 || y_ > 5 || y_ < 0 || (x_ == x && y_ == y)) goto GenerateNextAttack;
                x = x_; y = y_;
                playerSquare = transform.GetChild(x + 6 * y).GetComponent<PlayerSquare>();
                if (playerSquare.isAttacked || !playerSquare.isOpened || attackLine.Contains(playerSquare)) break;
                attackLine.Add(playerSquare);
            }
            int u = 1;
            foreach (PlayerSquare i in attackLine)
            {
                i.GetComponent<MeshRenderer>().material = greenMaterial;
                attackLineLengthText.text = (levelScriptableObject.attackLineLength - u++) + "";
                yield return new WaitForSeconds(0.5f);
            }
            gameController.enemyGunExplode.Play();
            gameController.audioSource.PlayOneShot(gameController.fireGunSound);
            gameController.audioSource.PlayOneShot(gameController.flyBulletSound);
            attackLineLengthText.text = levelScriptableObject.attackLineLength + "";
            yield return new WaitForSeconds(0.1f);
            attackLine.ForEach(i => i.Attack());
            yield return new WaitForSeconds(1);
            gameController.audioSource.PlayOneShot(gameController.explodeSound);
            if (humanCount == 0) gameController.LosePopup();
            else gameController.isPlayerTurn = true;
            gameController.imageEnemy.DOPause();
            gameController.imageEnemy.color = new Color(1, 1, 1, 1);
        }    
    }
}