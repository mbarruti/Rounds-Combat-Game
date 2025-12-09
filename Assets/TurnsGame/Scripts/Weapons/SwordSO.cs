using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordSO", menuName = "Weapons/Sword")]
public class SwordSO : WeaponSO
{
    [field: Header("Editor data")]
    [field: SerializeField] bool defaultSpecialActionsInitialized = false;

    #if UNITY_EDITOR
    void OnEnable()
    {
    }
    #endif
}
