namespace Chart
{
    public class Axis
    {
        private PointMapper mapper;
        private readonly AxisOrigin origin;

        public Axis(PointMapper mapper, AxisOrigin origin)
        {
            this.mapper = mapper;
            this.origin = origin;
        }

        public double[] Map(double[] input) => this.origin == AxisOrigin.X ? this.mapper.GetScreenXs(input, Min, Max) : this.mapper.GetScreenYs(input, Min, Max);
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
