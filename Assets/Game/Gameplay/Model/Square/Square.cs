using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
    internal class Square : MonoBehaviour
    {
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material yellowMaterial;
        [SerializeField] private Material redMaterial;
        [SerializeField] private Material grayMaterial;
        [SerializeField] private GameObject box;
        [SerializeField] private GameObject stickman;
        [SerializeField] private GameObject helmet;
        [SerializeField] private GameObject explode;
        [SerializeField] private GameObject headstone;
        [SerializeField] private GameObject bomb;
        internal bool isAttacked = false;
        internal bool hasEnemy = false;
        internal bool isOpened = false;
        private bool isRunning = true;
        private LevelController levelController;
        internal Material initialMaterial;

        private void Awake()
        {
            initialMaterial = whiteMaterial;
            int index = transform.GetSiblingIndex();
            if (((index - index % 6) / 6 % 2 == 0 && index % 6 % 2 == 0) || ((index - index % 6) / 6 % 2 == 1 && index % 6 % 2 == 1))
                initialMaterial = grayMaterial;
            GetComponent<MeshRenderer>().material = initialMaterial;

            levelController = transform.parent.GetComponent<LevelController>();
        }

        private void Start()
        {
            if (transform.GetSiblingIndex() < levelController.levelScriptableObject.openedSquareCount)
                isOpened = true;
            else box.SetActive(false);
        }

        private void Update()
        {
            if (levelController.gameController.gameState == GameState.LOSE)
                if (hasEnemy && !isAttacked && isRunning)
                    StartCoroutine("RunIEnumerator");
        }

        IEnumerator RunIEnumerator()  //Chạy khi xác chết của team bạn bắt đầu biến thành bia mộ
        {
            yield return new WaitForSeconds(4);
            stickman.SetActive(true);
            stickman.transform.LookAt(levelController.gameController.humanCastleGate);
            stickman.GetComponent<Animator>().SetTrigger("Run");
            stickman.transform.DOMove(levelController.gameController.humanCastleGate.position, 6);
            isRunning = false;
        }    

        private void OnMouseOver()
        {
            if (isAttacked || !isOpened || !levelController.isMouseDown || levelController.gameController.gameState == GameState.PAUSE) return;
            levelController.gameController.handImage.SetActive(false);
            levelController.gameController.StopSelectButtonSortPanelClickIEnumerator();
            if (levelController.attackLine.Count > 0)
            {
                if (Vector3.Distance(transform.position, levelController.attackLine[levelController.attackLine.Count - 1].transform.position) > 1.5f)
                    return;
                if (levelController.attackLine.Count >= 2)
                    if (levelController.attackLine[levelController.attackLine.Count - 2] == this)
                    {
                        levelController.attackLine[levelController.attackLine.Count - 1].GetComponent<MeshRenderer>().material = levelController.attackLine[levelController.attackLine.Count - 1].initialMaterial;
                        levelController.attackLine.RemoveAt(levelController.attackLine.Count - 1);
                        levelController.attackLineLengthText.text = (levelController.levelScriptableObject.attackLineLength - levelController.attackLine.Count) + "";
                        return;
                    }
            }
            if (levelController.attackLine.Count == levelController.levelScriptableObject.attackLineLength) return;
            if (levelController.attackLine.Contains(this)) return;

            GetComponent<MeshRenderer>().material = blueMaterial;
            levelController.gameController.audioSource.PlayOneShot(levelController.gameController.squareTapSound);
            levelController.attackLine.Add(this);
            levelController.attackLineLengthText.text = (levelController.levelScriptableObject.attackLineLength - levelController.attackLine.Count) + "";
        }

        internal void Attack()
        {
            StartCoroutine("AttackIEnumerator");
        }

        IEnumerator AttackIEnumerator()
        {
            GameObject i = Instantiate(bomb, levelController.gameController.headOfGunHuman.position, Quaternion.identity);
            i.transform.DOJump(transform.position, 5, 1, 1.1f);
            yield return new WaitForSeconds(1);
            i.SetActive(false);
            if (hasEnemy)
            {
                stickman.SetActive(true);
                stickman.GetComponent<Animator>().SetTrigger("Die");
                levelController.gameController.audioSource.PlayOneShot(levelController.gameController.dieSound);
                levelController.enemyCount--;
                levelController.enemyCountText.text = levelController.enemyCount + "";
                StartCoroutine("BlinkArmy");
                StartCoroutine("ShowHeadstone");
            }
            GetComponent<MeshRenderer>().material = initialMaterial;
            box.SetActive(false);
            explode.SetActive(true);
            isAttacked = true;
        }

        IEnumerator BlinkArmy()
        {
            yield return new WaitForSeconds(2.2f);
            for (int i = 0; i < 3; i++)
            {
                stickman.transform.GetChild(0).gameObject.SetActive(true);
                helmet.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                stickman.transform.GetChild(0).gameObject.SetActive(false);
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