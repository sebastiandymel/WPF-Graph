using System.Windows;
using System.Windows.Media;

namespace Chart
{
    public class AreaDrawing : IDrawing
    {
        private ResourceProvider resourceProvider = new ResourceProvider();

        public AreaDrawing()
        {
        }

        public Visual Draw(Curve c, Axis xAxis, Axis yAxis, string side)
        {
            var item = c as AreaCurve;
            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pointXs = xAxis.Map(item.F);
            var pointXs2 = xAxis.Map(item.F2);
            var pointYs = yAxis.Map(item.V);
            var pointYs2 = yAxis.Map(item.V2);

            var pathFigure = new PathFigure
            {
                StartPoint = new Point(pointXs[0], pointYs[0]),
                IsClosed = true,
                IsFilled = true
            };
            
            var polyLine = new PolyLineSegment();
            for (int i = 0; i < pointXs.Length; i++)
            {
                polyLine.Points.Add(new Point(pointXs[i], pointYs[i]));
            }
            pathFigure.Segments.Add(polyLine);
            polyLine = new PolyLineSegment();
            for (int i = pointXs2.Length - 1; i >= 0; i--)
            {
                polyLine.Points.Add(new Point(pointXs2[i], pointYs2[i]));
            }
            
            pathFigure.Segments.Add(polyLine);
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            
            var fillBrush = this.resourceProvider.GetAreaCurveFill(item, side);
            var strokeBrush = this.resourceProvider.GetAreaCurveBrush(item, side);
            var thickness = this.resourceProvider.GetCurveThickness(item, side);
            
            dc.DrawGeometry(fillBrush, new Pen(strokeBrush, thickness), pathGeometry);
            dc.Close();

            return dv;
        }
    }
}
