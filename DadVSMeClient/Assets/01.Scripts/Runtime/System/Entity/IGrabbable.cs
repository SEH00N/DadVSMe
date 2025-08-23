namespace DadVSMe.Entities
{
    public interface IGrabbable
    {
        public float Weight { get; set; }

        public void Grab(Entity performer);
        public void Release(Entity performer);
    }
}
