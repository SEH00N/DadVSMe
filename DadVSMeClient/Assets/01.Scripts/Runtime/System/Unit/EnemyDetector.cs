using System.Collections.Generic;
using H00N.AI.FSM;
using H00N.Extensions;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] string enemyTag = "Enemy";
        [SerializeField] FSMBrain fsmBrain = null;

        private HashSet<int> currentEnemyHashes = new HashSet<int>();
        private UnitFSMData fsmData = null;
        public Collider2D detectCollision;

        public void Initialize()
        {
            fsmData = fsmBrain.GetAIData<UnitFSMData>();
        }

        public void SetActive(bool active)
        {
            detectCollision.enabled = active;
        }

        private void Update()
        {
            fsmData.enemies.Sort((a, b) =>
            {
                if (a == null && b == null)
                    return 0;

                if (a == null)
                    return 1;

                if (b == null)
                    return -1;

                return fsmBrain.transform.DistanceCompare(a.transform, b.transform);
            });
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag(enemyTag) == false)
                return;

            int enemyHash = collider.gameObject.GetHashCode();
            if(currentEnemyHashes.Contains(enemyHash))
                return;

            if(collider.gameObject.TryGetComponent<Unit>(out Unit enemy) == false)
                return;

            if(fsmData.enemies.Count >= fsmData.enemyMaxCount)
                return;

            if(enemy.FSMBrain.GetAIData<UnitFSMData>().isDie)
                return;

            currentEnemyHashes.Add(enemyHash);
            fsmData.enemies.Add(enemy);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.CompareTag(enemyTag) == false)
                return;

            int enemyHash = collider.gameObject.GetHashCode();
            if(currentEnemyHashes.Contains(enemyHash) == false)
                return;

            currentEnemyHashes.Remove(enemyHash);
            if(collider.gameObject.TryGetComponent<Unit>(out Unit enemy) )
                fsmData.enemies.Remove(enemy);
        }
    }
}
