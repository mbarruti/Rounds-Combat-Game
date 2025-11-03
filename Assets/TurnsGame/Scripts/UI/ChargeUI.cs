using UnityEngine;
using UnityEngine.UI;

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
        if (value == 0.5f)
            barColor.color = Color.red;
        else
            barColor.color = Color.blue;
    }
}