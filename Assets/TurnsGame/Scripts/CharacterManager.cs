using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class CharacterManager : MonoBehaviour
{

    public int maxHP;
    public int currentHP;
    public TextMeshProUGUI healthText;

    public int maxShieldCharges;
    public int currentShieldCharges;

    public Action action;

    public void Setup(bool isPlayer)
    {
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
        if (action is not Block) RecoverShieldCharge();
    }

    public void TakeDamage(int damage)
    {
        UI.AddAnimation(UI.Instance.TypeTextCoroutine(name + " loses " + damage + " health points"));

        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;

        UI.AddAnimation(UI.Instance.UpdateHPText(this));
    }

    private void RecoverShieldCharge()
    {
        if (currentShieldCharges < maxShieldCharges) currentShieldCharges += 1;
    }

    public void LoseShieldCharge()
    {
        if (currentShieldCharges > 0) currentShieldCharges -= 1;
    }

    public bool IsDead()
    {
        if (currentHP <= 0) return true;
        return false;
    }
}
