namespace DadVSMe
{
    public class BGMCacheData
    {
        public BGMAudioLibraryTable audioData;
        public float progress;

        public bool IsValid => audioData != null && progress >= 0f;

        public void SaveCache(BGMAudioLibraryTable audioData, float progress)
        {
            this.audioData = audioData;
            this.progress = progress;
        }

        public void ResetCache()
        {
            audioData = null;
            progress = 0f;
        }
    }
}