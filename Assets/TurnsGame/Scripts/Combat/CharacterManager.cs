using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public TextMeshProUGUI healthText;

    public ShieldMeter shieldMeter;
    public ShieldMeterUI shieldMeterUI;

    public Action action;

    public void Setup(bool isPlayer)
    {
        shieldMeter = new ShieldMeter();
        shieldMeter.Setup();

        if (isPlayer)
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().playerMaterial;
            healthText = UI.Instance.playerHPText;
            shieldMeterUI = UI.Instance.playerShieldMeter;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().enemyMaterial;
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

    public void TakeDamage(int damage)
    {
        UI.AddAnimation(UI.Instance.WriteText(name + " loses " + damage + " health points"));

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
