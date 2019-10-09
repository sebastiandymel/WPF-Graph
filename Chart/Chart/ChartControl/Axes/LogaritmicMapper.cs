using System;
using System.Linq;
using System.Windows.Controls;

namespace Chart
{
    public class LogaritmicMapper : PointMapper
    {
        public LogaritmicMapper(Canvas canvas) : base(canvas)
        {
        }

        public override double[] GetScreenYs(double[] input, double min, double max)
        {
            var logValues = input.Select(x => Math.Log10(x)).ToArray();
            return base.GetScreenYs(logValues, Math.Log10(min), Math.Log10(max));
        }

        public override double[] GetScreenXs(double[] input, double min, double max)
        {            
            var logValues = input.Select(x => Math.Log10(x)).ToArray();
            return base.GetScreenXs(logValues, Math.Log10(min), Math.Log10(max));
        }
    }
}
