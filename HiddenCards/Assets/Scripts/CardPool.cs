using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scripts
{
    public class CardPool : MonoBehaviour
    {
        [SerializeField] CardView prefab;
        [SerializeField] int initialSize = 30;

        Queue<CardView> pool = new Queue<CardView>();

        void Awake()
        {
            for (int i = 0; i < initialSize; i++)
                Create();
        }

        CardView Create()
        {
            var card = Instantiate(prefab, transform);
            card.gameObject.SetActive(false);
            pool.Enqueue(card);
            return card;
        }

        public CardView Get()
        {
            if (pool.Count == 0)
                Create();

            var card = pool.Dequeue();
            card.gameObject.SetActive(true);
            return card;
        }

        public void Release(CardView card)
        {
            card.gameObject.SetActive(false);
            pool.Enqueue(card);
        }
    }
}