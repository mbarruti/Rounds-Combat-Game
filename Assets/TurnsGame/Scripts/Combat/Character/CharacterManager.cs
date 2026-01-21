using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MyProject.Constants;

public enum PlayerState
{
    Neutral,
    Offense,
    Defense,
    Counter,
    Recover,
    SpecialStance,
    //CRUSHED,
    Wait,
}

public class CharacterManager : MonoBehaviour
{
    [SerializeField] User user;
    public string username => user.Name;

    public Vector3 defaultPosition;
    public Vector3 expectedPosition;

    public WeaponSO weapon;
    public ShieldSO shield;

    // HP data
    public float maxHP;
    public float currentHP;
    public TextMeshProUGUI healthText;

    // Meter data
    public ShieldMeter shieldMeter;
    public ShieldMeterUI shieldMeterUI;

    // Weapon setup
    public bool isDual;

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

    // Stance
    public Stance chosenStance;
    public Stance stance;

    // Buff data
    public BuffsController activeBuffs;

    // Effects
    public Dictionary<EffectTrigger, List<IEffect>> effects;

    // Combat actions
    public AttackSO attackSO;
    public BlockSO blockSO;
    public CharacterAction action;
    public CharacterAction lastAction;
    public CharacterAction nextAction;
    public CharacterActionController actionController = new();

    public event Action OnPerformAction;

    public PlayerState state;

    CombatManager combatManager;

    void Awake()
    {
        //action = new();
        chosenStance = new DefaultStance(this);
        stance = chosenStance.Clone();
        activeBuffs = new(this);
        effects = new();
        shieldMeter = new();
        defaultPosition = transform.position;

        combatManager = CombatManager.Instance;
    }

    public void Setup(bool isPlayer)
    {
        if (isDual)
        {
            baseDamage = weapon.DualBaseDamage;
            meterDamage = weapon.DualMeterDamage;
            accuracy = weapon.DualAccuracy;
            prowess = weapon.DualProwess;
            counterChance = weapon.DualCounterChance;
            maxNumHits = weapon.DualNumHits;

            shieldMeter.Setup(3);
        }
        else
        {
            baseDamage = weapon.BaseDamage;
            meterDamage = weapon.MeterDamage;
            accuracy = weapon.Accuracy;
            prowess = weapon.Prowess;
            counterChance = weapon.CounterChance;
            maxNumHits = weapon.NumHits;

            parryChance = shield.ParryChance;
            shieldMeter.Setup(shield.MaxCharges);
        }

        if (isPlayer)
        {
            user.Name = "Player One";
            SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
            smr.material = CombatManager.Instance.playerMaterial;
            //gameObject.GetComponent<Renderer>().material = CombatManager.Instance.playerMaterial;
            healthText = CombatUI.Instance.playerHPText;
            shieldMeterUI = CombatUI.Instance.playerShieldMeter;

            TextMeshProUGUI weaponSpecialText =
                CombatUI.Instance.weaponSpecialButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI weaponSpecialTwoText =
                CombatUI.Instance.weaponSpecialButton2.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI shieldSpecialText =
                CombatUI.Instance.shieldSpecialButton.GetComponentInChildren<TextMeshProUGUI>();

            if (0 < weapon.SpecialActions.Count)
                weaponSpecialText.text = weapon.SpecialActions[0].Name;
            if (1 < weapon.SpecialActions.Count)
                weaponSpecialTwoText.text = weapon.SpecialActions[1].Name;
            if (0 < shield.SpecialActions.Count)
                shieldSpecialText.text = shield.SpecialActions[0].Name;
        }
        else
        {
            user.Name = "Player Two";
            SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
            smr.material = CombatManager.Instance.enemyMaterial;
            //gameObject.GetComponent<Renderer>().material = CombatManager.Instance.enemyMaterial;
            healthText = CombatUI.Instance.enemyHPText;
            shieldMeterUI = CombatUI.Instance.enemyShieldMeter;
        }
        // TODO: Enter chosen stance when combat starts


        currentHP = maxHP;
        healthText.text = $"{currentHP}/{maxHP}";
        shieldMeterUI.Setup(shieldMeter);
    }

    public void Reset()
    {
        // CharacterAction newAction = new(this, action);
        lastAction = action;
        numHits = maxNumHits;
        state = NEUTRAL;

        shieldMeterUI.SetChargesCopy();
    }

    public void PerformAction(CharacterManager target)
    {
        OnPerformAction?.Invoke();
        action?.Execute(this, target);
        if (action.Type == NO_TYPE || action.CanRecoverMeter) shieldMeter.RecoverCharges();
        if (action.Type == NO_TYPE)
        {
            ApplyEffects(NOTHING);
            return;
        }
        ApplyEffects(ANY_ACTION);
    }

    public void TakeDamage(float damage)
    {
        float previousHP = currentHP;
        damage -= damage * activeBuffs.DmgReduction;

        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;

        if (currentHP != previousHP)
            CombatUI.AddAnimation(CombatUI.Instance.UpdateHPText(this, currentHP));
    }

    public void TakeMeterDamage(float meterDamage)
    {
        shieldMeter.LoseCharges(meterDamage);
        if (shieldMeter.GetCurrentCharges() <= 0)
        {
            CrushedEffect crushedEffect = new();
            AddEffect(crushedEffect);
            CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{username} got crushed!"));
        }
    }

    public bool IsCounter()
    {
        int totalNumHits = maxNumHits + activeBuffs.NumHits;
        for (int hitsLeft = totalNumHits; hitsLeft > 0; hitsLeft--)
        {
            float totalCounterChance = counterChance + activeBuffs.CounterChance;
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (totalCounterChance >= randomValue) return true;
        }
        return false;
    }

    public void AddEffect(IEffect effect)
    {
        if (!effects.TryGetValue(effect.Trigger, out var list))
        {
            list = new List<IEffect>();
            effects.Add(effect.Trigger, list);
        }
        list.Add(effect);
    }

    public void ApplyEffects(EffectTrigger trigger)
    {
        //if (effects == null) return;
        if (effects.TryGetValue(trigger, out var list))
        {
            List<IEffect> copy = new(list);
            foreach (var effect in copy)
            {
                effect.Apply(this, null);
            }
        }
    }

    public void RemoveEffect(IEffect effect)
    {
        if (effects.TryGetValue(effect.Trigger, out var list))
        {
            list.Remove(effect);
            // if (list.Count == 0)
            //     effects.Remove(effect.Trigger);
        }
    }

    public void ConsumeEffects(EffectTrigger trigger)
    {
        if (effects.TryGetValue(trigger, out var list))
        {
            List<IEffect> copy = new(list);
            foreach (IEffect effect in copy)
            {
                effect.Consume(this, null);
                if (effect.Uses == effect.MaxUses) list.Remove(effect);
            }
        }
    }

    public bool IsDead()
    {
        if (Mathf.Approximately(currentHP, 0) || currentHP < 0) return true;
        return false;
    }

    public IEnumerator Move()
    {
        transform.position = expectedPosition;
        yield return new WaitForSeconds(1f);
    }
}
