using System.Windows;

namespace Chart
{
    public class DrawingProvider: DependencyObject
    {
        public string CurveName
        {
            get { return (string)GetValue(CurveNameProperty); }
            set { SetValue(CurveNameProperty, value); }
        }
        public static readonly DependencyProperty CurveNameProperty = DependencyProperty.Register("CurveName", typeof(string), typeof(DrawingProvider), new PropertyMetadata(null));
               
        public ChartType ChartType
        {
            get { return (ChartType)GetValue(ChartTypeProperty); }
            set { SetValue(ChartTypeProperty, value); }
        }
        public static readonly DependencyProperty ChartTypeProperty = DependencyProperty.Register("ChartType", typeof(ChartType), typeof(DrawingProvider), new PropertyMetadata(default(ChartType)));
        
        public IDrawing Value
        {
            get { return (IDrawing)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(IDrawing), typeof(DrawingProvider), new PropertyMetadata(0));

    }
}
