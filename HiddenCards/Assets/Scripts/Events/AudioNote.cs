using Core;

namespace CardGame
{
    public class AudioNote
    {
        public static readonly MsgID<Void> PlayFlip = new MsgID<Void>("Core.PlayFlip");
        public static readonly MsgID<Void> PlayMatch = new MsgID<Void>("Core.PlayMatch");
        public static readonly MsgID<Void> PlayMismatch = new MsgID<Void>("Core.PlayMismatch");
        public static readonly MsgID<Void> PlayGameOver = new MsgID<Void>("Core.PlayGameOver");
    }
}