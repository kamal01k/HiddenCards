using TMPro;
using UnityEngine;

public class MatchesText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI matchesText;

    private void SetMatches(int value)
    {
        matchesText.text = $"Matches: {value}";
    }
}
