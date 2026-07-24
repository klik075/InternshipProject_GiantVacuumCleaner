
using Scripts.Framework.Scenes.SO;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoadState
{
    #region Fields

    public GameSceneSO SceneToLoad { get; set; }
    public GameSceneSO CurrentLoadScene { get; set; }
    public SceneInstance GameplaySupporter { get; set; }
    
    public bool IsLoadingFlag { get; set; }
    public bool ShowLoadingCover { get; set; }

    #endregion
}