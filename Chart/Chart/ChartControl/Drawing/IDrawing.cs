using System.Windows.Media;

namespace Chart
{
    public interface IDrawing
    {
        Visual Draw(Curve item, Axis xAxis, Axis yAxis, string side);
    }
}
