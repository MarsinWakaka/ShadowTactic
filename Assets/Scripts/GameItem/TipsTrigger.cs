using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameItem
{
    public class TipsTrigger : MonoBehaviour
    {
        [SerializeField] [TextAreaAttribute] string Info;
        [SerializeField] Transform TipsBox;
        [SerializeField] Text textBox;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                TipsBox.gameObject.SetActive(true);
                textBox.text = Info;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                TipsBox.gameObject.SetActive(false);
            }
        }
    }
}