using System;

namespace Chart
{
    public class Curve
    {
        private readonly double[] frequencies = Array.Empty<double>();
        private readonly double[] values = Array.Empty<double>();

        public Curve(double[] frequencies, double[] values, ChartType type)
        {
            this.frequencies = frequencies;
            this.values = values;
            ChartType = type;
        }
        public string CurveName { get; set; }
        public double[] F => this.frequencies;
        public double[] V => this.values;
        public (double f, double v)this[int i] => (F[i], V[i]);
        public event EventHandler DataChanged;
        public ChartType ChartType { get; private set; }
        public void RequestUpdate()
        {
            DataChanged(this, EventArgs.Empty);
        }        
    }
}
