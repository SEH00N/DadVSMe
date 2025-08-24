using Cysharp.Threading.Tasks;
using DadVSMe.Core.Cam;
using DadVSMe.Core.UI;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class DamageTextUIHandlse : UIHandle<DamageTextUIHandleParameter>
    {
        public override void Execute(DamageTextUIHandleParameter handleParameter)
        {
            AddressableAsset<DamageText> textRef =
                handleParameter.attackData.GetFeedbackData(handleParameter.attackAttribute).hitText;
            textRef.InitializeAsync().Forget();

            DamageText text = PoolManager.Spawn(textRef).GetComponent<DamageText>();
            text.Setup(CameraManager.UICam);
            text.Play(handleParameter.target, handleParameter.upOffset, handleParameter.attackData.Damage, 
                handleParameter.isCritical, handleParameter.normalColor, handleParameter.criticalColor);
        }
    }

    public class DamageTextUIHandleParameter : UIHandleParameter
    {
        public Transform target;
        public Vector3 upOffset;
        public AttackDataBase attackData;
        public EAttackAttribute attackAttribute;
        public bool isCritical = false;
        public Color normalColor = Color.white;
        public Color criticalColor = Color.red;

        public DamageTextUIHandleParameter() { }
    }
}
