using UnityEngine;

namespace CardGame.Scripts
{
    public static class SaveSystem
    {
        public static void Save(int level)
        {
            PlayerPrefs.SetInt("level", level);
        }

        public static int Load()
        {
            return PlayerPrefs.GetInt("level", 1);
        }
    }
}