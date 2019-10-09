using System.Windows;
using System.Windows.Media;

namespace Chart
{
    public class ResourceProvider
    {        
        public Brush GetCurveBrush(Curve item, string side)
        {
            var lookupKeys = new[]
            {
                $"StrokeBrush.Curve.{item.ChartType}.{item.CurveName}.{side}",
                $"StrokeBrush.Curve.{item.ChartType}.{side}",
                $"StrokeBrush.Curve.{item.ChartType}.{item.CurveName}",
                $"StrokeBrush.Curve.{item.CurveName}",
                $"StrokeBrush.Curve.{item.ChartType}",
                "StrokeBrush.Curve"
            };

            var result = GetFromLookup<Brush>(lookupKeys);
            return result ?? new SolidColorBrush(Colors.Black);
        }

        public double GetCurveThickness(Curve item, string side)
        {
            return 2;
        }

        public Brush GetAreaCurveBrush(AreaCurve item, string side)
        {
            var lookupKeys = new[]
{
                $"StrokeBrush.Curve.{item.ChartType}.{item.CurveName}.{side}",
                $"StrokeBrush.Curve.{item.ChartType}.{side}",
                $"StrokeBrush.Curve.{item.ChartType}.{item.CurveName}",
                $"StrokeBrush.Curve.{item.CurveName}",
                $"StrokeBrush.Curve.{item.ChartType}",
                "StrokeBrush.Curve"
            };

            return GetFromLookup<Brush>(lookupKeys) ?? new SolidColorBrush(Colors.Black);
        }

        public Brush GetAreaCurveFill(AreaCurve item, string side)
        {
            var lookupKeys = new[]
{
                $"FillBrush.Curve.{item.ChartType}.{item.CurveName}.{side}",
                $"FillBrush.Curve.{item.ChartType}.{side}",
                $"FillBrush.Curve.{item.ChartType}.{item.CurveName}",
                $"FillBrush.Curve.{item.CurveName}",
                $"FillBrush.Curve.{item.ChartType}",
                "FillBrush.Curve"
            };

            return GetFromLookup<Brush>(lookupKeys) ?? new SolidColorBrush(Colors.Yellow);
        }

        public Brush GetBarCurveFill(Curve item, string side)
        {
            var lookupKeys = new[]
{
                $"FillBrush.Curve.{item.ChartType}.{item.CurveName}.{side}",
                $"FillBrush.Curve.{item.ChartType}.{side}",
                $"FillBrush.Curve.{item.ChartType}.{item.CurveName}",
                $"FillBrush.Curve.{item.CurveName}",
                $"FillBrush.Curve.{item.ChartType}",
                "FillBrush.Curve"
            };

            return GetFromLookup<Brush>(lookupKeys) ?? new SolidColorBrush(Colors.DarkGreen);
        }

        private T GetFromLookup<T>(string[] lookupKeys)
        {
            for (int i = 0; i < lookupKeys.Length; i++)
            {
                if (Application.Current.Resources.Contains(lookupKeys[i]))
                {
                    return (T)Application.Current.Resources[lookupKeys[i]];
                }
            }
            return default;
        }
    }
}
