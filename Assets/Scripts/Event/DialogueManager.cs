using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using CommonsPattern;

public class DialogueManager : SingletonManager<DialogueManager> {

	// Events
	public static event Action onDialogueStarted;
	public static event Action onDialogueEnded;
	
    public GameObject dialogueBox;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
    public GameObject characterImage;


    public static bool dialogueEnd = false;
    public static bool sentenceEnd = false;
	private Queue<string> sentences = new Queue<string>();

    private Queue<Dialogue> dialogueChunks = new Queue<Dialogue>();

    private Dialogue dequedDialogue;

    private void Start()
    {
	    // hide dialogue box on Start in case we forgot to hide it after editing
	    dialogueBox.SetActive(false);
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        if (dialogueBox == null) return;
        
        onDialogueStarted?.Invoke();

        // if player is talking then he cannot move
        FirstPersonController.isTalking = true;

        dialogueBox.SetActive(true);

        // clear current dialogue
        if(dialogueChunks.Count != 0)
            dialogueChunks.Clear();

        // add dialogues again to the queue
        foreach (Dialogue chunks in dialogues)
        {
            dialogueChunks.Enqueue(chunks);
        }

        NextChunk(); // read next chunk of dialogue
    }

    // got to the next character's turn
    private void NextChunk()
    {
        if (dialogueChunks.Count > 0)
        {
            // drop the first dialogue chunk
            Dialogue chunk = dialogueChunks.Dequeue();
            dequedDialogue = chunk; // assign it to dialogue to be removed

            nameText.GetComponent<TextMeshProUGUI>().text = chunk.speakerName;
            characterImage.GetComponent<Image>().sprite = chunk.characterImage;

            StartIndividualDialogue(chunk);
        }
        else
        {
            EndDialogue();
        }
    }

    // start a chunk of individual dialogue
    private void StartIndividualDialogue (Dialogue dialogue)
	{
		nameText.text = dialogue.speakerName;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}


    // show the next sentence
	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
            NextChunk(); // move on to the next chunk
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

    // typing a sentence out
	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
        sentenceEnd = false;
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
        sentenceEnd = true;
	}

    // ending the dialogue
	public void EndDialogue()
	{
        dialogueBox.SetActive(false);
        FirstPersonController.isTalking = false;
        dialogueEnd = true;
        sentenceEnd = true;
        
        onDialogueEnded?.Invoke();
	}
}
