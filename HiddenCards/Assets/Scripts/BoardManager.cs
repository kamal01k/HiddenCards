using Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Scripts
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] GridLayoutGroup grid;
        [SerializeField] CardPool pool;
        [SerializeField] CardSpriteLibrary spriteLibrary;

        List<CardView> activeCards = new();

        private void OnEnable()
        {
            MessageCenter.AddListener(BoardNote.Start, StartBoard);
            MessageCenter.AddListener(BoardNote.CheckLevelComplete, CheckLevelComplete);
        }

        private void OnDisable()
        {
            MessageCenter.RemoveListener(BoardNote.Start, StartBoard);
            MessageCenter.RemoveListener(BoardNote.CheckLevelComplete, CheckLevelComplete);
        }

        private void StartBoard(int level)
        {
            StartLevel(level);
        }

        Task StartLevel(int level)
        {
            var size = GetLevelSize(level);
            var data = GenerateCards(size.x * size.y);

            BuildBoard(size.x, size.y, data);

            StartPreview();
            return Task.CompletedTask;
        }

        async void StartPreview()
        {
            var cards = GetCards();

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

        public void BuildBoard(int rows, int cols, List<CardModel> data)
        {
            Clear();

            float size = CalculateCardSize(rows, cols);
            grid.cellSize = new Vector2(size, size);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = cols;

            foreach (var model in data)
            {
                var card = pool.Get();
                card.transform.SetParent(grid.transform, false);
                card.Init(model);
                activeCards.Add(card);
            }
        }

        private Vector2Int GetLevelSize(int level)
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

        void Clear()
        {
            foreach (var c in activeCards)
                pool.Release(c);

            activeCards.Clear();
        }

        float CalculateCardSize(int rows, int cols)
        {
            RectTransform rect = grid.GetComponent<RectTransform>();

            float totalWidth = rect.rect.width;
            float totalHeight = rect.rect.height;

            float spacingX = grid.spacing.x * (cols - 1);
            float spacingY = grid.spacing.y * (rows - 1);

            float paddingX = grid.padding.left + grid.padding.right;
            float paddingY = grid.padding.top + grid.padding.bottom;

            float availableWidth = totalWidth - spacingX - paddingX;
            float availableHeight = totalHeight - spacingY - paddingY;

            float cellWidth = availableWidth / cols;
            float cellHeight = availableHeight / rows;

            return Mathf.Floor(Mathf.Min(cellWidth, cellHeight));
        }

        private List<CardView> GetCards() => activeCards;

        void CheckLevelComplete()
        {
            if (GetCards().All(c => c.Model.IsMatched))
            {
                MessageCenter.Send(GameNote.OnComplete);
            }
        }
    }
}