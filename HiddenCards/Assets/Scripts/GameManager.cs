using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] BoardManager board;
        [SerializeField] CardSpriteLibrary spriteLibrary;

        List<CardView> revealed = new();

        int turns;
        int matches;
        int level = 1;
        bool isEvaluating;

        void Start()
        {
            LoadProgress();
            StartLevel();
        }

        void LoadProgress()
        {
            level = SaveSystem.Load();
            UIManager.Instance.SetLevel(level);
        }

        Vector2Int GetLevelSize(int level)
        {
            return level switch
            {
                1 => new Vector2Int(2, 2),
                2 => new Vector2Int(2, 3),
                3 => new Vector2Int(3, 4),
                4 => new Vector2Int(4, 5),
                _ => new Vector2Int(5, 6),
            };
        }

        List<CardModel> GenerateCards(int total)
        {
            int pairs = total / 2;

            var sprites = new List<Sprite>(spriteLibrary.availableSprites);

            if (sprites.Count < pairs)
            {
                Debug.LogError("Not enough sprites in library!");
                return null;
            }

            // Shuffle sprite list
            sprites = sprites.OrderBy(x => Random.value).ToList();

            List<CardModel> result = new();

            for (int i = 0; i < pairs; i++)
            {
                Sprite chosen = sprites[i];

                result.Add(new CardModel(i, chosen));
                result.Add(new CardModel(i, chosen));
            }

            return result.OrderBy(x => Random.value).ToList();
        }

        Task StartLevel()
        {
            var size = GetLevelSize(level);
            var data = GenerateCards(size.x * size.y);

            board.BuildBoard(size.x, size.y, data);

            foreach (var c in board.GetCards())
                c.OnClicked += OnCardClicked;

            StartPreview();
            return Task.CompletedTask;
        }

        async void StartPreview()
        {
            var cards = board.GetCards();

            // Flip all at same time
            var flipOpenTasks = new List<Task>();

            foreach (var c in cards)
                flipOpenTasks.Add(c.Flip(true));

            await Task.WhenAll(flipOpenTasks);

            await Task.Delay(1500);

            // Flip all back at same time
            var flipCloseTasks = new List<Task>();

            foreach (var c in cards)
                flipCloseTasks.Add(c.Flip(false));

            await Task.WhenAll(flipCloseTasks);
        }

        async void OnCardClicked(CardView card)
        {
            if (isEvaluating) return;
            if (card.Model.IsRevealed || card.Model.IsMatched) return;

            card.Model.IsRevealed = true;

            await card.Flip(true);
            AudioManager.Instance.PlayFlip();

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
            UIManager.Instance.SetTurns(turns);

            var a = revealed[0];
            var b = revealed[1];

            if (a.Model.Id == b.Model.Id)
            {
                matches++;
                UIManager.Instance.SetMatches(matches);
                AudioManager.Instance.PlayMatch();

                await Task.Delay(250);

                await a.HideMatched();
                await b.HideMatched();
            }
            else
            {
                AudioManager.Instance.PlayMismatch();

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
            if (board.GetCards().All(c => c.Model.IsMatched))
            {
                level++;
                SaveSystem.Save(level);
                AudioManager.Instance.PlayGameOver();
                StartLevel();
            }
        }
    }
}