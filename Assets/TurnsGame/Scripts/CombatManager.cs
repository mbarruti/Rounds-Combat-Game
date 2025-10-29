using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CombatState { START, CHOOSE, ACTION, END }

public class CombatManager : MonoBehaviour
{

    static CombatManager instance;
    public static CombatManager GetInstance() { return instance; }

    public CombatState state;

    [SerializeField] Transform prefabCharacter;
    public Material playerMaterial;
    public Material enemyMaterial;

    CharacterManager player;
    CharacterManager enemy;

    int roundNumber = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = SpawnCharacter(true);
        player.name = "PlayerOne";
        enemy = SpawnCharacter(false);
        enemy.name = "PlayerTwo";

        //Debug.Log("Begin match!");
        UI.Instance.WriteText("Begin match!");
        RoundAction();
    }

    CharacterManager SpawnCharacter(bool isPlayer)
    {
        Vector2 position;

        if (isPlayer)
        {
            position = new Vector2(-2.5f, 0.5f);
        }
        else
        {
            position = new Vector2(2.5f, 2.5f);
        }
        Transform characterTransform = Instantiate(prefabCharacter, position, Quaternion.identity);
        CharacterManager characterManager = characterTransform.GetComponent<CharacterManager>();
        characterManager.Setup(isPlayer);

        return characterManager;
    }

    public void RoundAction()
    {
        roundNumber++;

        UI.Instance.WriteText("Round " + roundNumber);
        UI.Instance.WriteText("Choose your action");
        StartCoroutine(UI.Instance.ExecuteAnimations());

        //state = CombatState.CHOOSE;
        AIAction();
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
        if (state != CombatState.CHOOSE) return;
        state = CombatState.ACTION;
        player.action = new Block();

        PerformRound();
    }

    void AIAction()
    {
        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 0) enemy.action = new Block();
        else if (randomChoice == 1) enemy.action = new Block();
    }

    void PerformRound()
    {
        switch ((player.action, enemy.action))
        {
            case (Attack playerAttack, Attack enemyAttack):
                Clash(playerAttack, enemyAttack);
                break;

            case (Attack playerAttack, Block enemyBlock):
                player.PerformAction(enemy);
                enemy.PerformAction(player);
                break;

            case (Block playerBlock, Block enemyBlock):
                UI.Instance.WriteText("Both players block what the fuck");
                break;

            case (Block playerBlock, Attack enemyAttack):
                enemy.PerformAction(player);
                player.PerformAction(enemy);
                break;
        }
        RoundAction();
        //StartCoroutine(UI.Instance.ExecuteAnimations());
    }

    void Clash(Attack playerAttack, Attack enemyAttack)
    {
        UI.Instance.WriteText("A clash is happening!");

        CharacterManager clashWinner = null;
        CharacterManager clashLoser = null;

        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 0)
        {
            clashWinner = player;
            clashLoser = enemy;
            UI.Instance.WriteText(clashWinner.name + " wins the clash!");
        }
        else
        {
            clashWinner = enemy;
            clashLoser = player;
            UI.Instance.WriteText(clashWinner.name + " wins the clash!");
        }
        //Debug.Log(clashWinner.name + " attacks " + clashLoser.name);
        clashWinner.PerformAction(clashLoser);
    }

}
