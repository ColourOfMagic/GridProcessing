using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GridProcessing.Model
{
    class MatrixBorder : Border
    {
        public int State
        {
            get => (int)GetValue(DependencyState);
            set => SetValue(DependencyState, value);
        }
        public Brush ActiveBrush
        {
            get => (Brush)GetValue(ActiveBrushProperty);
            set => SetValue(ActiveBrushProperty, value);
        }
        public Brush InactiveBrush
        {
            get => (Brush)GetValue(InactiveBrushProperty);
            set => SetValue(InactiveBrushProperty, value);
        }
        public static readonly DependencyProperty DependencyState;
        public static readonly DependencyProperty InactiveBrushProperty;
        public static readonly DependencyProperty ActiveBrushProperty;

        static MatrixBorder()
        {
            ActiveBrushProperty = DependencyProperty.Register(
                "ActiveBrush",
                typeof(Brush),
                typeof(MatrixBorder),
                new FrameworkPropertyMetadata(Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
                ));

            InactiveBrushProperty = DependencyProperty.Register(
                "InactiveBrush",
                typeof(Brush),
                typeof(MatrixBorder),
                new FrameworkPropertyMetadata(Brushes.White,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
                ));

            DependencyState = DependencyProperty.Register(
                "State",
                typeof(int),
                typeof(MatrixBorder),
                new FrameworkPropertyMetadata(2,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                (source, args) => { ((MatrixBorder)source).Call((int)args.NewValue); })
                );

        }

        private void Call(int i)
        {
            if (i == 0) Background = InactiveBrush;
            if (i == 1) Background = ActiveBrush;
        }

        public MatrixBorder() : base()
        {
            MouseDown += Click;
            MouseMove += BMouseMove;
        }

        private void BMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) State = 1;
            if (e.RightButton == MouseButtonState.Pressed) State = 0;
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            State = (State == 1) ? 0 : 1;
        }
    }
}
