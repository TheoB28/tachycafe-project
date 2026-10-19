using System.Collections;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerOverworld : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [Header("thoughts")]
    [SerializeField] TextMeshProUGUI thoghts;
    [SerializeField] float TextDelay = 0.1f;
    [SerializeField] float DisplayTime = 2;
    [SerializeField] float EraseTime = 0.05f;

    [Header("Menus")]
    [SerializeField] Canvas PauseMenu;
    [SerializeField] public Button MainMenuButton;
    [SerializeField] GameObject QuestMenu;
    [SerializeField] GameObject QuestHolder;
    [SerializeField] GameObject QuestPrefab;
    [SerializeField] TextMeshProUGUI QuestName;
    [SerializeField] TextMeshProUGUI QuestDescription;
    [SerializeField] float scrollIntencity;

    float baseTextDelay, BaseEraseDelay;
    public bool InMenu, InDialoge, writing;

    Vector2 movementInput;
    Vector2 lookvector;

    public SiteOfDisproportion currentSite;
    public NPC currentNPC;
    Rigidbody2D Rigidbody;
    EventSystem eventSystem;
    SceneLoader sceneLoader;
    PlayerDataHandler playerDataHandler;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        eventSystem = eventSystem ?? EventSystem.current;
        sceneLoader = FindAnyObjectByType<SceneLoader>();
        playerDataHandler = FindAnyObjectByType<PlayerDataHandler>();
        baseTextDelay = TextDelay;
        BaseEraseDelay = EraseTime;
    }

    void OnMove(InputValue value)
    {
        if (InMenu) 
        {
            if (currentSite != null)
            {
                currentSite.OnPlayerMove(value);
            }
            Rigidbody.linearVelocity = Vector2.zero;

            return; 
        }
        movementInput = value.Get<Vector2>();
        if (movementInput != Vector2.zero)
        {
            lookvector = movementInput;
        }
        Rigidbody.linearVelocity = movementInput * moveSpeed;
    }

    void OnCancel()
    {
        if (InMenu && InDialoge || currentSite == null) { return; }
        if (InMenu && currentSite != null)
        {
            currentSite.OnPlayerCancel();
        }
        else if (InMenu && currentSite.InMainTab)
        {
            currentSite = null;
        }
    }

    void OnSubmit()
    {
        if (InMenu && currentSite != null)
        {
            currentSite.OnPlayerSubmit();
        }
        else if (InMenu && InDialoge)
        {
            currentNPC.NextText();
        }
        else if (!InMenu)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(lookvector.x, lookvector.y, 0), lookvector, 1);
            if (hit.collider != null && hit.transform.CompareTag("NPC"))
            {
                hit.transform.GetComponent<NPC>().Interact(this);
                currentNPC = hit.transform.GetComponent<NPC>();
                InMenu = true;
                InDialoge = true;
            } else if (hit.collider != null && hit.transform.GetComponent<ObservableObject>())
            {
                observe(hit.transform.GetComponent<ObservableObject>().Descript(playerDataHandler.playerData[0].HasDysphoria));
            }
        }
    }

    void OnEsc()
    {
        Pause();
    }

    void Pause()
    {
        if (PauseMenu.gameObject.activeSelf) {
            PauseMenu.gameObject.SetActive(false); 
            InMenu = false;
            Time.timeScale = 1;
            return;
        }
        if (currentSite != null) {return; }
        PauseMenu.gameObject.SetActive(true);
        InMenu = true;
        Rigidbody.linearVelocity = Vector2.zero;
        Time.timeScale = 0;
        eventSystem.SetSelectedGameObject(MainMenuButton.gameObject);
    }

    public void ToMain()
    {
        InMenu = false;
        Time.timeScale = 1;
        sceneLoader.GoToMainMenu();
    }

    public void OpenQuestMenu()
    {
        QuestMenu.gameObject.SetActive(true);

        foreach (Quest quest in playerDataHandler.Quests)
        {
            GameObject newQuest = Instantiate(QuestPrefab, QuestHolder.transform);
            newQuest.GetComponent<TextMeshProUGUI>().text = quest.Name;
            newQuest.GetComponent<UIItem>().quest = quest;
        }
    }

    public void CloseQuestMenu()
    {
        QuestMenu.gameObject.SetActive(false);
    }

    public void MoveScroll(Transform newTransform)
    {
        RectTransform rectTransform = QuestHolder.GetComponent<RectTransform>();
        scrollIntencity = rectTransform.GetComponent<VerticalLayoutGroup>().spacing + newTransform.transform.localScale.y;
        if (lookvector.y > 0)
        {
            rectTransform.position -= new Vector3(0, scrollIntencity);
        }
        else if (lookvector.y < 0)
        {
            rectTransform.position += new Vector3(0, scrollIntencity);
        }

    }

    public void addQuestDescription(Quest quest)
    {
        QuestName.text = quest.Name;
        QuestDescription.text = quest.Description;
        QuestDescription.GetComponent<RectTransform>().sizeDelta = new Vector2(500, QuestDescription.fontSize * QuestDescription.textInfo.lineCount);
    }


    public void observe(string Text)
    {
        if (writing) { return; }
        StartCoroutine(ShowText(Text));
    }

    IEnumerator ShowText(string Text)
    {
        if (playerDataHandler.playerData[0].HasDysphoria)
        {
            TextDelay = baseTextDelay * 1.5f;
            EraseTime = BaseEraseDelay * 0.75f;
        }
        writing = true;
        string currentText;
        for (int i = 0; i < Text.Length; i++)
        {
            currentText = Text.Substring(0, i);
            thoghts.text = currentText;
            yield return new WaitForSeconds(TextDelay);
        }
        thoghts.text = Text;

        yield return new WaitForSeconds(DisplayTime);
        for (int i = Text.Length; i > 0; i--)
        {
            currentText = Text.Substring(0, i);
            thoghts.text = currentText;
            yield return new WaitForSeconds(EraseTime);
        }
        thoghts.text = "";
        writing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(lookvector.x, lookvector.y, 0), (Vector2)transform.position + lookvector);
    }
}
