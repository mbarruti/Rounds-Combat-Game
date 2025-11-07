using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] User user;
    public string username => user.Name;

    public float maxHP;
    public float currentHP;
    public TextMeshProUGUI healthText;

    public ShieldMeter shieldMeter;
    public ShieldMeterUI shieldMeterUI;

    [SerializeField] WeaponSO weapon;

    // Weapon data
    public float baseDamage;
    public float meterDamage;
    public float accuracy;
    public float prowess;
    public float counterChance;

    public CharacterAction action;

    CombatManager combatManager;

    private void Awake()
    {
        combatManager = CombatManager.Instance;
    }

    public void Setup(bool isPlayer)
    {
        baseDamage = weapon.BaseDamage;
        meterDamage = weapon.MeterDamage;
        accuracy = weapon.Accuracy;
        prowess = weapon.Prowess;
        counterChance = weapon.CounterChance;

        shieldMeter = new ShieldMeter();
        shieldMeter.Setup();

        if (isPlayer)
        {
            user.Name = "Player One";
            gameObject.GetComponent<Renderer>().material = combatManager.playerMaterial;
            healthText = CombatUI.Instance.playerHPText;
            shieldMeterUI = CombatUI.Instance.playerShieldMeter;
        }
        else
        {
            user.Name = "Player Two";
            gameObject.GetComponent<Renderer>().material = combatManager.enemyMaterial;
            healthText = CombatUI.Instance.enemyHPText;
            shieldMeterUI = CombatUI.Instance.enemyShieldMeter;
        }
        healthText.text = $"{currentHP}/{maxHP}";
        shieldMeterUI.Setup(shieldMeter);
    }

    public void PerformAction(CharacterManager target)
    {
        action?.Execute(this, target);
        if (action is not Block)
        {
            shieldMeter.RecoverCharges();
            Debug.Log("Number of charges of " + name + ": " + shieldMeter.GetAvailableCharges());
        }
        else if (shieldMeter.GetAvailableCharges() == 0)
            action = null;
    }

    public void TakeDamage(float damage)
    {
        //UI.AddAnimation(UI.Instance.WriteText(name + " loses " + damage + " health points"));
        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;

        CombatUI.AddAnimation(CombatUI.Instance.UpdateHPText(this));
    }

    public bool IsDead()
    {
        if (currentHP <= 0) return true;
        return false;
    }
}
