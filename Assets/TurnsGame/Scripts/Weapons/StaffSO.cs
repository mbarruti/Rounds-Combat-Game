using UnityEngine;

[CreateAssetMenu(fileName = "StaffSO", menuName = "Weapons/Staff")]
public class StaffSO : WeaponSO
{
    [field: Header("Editor data")]
    [field: SerializeField] bool defaultSpecialActionsInitialized = false;

    #if UNITY_EDITOR
    void OnEnable()
    {
    }
    #endif
}
