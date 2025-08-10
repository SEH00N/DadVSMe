namespace DadVSMe
{
    public class MoveSpeedUpSkill : StatUpSkill
    {
        public override void Execute()
        {
            base.Execute();

            IMovement movement = ownerComponent.GetComponent<IMovement>();

            movement.SetMoveSpeed(movement.GetMoveSpeed() + StatUpAmount()); //임의 수식. 나중에 테이블 만들어서 가져오든 해야할듯
        }
    }
}
