using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

public enum CombatState { START, CHOOSE, ACTION, END }

public class CombatManager : MonoBehaviour
{
    // PROVISIONAL
    [SerializeField] CharacterManager playerOne;
    [SerializeField] CharacterManager playerTwo;
    //

    public static CombatManager Instance { get; private set; }

    public CombatState state;

    [SerializeField] Transform prefabCharacter;
    public Material playerMaterial;
    public Material enemyMaterial;

    CharacterManager player;
    CharacterManager enemy;

    int roundNumber = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupMatch();
    }

    void SetupMatch()
    {
        //player = SpawnCharacter(true);
        //player.name = "PlayerOne";
        //enemy = SpawnCharacter(false);
        //enemy.name = "PlayerTwo";

        // PROVISIONAL
        Vector3 screenPosition = UI.Instance.uiPlayerOnePosition.position;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        playerOne.transform.position = worldPosition;
        playerOne.Setup(true);
        player = playerOne;

        screenPosition = UI.Instance.uiPlayerTwoPosition.position;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        playerTwo.transform.position = worldPosition;
        playerTwo.Setup(false);
        enemy = playerTwo;
        //

        UI.AddAnimation(UI.Instance.WriteText("Begin match"));
        RoundStart();
    }

    CharacterManager SpawnCharacter(bool isPlayer)
    {
        Vector3 screenPosition;
        Vector3 worldPosition;

        if (isPlayer)
        {
            screenPosition = UI.Instance.uiPlayerOnePosition.position;
            worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0f;
        }
        else
        {
            screenPosition = UI.Instance.uiPlayerTwoPosition.position;
            worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0f;
        }
        Transform characterTransform = Instantiate(prefabCharacter, worldPosition, Quaternion.identity);
        CharacterManager characterManager = characterTransform.GetComponent<CharacterManager>();
        characterManager.Setup(isPlayer);

        return characterManager;
    }

    public void RoundStart()
    {
        roundNumber++;

        UI.AddAnimation(UI.Instance.WriteText("Round " + roundNumber));
        UI.AddAnimation(UI.Instance.WriteText("Choose your action", waitTime: 0f));
        StartCoroutine(UI.Instance.ExecuteAnimations());



        //state = CombatState.CHOOSE;
        AIAction();
    }

    void AIAction()
    {
        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 1 && enemy.shieldMeter.GetAvailableCharges() > 0) enemy.action = new Block();
        else enemy.action = new Attack();
    }

    public void OnAttackButton()
    {
        if (state != CombatState.CHOOSE) return;
        state = CombatState.ACTION;
        player.action = new Attack();

        PerformRound();
    }

    public void OnBlockButton()
    {
        if (state != CombatState.CHOOSE || player.shieldMeter.GetAvailableCharges() <= 0) return;
        state = CombatState.ACTION;
        player.action = new Block();

        PerformRound();
    }

    void PerformRound()
    {
        switch ((player.action, enemy.action))
        {
            case (Attack playerAttack, Attack enemyAttack):
                Clash(playerAttack, enemyAttack);
                break;

            case (Attack, Block):
                UI.AddAnimation(UI.Instance.WriteText(player.name + " attacks " + enemy.name));
                player.PerformAction(enemy);
                enemy.PerformAction(player);
                break;

            case (Block, Block):
                UI.AddAnimation(UI.Instance.WriteText("Both players block what the fuck"));
                break;

            case (Block, Attack):
                UI.AddAnimation(UI.Instance.WriteText(enemy.name + " attacks " + player.name));
                enemy.PerformAction(player);
                player.PerformAction(enemy);
                break;
        }
        RoundEnd();
    }

    void Clash(Attack playerAttack, Attack enemyAttack)
    {
        UI.AddAnimation(UI.Instance.WriteText("A clash is happening!"));

        CharacterManager clashWinner = null;
        CharacterManager clashLoser = null;

        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 0)
        {
            clashWinner = player;
            clashLoser = enemy;
            enemyAttack.prowessBonus -= 0.8f;
        }
        else
        {
            clashWinner = enemy;
            clashLoser = player;
            playerAttack.prowessBonus -= 0.8f;
        }
        UI.AddAnimation(UI.Instance.WriteText(clashWinner.name + " wins the clash!"));
        clashWinner.PerformAction(clashLoser);
        clashLoser.PerformAction(clashWinner);
    }

    void RoundEnd()
    {
        CheckCombatState();
        if (state != CombatState.END) RoundStart();
    }

    void CheckCombatState()
    {
        if (player.IsDead() || enemy.IsDead())
        {
            state = CombatState.END;
            EndCombat();
        }
        else return;
    }

    void EndCombat()
    {
        if (player.IsDead() && enemy.IsDead()) 
            UI.AddAnimation(UI.Instance.WriteText("Both players have fallen. The match ends in a draw!"));
        else
        {
            (var winner, var loser) = player.IsDead() ? (enemy, player) : (player, enemy);
            UI.AddAnimation(UI.Instance.WriteText(winner.name + " wins the match!"));
        }
        StartCoroutine(UI.Instance.ExecuteAnimations());
    }
}
