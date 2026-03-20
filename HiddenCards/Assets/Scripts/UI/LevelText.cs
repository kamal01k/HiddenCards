using Core;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class LevelText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelText;

        private void OnEnable()
        {
            MessageCenter.AddListener(LevelNote.Set, SetLevel);
        }

        private void OnDisable()
        {
            MessageCenter.RemoveListener(LevelNote.Set, SetLevel);
        }

        private void SetLevel(int value)
        {
            levelText.text = $"Level: {value}";
        }
    }
}