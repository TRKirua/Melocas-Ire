using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiallogueEndManager : MonoBehaviour
{

    public Text nameText;
    public Text diallogueText;
    public Image BoxText;
    private Queue<string> sentences;
    private int i;

    private Color color;
    public Animator animator;
    public bool HasEnded;
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        
        color = nameText.color;
        color.a = 0;
        nameText.color = color;
        diallogueText.color = color;
        GetComponent<DiallogueEnd>().oldcolor = BoxText.color;
        BoxText.color = color;
    }

    public void StartDiallogue(DiallogueEnd dialogue)
    {
        animator.SetBool("isOpen", true);
        
        nameText.text = dialogue.name;

        sentences.Clear();
        if (dialogue.sentences.Length > i)
        {
            string sentence = dialogue.sentences[i];
            sentences.Enqueue(sentence);
            i += 1;
        }
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            animator.SetBool("isOpen", false);
            nameText.color = color;
            BoxText.color = color;
            diallogueText.color = color;
            HasEnded = true;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(Typesentence(sentence));
    }

    IEnumerator Typesentence(string sentence)
    {
        diallogueText.text = "";
        foreach (char letter in sentence)
        {
            diallogueText.text += letter;
            yield return null;
        }
    }
}