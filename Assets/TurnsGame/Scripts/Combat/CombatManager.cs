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
    int turnNumber = 0; // Each round has a set number of turns for each player

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

    void Update()
    {
       //Debug.Log(player.activeBuffs.BonusDamage);
    }

    void SetupMatch()
    {
        //player = SpawnCharacter(IS_PLAYER_ONE);
        //player.username = "PlayerOne";
        //enemy = SpawnCharacter(!IS_PLAYER_ONE);
        //enemy.username = "PlayerTwo";

        // PROVISIONAL
        // Vector3 screenPosition = CombatUI.Instance.uiPlayerOnePosition.position;
        // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        // worldPosition.z = 0f;
        // playerOne.transform.position = worldPosition;
        playerOne.Setup(IS_PLAYER_ONE);
        player = playerOne;

        // screenPosition = CombatUI.Instance.uiPlayerTwoPosition.position;
        // worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        // worldPosition.z = 0f;
        // playerTwo.transform.position = worldPosition;
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

        player.actionController.SetAvailableActions();
        enemy.actionController.SetAvailableActions();

        CombatUI.AddAnimation(CombatUI.Instance.ShowActionButtons(player.actionController));
        StartCoroutine(CombatUI.Instance.ExecuteAnimations());
    }

    public void RoundStart()
    {
        state = CombatState.CHOOSE;

        //AIAction();
        if (player.state == PlayerState.Wait)
        {
            state = CombatState.ACTION;
            PerformRound();
        }
    }

    void AIAction()
    {
        int randomChoice = Random.Range(0, 2);
        if (enemy.state == WAIT) return;
        if (randomChoice == 1 && enemy.shieldMeter.GetAvailableCharges() > 0)
            enemy.action = enemy.attackSO.CreateAction(); // BLOCK
        else enemy.action = enemy.attackSO.CreateAction(); // ATTACK
    }

    public void OnAttackButton()
    {
        if (!player.attackSO.CanCreateAction(player))
            return;
        player.state = OFFENSE;
        player.action = player.attackSO.CreateAction();

        PerformRound();
    }

    public void OnBlockButton()
    {
        if (!player.blockSO.CanCreateAction(player))
            return;
        player.state = DEFENSE;
        player.action = player.blockSO.CreateAction();

        PerformRound();
    }

    public void OnWeaponSpecialButton(int index)
    {
        if (index < 0 || index >= player.weapon.SpecialActions.Count
            || player.state == WAIT)
            return;
        if (!player.weapon.SpecialActions[index].CanCreateAction(player))
            return;

        //player.state = PlayerState.WAIT;
        player.action = player.weapon.SpecialActions[index].CreateAction();

        PerformRound();
    }

    public void OnShieldSpecialButton(int index)
    {
        if (index < 0 || index >= player.shield.SpecialActions.Count
            || player.state == WAIT)
            return;
        if (!player.shield.SpecialActions[index].CanCreateAction(player))
            return;

        //player.state = PlayerState.WAIT;
        player.action = player.shield.SpecialActions[index].CreateAction();

        PerformRound();
    }

    public void OnNothingButton()
    {
        if (player.state == WAIT) return;
        player.state = NEUTRAL;
        player.action = new(null);
        if (player.effects.TryGetValue(ON_STANCE, out var list))
        {
            player.ConsumeEffects(ON_STANCE);
            list.Clear();
        }

        PerformRound();
    }

    void PerformRound()
    {
        state = CombatState.ACTION;
        CombatUI.AddAnimation(CombatUI.Instance.HideActionButtons());

        AIAction();
        switch (player.action, enemy.action)
        {
            case (Attack playerOneAttack, Attack playerTwoAttack):
                Clash(playerOneAttack, playerTwoAttack);
                break;

            case (_, _):
                int playerLead = (int)(player.action?.Lead ?? NONE);
                int enemyLead  = (int)(enemy.action?.Lead  ?? NONE);
                (var leadActor, var secondActor) =
                    playerLead <= enemyLead ? (player, enemy) : (enemy, player);

                leadActor.PerformAction(secondActor);
                secondActor.PerformAction(leadActor);
                break;
        }
        turnNumber = 0;
        RoundEnd();
    }

    void Clash(Attack playerAttack, Attack enemyAttack)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText("A clash is happening!"));

        playerAttack.prowessBonus -= Random.Range(4, 8) / 10f;
        enemyAttack.prowessBonus -= Random.Range(4, 8) / 10f;

        float playerChance = player.counterChance;
        float enemyChance = enemy.counterChance;
        bool playerOneCounters = player.IsCounter();
        bool playerTwoCounters = enemy.IsCounter();

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

    void RoundEnd()
    {
        // TODO: perhaps an event for this
        player.ConsumeEffects(ROUND_START);
        enemy.ConsumeEffects(ROUND_START);
        player.ApplyEffects(ROUND_END);
        enemy.ApplyEffects(ROUND_END);
        player.ConsumeEffects(ROUND_END);
        enemy.ConsumeEffects(ROUND_END);

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
