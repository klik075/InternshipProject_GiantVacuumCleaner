
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Scenes.SO
{
    public abstract class GameSceneSO : DescriptionSO
    {
        #region Fields

        public GameSceneType SceneType;
        public AssetReference SceneReference;
        
        [Header("게임 플레이 서포터가 필요할 경우")]
        public bool IsNeedGameplaySupporter;

        public enum GameSceneType
        {
            // Playable
            Lobby,
            Gameplay,
        
            // Specials
            Initialization,
            SplashScene,
            PersistentSystem,
            GameplaySupporter,
            LoadRequest
        }

        [Header("Scene Needs Addressable")]
        public List<AssetLabelReference> AssetLabels;

        #endregion
    }
}
