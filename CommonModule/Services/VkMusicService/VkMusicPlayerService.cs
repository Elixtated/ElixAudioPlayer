using NAudio.Wave;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CommonModule.Services.VkMusicService
{
    public class VkMusicPlayerService : IDisposable
    {
        private static readonly Lazy<VkMusicPlayerService> _instance = new Lazy<VkMusicPlayerService>(() => new VkMusicPlayerService());

        public static VkMusicPlayerService Instance { get { return _instance.Value; } }


        private VkTrack _vkTrack;


        public void PlayVkMusic(string url)
        {
            _vkTrack = new VkTrack(url);
            _vkTrack.PlayTrack();
        }

        public void PauseVkMusic()
        {
            _vkTrack.PauseTrack();
        }

        public void ResumeVkMusic()
        {
            _vkTrack.ResumeTrack();

        }

        public void StopVkMusic()
        {
            _vkTrack.StopTrack();
        }

        public bool HasTrack()
        {
            return _vkTrack == null ? false : true;
        }

        public TimeSpan GetPositionTrack()
        {
            return TimeSpan.FromSeconds(_vkTrack.GetPosition());
        }

        public void SetPositionTrack(double position)
        {
            _vkTrack.SetPosition(position);
        }

        public void Dispose()
        {
            _vkTrack.Dispose();
        }


        private class VkTrack : IDisposable
        {
            private WaveStream _waveStream;
            private MemoryStream _memoryStream;
            private Stream _responseStream;
            private WaveOut _waveOut;

            public VkTrack(string uri)
            {
                _memoryStream = new MemoryStream();


                _responseStream = WebRequest.Create(uri)
                   .GetResponse().GetResponseStream();

                byte[] buffer = new byte[32768];
                int read;
                while ((read = _responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _memoryStream.Write(buffer, 0, read);
                }


                _memoryStream.Position = 0;

                _waveStream = new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(_memoryStream)));
                _waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
                _waveOut.Init(_waveStream);
            }

            public void PlayTrack()
            {
                _waveOut.Play();
            }

            public void PauseTrack()
            {
                _waveOut.Pause();
            }

            public void ResumeTrack()
            {
                _waveOut.Init(_waveStream);
                _waveOut.Play();
                //_waveOut.Resume(); bugged

            }

            public void StopTrack()
            {
                _waveOut.Stop();
            }
            public long GetPosition()
            {
                return (long)(_waveOut.GetPosition() * 1d / _waveOut.OutputWaveFormat.AverageBytesPerSecond);
            }

            public void SetPosition(double position)
            {
                long adj = (long)position % _waveStream.BlockAlign;
                long newPos = Math.Max(0, Math.Min(_waveStream.Length, (long)position - adj));
                _waveStream.Position = newPos;
            }

            public void Dispose()
            {
                _waveStream.Dispose();
                _memoryStream.Dispose();
                _responseStream.Dispose();
            }
        }
    }
}
