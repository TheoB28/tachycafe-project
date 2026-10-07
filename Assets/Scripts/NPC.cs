using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [SerializeField] string npcName;
    [SerializeField] Canvas canvas;
    [SerializeField] string[] dialogueLines;
    public int currentLineIndex = 0;

    PlayerOverworld player;
    Animator animator;

    private void Start()
    {

        canvas.gameObject.SetActive(false);
    }

    public void Interact(PlayerOverworld player)
    {
        canvas.gameObject.SetActive(true);
        animator = GetComponentInChildren<Animator>();
        NextText();
        this.player = player;
    }

    public void NextText()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = dialogueLines[currentLineIndex];
            animator.Play("bob");
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
}
