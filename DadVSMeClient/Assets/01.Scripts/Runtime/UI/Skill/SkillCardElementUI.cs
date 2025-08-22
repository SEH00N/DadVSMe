namespace DadVSMe.UI.Skills
{
    public class SkillCardElementUI : PoolableBehaviourUI<SkillCardElementUI.ICallback>
    {
        public interface ICallback : IUICallback
        {
            void OnSelectCard(SkillData skillData);
        }

        private SkillData skillData = null;

        public void Initialize(SkillData skillData, int currentLevel, ICallback callback)
        {
            base.Initialize(callback);
            this.skillData = skillData;
        }

        public void OnTouchThis()
        {
            callback.OnSelectCard(skillData);
        }
    }
}