using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Chart
{
    public class ViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer dt = new DispatcherTimer();

        public ObservableCollection<Curve> Curves { get; set; }

        public string FrameRate { get; set; }

        public string NumberOfRenderedPoints { get; set; }

        public double Rate
        {
            get => rate;
            set
            {
                rate = value;
                this.intervalMs = 1000 / rate;
                this.dt.Interval = TimeSpan.FromMilliseconds(this.intervalMs);
                FrameRate = $"Frame rate: {Math.Round(1 / (intervalMs * 1e-3), 1)} fr/s";
                OnPropertyChanged(nameof(FrameRate));
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            Curves = new ObservableCollection<Curve>();
            AddLimits();
            AddArea("first");
            AddArea("second");
            AddArea("third");
            AddCurve();
            AddCurve();
            AddCurve();
            AddCurve();
            AddCurve();
            AddCurve();
            AddOther();
            AddBarCurve();
            this.dt = new DispatcherTimer();
            this.dt.Interval = TimeSpan.FromMilliseconds(intervalMs);
            this.dt.Tick += OnTick;
            dt.Start();

            Rate = 25;

            NumberOfRenderedPoints = $"Number of points in each graph: {Curves.Select(x => x.V).SelectMany(v => v).Count()}";
        }

        private void AddBarCurve()
        {
            var fs = new[] { 200.0, 250,    1000, 1500,   2000, 3000,   4000, 5000};
            var vs = new[] { 0.0,   20,     0.0,  50,     0,    35,     0,    40 };
            
            Curves.Add(new Curve(fs, vs, ChartType.Bar) { CurveName = "MyBars" });
        }

        private void AddLimits()
        {
            var fs = new[] { 125.0, 250, 500, 750, 1000, 2000, 3000, 5000, 7000, 8000, 10_000 };
            var vs = new[] { 20.0, 25, 20, 25, 20, 25, 20, 25, 20, 25, 30 };
            var vs_up = new[] { 0.0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Curves.Add(new AreaCurve(fs, vs_up, fs, vs, ChartType.Area) { CurveName = "TopLimit" });
        }

        private void AddCurve()
        {
            var pointsCount = Frequencies.Length;
            var f = new double[pointsCount];
            var v = new double[pointsCount];
            for (int i = 0; i < pointsCount; i++)
            {
                f[i] = Frequencies[i];
                v[i] = 10;
            }
            Curves.Add(new Curve(f, v, ChartType.Line));
        }

        private void AddArea(string name)
        {
            var pointsCount = Frequencies.Length;
            var f = new double[pointsCount];
            var v = new double[pointsCount];
            var v2 = new double[pointsCount];
            for (int i = 0; i < pointsCount; i++)
            {
                f[i] = Frequencies[i];
                v[i] = 10 + 10 * Math.Sin(10 * Math.Log(f[i]));
                v2[i] = 30 + 4 * Math.Cos(15 * Math.Log(f[i]));
            }
            Curves.Add(new AreaCurve(f, v, f, v2, ChartType.Area) { CurveName = name });
        }

        public void AddOther()
        {
            var fs = new[] { 125.0, 250, 500, 750, 1000, 2000, 3000, 5000, 7000, 8000, 10_000 };
            var vs = new[] { 30.0, 40, 30, 40, 30, 40, 30, 40, 30, 40, 30 };
            Curves.Add(new Curve(fs, vs, ChartType.Line));
        }

        private void OnTick(object sender, EventArgs e)
        {
            var ssize = this.intervalMs / 12.5;
            offset = offset + (increase ? ssize : -ssize);
            int areaOffset = 0;

            for (var ci = 0; ci < Curves.Count - 2; ci++)
            {
                var c = Curves[ci];
                for (int i = 0; i < c.V.Length; i++)
                {
                    if (c is AreaCurve ac)
                    {
                        if (c.CurveName == "TopLimit")
                        {
                            continue;
                        }
                        ac.V[i] = areaOffset + 10 + 4 * Math.Sin(offset / 1.5 * Math.Log(ac.F[i]));
                        ac.V2[i] = areaOffset + 20 + (ci + 1) * Math.Cos(offset / 1.5 * Math.Log(ac.F[i]));
                    }
                    else if (ci == 5)
                    {
                        c.V[i] = constOffsets[ci] + Math.Log10(c.F[i]) * offset / 3 + 2 * Math.Sin(c.F[i] / offset) + 10 * Math.Sin(Math.Log10(c.F[i]) / (Math.PI));
                    }
                    else if (ci == 5)
                    {
                        c.V[i] = constOffsets[ci] + offset + 3 * Math.Sin(Math.Log10(c.F[i]) * 20);
                    }
                    else
                    {
                        c.V[i] = constOffsets[ci] + offset + 5 * Math.Cos(Math.Log10(c.F[i]) * 10) + this.rnd.NextDouble()*3;
                    }
                }
                if (c is AreaCurve)
                {
                    areaOffset += 20;
                }

                c.RequestUpdate();
            }

            var cbar = Curves.Single(x => x.ChartType == ChartType.Bar);
            for (int i = 1; i < cbar.V.Length; i+=2)
            {
                cbar.V[i] = i * 10 + this.rnd.Next(20);
            }
            cbar.RequestUpdate();




            if (offset > 80)
            {
                increase = false;
            }
            if (offset <= 0)
            {
                increase = true;
            }
        }

        private double intervalMs = 40.0;
        private bool increase = true;
        private double offset = 0;
        private Random rnd = new Random();
        private double[] constOffsets = new double[] { 0, 0, 0, 0, 20.0, 40, 60, 70, 75, 80 };
        private double rate;
        public static readonly double[] Frequencies = new[]
{
            100.000,102.920,105.925,109.018,112.202,115.478,118.850,122.321,125.893,129.569,133.352,
            137.246,141.254,145.378,149.624,153.993,158.489,163.117,167.880,172.783,177.828,183.021,
            188.365,193.865,199.526,205.353,211.349,217.520,223.872,230.409,237.137,244.062,251.189,
            258.523,266.073,273.842,281.838,290.068,298.538,307.256,316.228,325.462,334.965,344.747,
            354.813,365.174,375.837,386.812,398.107,409.732,421.697,434.010,446.684,459.727,473.151,
            486.968,501.187,515.822,530.884,546.387,562.341,578.762,595.662,613.056,630.957,649.382,
            668.344,687.860,707.946,728.618,749.894,771.792,794.328,817.523,841.395,865.964,891.251,
            917.276,944.061,971.628,1000.000,1029.201,1059.254,1090.184,1122.018,1154.782,1188.502,
            1223.20,1258.925,1295.687,1333.521,1372.461,1412.538,1453.784,1496.236,1539.927,1584.893,
            1631.17,1678.804,1727.826,1778.279,1830.206,1883.649,1938.653,1995.262,2053.525,2113.489,
            2175.20,2238.721,2304.093,2371.374,2440.619,2511.886,2585.235,2660.725,2738.420,2818.383,
            2900.68,2985.383,3072.557,3162.278,3254.618,3349.654,3447.466,3548.134,3651.741,3758.374,
            3868.12,3981.072,4097.321,4216.965,4340.103,4466.836,4597.270,4731.513,4869.675,5011.872,
            5158.22,5308.844,5463.865,5623.413,5787.620,5956.621,6130.558,6309.573,6493.816,6683.439,
            6878.59,7079.458,7286.182,7498.942,7717.915,7943.282,8175.230,8413.951,8659.643,8912.509,
            9172.75,9440.609,9716.280,10000.000
        };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

}
