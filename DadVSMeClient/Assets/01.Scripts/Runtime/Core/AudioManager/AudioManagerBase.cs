using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DadVSMe
{
    public enum EAudioChannel
    {
        None = 0,
        BGM = 1 << 0,
        SFX = 1 << 1,
        Master = BGM | SFX
    }

    public class ChannelData
    {
        public EAudioChannel channel;
        public string channelName;
        public float volume;

        public ChannelData(EAudioChannel channel)
        {
            this.channel = channel;
            channelName = channel.ToString();
            volume = 0f;
        }
    }

    public abstract class AudioManagerBase : MonoBehaviour
    {
        public const float AUDIO_MIN_VOLUME = -80f;

        private AudioMixer audioMixer = null;
        private EAudioChannel baseModeChannelFlag = EAudioChannel.None;
        private Dictionary<EAudioChannel, ChannelData> channelDictionary = new Dictionary<EAudioChannel, ChannelData>();
        
        protected virtual void Awake()
        {
            channelDictionary = new Dictionary<EAudioChannel, ChannelData>();
            foreach (EAudioChannel channel in Enum.GetValues(typeof(EAudioChannel)))
            {
                if (channel == EAudioChannel.None)
                    continue;

                ChannelData channelData = new ChannelData(channel);
                channelDictionary.Add(channel, channelData);
            }
        }

        protected virtual void Initialize(AudioMixer audioMixer)
        {
            this.audioMixer = audioMixer;
        }

        public void SetBassMode(EAudioChannel channel, bool isOn)
        {
            if (isOn)
                baseModeChannelFlag |= channel;
            else
                baseModeChannelFlag &= ~channel;

            foreach(EAudioChannel singleChannel in Enum.GetValues(typeof(EAudioChannel)))
            {
                if (singleChannel == EAudioChannel.None)
                    continue;

                if ((channel & singleChannel) == singleChannel)
                    SetVolume(singleChannel, channelDictionary[singleChannel].volume);
            }
        }

        public void SetVolume(EAudioChannel channel, float volume)
        {
            if (volume < 0f || volume > 1f)
            {
                Debug.LogError("Volume must be between 0 and 1");
                return;
            }

            ChannelData channelData = channelDictionary[channel];
            channelData.volume = volume;

            if (IsBassMode(channel))
                volume *= 0.5f;

            float resizedVolume = (volume <= 0f) ? AUDIO_MIN_VOLUME : (Mathf.Log10(volume) * 20f);
            bool result = audioMixer.SetFloat(channelDictionary[channel].channelName, resizedVolume);
            if (result == false)
                Debug.LogWarning("Failed to set volume");
        }

        public float GetVolume(EAudioChannel channel)
        {
            return channelDictionary[channel].volume;
        }

        public bool IsBassMode(EAudioChannel channel) 
        {
            return (baseModeChannelFlag & channel) == channel;
        }
    }
}