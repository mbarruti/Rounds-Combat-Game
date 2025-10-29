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

    public void RoundAction()
    {
        roundNumber++;

        UI.Instance.WriteText("Round " + roundNumber);
        UI.Instance.WriteText("Choose your action");
        StartCoroutine(UI.Instance.ExecuteAnimations());

        //state = CombatState.CHOOSE;
        AIAction();
    }

    void AIAction()
    {
        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 1 && enemy.currentShieldCharges > 0) enemy.action = new Block();
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
        if (state != CombatState.CHOOSE || player.currentShieldCharges <= 0) return;
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
        RoundEnd();
    }

    void RoundEnd()
    {
        if (player.action is not Block) player.RecoverShieldCharge();
        if (enemy.action is not Block) enemy.RecoverShieldCharge();

        RoundAction();
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
