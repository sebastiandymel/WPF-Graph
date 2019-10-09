using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart
{
    public class ChartControl : Control
    {
        private Dictionary<Curve, Host> CurveVisual = new Dictionary<Curve, Host>();
        private DrawingSelector drawingSelector = new DrawingSelector();
        private Axis xAxis;
        private Axis yAxis;
        private Canvas chartArea;
        private Canvas xAxisArea;
        private Canvas yAxisArea;

        public ChartControl()
        {
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReDrawAll();
        }

        public IEnumerable CurvesSource
        {
            get { return (IEnumerable)GetValue(CurvesSourceProperty); }
            set { SetValue(CurvesSourceProperty, value); }
        }
        public static readonly DependencyProperty CurvesSourceProperty = DependencyProperty.Register(
            "CurvesSource",
            typeof(IEnumerable),
            typeof(ChartControl),
            new PropertyMetadata(null, OnCurvesChanged));

        private static void OnCurvesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ChartControl)d).UpdateCurves();
        }

        private void UpdateCurves()
        {
            if (CurvesSource is IEnumerable<Curve> source)
            {
                foreach (var item in source)
                {
                    item.DataChanged += OnCurveDataChanged;
                }
            }
            ReDrawAll();
        }
                
        public string Side
        {
            get { return (string)GetValue(SideProperty); }
            set { SetValue(SideProperty, value); }
        }
        public static readonly DependencyProperty SideProperty = DependencyProperty.Register("Side", typeof(string), typeof(ChartControl), new PropertyMetadata(""));
        
        public List<DrawingProvider> CurstomDrawers
        {
            get { return (List<DrawingProvider>)GetValue(CurstomDrawersProperty); }
            set { SetValue(CurstomDrawersProperty, value); }
        }
        public static readonly DependencyProperty CurstomDrawersProperty = DependencyProperty.Register("CurstomDrawers", typeof(List<DrawingProvider>), typeof(ChartControl), new PropertyMetadata(null, OnCustomDrawersChanged));

        private static void OnCustomDrawersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (ChartControl)d;
            chart.RegisterCustomDrawers(e.NewValue as List<DrawingProvider>);
            chart.UnregisterCustomDrawers(e.OldValue as List<DrawingProvider>);
        }

        private void RegisterCustomDrawers(List<DrawingProvider> newDrawers)
        {
            if (newDrawers == null)
            {
                return;
            }
            foreach (var item in newDrawers)
            {
                this.drawingSelector.RegisterCustom(item.ChartType, item.CurveName, item.Value);
            }
        }

        private void UnregisterCustomDrawers(List<DrawingProvider> oldDrawers)
        {
            if (oldDrawers == null)
            {
                return;
            }
            foreach (var item in oldDrawers)
            {
                this.drawingSelector.UnRegisterCustom(item.ChartType, item.CurveName, item.Value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.chartArea = Template.FindName("PART_CHART", this) as Canvas;
            this.xAxisArea = Template.FindName("PART_XAXIS", this) as Canvas;
            this.yAxisArea = Template.FindName("PART_YAXIS", this) as Canvas;
            this.xAxis = new Axis(new LogaritmicMapper(this.chartArea), AxisOrigin.X);
            this.xAxis.Min = 125;
            this.xAxis.Max = 10_000;
            this.yAxis = new Axis(new PointMapper(this.chartArea), AxisOrigin.Y);
            this.yAxis.Min = 0;
            this.yAxis.Max = 120;

            ReDrawAll();
        }

        private void OnCurveDataChanged(object sender, EventArgs e)
        {
            var curve = (Curve)sender;
            if (CurveVisual.TryGetValue(curve, out var host))
            {
                var dv = GetCurveVisual(curve);
                host.Visual = dv;
                host.InvalidateVisual();
            }
            else
            {
                DrawCurveAndAddToChart(curve);
            }
        }

        private void ReDrawAll()
        {
            this.yAxisArea.Children.Clear();
            this.xAxisArea.Children.Clear();
            this.chartArea.Children.Clear();
            this.CurveVisual.Clear();
            DrawLegend();
            DrawCurves();
        }

        private void DrawCurves()
        {
            if (CurvesSource is IEnumerable<Curve> source)
            {
                foreach (var item in source)
                {
                    DrawCurveAndAddToChart(item);
                }
            }
        }

        private void DrawCurveAndAddToChart(Curve item)
        {
            var dv = GetCurveVisual(item);
            var host = new Host();
            host.Visual = dv;
            this.chartArea.Children.Add(host);
            CurveVisual[item] = host;
        }

        private Visual GetCurveVisual(Curve item)
        {
            if (this.drawingSelector.CanDraw(item.ChartType, item.CurveName))
            {
                return this.drawingSelector[item.ChartType, item.CurveName].Draw(item, this.xAxis, this.yAxis, Side);
            }
            return this.drawingSelector[item.ChartType].Draw(item, this.xAxis, this.yAxis, Side);
        }

        private void DrawLegend()
        {
            var ypoints = new List<double>();
            for (int i= (int)this.yAxis.Min; i<= yAxis.Max; i+=10)
            {
                ypoints.Add(i);
            }
            var screenYs = this.yAxis.Map(ypoints.ToArray());
            for (int i =0; i< screenYs.Length; i++)
            {
                var line = new Line();
                line.X1 = 0;
                line.X2 = this.chartArea.ActualWidth;
                line.Y1 = screenYs[i];
                line.Y2 = screenYs[i];
                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 0.5;
                this.chartArea.Children.Add(line);

                if (i > 0 && i < screenYs.Length)
                {
                    var ypos = screenYs[i-1] + (screenYs[i] - screenYs[i - 1])/2.0;
                    var minorline = new Line();
                    minorline.StrokeDashArray = new DoubleCollection(new []{ 3.0, 3.0 });
                    minorline.X1 = 0;
                    minorline.X2 = this.chartArea.ActualWidth;
                    minorline.Y1 = ypos;
                    minorline.Y2 = ypos;
                    minorline.Stroke = new SolidColorBrush(Colors.DarkGray);
                    minorline.StrokeThickness = 1;
                    this.chartArea.Children.Add(minorline);
                }

                var tb = new TextBlock { Width = 20, HorizontalAlignment = HorizontalAlignment.Right, TextAlignment = TextAlignment.Center, FontFamily = new FontFamily("Consolas"), Text = ypoints[i].ToString(), RenderTransformOrigin = new Point(0.5, 0.5) };
                Canvas.SetTop(tb, screenYs[i] - 8);
                this.yAxisArea.Children.Add(tb);
            }

            var legendPoints = new[] { 125.0, 250, 500, 750, 1000, 2000, 3000, 5000, 8000, 10_000 };
            var legendText = new[] { "125", "250", "500", "750", "1k", "2k", "3k", "5k", "8k", "10k" };
            var screenPoints = this.xAxis.Map(legendPoints);

            for (int i = 0; i < legendPoints.Length; i++)
            {
                var line = new Line();
                line.X1 = screenPoints[i];
                line.X2 = screenPoints[i];
                line.Y1 = 0;
                line.Y2 = screenYs[screenYs.Length-1];
                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 0.5;
                this.chartArea.Children.Add(line);

                var tb = new TextBlock { Width = 20, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center, FontFamily = new FontFamily("Consolas"), Text = legendText[i], RenderTransformOrigin = new Point(0.5, 0.5) };
                Canvas.SetLeft(tb, screenPoints[i] - 10);
                this.xAxisArea.Children.Add(tb);
            }
        }

        private class Host : FrameworkElement
        {
            private Visual visual;

            public Visual Visual
            {
                get => visual;
                set
                {
                    this.RemoveVisualChild(visual);
                    visual = value;
                    this.AddVisualChild(visual);                    
                }
            }

            protected override int VisualChildrenCount
            {
                get { return Visual != null ? 1 : 0; }
            }

            protected override Visual GetVisualChild(int index)
            {
                return Visual;
            }
        }
    }
}
