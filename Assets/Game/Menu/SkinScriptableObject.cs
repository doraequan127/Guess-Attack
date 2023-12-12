using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funzilla
{
    internal class SkinScriptableObject : ScriptableObject
    {
        public List<string> armySkinName = new List<string>();
        public List<int> armySkinPrice = new List<int>();
        public List<string> castleSkinName = new List<string>();
        public List<int> castleSkinPrice = new List<int>();
        public List<string> boardSkinName = new List<string>();
        public List<int> boardSkinPrice = new List<int>();
    }
}