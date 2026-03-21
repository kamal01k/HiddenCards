using CardGame.Scripts;
using Core;

namespace CardGame
{
    public class CardNote
    {
        public static readonly MsgID<CardView> OnClick = new MsgID<CardView>("CardNote.OnClick");
    }
}