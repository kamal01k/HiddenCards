using UnityEngine;
using UnityEditor;

namespace CardGame
{
    public class PlayerPrefsTools
    {
        [MenuItem("Tools/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs cleared!");
        }
    }
}