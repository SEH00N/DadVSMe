using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class ShootAttackAction : AttackActionBase
    {
        [SerializeField] ShootAttackData attackData = null;
        [SerializeField] Transform firePosition = null;
        [SerializeField] Transform targetPosition = null;
        [SerializeField] AddressableAsset<Projectile> projectilePrefab = null;

        protected override IAttackFeedbackDataContainer FeedbackDataContainer => attackData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            projectilePrefab.InitializeAsync().Forget();
            attackData.shootSound.InitializeAsync().Forget();
        }

        public override void EnterState()
        {
            base.EnterState();

            if(unitFSMData.enemies.Count == 0)
                brain.SetAsDefaultState();

            int forwardDirection = unitFSMData.enemies[0].transform.position.x > brain.transform.position.x ? 1 : -1;
            unitFSMData.forwardDirection = forwardDirection;

            float currentLossyScaleX = brain.transform.lossyScale.x;
            brain.transform.localScale = new Vector3(brain.transform.localScale.x * (unitFSMData.forwardDirection / currentLossyScaleX), 1, 1);
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            Projectile projectile = PoolManager.Spawn<Projectile>(projectilePrefab.Key, GameInstance.GameCycle.transform);
            projectile.transform.position = firePosition.position;
            projectile.Initialize(unitFSMData.unit, (Vector2)targetPosition.position);

            _ = new PlaySound(attackData.shootSound);
        }
    }
}
