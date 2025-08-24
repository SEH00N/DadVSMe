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

            if (textRef == null)
                return;
                
            textRef.InitializeAsync().Forget();

            DamageText text = PoolManager.Spawn(textRef).GetComponent<DamageText>();
            text.Setup(CameraManager.UICam);
            text.Play(handleParameter.target, handleParameter.upOffset, (int)handleParameter.damage,
                handleParameter.isCritical, GetColor(handleParameter.attackAttribute), handleParameter.criticalColor);
        }

        Color GetColor(EAttackAttribute attackAttribute)
        {
            switch (attackAttribute)
            {
                case EAttackAttribute.Normal:
                    return Color.white;
                case EAttackAttribute.Crazy:
                    return Color.blue;
                case EAttackAttribute.Fire:
                    return Color.red;
                default:
                    return Color.white;
            }
        }
    }

    public class DamageTextUIHandleParameter : UIHandleParameter
    {
        public Transform target;
        public Vector3 upOffset;
        public AttackDataBase attackData;
        public float damage;
        public EAttackAttribute attackAttribute;
        public bool isCritical = false;
        public Color criticalColor = Color.red;

        public DamageTextUIHandleParameter() { }
    }
}
