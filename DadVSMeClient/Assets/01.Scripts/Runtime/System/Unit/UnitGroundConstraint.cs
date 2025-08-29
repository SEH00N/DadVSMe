using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitGroundConstraint : MonoBehaviour
    {
        [SerializeField] Unit unit = null;
        private UnitFSMData unitFSMData = null;

        private void Awake()
        {
            unit.OnInitializedEvent += Initialize;
        }

        private void Initialize(IEntityData data)
        {
            unitFSMData = unit.FSMBrain.GetAIData<UnitFSMData>();
        }

        private void LateUpdate()
        {
            if(unitFSMData == null)
                return;

            transform.position = new Vector3(transform.position.x, unitFSMData.groundPositionY, transform.position.z);
        }
    }
}