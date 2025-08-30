using UnityEngine;

namespace DadVSMe
{
    public static class GameDefine
    {
        public const string INTRO_SCENE_NAME = "IntroScene";
        public const string TITLE_SCENE_NAME = "TitleScene";
        public const string GAME_SCENE_NAME = "GameScene";
        public const string GAME_OVER_SCENE_NAME = "GameOverScene";
        public const string GAME_CLEAR_SCENE_NAME = "GameClearScene";

        public const string ADDRESSABLES_LABEL_GAME_ASSETS = "game-assets";

        public static readonly TagHandle EntitySortingOrderProviderTag = TagHandle.GetExistingTag("EntitySortingOrderProvider");
        public static readonly TagHandle EnemyTag = TagHandle.GetExistingTag("Enemy");
        public static readonly TagHandle PlayerTag = TagHandle.GetExistingTag("Player");

        public const int BOUNDARY_LAYER_MASK = 1 << 9;
        public const int ITEM_LAYER_MASK = 1 << 12;

        public const float GRAVITY_SCALE = 6.5f;

        public const int MAX_SKILL_LEVEL = 5;

        public const float DEFAULT_FADE_DURATION = 0.75f;
        public const float DEFAULT_TIME_SCALE = 1f;
    }
}