using UnityEngine;

[CreateAssetMenu(fileName = "DaggerSO", menuName = "Weapons/Dagger")]
public class DaggerSO : WeaponSO
{
    [field: Header("Editor data")]
    [field: SerializeField] bool defaultSpecialActionsInitialized = false;

    #if UNITY_EDITOR
    void OnEnable()
    {
    }
    #endif
}

