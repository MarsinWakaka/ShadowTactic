using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    SpriteRenderer sr;
    AudioSource source;
    [SerializeField] Transform bc;
    [SerializeField] Sprite state1;
    [SerializeField] Sprite state2;
    [SerializeField] AudioData state1EnterAudio;
    [SerializeField] AudioData state2EnterAudio;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            SoundManager.Instance.PlaySFX(source, state2EnterAudio);
            sr.sprite = state2;
            bc.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            SoundManager.Instance.PlaySFX(source, state1EnterAudio);
            sr.sprite = state1;
            bc.gameObject.SetActive(true);
        }
    }
}
