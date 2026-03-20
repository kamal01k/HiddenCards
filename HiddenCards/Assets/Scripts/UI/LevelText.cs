using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;

    private void SetLevel(int value)
    {
        levelText.text = $"Level: {value}";
    }
}
