using System.Collections;
using UnityEngine;

namespace CardGame.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource source;
        public AudioClip flip, match, mismatch, gameOver;

        void Awake() => Instance = this;

        public void PlayFlip() => source.PlayOneShot(flip);
        public void PlayMatch() => source.PlayOneShot(match);
        public void PlayMismatch() => source.PlayOneShot(mismatch);
        public void PlayGameOver() => source.PlayOneShot(gameOver);
    }
}