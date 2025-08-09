namespace DadVSMe.Entities
{
    public interface IGrabbable
    {
        public void Grab(Entity performer);
        public void Release(Entity performer);
    }
}
