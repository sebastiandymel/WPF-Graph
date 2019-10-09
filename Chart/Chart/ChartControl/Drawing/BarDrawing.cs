using System.Windows;
using System.Windows.Media;

namespace Chart
{
    public class BarDrawing: IDrawing
    {
        private ResourceProvider resourceProvider = new ResourceProvider();

        public BarDrawing()
        {
        }

        public Visual Draw(Curve item, Axis xAxis, Axis yAxis, string side)
        {
            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pointXs = xAxis.Map(item.F);
            var pointYs = yAxis.Map(item.V);

            var brush = this.resourceProvider.GetCurveBrush(item, side);
            var fillBrush = this.resourceProvider.GetBarCurveFill(item, side);
            var thickness = this.resourceProvider.GetCurveThickness(item, side);

            for (int i =0; i< pointXs.Length; i+=2)
            {
                var rect = new Rect(pointXs[i], pointYs[i], pointXs[i + 1] - pointXs[i], pointYs[i + 1] - pointYs[i]);
                dc.DrawRectangle(fillBrush, new Pen(brush, thickness), rect);
            }           
            
            dc.Close();

            return dv;
        }
    }
}
