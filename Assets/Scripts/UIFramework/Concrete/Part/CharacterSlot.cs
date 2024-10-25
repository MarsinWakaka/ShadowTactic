using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIFramework.Concrete.Part
{
    public class CharacterSlot : MonoBehaviour, IPointerClickHandler
    {
        private RectTransform _rectTransform;
        public Action<int, float> OnClick;
        public int slotIndex;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(slotIndex, _rectTransform.position.x);
        }
    }
}