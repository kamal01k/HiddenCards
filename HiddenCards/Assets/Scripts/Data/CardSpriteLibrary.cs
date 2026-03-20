using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scripts
{
    [CreateAssetMenu(menuName = "MemoryGame/Sprite Library")]
    public class CardSpriteLibrary : ScriptableObject
    {
        public List<Sprite> availableSprites;
    }
}