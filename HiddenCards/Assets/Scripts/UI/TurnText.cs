using Core;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class TurnText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI turnsText;

        private void OnEnable()
        {
            MessageCenter.AddListener(TurnNote.Set, SetTurns);
        }

        private void OnDisable()
        {
            MessageCenter.RemoveListener(TurnNote.Set, SetTurns);
        }

        private void SetTurns(int value)
        {
            turnsText.text = $"Turns: {value}";
        }
    }
}