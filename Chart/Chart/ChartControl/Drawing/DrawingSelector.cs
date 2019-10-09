using System.Collections.Generic;

namespace Chart
{
    public class DrawingSelector
    {
        private Dictionary<string, IDrawing> customMapping = new Dictionary<string, IDrawing>();
        private Dictionary<ChartType, IDrawing> mapping = new Dictionary<ChartType, IDrawing>()
        {
            { ChartType.Line, new LineDrawing() },
            { ChartType.Area, new AreaDrawing() },
            { ChartType.Bar, new BarDrawing() }
        };

        public IDrawing this[ChartType type] => this.mapping[type];
        public IDrawing this[ChartType type, string name] => this.customMapping[$"{type}.{name}"];
        public bool CanDraw(ChartType type, string name) => this.customMapping.ContainsKey($"{type}.{name}");
        public bool CanDraw(ChartType type) => this.mapping.ContainsKey(type);
        public void RegisterCustom(ChartType type,  string name, IDrawing drawing)
        {
            this.customMapping[$"{type}.{name}"] = drawing;
        }
        public void UnRegisterCustom(ChartType type, string name, IDrawing drawing)
        {
            this.customMapping.Remove($"{type}.{name}");
        }
    }
}
