#region References 

using System.Linq;
using System.Timers;
using System.Threading;
using Watch = System.Timers.Timer;

using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.SoundOut;

using LED.FTT;
using static LED.Values.Constants;

#endregion

namespace LED.Effects
{
    public class Visualization
    {
        private WasapiCapture _soundIn;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private LineSpectrum _LS_High;
        private LineSpectrum _LS_Mid;
        private LineSpectrum _LS_Low;

        public static int highVal = 0;
        public static int midVal = 0;
        public static int lowVal = 0;

        private Watch _Timer = new Watch()
        {
            Enabled = false,
            Interval = 33,
        };

        public Visualization()
        {
            new Thread(() =>
            {
                Stop();

                //open the default device 
                _soundIn = new WasapiLoopbackCapture();
                _soundIn.Initialize();

                var soundInSource = new SoundInSource(_soundIn);

                SetupSampleSource(soundInSource.ToSampleSource());

                // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
                byte[] buffer = new byte[_source.WaveFormat.BytesPerSecond / 2];
                soundInSource.DataAvailable += (s, aEvent) =>
                {
                    int read;
                    while ((read = _source.Read(buffer, 0, buffer.Length)) > 0) ;
                };

                _soundIn.Start(); //play the audio

                _Timer.Elapsed += new ElapsedEventHandler(GenerateEvent);
                _Timer.Start();
            }).Start();
        }

        static void _Scroll(byte[] rgb)
        {
            switch (Scroll)
            {
                case 0:
                    {
                        // MIDDLE MIRROR
                        // Divides array in half and progresses both halves
                        byte[] half1 = rgb.Concat(Program.data.Skip(LED_COUNT / 2 * 3).Take((LED_COUNT / 2 * 3) - 3)).ToArray();
                        byte[] half2 = Program.data.Take(LED_COUNT / 2 * 3).Skip(3).Concat(rgb).ToArray();
                        // Merges both halves together
                        Program.data = half2.Concat(half1).ToArray();
                        break;
                    }
                case 1:
                    {
                        // LEFT FULL
                        // Shifts all the data to the left and adds in the new color in the front
                        Program.data = Program.data.Skip(3).Concat(new byte[] { rgb[0], rgb[1], rgb[2] }).ToArray();
                        break;
                    }
                case 2:
                    {
                        // RIGHT FULL
                        // Shifts all the data to the right and adds in the new color in the front
                        Program.data = (new byte[] { rgb[0], rgb[1], rgb[2] }).Concat(Program.data.Take(Program.data.Length - 3)).ToArray();
                        break;
                    }
            }
        }

        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _LS_High = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 1,//50,
                BarSpacing = 0,//2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt,
                MaximumFrequency = 24000,
                MinimumFrequency = 4000
            };

            _LS_Mid = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 1,//50,
                BarSpacing = 0,//2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt,
                MaximumFrequency = 4000,
                MinimumFrequency = 250
            };

            _LS_Low = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 1,//50,
                BarSpacing = 0,//2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt,
                MaximumFrequency = 250,
                MinimumFrequency = 20
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);
        }

        public void Stop()
        {
            // Stops all the actions
            _Timer.Stop();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_soundIn != null)
            {
                _soundIn.Stop();
                _soundIn.Dispose();
                _soundIn = null;
            }
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }

        private void GenerateEvent(object source, ElapsedEventArgs e)
        {
            _LS_High.CreateSpectrumLine(0);
            _LS_Mid.CreateSpectrumLine(1);
            _LS_Low.CreateSpectrumLine(2);

            byte[] max = new byte[3] { 0, 0, 0 };

            // Only show the values if they reach past a certain threshold
            if (lowVal > 180)
                max[0] = (byte)lowVal;

            if (midVal > 88)
                max[1] = (byte)midVal;

            if (highVal > 37)
                max[2] = (byte)highVal;

            // Shifts the array
            _Scroll(max);
        }
    }
}
