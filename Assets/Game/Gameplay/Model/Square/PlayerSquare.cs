using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
    internal class PlayerSquare : MonoBehaviour
    {
        [SerializeField] private Material whiteMaterial;
        [SerializeField] internal Material blueMaterial;
        [SerializeField] private Material yellowMaterial;
        [SerializeField] private Material redMaterial;
        [SerializeField] private Material grayMaterial;
        [SerializeField] private GameObject box;
        [SerializeField] internal GameObject stickman;
        [SerializeField] private GameObject helmet;
        [SerializeField] private GameObject explode;
        [SerializeField] private GameObject headstone;
        [SerializeField] private GameObject bomb;
        internal bool isAttacked = false;
        [SerializeField] internal bool hasHuman = false;
        internal bool isOpened = false;
        internal Material initialMaterial;
        private PlayerController playerController;
        private bool isRunning = true;

        private void Awake()
        {
            playerController = transform.parent.GetComponent<PlayerController>();
            stickman = transform.Find(PlayerPrefs.GetString("Current Army Skin")).gameObject;
            //stickman = transform.Find(playerController.gameController.saveGameScriptableObject.currentArmySkin).gameObject;
            //stickman.SetActive(true);
            initialMaterial = whiteMaterial;
            int index = transform.GetSiblingIndex();
            if (((index - index % 6) / 6 % 2 == 0 && index % 6 % 2 == 0) || ((index - index % 6) / 6 % 2 == 1 && index % 6 % 2 == 1))
                initialMaterial = grayMaterial;
            GetComponent<MeshRenderer>().material = initialMaterial;
        }

        private void Start()
        {
            if (transform.GetSiblingIndex() < playerController.levelScriptableObject.openedSquareCountPlayer)
                isOpened = true;
            else box.SetActive(false);
            if (!hasHuman) stickman.SetActive(false);
        }

        //private void Update()
        //{
        //    if (playerController.gameController.gameState == GameState.WIN)
        //        if (hasHuman && !isAttacked && isRunning)
        //            StartCoroutine("RunIEnumerator");
        //}

        IEnumerator RunIEnumerator()
        {
            yield return new WaitForSeconds(4);
            stickman.transform.LookAt(playerController.gameController.enemyCastleGate);
            stickman.GetComponent<Animator>().SetTrigger("Run");
            stickman.transform.DOMove(playerController.gameController.enemyCastleGate.position, 5);
            isRunning = false;
        }    

        internal void Attack()
        {
            StartCoroutine("AttackIEnumerator");
        }

        IEnumerator AttackIEnumerator()
        {
            GameObject i = Instantiate(bomb, playerController.gameController.headOfGunEnemy.position, Quaternion.identity);
            i.transform.DOJump(transform.position, 6, 1, 1.1f);
            yield return new WaitForSeconds(1);
            i.SetActive(false);
            isAttacked = true;
            explode.SetActive(true);
            if (hasHuman)
            {
                stickman.GetComponent<Animator>().SetTrigger("Die");
                playerController.gameController.audioSource.PlayOneShot(playerController.gameController.dieSound);
                playerController.humanCount--;
                playerController.humanCountText.text = playerController.humanCount + "";
                StartCoroutine("BlinkArmy");
                StartCoroutine("ShowHeadstone");
            }
            GetComponent<MeshRenderer>().material = initialMaterial;
            box.SetActive(false);
        }

        IEnumerator BlinkArmy()
        {
            yield return new WaitForSeconds(2.2f);
            for (int i = 0; i < 3; i++)
            {
                stickman.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                helmet.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                stickman.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                helmet.SetActive(false);
                yield return new WaitForSeconds(0.2f);
            }
        }

        IEnumerator ShowHeadstone()
        {
            yield return new WaitForSeconds(4);
            stickman.SetActive(false);
            headstone.SetActive(true);
        }
    }
}