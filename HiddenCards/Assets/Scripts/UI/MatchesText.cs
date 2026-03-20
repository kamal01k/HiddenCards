using Core;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class MatchesText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI matchesText;

        private void OnEnable()
        {
            MessageCenter.AddListener(MatchesNote.Set, SetMatches);
        }

        private void OnDisable()
        {
            MessageCenter.RemoveListener(MatchesNote.Set, SetMatches);
        }

        private void SetMatches(int value)
        {
            matchesText.text = $"Matches: {value}";
        }
    }
}