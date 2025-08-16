namespace DadVSMe
{
    public partial class SourceVolumeFader
    {
        public enum EFadeState
        {
            None = 0,
            FadeIn = 1,
            Playing = 1 << 1,
            FadeOut = 1 << 2,
            Transitioning = FadeIn | FadeOut
        }
    }
}