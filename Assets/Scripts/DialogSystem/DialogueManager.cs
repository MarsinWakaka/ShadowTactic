
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public DialogueData_SO[] dialogue_SO;
    [HideInInspector] public int dialogueIndex = 0;
    private int pageIndex = 0;
    private float IntervTime = 0.02f;

    Coroutine coroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    /// <summary>
    /// ��ȡ��ǰ�����µ��ı�
    /// </summary>
    /// <returns></returns>
    public string GetCurrentText()
    {
        return dialogue_SO[dialogueIndex].dialoguePieces[pageIndex].text;
    }

    /// <summary>
    /// ���öԻ��ļ�
    /// </summary>
    /// <param name="dialogData_SO"></param>
    public void SetDialogue_SO(int index)
    {
        if(dialogue_SO.Length > index && index >= 0)
        {
            pageIndex = 0;
            dialogueIndex = index;
        }
        else
        {
            Debug.LogWarning($"illegal Index dialogue_SO.Length:{dialogue_SO.Length}, Index:{index}");
        }
    }

    public bool HasNextPage()
    {
        if(dialogue_SO[dialogueIndex].dialoguePieces.Count - 1 > pageIndex)
            return true;

        return false;
    }

    /// <summary>
    /// ����һҳ���������,����true��û�з���false
    /// </summary>
    /// <returns></returns>
    public bool NextPage(Text textBox)
    {
        if (!HasNextPage())
        {
            if(coroutine != null)//û����һҳ���ǵ�ǰҳû����ʾ��ȫ
            {
                StopCoroutine(coroutine);
                coroutine = null;
                textBox.text = dialogue_SO[dialogueIndex].dialoguePieces[pageIndex].text;
                return true;
            }
            else
            {
                return false;
            }
        }
        
        if(coroutine == null)
        {
            pageIndex++;
            coroutine = StartCoroutine(StringDamp(textBox));
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = null;
            textBox.text = dialogue_SO[dialogueIndex].dialoguePieces[pageIndex].text;
        }
        return true;
    }

    public bool HasPreviousPage()
    {
        if (pageIndex > 0)
            return true;

        return false;
    }

    public string PreviousPage()
    {
        if (!HasPreviousPage())
            return null;

        pageIndex--;
        return dialogue_SO[dialogueIndex].dialoguePieces[pageIndex].text;
    }

    IEnumerator StringDamp(Text textBox)
    {
        //StringBuilder str = new StringBuilder();
        textBox.text = "";
        //int strIndex = 0;
        string str = dialogue_SO[dialogueIndex].dialoguePieces[pageIndex].text;
        for (int strIndex = 0; strIndex < str.Length; strIndex++)
        {
            //str.Append(cr);
            textBox.text += str[strIndex];
            yield return new WaitForSecondsRealtime(IntervTime);
        }

        coroutine = null;
    }
}
