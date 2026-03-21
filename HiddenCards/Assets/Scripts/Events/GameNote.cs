using Core;

namespace CardGame
{
    public class GameNote
    {
        public static readonly MsgID<Void> OnComplete = new MsgID<Void>("GameNote.OnComplete");
    }
}