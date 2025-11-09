using UnityEngine;
using static MyProject.Constants;

public enum CombatState { START, CHOOSE, ACTION, END }

public class CombatManager : MonoBehaviour
{
    // PROVISIONAL (WHEN USELESS, PLAYER AND ENEMY WILL GET THESE NAMES)
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
        //player = SpawnCharacter(IS_PLAYER_ONE);
        //player.name = "PlayerOne";
        //enemy = SpawnCharacter(!IS_PLAYER_ONE);
        //enemy.name = "PlayerTwo";

        // PROVISIONAL
        Vector3 screenPosition = CombatUI.Instance.uiPlayerOnePosition.position;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        playerOne.transform.position = worldPosition;
        playerOne.Setup(IS_PLAYER_ONE);
        player = playerOne;

        screenPosition = CombatUI.Instance.uiPlayerTwoPosition.position;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        playerTwo.transform.position = worldPosition;
        playerTwo.Setup(!IS_PLAYER_ONE);
        enemy = playerTwo;
        //

        CombatUI.AddAnimation(CombatUI.Instance.WriteText("Begin match"));
        RoundStart();
    }

    CharacterManager SpawnCharacter(bool isPlayer)
    {
        Vector3 screenPosition;
        Vector3 worldPosition;

        if (isPlayer)
        {
            screenPosition = CombatUI.Instance.uiPlayerOnePosition.position;
            worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0f;
        }
        else
        {
            screenPosition = CombatUI.Instance.uiPlayerTwoPosition.position;
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
        player.state = PlayerState.CHOOSE;
        enemy.state = PlayerState.CHOOSE;
        player.action = new();
        enemy.action = new();
        player.ApplyEffects();
        enemy.ApplyEffects();

        state = CombatState.CHOOSE;
        // TO-DO: activate attack and block buttons

        AIAction();
        if (player.state == PlayerState.WAIT)
        {
            state = CombatState.ACTION;
            PerformRound();
        }
    }

    void AIAction()
    {
        int randomChoice = Random.Range(0, 2);
        if (enemy.state == PlayerState.WAIT) return;
        if (randomChoice == 1 && enemy.shieldMeter.GetAvailableCharges() > 0) enemy.action = new Block();
        else enemy.action = new Attack();
    }

    public void OnAttackButton()
    {
        if (player.state != PlayerState.CHOOSE) return;
        player.state = PlayerState.WAIT;
        state = CombatState.ACTION;
        player.action = new Attack();

        PerformRound();
    }

    public void OnBlockButton()
    {
        if (player.state != PlayerState.CHOOSE || player.shieldMeter.GetAvailableCharges() <= 0) return;
        player.state = PlayerState.WAIT;
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
                CombatUI.AddAnimation(CombatUI.Instance.WriteText(player.name + " attacks " + enemy.name));
                player.PerformAction(enemy);
                enemy.PerformAction(player);
                break;

            case (Block, Block):
                CombatUI.AddAnimation(CombatUI.Instance.WriteText("Both players block what the fuck"));
                break;

            case (Block, Attack):
                CombatUI.AddAnimation(CombatUI.Instance.WriteText(enemy.name + " attacks " + player.name));
                enemy.PerformAction(player);
                player.PerformAction(enemy);
                break;
        }
        RoundEnd();
    }

    void Clash(Attack playerAttack, Attack enemyAttack)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText("A clash is happening!"));

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
                    playerChance > enemyChance ? (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) :
                    playerChance < enemyChance ? (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN) :
                    (Random.value < 0.5f ? (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) :
                                            (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN));

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            string counterWinner = playerGain > enemyGain ? player.username : enemy.username;
            CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{counterWinner} gets a counter!"));
        }
        else if (IsCounter(playerChance) || IsCounter(enemyChance))
        {
            (float playerGain, float enemyGain) = IsCounter(playerChance) ? 
                (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) : 
                (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN);

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            string counterWinner = playerGain > enemyGain ? player.username : enemy.username;
            CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{counterWinner} gets a counter!"));
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
        if (state != CombatState.END)
        {
            roundNumber++;

            CombatUI.AddAnimation(CombatUI.Instance.WriteText("Round " + roundNumber));
            CombatUI.AddAnimation(CombatUI.Instance.WriteText("Choose your action", waitTime: 0f));
            StartCoroutine(CombatUI.Instance.ExecuteAnimations());
        }
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
            CombatUI.AddAnimation(CombatUI.Instance.WriteText("Both players have fallen. The match ends in a draw!"));
        else
        {
            (var winner, var loser) = player.IsDead() ? (enemy, player) : (player, enemy);
            CombatUI.AddAnimation(CombatUI.Instance.WriteText(winner.name + " wins the match!"));
        }
        StartCoroutine(CombatUI.Instance.ExecuteAnimations());
    }
}
