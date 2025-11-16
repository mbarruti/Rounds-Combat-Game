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
        //player.username = "PlayerOne";
        //enemy = SpawnCharacter(!IS_PLAYER_ONE);
        //enemy.username = "PlayerTwo";

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
        PreRound();
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

    void PreRound()
    {
        roundNumber++;

        CombatUI.AddAnimation(CombatUI.Instance.WriteText("Round " + roundNumber));

        player.Reset();
        enemy.Reset();

        player.ApplyEffects(ROUND_START);
        enemy.ApplyEffects(ROUND_START);

        player.actionController.SetAvailableActions(player.action);
        enemy.actionController.SetAvailableActions(enemy.action);

        CombatUI.AddAnimation(CombatUI.Instance.ShowActionButtons(player.actionController));

        StartCoroutine(CombatUI.Instance.ExecuteAnimations());
    }

    public void RoundStart()
    {
        state = CombatState.CHOOSE;

        //AIAction();
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
        if (randomChoice == 1 && enemy.shieldMeter.GetAvailableCharges() > 0 &&
            player.state != PlayerState.WAIT) enemy.action = new Block(enemy, enemy.action);
        else enemy.action = new Attack(enemy, enemy.action);
    }

    public void OnAttackButton()
    {
        if (player.state != PlayerState.CHOOSE) return;
        player.state = PlayerState.WAIT;
        player.action = new Attack(player, player.action);

        PerformRound();
    }

    public void OnChargeButton()
    {
        if (player.state != PlayerState.CHOOSE) return;
        player.state = PlayerState.WAIT;
        player.action = new Charge(player, player.action);

        PerformRound();
    }

    public void OnTackleButton()
    {
        if (player.state != PlayerState.CHOOSE) return;
        player.state = PlayerState.WAIT;
        player.action = new Tackle(player, player.action);

        PerformRound();
    }

    public void OnBlockButton()
    {
        if (player.state != PlayerState.CHOOSE || player.shieldMeter.GetAvailableCharges() <= 0)
            return;
        player.state = PlayerState.WAIT;
        player.action = new Block(player, player.action);

        PerformRound();
    }

    void PerformRound()
    {
        state = CombatState.ACTION;
        CombatUI.AddAnimation(CombatUI.Instance.HideActionButtons());

        switch ((player.action, enemy.action))
        {
            case (Attack playerAttack, Attack enemyAttack):
                Clash(playerAttack, enemyAttack);
                break;

            case (Attack, Block):
                player.PerformAction(enemy);
                enemy.PerformAction(player);
                break;

            case (Block, Block):
                CombatUI.AddAnimation(
                    CombatUI.Instance.WriteText("Both players block what the fuck"));
                break;

            case (Block, Attack):
                enemy.PerformAction(player);
                player.PerformAction(enemy);
                break;
            case (_, _):
                player.PerformAction(enemy);
                enemy.PerformAction(player);
                break;
        }
        RoundEnd();
    }

    void Clash(Attack playerAttack, Attack enemyAttack)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText("A clash is happening!"));

        playerAttack.prowessBonus -= Random.Range(4, 8) / 10f;
        enemyAttack.prowessBonus -= Random.Range(4, 8) / 10f;

        float playerChance = player.counterChance;
        float enemyChance = enemy.counterChance;
        (bool playerOneCounters, int playerOneHits) = IsCounter(playerChance, player.maxNumHits);
        (bool playerTwoCounters, int playerTwoHits) = IsCounter(enemyChance, enemy.maxNumHits);
        player.numHits = playerOneHits;
        enemy.numHits = playerTwoHits;

        if (playerOneCounters && playerTwoCounters)
        {
            (float playerGain, float enemyGain) =
                playerChance > enemyChance ? (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) :
                playerChance < enemyChance ? (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN) :
                (Random.value < 0.5f ? (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) :
                                        (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN));

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            (var counterWinner, var counterLoser) =
                playerGain > enemyGain ? (player, enemy) : (enemy, player);
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{counterWinner.username} gets a counter!"));
            counterWinner.PerformAction(enemy);
            counterLoser.PerformAction(player);
        }
        else if (playerOneCounters || playerTwoCounters)
        {
            (float playerGain, float enemyGain) = playerOneCounters ?
                (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) :
                (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN);

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            (var counterWinner, var counterLoser) =
                playerGain > enemyGain ? (player, enemy) : (enemy, player);
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{counterWinner.username} gets a counter!"));
            counterWinner.PerformAction(counterLoser);
            counterLoser.PerformAction(counterWinner);
        }
        else
        {
            player.PerformAction(enemy);
            enemy.PerformAction(player);
        }
    }

    (bool, int) IsCounter(float counterChance, int numHits)
    {
        for (int hitsLeft = numHits; hitsLeft > 0; hitsLeft--)
        {
            float randomValue = Random.Range(0f, 1f);
            if (counterChance >= randomValue)
            {
                //return (true, hitsLeft);
                return (true, numHits);
            }
        }
        return (false, numHits);
    }

    void RoundEnd()
    {
        CheckCombatState();
        if (state != CombatState.END)
        {
            PreRound();
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
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText("Both players have fallen. The match ends in a draw!"));
        }
        else
        {
            (var winner, var loser) = player.IsDead() ? (enemy, player) : (player, enemy);
            CombatUI.AddAnimation(CombatUI.Instance.WriteText(winner.name + " wins the match!"));
        }
        StartCoroutine(CombatUI.Instance.ExecuteAnimations());
    }
}
