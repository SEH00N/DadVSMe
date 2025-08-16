namespace DadVSMe
{
    using static SourceVolumeFader;

    public partial class BGMController
    {

        private class Scheduler
        {
            private readonly BGMPlayer aChannel = null;
            private readonly BGMPlayer bChannel = null;

            private bool channelToggle = false; // 정보는 딱히 중요하지 않음. 그냥 토글되기만 하면 됨.
            public BGMPlayer CurrentBGMPlayer => channelToggle ? aChannel : bChannel;

            public Scheduler(BGMPlayer aChannel, BGMPlayer bChannel)
            {
                this.aChannel = aChannel;
                this.bChannel = bChannel;
            }

            public void PauseChannel()
            {
                aChannel.PauseBGM();
                bChannel.PauseBGM();

                aChannel.DoReset();
                bChannel.DoReset();
            }

            public void ToggleChannel()
            {
                channelToggle = !channelToggle;
            }

            public void ReschedulePlayer()
            {
                if (CheckPlayerState(EFadeState.Playing))
                {
                    HandleFadeState(aChannel.FadeState == EFadeState.Playing);
                    return;
                }

                if (CheckPlayerState(EFadeState.None))
                {
                    HandleFadeState(aChannel.FadeState != EFadeState.None);
                    return;
                }

                if (CheckPlayerState(EFadeState.Transitioning))
                {
                    HandleFadeState(aChannel.FadeState == EFadeState.FadeIn);
                    return;
                }
            }

            private bool CheckPlayerState(EFadeState state)
            {
                return ((aChannel.FadeState | bChannel.FadeState) & state) == state;
            }

            private void HandleFadeState(bool resetToggle)
            {
                if (resetToggle)
                {
                    aChannel.DoFadeOut();
                    bChannel.DoReset();
                    channelToggle = true;
                }
                else
                {
                    aChannel.DoReset();
                    bChannel.DoFadeOut();
                    channelToggle = false;
                }
            }
        }
    }
}