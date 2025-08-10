using UnityEngine;

namespace DadVSMe
{
    // 현재 시간에 따라 스폰 빈도가 다르다.
    // 스폰되는 적의 종류는 맵에 존재하는 적의 종류의 수에 따라 결정된다.
    // 적의 종류에 따라 필드에 존재할 수 있는 최대 수가 다르다.
    // 필드에 존재하는 모든 적의 합은 최대치를 넘을 수 없다.

    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "CreateSO/Entity/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        
    }
}
