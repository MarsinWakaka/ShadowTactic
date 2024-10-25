using Assets.Scripts.GameItem;
using System.Collections;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioReplace : MonoBehaviour
{
    [FormerlySerializedAs("EnterAudio")] [SerializeField] FootStepAudioType enterAudioType;
    [FormerlySerializedAs("LeaveAudio")] [SerializeField] FootStepAudioType leaveAudioType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<BasePlayer>().SetStepAudio(enterAudioType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<BasePlayer>().SetStepAudio(leaveAudioType);
        }
    }
}