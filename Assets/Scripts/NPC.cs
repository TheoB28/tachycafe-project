using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NPC : MonoBehaviour
{

    [SerializeField] string npcName;
    [SerializeField] Canvas canvas;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] string[] dialogueLines;
    [SerializeField] Sprite[] portraits;

    [SerializeField] float TextDelay = 0.1f;
    public int currentLineIndex = 0;
    string currentText;
    bool writing;

    PlayerOverworld player;
    Animator animator;

    private void Start()
    {

        canvas.gameObject.SetActive(false);
    }

    public void Interact(PlayerOverworld player)
    {
        name.text = npcName;
        canvas.gameObject.SetActive(true);
        animator = GetComponentInChildren<Animator>();
        NextText();
        this.player = player;
    }

    public void NextText()
    {
        if (writing && currentLineIndex < dialogueLines.Length)
        {
            StopAllCoroutines();
            dialogText.text = dialogueLines[currentLineIndex];
            writing = false;
            return;
        }
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            StartCoroutine(ShowText());
            animator.Play("bob");
            NextPortrait();
            currentLineIndex++;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            currentLineIndex = 0;
            player.InMenu = false;
            player.InDialoge = false;
        }
    }

    public void NextPortrait()
    {
        if(currentLineIndex >= portraits.Length) { return; }
        portrait.sprite = portraits[currentLineIndex];
    }

    IEnumerator ShowText()
    {
        if(dialogueLines.Length <= currentLineIndex) {yield break; }
        writing = true;
        for (int i = 0; i < dialogueLines[currentLineIndex].Length; i++)
        {
            currentText = dialogueLines[currentLineIndex].Substring(0, i);
            dialogText.text = currentText;
            yield return new WaitForSeconds(TextDelay);
        }
        dialogText.text = dialogueLines[currentLineIndex];
        writing = false;
    }
}
