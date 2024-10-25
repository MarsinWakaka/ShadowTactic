using System.Collections;
using UnityEngine;
using Universal.AudioSystem;
using Utilities;

public class DestoryFX : MonoBehaviour
{
    [SerializeField] GameObject Item;
    [SerializeField] GameObject Broken;

    [SerializeField] AudioData[] DestoryAudio;
    [SerializeField] GameObject HitFX;
    [SerializeField] Transform HitFXPos;

    SpriteRenderer sr;

    AudioSource source;
    [SerializeField] Color highLightColor = Color.red;

    //Coroutine coroutine;

    private void OnEnable()
    {
        sr = Item.GetComponent<SpriteRenderer>();
        StartCoroutine(HighLight());
    }

    IEnumerator HighLight()
    {
        while (sr != null)
        {
            sr.color = MyMath.Lerp(Color.white, highLightColor, GameManager.Instance.SinValueWithNormalize());
            yield return null;
        }
    }

    public void DestroyItem()
    {
        StopCoroutine(HighLight());

        if(source == null)
            source = gameObject.AddComponent<AudioSource>();
        if(DestoryAudio.Length > 0)
            SoundManager.Instance.PlaySFX(source, DestoryAudio);

        GameObject go = Instantiate(HitFX, HitFXPos.transform.position, HitFXPos.transform.rotation);
        go.transform.localScale = new Vector3(-transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
        Destroy(Item);
        Broken.SetActive(true);

        EnterHibernate();
    }

    void EnterHibernate()
    {
        var rg = gameObject.GetComponent<Rigidbody2D>();
        if(rg != null) 
        {
            rg.gravityScale = 1;
            rg.velocity = new Vector2(rg.velocity.x / 2, 2f);
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(StopAudio());
    }

    IEnumerator StopAudio()
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        source.enabled = false;
    }
}