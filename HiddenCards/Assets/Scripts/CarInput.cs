using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGame.Scripts
{
    public class CardInput : MonoBehaviour, IPointerClickHandler
    {
        // Cached Reference
        [SerializeField] private CardView view;

        public void OnPointerClick(PointerEventData eventData)
        {
            view.Click();
        }
    }
}