using UnityEngine;

[CreateAssetMenu(fileName = "WarhammerSO", menuName = "Weapons/Warhammer")]
public class WarhammerSO : WeaponSO
{
    [field: Header("Editor data")]
    [field: SerializeField] bool defaultSpecialActionsInitialized = false;

    #if UNITY_EDITOR
    void OnEnable()
    {
        if (!Application.isPlaying && !defaultSpecialActionsInitialized)
        {
            if (SpecialActions == null) SpecialActions = new();

            ChargeSO chargeSO = ScriptableObjectUtils.GetAsset(
                                    "Charge", typeof(ChargeSO)) as ChargeSO;
            TackleSO tackleSO = ScriptableObjectUtils.GetAsset(
                                    "Tackle", typeof(TackleSO)) as TackleSO;

            if(chargeSO != null && !SpecialActions.Contains(chargeSO))
                SpecialActions.Add(chargeSO);

            if(tackleSO != null && !SpecialActions.Contains(tackleSO))
                SpecialActions.Add(tackleSO);
        }

        defaultSpecialActionsInitialized = true;

        UnityEditor.EditorUtility.SetDirty(this);
    }
    #endif
}
