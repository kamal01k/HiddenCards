using Core;
using UnityEngine;

namespace CardGame.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip flip, match, mismatch, gameOver;

        private void OnEnable()
        {
            MessageCenter.AddListener(AudioNote.PlayFlip, PlayFlip);
            MessageCenter.AddListener(AudioNote.PlayMatch, PlayMatch);
            MessageCenter.AddListener(AudioNote.PlayMismatch, PlayMismatch);
            MessageCenter.AddListener(AudioNote.PlayGameOver, PlayGameOver);
        }

        private void OnDisable()
        {
            MessageCenter.RemoveListener(AudioNote.PlayFlip, PlayFlip);
            MessageCenter.RemoveListener(AudioNote.PlayMatch, PlayMatch);
            MessageCenter.RemoveListener(AudioNote.PlayMismatch, PlayMismatch);
            MessageCenter.RemoveListener(AudioNote.PlayGameOver, PlayGameOver);
        }

        private void PlayFlip() => source.PlayOneShot(flip);
        private void PlayMatch() => source.PlayOneShot(match);
        private void PlayMismatch() => source.PlayOneShot(mismatch);
        private void PlayGameOver() => source.PlayOneShot(gameOver);
    }
}