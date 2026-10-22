using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NPC : MonoBehaviour
{
    enum Lines { normal, giveQuest, rewardQuest}

    [SerializeField] string npcName;
    [SerializeField] Canvas canvas;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] string[] dialogueLines;
    [SerializeField] Sprite[] portraits;
    [SerializeField] float TextDelay = 0.1f;
    public int currentLineIndex = 0;

    [Header("Quests")]
    [SerializeField] bool hasQuest;
    [SerializeField] Quest quest;
    [SerializeField] string[] dialogueLinesGive;
    [SerializeField] string[] dialogueLinesReward;
    [SerializeField] string[] dialogueLinesAksAboutQuest;
    public bool hasGivenReward;

    public string[] LinesToWrite;

    string currentText;
    bool writing;

    PlayerOverworld player;
    Animator animator;
    PlayerDataHandler playerDataHandler;

    private void Start()
    {
        playerDataHandler = FindAnyObjectByType<PlayerDataHandler>();
        canvas.gameObject.SetActive(false);
    }

    public void Interact(PlayerOverworld player)
    {
        this.player = player;
        LinesToWrite = dialogueLines;
        name.text = npcName;
        canvas.gameObject.SetActive(true);
        animator = GetComponentInChildren<Animator>();
        if (hasQuest)
        {
            QuestDialoge();
        }
        NextText();
  
    }

    public void NextText()
    {
        if (writing && currentLineIndex < dialogueLines.Length)
        {
            StopAllCoroutines();

            dialogText.text = LinesToWrite[currentLineIndex];
            writing = false;
            return;
        }
        if (currentLineIndex < LinesToWrite.Length - 1)
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

    void QuestDialoge()
    {
        foreach (Quest quests in playerDataHandler.Quests)
        {
            if(quests == quest) { LinesToWrite = dialogueLinesAksAboutQuest; }
            return;
        }
        foreach (Quest quests in playerDataHandler.CompleteQuests)
        {
            if (quests == quest && !hasGivenReward) { LinesToWrite = dialogueLinesReward; }
            return;
        }
        LinesToWrite = dialogueLinesGive;
        playerDataHandler.AddQuest(quest);
    }

    IEnumerator ShowText()
    {
        if(LinesToWrite.Length <= currentLineIndex) {yield break; }
        writing = true;
        for (int i = 0; i < LinesToWrite[currentLineIndex].Length; i++)
        {
            currentText = LinesToWrite[currentLineIndex].Substring(0, i);
            dialogText.text = currentText;
            yield return new WaitForSeconds(TextDelay);
        }
        dialogText.text = LinesToWrite[currentLineIndex];
        writing = false;
    }
}
