using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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

    public CharacterManager Player { get; private set; }
    public CharacterManager Enemy { get; private set; }

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

    void SetupMatch()
    {
        //Player = SpawnCharacter(IS_PLAYER_ONE);
        //Player.username = "PlayerOne";
        //Enemy = SpawnCharacter(!IS_PLAYER_ONE);
        //Enemy.username = "PlayerTwo";

        // PROVISIONAL
        // Vector3 screenPosition = CombatUI.Instance.uiPlayerOnePosition.position;
        // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        // worldPosition.z = 0f;
        // playerOne.transform.position = worldPosition;
        playerOne.Setup(IS_PLAYER_ONE);
        Player = playerOne;

        // screenPosition = CombatUI.Instance.uiPlayerTwoPosition.position;
        // worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        // worldPosition.z = 0f;
        // playerTwo.transform.position = worldPosition;
        playerTwo.Setup(!IS_PLAYER_ONE);
        Enemy = playerTwo;
        // PROVISIONAL END (DELETE LATER)

        Player.transform.LookAt(Enemy.transform);
        Enemy.transform.LookAt(Player.transform);

        Anim.Sequence(Anim.Do(() => CombatUI.Instance.WriteText("Begin match")));
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

        // Anim.Sequence(Anim.Do(() => CombatUI.Instance.WriteText($"Round {roundNumber}")));

        Player.Reset();
        Enemy.Reset();

        Player.ApplyEffects(ROUND_START);
        Enemy.ApplyEffects(ROUND_START);

        Player.actionController.SetAvailableActions();
        Enemy.actionController.SetAvailableActions();

        Anim.Sequence(
            Anim.Do(() => CombatUI.Instance.WriteText($"Round {roundNumber}")),
            Anim.Do(() => CombatUI.Instance.ShowActionButtons(Player.actionController, waitTime: 0))
        );
        //await AnimationLoop();
        AnimationManager.Instance.RunAnimations().Forget();
    }

    public void RoundStart()
    {
        state = CombatState.CHOOSE;

        //AIAction();
        if (Player.state == PlayerState.Wait)
        {
            state = CombatState.ACTION;
            PerformRound();
        }
    }

    void AIAction()
    {
        //return;
        int randomChoice = Random.Range(0, 2);
        if (Enemy.state == WAIT) return;
        if (/* randomChoice == 1 &&  */Enemy.shieldMeter.GetAvailableCharges() > 0)
            Enemy.action = Enemy.attackSO.CreateAction(); // BLOCK
        else Enemy.action = Enemy.attackSO.CreateAction(); // ATTACK
    }

    public void OnAttackButton()
    {
        if (!Player.attackSO.CanCreateAction(Player))
            return;
        Player.state = OFFENSE;
        Player.action = Player.attackSO.CreateAction();

        PerformRound();
    }

    public void OnBlockButton()
    {
        if (!Player.blockSO.CanCreateAction(Player))
            return;
        Player.state = DEFENSE;
        Player.action = Player.blockSO.CreateAction();

        PerformRound();
    }

    public void OnWeaponSpecialButton(int index)
    {
        if (index < 0 || index >= Player.weapon.SpecialActions.Count
            || Player.state == WAIT)
            return;
        if (!Player.weapon.SpecialActions[index].CanCreateAction(Player))
            return;

        //player.state = PlayerState.WAIT;
        Player.action = Player.weapon.SpecialActions[index].CreateAction();

        PerformRound();
    }

    public void OnShieldSpecialButton(int index)
    {
        if (index < 0 || index >= Player.shield.SpecialActions.Count
            || Player.state == WAIT)
            return;
        if (!Player.shield.SpecialActions[index].CanCreateAction(Player))
            return;

        //player.state = PlayerState.WAIT;
        Player.action = Player.shield.SpecialActions[index].CreateAction();

        PerformRound();
    }

    public void OnNothingButton()
    {
        if (Player.state == WAIT) return;
        Player.state = NEUTRAL;
        Player.action = new(null);
        if (Player.effects.TryGetValue(ON_STANCE, out var list))
        {
            Player.ConsumeEffects(ON_STANCE);
            list.Clear();
        }

        PerformRound();
    }

    void PerformRound()
    {
        state = CombatState.ACTION;
        //CombatUI.AddAnimation(CombatUI.Instance.HideActionButtons());
        Anim.Sequence(
            Anim.Do(() => CombatUI.Instance.HideActionButtons())
        );

        AIAction();
        switch (Player.action, Enemy.action)
        {
            case (Attack playerOneAttack, Attack playerTwoAttack):
                Clash(playerOneAttack, playerTwoAttack);
                break;

            case (_, _):
                int playerLead = (int)(Player.action?.Lead ?? NONE);
                int enemyLead  = (int)(Enemy.action?.Lead  ?? NONE);
                (var leadActor, var secondActor) =
                    playerLead <= enemyLead ? (Player, Enemy) : (Enemy, Player);

                leadActor.PerformAction(secondActor);
                secondActor.PerformAction(leadActor);
                break;
        }
        turnNumber = 0;
        RoundEnd();
    }

    void Clash(Attack playerAttack, Attack enemyAttack)
    {
        float playerOneTargetZ = Player.rigController.CalculateTargetZ(0.75f);
        Player.expectedPosition.z = playerOneTargetZ;
        float playerTwoTargetZ = Enemy.rigController.CalculateTargetZ(-0.75f);
        Enemy.expectedPosition.z = playerTwoTargetZ;

        Anim.Sequence(
            Anim.Parallel(
                Anim.Do(() => CombatUI.Instance.WriteText("A clash is happening!", waitTime: 0)),
                Anim.Do(() => Player.rigController.Move(playerOneTargetZ)),
                Anim.Do(() => Enemy.rigController.Move(playerTwoTargetZ))
            )
        );

        playerAttack.prowessBonus -= Random.Range(4, 8) / 10f;
        enemyAttack.prowessBonus -= Random.Range(4, 8) / 10f;

        float playerChance = Player.counterChance;
        float enemyChance = Enemy.counterChance;
        bool playerOneCounters = Player.IsCounter();
        bool playerTwoCounters = Enemy.IsCounter();

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
                playerGain > enemyGain ? (Player, Enemy) : (Enemy, Player);

            // CombatUI.AddAnimation(
            //     CombatUI.Instance.WriteText($"{counterWinner.username} gets a counter!"));
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText($"{counterWinner.username} gets a counter!")
                )
            );

            counterWinner.PerformAction(Enemy);
            counterLoser.PerformAction(Player);
        }
        else if (playerOneCounters || playerTwoCounters)
        {
            (float playerGain, float enemyGain) = playerOneCounters ?
                (COUNTER_PROWESS_GAIN, COUNTER_PROWESS_LOSS) :
                (COUNTER_PROWESS_LOSS, COUNTER_PROWESS_GAIN);

            playerAttack.prowessBonus += playerGain;
            enemyAttack.prowessBonus += enemyGain;

            (var counterWinner, var counterLoser) =
                playerGain > enemyGain ? (Player, Enemy) : (Enemy, Player);

            // CombatUI.AddAnimation(
            //     CombatUI.Instance.WriteText($"{counterWinner.username} gets a counter!"));
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText($"{counterWinner.username} gets a counter!")
                )
            );

            counterWinner.PerformAction(counterLoser);
            counterLoser.PerformAction(counterWinner);
        }
        else
        {
            Player.PerformAction(Enemy);
            Enemy.PerformAction(Player);
        }
    }

    void RoundEnd()
    {
/*         if (Player.expectedPosition != Player.defaultPosition ||
            Enemy.expectedPosition != Enemy.defaultPosition)
        { */
        Anim.Sequence(
            Anim.Parallel(
                Anim.Do(async () =>
                {
                    await Player.rigController.Move(Player.defaultPosition.z);
                    //await Player.rigController.IdleAnimation("DefaultIdle");
                    await IdleAnimation(Player, Enemy);
                }),
                Anim.Do(async () =>
                {
                    await Enemy.rigController.Move(Enemy.defaultPosition.z);
                    //await Enemy.rigController.IdleAnimation("DefaultIdle");
                    await IdleAnimation(Enemy, Player);
                })
            )
        );
/*         } */

        // TODO: perhaps an event for this
        Player.ConsumeEffects(ROUND_START);
        Enemy.ConsumeEffects(ROUND_START);
        Player.ApplyEffects(ROUND_END);
        Enemy.ApplyEffects(ROUND_END);
        Player.ConsumeEffects(ROUND_END);
        Enemy.ConsumeEffects(ROUND_END);

        CheckCombatState();
        if (state != CombatState.END)
        {
            PreRound();
        }
    }

    void CheckCombatState()
    {
        if (Player.IsDead() || Enemy.IsDead())
        {
            state = CombatState.END;
            EndCombat();
        }
        else return;
    }

    void EndCombat()
    {
        if (Player.IsDead() && Enemy.IsDead())
        {
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText(
                        "Both players have fallen. The match ends in a draw!")
                )
            );
        }
        else
        {
            (var winner, var loser) = Player.IsDead() ? (Enemy, Player) : (Player, Enemy);
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText(winner.name + " wins the match!")
                )
            );
        }
        AnimationManager.Instance.RunAnimations().Forget();
    }

    // TEST FUNCTIONS (DELETE LATER)
    async UniTask AnimationLoop()
    {
        Anim.Sequence(
            Anim.Do(() => CombatUI.Instance.WriteText("Begin match")),
            Anim.Do(() => CombatUI.Instance.WriteText($"Round {roundNumber}")),
            Anim.Do(() => CombatUI.Instance.ShowActionButtons(Player.actionController, waitTime: 1))
        );
        Anim.Sequence(
            Anim.Parallel(
                Anim.Do(() => CombatUI.Instance.HideActionButtons()),
                Anim.Do(() => CombatUI.Instance.WriteText($"{Player.username} goes to attack", delay: 0.01f, waitTime: 0)),
                Anim.Do(() => Player.rigController.Move(Enemy.transform.position.z-1.5f))
            ),
            Anim.Parallel(
                Anim.Do(() => CombatUI.Instance.WriteText($"{Player.username} attacks {Enemy.username}", delay: 0.01f, waitTime: 0)),
                Anim.Do(() => AttackAnimation())
            )
        );
        Anim.Sequence(
            Anim.Do(() => Player.rigController.Move(Player.defaultPosition.z)),
            Anim.Do(() => IdleAnimation(Player, Enemy, waitTime: 1))
            //Anim.Do(() => CombatUI.Instance.ShowActionButtons(Player.actionController))
        );
        await AnimationManager.Instance.RunAnimations();
        await AnimationLoop();
        //AnimationManager.Instance.RunAnimations().Forget();
    }

    async UniTask AttackAnimation()
    {
        Player.isPerformingAction = true;
        Player.animator.CrossFadeInFixedTime("Attack", 0.2f);

        while (Player.isPerformingAction)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    async UniTask IdleAnimation(CharacterManager player, CharacterManager target, float waitTime = 0)
    {
        player.isPerformingAction = true;
        player.animator.CrossFadeInFixedTime("DefaultIdle", 0.2f);
        player.transform.LookAt(target.transform);

        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }
}
