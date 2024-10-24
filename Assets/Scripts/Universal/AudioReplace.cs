using Assets.Scripts.GameItem;
using System.Collections;
using UnityEngine;

public class AudioReplace : MonoBehaviour
{
    [SerializeField] StepAudio EnterAudio;
    [SerializeField] StepAudio LeaveAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().SetStepAudio(EnterAudio);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().SetStepAudio(LeaveAudio);
        }
    }
}