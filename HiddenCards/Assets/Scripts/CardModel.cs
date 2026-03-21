using UnityEngine;

namespace CardGame.Scripts
{
    public class CardModel
    {
        public int Id;
        public bool IsMatched;
        public bool IsRevealed;
        public Sprite Sprite;

        public CardModel(int id, Sprite sprite)
        {
            Id = id;
            Sprite = sprite;
            IsMatched = false;
            IsRevealed = false;
        }
    }
}