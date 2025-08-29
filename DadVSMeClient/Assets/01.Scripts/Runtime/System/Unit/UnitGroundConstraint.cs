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

            Vector3 position = new Vector3(unit.transform.position.x, unitFSMData.groundPositionY, unit.transform.position.z);
            transform.SetPositionAndRotation(position, Quaternion.identity);
        }
    }
}