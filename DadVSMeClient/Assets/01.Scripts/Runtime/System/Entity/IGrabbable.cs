namespace DadVSMe.Entities
{
    public interface IGrabbable
    {
        public float Weight { get; set; }

        public void Grab(Unit performer);
        public void Release(Unit performer);
    }
}
