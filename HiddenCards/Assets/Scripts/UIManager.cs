using TMPro;
using UnityEngine;

namespace CardGame.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] TextMeshProUGUI turnsText;
        [SerializeField] TextMeshProUGUI matchesText;
        [SerializeField] TextMeshProUGUI levelText;

        void Awake()
        {
            Instance = this;
        }

        public void SetTurns(int value)
        {
            turnsText.text = $"Turns: {value}";
        }

        public void SetMatches(int value)
        {
            matchesText.text = $"Matches: {value}";
        }

        public void SetLevel(int value)
        {
            levelText.text = $"Level: {value}";
        }
    }
}