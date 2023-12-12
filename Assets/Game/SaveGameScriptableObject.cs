using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
    internal class SaveGameScriptableObject : ScriptableObject
    {
        public int level;
        public int gold;
        public string currentArmySkin;
        public string currentCastleSkin;
        public string currentBoardSkin;
    }
}