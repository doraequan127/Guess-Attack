using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
    internal class LevelScriptableObject : ScriptableObject
    {
        public List<int> enemyPositionList = new List<int>();
        public int openedSquareCount;
        public int attackLineLength;
        public int openedSquareCountPlayer;

        public List<int> playerPositionList1 = new List<int>();
        public List<int> playerPositionList2 = new List<int>();
        public List<int> playerPositionList3 = new List<int>();
    }
}
