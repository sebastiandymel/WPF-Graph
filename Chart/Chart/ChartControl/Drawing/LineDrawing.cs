using System.Windows;
using System.Windows.Media;

namespace Chart
{
    public class LineDrawing : IDrawing
    {
        private ResourceProvider resourceProvider = new ResourceProvider();

        public LineDrawing()
        {
        }

        public Visual Draw(Curve item, Axis xAxis, Axis yAxis, string side)
        {
            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pointXs = xAxis.Map(item.F);
            var pointYs = yAxis.Map(item.V);

            var pathFigure = new PathFigure
            {
                StartPoint = new Point(pointXs[0], pointYs[0]),
                IsClosed = false,
                IsFilled = false
            };
            var polyLine = new PolyLineSegment();
            for (int i = 0; i < pointXs.Length; i++)
            {
                polyLine.Points.Add(new Point(pointXs[i], pointYs[i]));
            }
            pathFigure.Segments.Add(polyLine);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            var brush = this.resourceProvider.GetCurveBrush(item, side);
            var thickness = this.resourceProvider.GetCurveThickness(item, side);
            dc.DrawGeometry(brush, new Pen (brush, thickness), pathGeometry);
            dc.Close();

            return dv;
        }
    }
}
