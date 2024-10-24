using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable : MonoBehaviour
{
    //bool isCoolDown = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //if (isCoolDown)
            //{
            //    GameObject player = collision.gameObject;
            //    player.GetComponent<Player>().TryHide();

            //    isCoolDown = false;
            //    StartCoroutine(coolDown());
            //}
            GameObject player = collision.gameObject;
            player.GetComponent<Player>().TryHide();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Player>().CancelHideStateWithRefresh();
        }
    }

    //private IEnumerator coolDown()
    //{
    //    float scoreTimer = 0;
    //    while(scoreTimer < 2)
    //    {
    //        scoreTimer += Time.deltaTime;
    //        yield return null;
    //    }
    //    isCoolDown = true;
    //}
}
