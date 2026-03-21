using Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.Scripts
{
    public class GameManager : MonoBehaviour
    {
        List<CardView> revealed = new();

        int turns;
        int matches;
        int level = 1;
        bool isEvaluating;

        private void OnEnable()
        {
            MessageCenter.AddListener(CardNote.OnClick, OnCardClicked);
            MessageCenter.AddListener(GameNote.OnComplete, OnLevelComplete);
        }

        private void OnDisable()
        {
            MessageCenter.RemoveListener(CardNote.OnClick, OnCardClicked);
            MessageCenter.RemoveListener(GameNote.OnComplete, OnLevelComplete);
        }

        void Start()
        {
            LoadProgress();
            StartLevel();
        }

        void LoadProgress()
        {
            level = SaveSystem.Load();
            MessageCenter.Send(LevelNote.Set, level);
        }

        void StartLevel()
        {
            MessageCenter.Send(BoardNote.Start, level);
        }

        async void OnCardClicked(CardView card)
        {
            if (isEvaluating) return;
            if (card.Model.IsRevealed || card.Model.IsMatched) return;

            card.Model.IsRevealed = true;

            await card.Flip(true);
            MessageCenter.Send(AudioNote.PlayFlip);

            revealed.Add(card);

            if (revealed.Count == 2)
            {
                isEvaluating = true;
                await EvaluatePair();
                isEvaluating = false;
            }
        }

        async Task EvaluatePair()
        {
            turns++;
            MessageCenter.Send(TurnNote.Set, turns);

            var a = revealed[0];
            var b = revealed[1];

            if (a.Model.Id == b.Model.Id)
            {
                matches++;
                MessageCenter.Send(MatchesNote.Set, matches);
                MessageCenter.Send(AudioNote.PlayMatch);

                await Task.Delay(250);

                await a.HideMatched();
                await b.HideMatched();
            }
            else
            {
                MessageCenter.Send(AudioNote.PlayMismatch);

                await Task.Delay(600);
                
                await a.Flip(false);
                await b.Flip(false);

                a.Model.IsRevealed = false;
                b.Model.IsRevealed = false;
            }

            revealed.Clear();
            CheckLevelComplete();
        }

        void CheckLevelComplete()
        {
            MessageCenter.Send(BoardNote.CheckLevelComplete);
        }

        void OnLevelComplete()
        {
            level++;
            SaveSystem.Save(level);
            MessageCenter.Send(LevelNote.Set, level);
            MessageCenter.Send(AudioNote.PlayGameOver);
            StartLevel();
        }
    }
}