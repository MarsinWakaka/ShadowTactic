using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace UIFramework.Concrete.Part
{
    public class CharacterChooseUI : MonoBehaviour
    {
        [SerializeField] private RectTransform chooseBorder;
        [SerializeField] private CharacterSlot[] slots;

        private void Start()
        {
            slots = GetComponentsInChildren<CharacterSlot>();
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotIndex = i;
                slots[i].OnClick += UpdateBorderPosition;
            }
        }

        private void UpdateBorderPosition(int slotIndex, float p)
        {
            GameManager.Instance.SetCharacterSelected(slotIndex);
            chooseBorder.position += new Vector3(p - chooseBorder.position.x, 0, 0);
            // StopCoroutine(UpdatePosition(p));
            // StartCoroutine(UpdatePosition(p));
        }
        //
        // readonly float _duration = .5f;
        // // 缓慢
        // private IEnumerator UpdatePosition(float p)
        // {
        //     var timer = 0f;
        //     var startP = chooseBorder.position;
        //     var deltaX = p - chooseBorder.position.x;
        //     var factor = deltaX / _duration;
        //     while (timer <= _duration)
        //     {
        //         timer += Time.deltaTime;
        //         chooseBorder.position = startP + new Vector3(timer * factor, 0, 0);
        //         yield return null;
        //     }
        // }
    }
}