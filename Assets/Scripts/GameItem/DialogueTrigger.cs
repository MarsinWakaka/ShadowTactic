using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int DialogueID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DialogueManager.Instance.SetDialogue_SO(DialogueID);
        GameRoot.Instance.Push(new DialogPanel());
        Destroy(gameObject);
    }
}
