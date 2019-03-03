#region References 

using System;

using CSCore.DSP;

using LED.Effects;

#endregion

namespace LED.FTT
{
    public class LineSpectrum : SpectrumBase
    {
        private int _barCount;
        private double _barSpacing;

        public LineSpectrum(FftSize fftSize)
        {
            FftSize = fftSize;
        }

        public double BarSpacing
        {
            get { return _barSpacing; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _barSpacing = value;
                UpdateFrequencyMapping();

                RaisePropertyChanged("BarSpacing");
                RaisePropertyChanged("BarWidth");
            }
        }

        public int BarCount
        {
            get { return _barCount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _barCount = value;
                SpectrumResolution = value;
                UpdateFrequencyMapping();

                RaisePropertyChanged("BarCount");
                RaisePropertyChanged("BarWidth");
            }
        }

        public void CreateSpectrumLine(int type)
        {
            var fftBuffer = new float[(int)FftSize];

            //get the fft result from the spectrum provider
            if (!SpectrumProvider.GetFftData(fftBuffer, this))
            {
                return;
            }
            CreateSpectrumLineInternal(fftBuffer, type);
        }

        private void CreateSpectrumLineInternal(float[] fftBuffer, int type)
        {
            //prepare the fft result for rendering 
            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(255, fftBuffer);
            //connect the calculated points with lines
            for (int i = 0; i < spectrumPoints.Length; i++)
            {
                int p = (int)spectrumPoints[i].Value;

                //sort the auido to the correct channels
                switch (type)
                {
                    case 0:
                        {
                            Visualization.highVal = p;
                            break;
                        }
                    case 1:
                        {
                            Visualization.midVal = p;
                            break;
                        }
                    case 2:
                        {
                            Visualization.lowVal = p;
                            break;
                        }
                }
            }
        }

        protected override void UpdateFrequencyMapping()
        {
            base.UpdateFrequencyMapping();
        }
    }
}
