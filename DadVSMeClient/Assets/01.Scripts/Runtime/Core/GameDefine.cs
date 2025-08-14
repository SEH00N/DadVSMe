using UnityEngine;

namespace DadVSMe
{
    public static class GameDefine
    {
        public static readonly TagHandle EntitySortingOrderProviderTag = TagHandle.GetExistingTag("EntitySortingOrderProvider");
        public static readonly TagHandle EnemyTag = TagHandle.GetExistingTag("Enemy");

        public const float GRAVITY_SCALE = 6.5f;
    }
}