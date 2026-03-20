using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Scripts
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] GridLayoutGroup grid;
        [SerializeField] CardPool pool;

        List<CardView> activeCards = new();

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

        public List<CardView> GetCards() => activeCards;
    }
}