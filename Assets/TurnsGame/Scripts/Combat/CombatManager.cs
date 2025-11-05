using UnityEngine;

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
        //player = SpawnCharacter(Constants.IS_PLAYER_ONE);
        //player.name = "PlayerOne";
        //enemy = SpawnCharacter(!Constants.IS_PLAYER_ONE);
        //enemy.name = "PlayerTwo";

        // PROVISIONAL
        Vector3 screenPosition = UI.Instance.uiPlayerOnePosition.position;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        playerOne.transform.position = worldPosition;
        playerOne.Setup(Constants.IS_PLAYER_ONE);
        player = playerOne;

        screenPosition = UI.Instance.uiPlayerTwoPosition.position;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        playerTwo.transform.position = worldPosition;
        playerTwo.Setup(!Constants.IS_PLAYER_ONE);
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
                player.PerformAction(enemy);
                enemy.PerformAction(player);
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

        float playerChance = player.counterChance;
        float enemyChance = enemy.counterChance;

        for (int i = 1; i <= 3; i++)
        {
            playerAttack.prowessBonus -= Random.Range(0, 4) / 10f;
            enemyAttack.prowessBonus -= Random.Range(0, 4) / 10f;
        }
        if (IsCounter(playerChance) && IsCounter(enemyChance))
        {
            (float playerGain, float enemyGain) =
                    playerChance > enemyChance ? (Constants.COUNTER_PROWESS_GAIN, Constants.COUNTER_PROWESS_LOSS) :
                    playerChance < enemyChance ? (Constants.COUNTER_PROWESS_LOSS, Constants.COUNTER_PROWESS_GAIN) :
                    (Random.value < 0.5f ? (Constants.COUNTER_PROWESS_GAIN, Constants.COUNTER_PROWESS_LOSS) :
                                            (Constants.COUNTER_PROWESS_LOSS, Constants.COUNTER_PROWESS_GAIN));

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            string counterWinner = playerGain > enemyGain ? player.username : enemy.username;
            UI.AddAnimation(UI.Instance.WriteText($"{counterWinner} gets a counter!"));
        }
        else if (IsCounter(playerChance) || IsCounter(enemyChance))
        {
            (float playerGain, float enemyGain) = IsCounter(playerChance) ? 
                (Constants.COUNTER_PROWESS_GAIN, Constants.COUNTER_PROWESS_LOSS) : 
                (Constants.COUNTER_PROWESS_LOSS, Constants.COUNTER_PROWESS_GAIN);

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            string counterWinner = playerGain > enemyGain ? player.username : enemy.username;
            UI.AddAnimation(UI.Instance.WriteText($"{counterWinner} gets a counter!"));
        }
        Debug.Log(playerAttack.prowessBonus);
        Debug.Log(enemyAttack.prowessBonus);
    }

    bool IsCounter(float chance)
    {
        float randomValue = Random.Range(0f, 1f);
        return chance >= randomValue;
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
