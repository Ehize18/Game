using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Assets.Scripts;
using System.Linq;

public class Robot : MonoBehaviour
{
    public string Name = "Гарри";
    public int CollectedObjectsCount;
    public int objectsToCollectTargetCount = -1;
    public List<GameObject> ObjectsToCollect;
    public GameObject Controller;

    [SerializeField] private GameObject mailCounter;
    [SerializeField] private GameObject mailStack;
    [SerializeField] private Transform interactPos;
    [SerializeField] private float interactRadius;
    [SerializeField] private Text textForLog;
    [SerializeField] private string expectedText = "";
    [SerializeField] private LayerMask layer;
    [SerializeField] private Tilemap mazeTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private bool isSortMailLevel;

    private readonly float cellSize = 1.5f;
    private readonly float moveTime = 1;
    private float elapsedTime;
    private int mailCount = 7;
    private int lettersCount;
    private int adsCount;
    private int taxesCount;
    private Stack<MailType> mail;
    private List<MailType> lettersBasket;
    private List<MailType> adsBasket;
    private List<MailType> taxesBasket;
    private LinkedList<IOperation> operations;
    private LinkedListNode<IOperation> currentOperation;
    private bool isExecuting;
    private Vector3 levelStartPosition;
    private LevelsManager levelsManager;
    private SpriteRenderer spriteRenderer;

    public MailType Arm { get; private set; }

    private void Start()
    {
        Init();
        
        ResetPosition();

        if (interactPos != null)
            interactPos.position = transform.position;

        levelsManager = Controller.GetComponent<LevelsManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FlipSprite();
        if (mailCount == 0)
        {
            mailStack.SetActive(false);
            mailCounter.SetActive(false);

            if (taxesBasket.Count == taxesCount
                && lettersBasket.Count == lettersCount
                && adsBasket.Count == adsCount)
                HandleLevelCompletion();
        }

        if (isExecuting)
            ExecuteOperations();
    }

    public void Say(string text)
    {
        if (text == expectedText)
        {
            HandleLevelCompletion();
            Log(text);
        }
        else
            Log($"Неверный вывод: {text}\nОжидалось: {expectedText}", 30);
    }
    
    public void Move(Vector3 direction)
    {
        var lastOperation = operations.Last;
        var startPosition = levelStartPosition;

        if (operations.Count > 0)
        {
            var lastStep = operations.Where(x => x is Step)
                .Select(x => x as Step)
                .LastOrDefault();

            if (lastStep != null)
                startPosition = lastStep.EndPosition;
        }
        
        operations.AddLast(new Step(startPosition, direction));     
    }

    public void Interact(Vector3 interactDirection)
    {
        operations.AddLast(new Interaction(interactDirection, OperationType.None));
    }

    public void Take(Vector3 interactDirection)
    {
        operations.AddLast(new Interaction(interactDirection, OperationType.Take));
        Arm = mail.Pop();
    }

    public void Put(Vector3 interactDirection)
    {
        operations.AddLast(new Interaction(interactDirection, OperationType.Put));
        Arm = MailType.None;
    }

    public void Execute()
    {
        currentOperation = operations.First;
        isExecuting = true;
    }

    public void ClearLogs()
    {
        Log(string.Empty);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void Log(string text, int fontSize = 40)
    {
        textForLog.fontSize = fontSize;
        textForLog.text = text;
    }

    public void ResetOperations()
    {
        operations = new LinkedList<IOperation>();
    }

    public void ResetPosition()
    {
        transform.position = levelStartPosition;
    }

    public void Init()
    {
        Arm = MailType.None;

        if (isSortMailLevel)
        {
            ResetMails();
            FIllMail();
            CountMails();
        }

        isExecuting = false;

        ResetOperations();

        ResetPositions();
    }

    private void ResetMails()
    {
        mail = new Stack<MailType>();
        lettersBasket = new List<MailType>();
        adsBasket = new List<MailType>();
        taxesBasket = new List<MailType>();
    }

    private void FIllMail()
    {
        var rnd = new System.Random();

        for (var i = 0; i < 7; i++)
        {
            mail.Push((MailType)rnd.Next(3));
            Debug.Log($"push {mail.Peek()}");
        }
    }

    private void CountMails()
    {
        lettersCount = mail.Where(x => x is MailType.Letter).Count();
        taxesCount = mail.Where(x => x is MailType.Tax).Count();
        adsCount = mail.Where(x => x is MailType.Ad).Count();
    }

    private void ResetPositions()
    {
        levelStartPosition = transform.position;
        if (interactPos != null)
            interactPos.position = transform.position;
    }

    private void ExecuteOperations()
    {
        if (currentOperation is null)
        {
            isExecuting = false;
            return;
        }

        switch (currentOperation.Value.Type)
        {
            case OperationType.Step:
                SmoothMove();
                break;
            case OperationType.Take:
                TakeItem();
                break;
            case OperationType.Put:
                PutItem();
                break;
            case OperationType.None:
                InteractWithItem();
                break;
        }
    }

    private void SmoothMove()
    {
        var step = currentOperation.Value as Step;

        elapsedTime += Time.deltaTime;

        if (CanMove(step.EndPosition))
        {
            transform.position = Vector3.Lerp(step.StartPosition,
                step.EndPosition, elapsedTime / moveTime);
            Log($"Move {step.Direction.GetDirectionName()}");
        }
        else
        {
            Log($"Разбился о стену! (Move {step.Direction.GetDirectionName()})");
            isExecuting = false;
            ResetOperations();
            return;
        }

        if (elapsedTime > moveTime)
        {
            currentOperation = currentOperation.Next;
            elapsedTime = 0;
        }
    }

    private void TakeItem()
    {
        mailCount--;
        var obj = GetObjectToInteract();

        if (obj != null)
        {
            var counter = GameObject.Find(obj.name).GetComponent<Text>();
            counter.text = mailCount.ToString();
        }

        currentOperation = currentOperation.Next;
    }

    private void PutItem()
    {
        var obj = GetObjectToInteract();
        Debug.Log(obj.name);

        if (obj != null)
        {
            var counter = GameObject.Find(obj.name).GetComponent<Text>();
            switch (obj.name)
            {
                case "Taxes Counter":
                    taxesBasket.Add(Arm);
                    counter.text = taxesBasket.Count.ToString();
                    break;
                case "Letters Counter":
                    lettersBasket.Add(Arm);
                    counter.text = lettersBasket.Count.ToString();
                    break;
                case "Ads Counter":
                    adsBasket.Add(Arm);
                    counter.text = adsBasket.Count.ToString();
                    break;
            }
        }

        currentOperation = currentOperation.Next;
    }

    private void InteractWithItem()
    {
        var obj = GetObjectToInteract();

        if (obj != null)
        {
            obj.gameObject.SetActive(false);
            CollectedObjectsCount++;
        }

        currentOperation = currentOperation.Next;
    }

    private void HandleLevelCompletion()
    {
        levelsManager.UnlockNextLevel();
        levelsManager.ShowNextLevelButton();
    }

    private Collider2D GetObjectToInteract()
    {
        var interaction = currentOperation.Value as Interaction;
        interactPos.position = transform.position + interaction.Direction * cellSize;

        return Physics2D.OverlapCircle(interactPos.position, interactRadius);
    }

    private bool CanMove(Vector3 end)
    {
        var gridPosition = mazeTilemap.WorldToCell(end);

        return !collisionTilemap.HasTile(gridPosition);
    }

    private void FlipSprite()
    {
        if (currentOperation is null || Name != "Ларри") return;
        
        var direction = currentOperation.Value.Direction;
        if (direction == Vector3.right)
            transform.eulerAngles = new Vector3(0, 0, -90);
        if (direction == Vector3.left)
            transform.eulerAngles = new Vector3(0, 0, 90);
        if (direction == Vector3.up)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (direction == Vector3.down)
            transform.eulerAngles = new Vector3(0, 0, 180);
    }
}