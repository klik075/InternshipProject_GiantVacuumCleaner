
using Scripts.Framework.Events.SO;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolEventChannel", menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : GameEventSO<bool>
{
    #region Raised

    public override void ResetListeners()
    {
        // Nothing
    }

    #endregion
}