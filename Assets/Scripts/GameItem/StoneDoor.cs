using System.Collections;
using System.Threading;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class StoneDoor : MonoBehaviour
{
    private AudioSource source;
    DestoryFX dfx;
    [SerializeField] AudioData closeAudio;
    [SerializeField] AudioData fallingAudio;

    bool isTrigger;

    //Rigidbody2D rg;
    // Use this for initialization
    void Start()
    {
        //rg = GetComponent<Rigidbody2D>();
        dfx = GetComponent<DestoryFX>();
        source = GetComponent<AudioSource>();
        dfx.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
            return;
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(source, fallingAudio);
            StartCoroutine(StartFall());
            isTrigger = true;
            //this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //public void SetDestoryable()
    //{
    //    dfx.enabled = true;
    //}

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