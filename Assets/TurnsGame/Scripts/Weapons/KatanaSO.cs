using UnityEngine;

[CreateAssetMenu(fileName = "KatanaSO", menuName = "Weapons/Katana")]
public class KatanaSO : WeaponSO
{
    [field: Header("Editor data")]
    [field: SerializeField] bool defaultSpecialActionsInitialized = false;

    #if UNITY_EDITOR
    void OnEnable()
    {
        if (!Application.isPlaying && !defaultSpecialActionsInitialized)
        {
            if (SpecialActions == null) SpecialActions = new();

            SheatheSO sheatheSO = ScriptableObjectUtils.GetAsset(
                                    "Sheathe", typeof(SheatheSO)) as SheatheSO;

            if(sheatheSO != null && !SpecialActions.Contains(sheatheSO))
                SpecialActions.Add(sheatheSO);
        }

        defaultSpecialActionsInitialized = true;

        UnityEditor.EditorUtility.SetDirty(this);
    }
    #endif
}
