using System.Threading.Tasks;
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
        public override async void Execute(DamageTextUIHandleParameter handleParameter)
        {
            // AddressableAsset<DamageText> textRef =
            //     handleParameter.feedbackData.GetFeedbackData(handleParameter.attackAttribute)?.hitText;

            if (handleParameter.damageTextPrefab == null)
                return;
            if (string.IsNullOrEmpty(handleParameter.damageTextPrefab.Key))
                return;
                
            await handleParameter.damageTextPrefab.InitializeAsync();

            DamageText text = PoolManager.Spawn(handleParameter.damageTextPrefab).GetComponent<DamageText>();

            if (text == null)
                return;

            text.Setup(CameraManager.UICam);
            text.Play(handleParameter.target, handleParameter.upOffset, (int)handleParameter.damage,
                handleParameter.isCritical, handleParameter.attackAttribute);
        }

        // private Color GetColor(EAttackAttribute attackAttribute)
        // {
        //     switch (attackAttribute)
        //     {
        //         case EAttackAttribute.Normal:
        //             return Color.white;
        //         case EAttackAttribute.Crazy:
        //             return Color.blue;
        //         case EAttackAttribute.Fire:
        //             return Color.red;
        //         default:
        //             return Color.white;
        //     }
        // }
    }

    public class DamageTextUIHandleParameter : UIHandleParameter
    {
        public Transform target;
        public Vector3 upOffset;
        public AddressableAsset<DamageText> damageTextPrefab;
        public float damage;
        public EAttackAttribute attackAttribute;
        public bool isCritical = false;

        public DamageTextUIHandleParameter() { }
    }
}
