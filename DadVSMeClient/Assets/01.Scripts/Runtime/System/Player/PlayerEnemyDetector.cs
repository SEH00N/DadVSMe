using System.Collections.Generic;
using DadVSMe.Enemies;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using H00N.Extensions;
using UnityEngine;

namespace DadVSMe.Players
{
    public class PlayerEnemyDetector : MonoBehaviour
    {
        [SerializeField] FSMBrain fsmBrain = null;

        private HashSet<int> currentEnemyHashes = new HashSet<int>();
        private PlayerFSMData fsmData = null;

        public void Initialize()
        {
            fsmData = fsmBrain.GetAIData<PlayerFSMData>();
        }

        private void Update()
        {
            fsmData.enemies.Sort((a, b) => {
                if(a == null && b == null)
                    return 0;

                if(a == null)
                    return 1;

                if(b == null)
                    return -1;

                return fsmBrain.transform.DistanceCompare(a.transform, b.transform);
            });
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag(GameDefine.EnemyTag) == false)
                return;

            int enemyHash = collider.gameObject.GetHashCode();
            if(currentEnemyHashes.Contains(enemyHash))
                return;

            if(collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy) == false)
                return;

            currentEnemyHashes.Add(enemyHash);
            fsmData.enemies.Add(enemy);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.CompareTag(GameDefine.EnemyTag) == false)
                return;

            int enemyHash = collider.gameObject.GetHashCode();
            if(currentEnemyHashes.Contains(enemyHash) == false)
                return;

            currentEnemyHashes.Remove(enemyHash);
            if(collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy) )
                fsmData.enemies.Remove(enemy);
        }
    }
}
