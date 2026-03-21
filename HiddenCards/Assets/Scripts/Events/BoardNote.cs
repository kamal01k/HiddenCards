using Core;

namespace CardGame
{
    public class BoardNote
    {
        public static readonly MsgID<int> Start = new MsgID<int>("BoardNote.Start");
        public static readonly MsgID<Void> CheckLevelComplete = new MsgID<Void>("BoardNote.CheckLevelComplete");
    }
}