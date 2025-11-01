using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public TextMeshProUGUI healthText;

    public ShieldMeter shieldMeter;

    public Action action;

    public void Setup(bool isPlayer)
    {
        shieldMeter = new ShieldMeter();
        shieldMeter.Setup();

        if (isPlayer)
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().playerMaterial;
            healthText = UI.Instance.playerHPText;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().enemyMaterial;
            healthText = UI.Instance.enemyHPText;
        }
        healthText.text = $"{currentHP}/{maxHP}";
    }

    public void PerformAction(CharacterManager target)
    {
        action?.Execute(this, target);
        if (action is not Block)
        {
            shieldMeter.RecoverCharges();
            Debug.Log("Number of charges of " + name + ": " + shieldMeter.GetAvailableCharges());
        }
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
