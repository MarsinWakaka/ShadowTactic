using System.Collections;
using UnityEngine;
using Universal.AudioSystem;

namespace GameItem
{
    public class StoneDoor : MonoBehaviour
    {
        private AudioSource _source;
        DestoryFX _dfx;
        [SerializeField] AudioData closeAudio;
        [SerializeField] AudioData fallingAudio;

        bool _isTrigger;
    
        void Start()
        {
            _dfx = GetComponent<DestoryFX>();
            _source = GetComponent<AudioSource>();
            _dfx.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isTrigger)
                return;
        
            if (collision.CompareTag("Player"))
            {
                SoundManager.Instance.PlaySFX(_source, fallingAudio);
                StartCoroutine(StartFall());
                _isTrigger = true;
            }
        }

        IEnumerator StartFall()
        {
            float timer = 0;
            while (timer < 1f)
            {
                timer += Time.deltaTime;
                transform.position -= new Vector3 (0.0f, 1.5f, 0.0f) * Time.deltaTime;
                yield return null;
            }
            //SoundManager.Instance.PlaySFX(source, closeAudio);
        }
    }
}