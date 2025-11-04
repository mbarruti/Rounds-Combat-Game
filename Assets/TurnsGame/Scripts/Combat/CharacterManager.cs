using NUnit.Framework.Internal.Execution;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
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

    private void Awake()
    {
        action = new CharacterAction(this);
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
            gameObject.GetComponent<Renderer>().material = CombatManager.Instance.playerMaterial;
            healthText = UI.Instance.playerHPText;
            shieldMeterUI = UI.Instance.playerShieldMeter;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.Instance.enemyMaterial;
            healthText = UI.Instance.enemyHPText;
            shieldMeterUI = UI.Instance.enemyShieldMeter;
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

        UI.AddAnimation(UI.Instance.UpdateHPText(this));
    }

    public bool IsDead()
    {
        if (currentHP <= 0) return true;
        return false;
    }
}
