using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Scripts
{
    public class CardView : MonoBehaviour
    {
        public Action<CardView> OnClicked;

        [SerializeField] Image frontImage;
        [SerializeField] Transform visual;
        [SerializeField] GameObject front;
        [SerializeField] GameObject back;

        CanvasGroup group;

        public CardModel Model { get; private set; }

        bool animating;

        public void Init(CardModel model)
        {
            if (!group && group == null)
            {
                group = GetComponent<CanvasGroup>();
            }
            Model = model;

            frontImage.sprite = model.Sprite;

            ShowBackInstant();
            transform.localScale = Vector3.one;
            group.alpha = 1f;
        }

        public void Click()
        {
            if (animating || Model.IsMatched) return;
            OnClicked?.Invoke(this);
        }

        public async Task Flip(bool showFront)
        {
            if (animating) return;
            animating = true;

            await RotateY(0, 90, 0.15f);

            front.SetActive(showFront);
            back.SetActive(!showFront);

            await RotateY(90, 0, 0.15f);

            animating = false;
        }

        public async Task HideMatched()
        {
            Model.IsMatched = true;

            float t = 0;
            float duration = 0.3f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float normalized = t / duration;

                group.alpha = Mathf.Lerp(1f, 0f, normalized);
                await Task.Yield();
            }

            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        async Task RotateY(float from, float to, float duration)
        {
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                float angle = Mathf.Lerp(from, to, t);
                visual.localRotation = Quaternion.Euler(0, angle, 0);
                await Task.Yield();
            }
        }

        void ShowBackInstant()
        {
            front.SetActive(false);
            back.SetActive(true);
            visual.localRotation = Quaternion.identity;
        }
    }
}