using UnityEngine;
using UnityEngine.UI;
using static MyProject.Constants;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] Image barColor;
    public Vector2 nextPosition;

    private void Start()
    {
        nextPosition = transform.position;
    }

    public void UpdateBarColor(float value)
    {
        if (value == HALF_CHARGE)
            barColor.color = Color.red;
        else
            barColor.color = Color.blue;
    }
}