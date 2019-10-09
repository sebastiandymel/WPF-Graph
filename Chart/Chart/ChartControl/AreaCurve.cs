using System;

namespace Chart
{
    public class AreaCurve: Curve
    {
        private readonly double[] frequencies2 = Array.Empty<double>();
        private readonly double[] values2 = Array.Empty<double>();

        public AreaCurve(double[] frequencies, double[] values, double[] frequencies2, double[] values2, ChartType type) 
            : base(frequencies, values, type)
        {
            this.frequencies2 = frequencies2;
            this.values2 = values2;
        }

        public double[] F2 => this.frequencies2;
        public double[] V2 => this.values2;
        public (double f, double v, double f2, double v2) this[int i] => (F[i], V[i], F2[i], V2[i]);      
    }
}
