using System.Windows.Controls;

namespace Chart
{
    public class PointMapper
    {
        protected Canvas Canvas { get; }

        public PointMapper(Canvas canvas)
        {
            Canvas = canvas;
        }

        public virtual double[] GetScreenXs(double[] input, double min, double max)
        {
            var width = Canvas.ActualWidth;
            var result = new double[input.Length];
            var ratio = width / (max - min);
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ratio * (input[i] - min);
            }
            return result;
        }

        public virtual double[] GetScreenYs(double[] input, double min, double max)
        {
            var height = Canvas.ActualHeight;
            var result = new double[input.Length];
            var ratio = height / (max - min);
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ratio * input[i];
            }
            return result;
        }
    }   
}
