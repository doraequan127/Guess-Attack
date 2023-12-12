using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Funzilla
{
    internal class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject sortPanel;
        [SerializeField] private GameObject chooseButtonGroup;
        [SerializeField] private GameObject AButton;
        [SerializeField] private GameObject BButton;
        [SerializeField] private GameObject CButton;
        [SerializeField] private GameObject squareGroup;
        [SerializeField] private Text levelText;
        [SerializeField] private GameObject reskinPanel;
        [SerializeField] private Text goldText;
        [SerializeField] private Text inputFieldText;
        [SerializeField] private SkinScriptableObject skinScriptableObject;
        [SerializeField] private GameObject skinContent, armyButtonReskinPanel, castleButtonReskinPanel, boardButtonReskinPanel;
        
        [SerializeField] private SaveGameScriptableObject saveGameScriptableObject;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("Level")) PlayerPrefs.SetInt("Level", 1);
            if (!PlayerPrefs.HasKey("Gold")) PlayerPrefs.SetInt("Gold", 0);
            if (!PlayerPrefs.HasKey("Current Army Skin")) PlayerPrefs.SetString("Current Army Skin", "Stickman");
            if (!PlayerPrefs.HasKey("Current Castle Skin")) PlayerPrefs.SetString("Current Castle Skin", "B");
            if (!PlayerPrefs.HasKey("Current Board Skin")) PlayerPrefs.SetString("Current Board Skin", "C");
            ArmyButtonReskinPanelClick();
            goldText.text = PlayerPrefs.GetInt("Gold") + "";
            //goldText.text = saveGameScriptableObject.gold + "";
        }

        public void PlayButtonClick()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void StartButtonClick()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void ReskinButtonClick()
        {
            reskinPanel.SetActive(true);
        }

        public void BackButtonReskinPanelClick()
        {
            reskinPanel.SetActive(false);
        }    

        public void BackButtonSortPanelClick()
        {
            sortPanel.SetActive(false);
        }

        public void ArmyButtonReskinPanelClick()
        {
            armyButtonReskinPanel.GetComponent<Image>().color = Color.yellow;
            castleButtonReskinPanel.GetComponent<Image>().color = Color.white;
            boardButtonReskinPanel.GetComponent<Image>().color = Color.white;
            for (int i = 0; i < 20; i++)
            {
                GameObject u = skinContent.transform.GetChild(i).gameObject;
                if (!u.active) break;
                u.GetComponent<Button>().onClick.RemoveAllListeners();
                u.GetComponent<Image>().color = Color.white;
                u.SetActive(false);
            }
            for (int i = 0; i < skinScriptableObject.armySkinName.Count; i++)
            {
                GameObject u = skinContent.transform.GetChild(i).gameObject;
                u.SetActive(true);
                u.GetComponentInChildren<Text>().text = skinScriptableObject.armySkinName[i];
                if (skinScriptableObject.armySkinName[i] == PlayerPrefs.GetString("Current Army Skin"))
                //if (skinScriptableObject.armySkinName[i] == saveGameScriptableObject.currentArmySkin)
                    u.GetComponent<Image>().color = Color.yellow;
                u.GetComponent<Button>().onClick.AddListener(() =>
                {
                    PlayerPrefs.SetString("Current Army Skin", u.GetComponentInChildren<Text>().text);
                    //saveGameScriptableObject.currentArmySkin = u.GetComponentInChildren<Text>().text;
                    for (int i = 0; i < 20; i++)
                    {
                        GameObject p = skinContent.transform.GetChild(i).gameObject;
                        if (!p.active) break;
                        p.GetComponent<Image>().color = Color.white;
                    }
                    u.GetComponent<Image>().color = Color.yellow;
                });
            }
        }

        public void CastleButtonReskinPanelClick()
        {
            armyButtonReskinPanel.GetComponent<Image>().color = Color.white;
            castleButtonReskinPanel.GetComponent<Image>().color = Color.yellow;
            boardButtonReskinPanel.GetComponent<Image>().color = Color.white;
            for (int i = 0; i < 20; i++)
            {
                GameObject u = skinContent.transform.GetChild(i).gameObject;
                if (!u.active) break;
                u.GetComponent<Button>().onClick.RemoveAllListeners();
                u.GetComponent<Image>().color = Color.white;
                u.SetActive(false);
            }
            for (int i = 0; i < skinScriptableObject.castleSkinName.Count; i++)
            {
                GameObject u = skinContent.transform.GetChild(i).gameObject;
                u.SetActive(true);
                u.GetComponentInChildren<Text>().text = skinScriptableObject.castleSkinName[i];
                if (skinScriptableObject.castleSkinName[i] == PlayerPrefs.GetString("Current Castle Skin"))
                //if (skinScriptableObject.castleSkinName[i] == saveGameScriptableObject.currentCastleSkin)
                    u.GetComponent<Image>().color = Color.yellow;
                u.GetComponent<Button>().onClick.AddListener(() =>
                {
                    PlayerPrefs.SetString("Current Castle Skin", u.GetComponentInChildren<Text>().text);
                    //saveGameScriptableObject.currentCastleSkin = u.GetComponentInChildren<Text>().text;
                    for (int i = 0; i < 20; i++)
                    {
                        GameObject p = skinContent.transform.GetChild(i).gameObject;
                        if (!p.active) break;
                        p.GetComponent<Image>().color = Color.white;
                    }
                    u.GetComponent<Image>().color = Color.yellow;
                });
            }
        }

        public void BoardButtonReskinPanelClick()
        {
            armyButtonReskinPanel.GetComponent<Image>().color = Color.white;
            castleButtonReskinPanel.GetComponent<Image>().color = Color.white;
            boardButtonReskinPanel.GetComponent<Image>().color = Color.yellow;
            for (int i = 0; i < 20; i++)
            {
                GameObject u = skinContent.transform.GetChild(i).gameObject;
                if (!u.active) break;
                u.GetComponent<Button>().onClick.RemoveAllListeners();
                u.GetComponent<Image>().color = Color.white;
                u.SetActive(false);
            }
            for (int i = 0; i < skinScriptableObject.boardSkinName.Count; i++)
            {
                GameObject u = skinContent.transform.GetChild(i).gameObject;
                u.SetActive(true);
                u.GetComponentInChildren<Text>().text = skinScriptableObject.boardSkinName[i];
                if (skinScriptableObject.boardSkinName[i] == PlayerPrefs.GetString("Current Board Skin"))
                //if (skinScriptableObject.castleSkinName[i] == saveGameScriptableObject.currentBoardSkin)
                    u.GetComponent<Image>().color = Color.yellow;
                u.GetComponent<Button>().onClick.AddListener(() =>
                {
                    PlayerPrefs.SetString("Current Board Skin", u.GetComponentInChildren<Text>().text);
                    //saveGameScriptableObject.currentBoardSkin = u.GetComponentInChildren<Text>().text;
                    for (int i = 0; i < 20; i++)
                    {
                        GameObject p = skinContent.transform.GetChild(i).gameObject;
                        if (!p.active) break;
                        p.GetComponent<Image>().color = Color.white;
                    }
                    u.GetComponent<Image>().color = Color.yellow;
                });
            }
        }

        public void SelectLevelButtonClick()
        {
            int a;
            if (!int.TryParse(inputFieldText.text, out a)) return;
            if (Resources.Load<LevelScriptableObject>("Level/Level" + a) == null) return;
            PlayerPrefs.SetInt("Level", a);
            //saveGameScriptableObject.level = a;
            SceneManager.LoadScene("Menu");
        }
    }
}
