using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Dialogue
{
    public class DialogueInput : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}