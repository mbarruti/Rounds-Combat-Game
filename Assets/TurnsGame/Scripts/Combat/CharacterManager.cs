using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MyProject.Constants;

public enum PlayerState { CHOOSE, WAIT }

public class CharacterManager : MonoBehaviour
{
    [SerializeField] User user;
    [SerializeField] WeaponSO weapon;
    [SerializeField] ShieldSO shield;

    public string username => user.Name;

    // HP data
    public float maxHP;
    public float currentHP;
    public TextMeshProUGUI healthText;

    // Meter data
    public ShieldMeter shieldMeter;
    public ShieldMeterUI shieldMeterUI;

    // Weapon data
    public float baseDamage;
    public float meterDamage;
    public float accuracy;
    public float prowess;
    public float counterChance;
    public int maxNumHits;
    public int numHits;

    // Shield data
    public float parryChance;

    // Buff data
    public Dictionary<BuffType, Buff> activeBuffs;

    // Effects
    List<IEffect> effects;

    public CharacterAction action;
    public PlayerState state;

    CombatManager combatManager;

    void Awake()
    {
        action = new(this, null);
        activeBuffs = new()
        {
            { DAMAGE, new DamageBuff() }
        };
        effects = new();
        combatManager = CombatManager.Instance;
    }

    public void Setup(bool isPlayer)
    {
        baseDamage = weapon.BaseDamage;
        meterDamage = weapon.MeterDamage;
        accuracy = weapon.Accuracy;
        prowess = weapon.Prowess;
        counterChance = weapon.CounterChance;
        maxNumHits = weapon.NumHits;

        parryChance = shield.ParryChance;
        shieldMeter = new ShieldMeter();
        shieldMeter.Setup(shield.MaxCharges);

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

    public void Reset()
    {
        // CharacterAction newAction = new(this, action);
        // action = newAction;
        numHits = maxNumHits;
        state = PlayerState.CHOOSE;
    }

    public void PerformAction(CharacterManager target)
    {
        action?.Execute(target);
        if (action is not Block)
        {
            shieldMeter.RecoverCharges();
            Debug.Log("Number of charges of " + name + ": " + shieldMeter.GetAvailableCharges());
        }
    }

    public void TakeDamage(float damage)
    {
        //UI.AddAnimation(UI.Instance.WriteText(name + " loses " + damage + " health points"));
        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;

        CombatUI.AddAnimation(CombatUI.Instance.UpdateHPText(this, currentHP));
    }

    public void TakeMeterDamage(float meterDamage)
    {
        shieldMeter.LoseCharges(meterDamage);
        if (shieldMeter.GetCurrentCharges() <= 0)
        {
            IEffect crushedEffect = new Crushed(this);
            crushedEffect.GetAdded();
        }
    }

    public void AddEffect(IEffect effect)
    {
        effects.Add(effect);
    }

    public void ApplyEffects(EffectTrigger trigger)
    {
        if (effects.Count == 0) return;
        var effectsCopy = new List<IEffect>(effects);
        foreach (var effect in effectsCopy)
        {
            if (effect.Trigger == trigger) effect.Apply();
            //if (effect.Duration == 0) RemoveEffect(effect);
        }
    }

    public void RemoveEffect(IEffect effect)
    {
        effects.Remove(effect);
    }

    public bool IsDead()
    {
        if (currentHP <= 0) return true;
        return false;
    }
}
