using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor; //
using UnityEngine;
using UnityEngine.SceneManagement; //
using UnityEngine.UI; //

namespace Funzilla
{
    internal class GameController : MonoBehaviour
    {
        [SerializeField] private Text resultText;
        [SerializeField] private Button levelButton;
        [SerializeField] internal Image imageEnemy;
        [SerializeField] internal Image imagePlayer;
        [SerializeField] private Text levelText;
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private Text processText;
        [SerializeField] private GameObject giftSkinPanel;
        [SerializeField] private Text levelCompleteText;
        [SerializeField] internal Transform enemyCastleGate;
        [SerializeField] internal Transform humanCastleGate;
        [SerializeField] internal Transform headOfGunHuman;
        [SerializeField] internal Transform headOfGunEnemy;
        [SerializeField] internal ParticleSystem enemyGunExplode;
        [SerializeField] internal ParticleSystem humanGunExplode;
        [SerializeField] private Button helpButton;
        [SerializeField] private GameObject sortPanel;
        [SerializeField] internal GameObject handImage;
        [SerializeField] internal RectTransform selectButtonSortPanel;
        [SerializeField] internal GameObject handImageSelectButtonSortPanel;
        [SerializeField] private GameObject castleRed1, castleRed2, castleRed3, castleRedExplode;
        [SerializeField] private ParticleSystem castleRedExplodeParticleSystem;
        [SerializeField] private RectTransform canvas;
        internal int level;
        [SerializeField] internal SaveGameScriptableObject saveGameScriptableObject;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private LevelController levelController;
        [SerializeField] internal AudioSource audioSource;
        [SerializeField] internal AudioClip explodeSound;
        [SerializeField] internal AudioClip winSound;
        [SerializeField] internal AudioClip loseSound;
        [SerializeField] internal AudioClip squareTapSound;
        [SerializeField] internal AudioClip dieSound;
        [SerializeField] internal AudioClip fireGunSound;
        [SerializeField] internal AudioClip flyBulletSound;
        internal bool isPlayerTurn = true;
        internal GameState gameState = GameState.PAUSE;

        private void Awake()
        {
            level = PlayerPrefs.GetInt("Level");
            //level = saveGameScriptableObject.level;
            levelText.text = "Level " + level;
            if (PlayerPrefs.GetInt("Level") > 1) handImage.SetActive(false);
            //if (saveGameScriptableObject.level > 1) handImage.SetActive(false);
            else helpButton.interactable = false;
            levelController.Awake_();
            playerController.Awake_();
        }

        private void Start()
        {
            StartCoroutine("iconAnimation");
            StartCoroutine("HandImageAnimation");
        }

        IEnumerator HandImageAnimation()
        {
            handImage.transform.DOMoveX(playerController.BButton.transform.position.x, 1);
            yield return new WaitForSeconds(1);
            handImage.transform.DOMoveX(playerController.CButton.transform.position.x, 1);
            yield return new WaitForSeconds(1);
            handImage.transform.DOMoveX(playerController.AButton.transform.position.x, 1);
            yield return new WaitForSeconds(1);
            StartCoroutine("HandImageAnimation");
        }

        internal void StopHandImageAnimation()
        {
            StopCoroutine("HandImageAnimation");
        }

        IEnumerator iconAnimation()
        {
            if (isPlayerTurn)
                imagePlayer.DOColor(new Color(1, 1, 1, imagePlayer.color.a == 1 ? 0 : 1), 0.5f);
            else imageEnemy.DOColor(new Color(1, 1, 1, imageEnemy.color.a == 1 ? 0 : 1), 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("iconAnimation");
        }

        internal void WinPopup()
        {
            gameState = GameState.WIN;
            processText.text = "Process Army Skin " + (level % 10 == 0 ? 100 : level % 10 * 10) + "%";
            levelCompleteText.text = "LEVEL " + level + " COMPLETE";
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            //saveGameScriptableObject.level++;
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 100);
            //saveGameScriptableObject.gold += 100;
            StartCoroutine("WinPopupIEnumerator");
            StopCoroutine("iconAnimation");
        }

        IEnumerator WinPopupIEnumerator()
        {
            yield return new WaitForSeconds(4);
            castleRed1.SetActive(false);
            castleRed2.SetActive(false);
            castleRed3.SetActive(false);
            castleRedExplode.SetActive(true);
            castleRedExplodeParticleSystem.Play();
            audioSource.PlayOneShot(explodeSound);
            yield return new WaitForSeconds(2);
            //yield return new WaitForSeconds(7);
            audioSource.Stop();
            audioSource.PlayOneShot(winSound);
            //audioSource.PlayOneShot(explodeSound);
            rewardPanel.SetActive(true);
            if (level % 10 == 0) 
            {
                yield return new WaitForSeconds(1);
                giftSkinPanel.SetActive(true);
            } 
        }

        internal void LosePopup()
        {
            gameState = GameState.LOSE;
            StartCoroutine("LosePopupIEnumerator");
            StopCoroutine("iconAnimation");
        }

        IEnumerator LosePopupIEnumerator()
        {
            yield return new WaitForSeconds(7);
            audioSource.Stop();
            audioSource.PlayOneShot(loseSound);
            resultText.gameObject.SetActive(true);
            resultText.text = "LEVEL " + level + " FAIL";
            levelButton.gameObject.SetActive(true);
            levelButton.GetComponentInChildren<Text>().text = "Replay";
        }

        internal void AttackPlayer()
        {
            StartCoroutine("AttackPlayerIEnumerator");
        }

        IEnumerator AttackPlayerIEnumerator()
        {
            yield return new WaitForSeconds(1.5f);
            playerController.Attack();
        }

        public void levelButtonClick()
        {
            SceneManager.LoadScene("Menu");
        }

        public void HelpPanelButtonClick()
        {
            if (!isPlayerTurn) return;
            helpPanel.SetActive(true);
            gameState = GameState.PAUSE;
        }

        public void NoThanksButtonHelpPanelClick()
        {
            helpPanel.SetActive(false);
            gameState = GameState.PLAYING;
        }

        public void AdsButtonHelpPanelClick()
        {
            List<Square> attackLine = new List<Square>();
            for (int i = 0; i < levelController.levelScriptableObject.attackLineLength; i++)
            {
                Square u = levelController.transform.GetChild(levelController.levelScriptableObject.enemyPositionList[i]).GetComponent<Square>();
                if (u.isAttacked && attackLine.Count == 0) continue;
                if (u.isAttacked && attackLine.Count > 0) break;
                attackLine.Add(u);
            }
            helpPanel.SetActive(false);
            helpButton.interactable = false;
            gameState = GameState.PLAYING;
            StartCoroutine(levelController.MouseButtonUpIEnumerator(attackLine));
        }

        public void NoThanksButtonRewardPanelClick()
        {
            SceneManager.LoadScene("Menu");
        }

        public void AdsBonusRewardPanelClick()
        {
            SceneManager.LoadScene("Menu");
        }

        public void ReceiveButtonGiftSkinPanelClick()
        {
            SceneManager.LoadScene("Menu");
        }

        public void SelectButtonSortPanelClick()
        {
            sortPanel.SetActive(false);
            handImage.SetActive(false);
            StopCoroutine("HandImageAnimation");
            gameState = GameState.PLAYING;
            if (PlayerPrefs.GetInt("Level") == 1) StartCoroutine("SelectButtonSortPanelClickIEnumerator");
            //if (saveGameScriptableObject.level == 1) StartCoroutine("SelectButtonSortPanelClickIEnumerator");
        }

        IEnumerator SelectButtonSortPanelClickIEnumerator()
        {
            yield return new WaitForSeconds(1);
            //Vector2 a = Camera.main.WorldToScreenPoint(levelController.transform.GetChild(levelController.levelScriptableObject.enemyPositionList[0]).position) - new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight + 180, 0) / 2;
            //Vector2 b = Camera.main.WorldToScreenPoint(levelController.transform.GetChild(levelController.levelScriptableObject.enemyPositionList[1]).position) - new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight + 180, 0) / 2;
            Vector2 a = (Camera.main.WorldToViewportPoint(levelController.transform.GetChild(levelController.levelScriptableObject.enemyPositionList[0]).position) - new Vector3(0.5f, 0.5f + 0.05f, 0)) * canvas.sizeDelta;
            Vector2 b = (Camera.main.WorldToViewportPoint(levelController.transform.GetChild(levelController.levelScriptableObject.enemyPositionList[1]).position) - new Vector3(0.5f, 0.5f + 0.05f, 0)) * canvas.sizeDelta;
            handImage.GetComponent<RectTransform>().anchoredPosition = a;
            handImage.SetActive(true);
            TutorialAnimation:
            handImage.GetComponent<RectTransform>().DOAnchorPos(b, 1);
            yield return new WaitForSeconds(1);
            handImage.GetComponent<RectTransform>().DOAnchorPos(a, 1);
            yield return new WaitForSeconds(1);
            goto TutorialAnimation;
        }

        internal void StopSelectButtonSortPanelClickIEnumerator()
        {
            StopCoroutine("SelectButtonSortPanelClickIEnumerator");
        }

#if UNITY_EDITOR
        [MenuItem("Hê lô/Tạo Level/Tạo Level")]
        static void GenerateLevel()
        {
            LevelScriptableObject levelScriptableObject = new LevelScriptableObject();
            GameObject level = GameObject.FindGameObjectWithTag("Level");
            for (int i = 0; i < level.transform.childCount; i++)
            {
                if (level.transform.GetChild(i).GetComponent<MeshRenderer>().material.name.Contains("RedMaterial"))
                    levelScriptableObject.enemyPositionList.Add(i);
            }
            AssetDatabase.CreateAsset(levelScriptableObject, "Assets/Game/Gameplay/Resources/Level/" + level.name + ".asset");
        }

        [MenuItem("Hê lô/Load Level")]
        static void LoadLevel()
        {
            LevelScriptableObject levelScriptableObject = new LevelScriptableObject();
            GameObject level = GameObject.FindGameObjectWithTag("Level");
            for (int i = 0; i < 36; i++)
                if (level.transform.GetChild(i).GetComponent<MeshRenderer>().material.name.Contains("RedMaterial")) 
                    level.transform.GetChild(i).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/WhiteMaterial");
            levelScriptableObject = Resources.Load<LevelScriptableObject>("Level/" + level.name);
            foreach (int i in levelScriptableObject.enemyPositionList)
                level.transform.GetChild(i).GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/RedMaterial");
        }

        [MenuItem("Hê lô/Reset Level")]
        static void ResetLevel()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Hê lô/Choose Level")]
        static void ChooseLevel()
        {
            PlayerPrefs.SetInt("Level", int.Parse(GameObject.FindGameObjectWithTag("ChooseLevel").name));
        }
#endif
    }

    internal enum GameState
    { PLAYING, PAUSE, WIN, LOSE }
}