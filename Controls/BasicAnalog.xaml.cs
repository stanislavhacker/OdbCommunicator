using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Microsoft.Expression.Shapes;

namespace OdbCommunicator.Controls
{
    public partial class BasicAnalog : UserControl
    {
        #region DependencyProperty

        public static DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(250.0, StartPropertyChange));
        private static void StartPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.Start = (Double)e.NewValue;
        }

        public static DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(470.0, EndPropertyChange));
        private static void EndPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.End = (Double)e.NewValue;
        }


        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(0.0, ValuePropertyChange));
        private static void ValuePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.Value = (Double)e.NewValue;
        }

        public static DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(String), typeof(BasicAnalog), new PropertyMetadata("", UnitPropertyChange));
        private static void UnitPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.Unit = (String)e.NewValue;
        }

        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(String), typeof(BasicAnalog), new PropertyMetadata("", DescriptionPropertyChange));
        private static void DescriptionPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.Description = (String)e.NewValue;
        }

        public static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(0.0, MinValuePropertyChange));
        private static void MinValuePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.MinValue = (Double)e.NewValue;
        }

        public static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(100.0, MaxValuePropertyChange));
        private static void MaxValuePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.MaxValue = (Double)e.NewValue;
        }

        public static DependencyProperty BarColorProperty = DependencyProperty.Register("BarColor", typeof(Brush), typeof(BasicAnalog), new PropertyMetadata(new SolidColorBrush(Colors.Red), BarColorPropertyChange));
        private static void BarColorPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.BarColor = (Brush)e.NewValue;
        }

        public static DependencyProperty BarBackgroundColorProperty = DependencyProperty.Register("BarBackgroundColor", typeof(Brush), typeof(BasicAnalog), new PropertyMetadata(new SolidColorBrush(Colors.Gray), BarBackgroundColorPropertyChange));
        private static void BarBackgroundColorPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.BarBackgroundColor = (Brush)e.NewValue;
        }

        public static DependencyProperty ScaleColorProperty = DependencyProperty.Register("ScaleColor", typeof(Brush), typeof(BasicAnalog), new PropertyMetadata(new SolidColorBrush(Colors.White), ScaleColorPropertyChange));
        private static void ScaleColorPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.ScaleColor = (Brush)e.NewValue;
        }

        public static DependencyProperty MainStepCountProperty = DependencyProperty.Register("MainStepCount", typeof(Int32), typeof(BasicAnalog), new PropertyMetadata(6, MainStepCountPropertyChange));
        private static void MainStepCountPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.MainStepCount = (Int32)e.NewValue;
        }

        public static DependencyProperty MinorStepCountProperty = DependencyProperty.Register("MinorStepCount", typeof(Int32), typeof(BasicAnalog), new PropertyMetadata(6, MinorStepCountPropertyChange));
        private static void MinorStepCountPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.MinorStepCount = (Int32)e.NewValue;
        }

        public static DependencyProperty ValueRoundProperty = DependencyProperty.Register("ValueRound", typeof(Int32), typeof(BasicAnalog), new PropertyMetadata(0, ValueRoundPropertyChange));
        private static void ValueRoundPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.ValueRound = (Int32)e.NewValue;
        }

        public static DependencyProperty ValueFontSizeProperty = DependencyProperty.Register("ValueFontSize", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(40.0, ValueFontSizePropertyChange));
        private static void ValueFontSizePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.ValueFontSize = (Double)e.NewValue;
        }

        public static DependencyProperty DescriptionFontSizeProperty = DependencyProperty.Register("DescriptionFontSize", typeof(Double), typeof(BasicAnalog), new PropertyMetadata(12.0, DescriptionFontSizePropertyChange));
        private static void DescriptionFontSizePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicAnalog control = d as BasicAnalog;
            control.DescriptionFontSize = (Double)e.NewValue;
        }

        #endregion

        #region Property

        /// <summary>
        /// Start
        /// </summary>
        public Double Start
        {
            get
            {
                return (Double)GetValue(StartProperty);
            }
            set
            {
                SetValue(StartProperty, value);
                UpdateSize();
            }
        }

        /// <summary>
        /// End
        /// </summary>
        public Double End
        {
            get
            {
                return (Double)GetValue(EndProperty);
            }
            set
            {
                SetValue(EndProperty, value);
                UpdateSize();
            }
        }

        /// <summary>
        /// DescriptionFontSize
        /// </summary>
        public Double DescriptionFontSize
        {
            get
            {
                return (Double)GetValue(DescriptionFontSizeProperty);
            }
            set
            {
                SetValue(DescriptionFontSizeProperty, value);
                UpdateValueAndDescription();
            }
        }

        /// <summary>
        /// ValueFontSize
        /// </summary>
        public Double ValueFontSize
        {
            get
            {
                return (Double)GetValue(ValueFontSizeProperty);
            }
            set
            {
                SetValue(ValueFontSizeProperty, value);
                UpdateValueAndDescription();
            }
        }

        /// <summary>
        /// ValueRound
        /// </summary>
        public Int32 ValueRound
        {
            get
            {
                return (Int32)GetValue(ValueRoundProperty);
            }
            set
            {
                SetValue(ValueRoundProperty, value);
                UpdateValueAndDescription();
            }
        }

        /// <summary>
        /// MinorStepCount
        /// </summary>
        public Int32 MinorStepCount
        {
            get
            {
                return (Int32)GetValue(MinorStepCountProperty);
            }
            set
            {
                SetValue(MinorStepCountProperty, value);
                GenerateScale();
            }
        }

        /// <summary>
        /// MainStepCount
        /// </summary>
        public Int32 MainStepCount
        {
            get
            {
                return (Int32)GetValue(MainStepCountProperty);
            }
            set
            {
                SetValue(MainStepCountProperty, value);
                GenerateScale();
            }
        }

        /// <summary>
        /// Scale Color
        /// </summary>
        public Brush ScaleColor
        {
            get
            {
                return (Brush)GetValue(ScaleColorProperty);
            }
            set
            {
                SetValue(ScaleColorProperty, value);
                GenerateScale();
            }
        }

        /// <summary>
        /// Bar Background Color
        /// </summary>
        public Brush BarBackgroundColor
        {
            get
            {
                return (Brush)GetValue(BarBackgroundColorProperty);
            }
            set
            {
                SetValue(BarBackgroundColorProperty, value);
                GenerateScale();
            }
        }

        /// <summary>
        /// Bar Color
        /// </summary>
        public Brush BarColor
        {
            get
            {
                return (Brush)GetValue(BarColorProperty);
            }
            set
            {
                SetValue(BarColorProperty, value);
                GenerateScale();
            }
        }

        /// <summary>
        /// Value
        /// </summary>
        public Double Value
        {
            get
            {
                return (Double)GetValue(ValueProperty);
            }
            set
            {
                if (value < MinValue)
                {
                    SetValue(ValueProperty, MinValue);
                }
                else if (value > MaxValue)
                {
                    SetValue(ValueProperty, MaxValue);
                }
                else
                {
                    SetValue(ValueProperty, value);
                }
                UpdateValueAndDescription();
                ResolveValueAngle();
            }
        }

        /// <summary>
        /// MinValue
        /// </summary>
        public Double MinValue
        {
            get
            {
                return (Double)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
                ResolveValueAngle();
                GenerateScale();
            }
        }

        /// <summary>
        /// MaxValue
        /// </summary>
        public Double MaxValue
        {
            get
            {
                return (Double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
                ResolveValueAngle();
                GenerateScale();
            }
        }

        /// <summary>
        /// Unit
        /// </summary>
        public String Unit
        {
            get
            {
                return (String)GetValue(UnitProperty);
            }
            set
            {
                SetValue(UnitProperty, value);
                UpdateValueAndDescription();
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public String Description
        {
            get
            {
                return (String)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
                UpdateValueAndDescription();
            }
        }


        #endregion


        /// <summary>
        /// Basic analog control
        /// </summary>
        public BasicAnalog()
        {
            InitializeComponent();

            this.SizeChanged += triggerSizeChange;

            UpdateSize();
        }


        /// <summary>
        /// Size change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void triggerSizeChange(object sender, SizeChangedEventArgs e)
        {
            UpdateSize();
        }

        /// <summary>
        /// Resolve size
        /// </summary>
        private void UpdateSize()
        {
            ResolveValueAngle();
            GenerateScale();
            UpdateValueAndDescription();
        }

        /// <summary>
        /// Update value and description
        /// </summary>
        private void UpdateValueAndDescription()
        {
            ValueText.Text = Math.Round(this.Value, this.ValueRound).ToString();
            ValueText.FontSize = this.ValueFontSize;
            UnitText.Text = this.Unit;
            UnitText.FontSize = this.DescriptionFontSize;
            DescriptionText.Text = this.Description;
            DescriptionText.FontSize = this.DescriptionFontSize;
        }

        /// <summary>
        /// Resolve value
        /// </summary>
        private void ResolveValueAngle()
        {
            var start = this.Start;
            var end = this.End;
            var angleRange = end - start;

            var scaleRange = MaxValue - MinValue;
            var percent = (Value - MinValue) / scaleRange;

            MeterBackground.StartAngle = start;
            MeterBackground.EndAngle = end;
            Scale.StartAngle = start;
            Scale.EndAngle = end;

            Meter.StartAngle = start;
            Meter.EndAngle = start + (angleRange * percent);
        }

        /// <summary>
        /// Scale generator
        /// </summary>
        private void GenerateScale()
        {
            Values.Children.Clear();
            
            MeterBackground.Fill = this.BarBackgroundColor;
            Scale.Fill = this.ScaleColor;
            Meter.Fill = this.BarColor;

            var range = this.End - this.Start;
            var step = range / MainStepCount;

            for (var i = 0; i <= MainStepCount; i++)
            {
                //big range
                Arc arc = new Arc();
                arc.Margin = new Thickness(6);
                arc.ArcThickness = 20;
                arc.ArcThicknessUnit = Microsoft.Expression.Media.UnitType.Pixel;
                arc.Fill = this.ScaleColor;
                arc.Stretch = Stretch.None;

                if (i == MainStepCount)
                {
                    arc.EndAngle = this.Start + (step * i);
                    arc.StartAngle = arc.EndAngle - 2;
                }
                else
                {
                    arc.StartAngle = this.Start + (step * i);
                    arc.EndAngle = arc.StartAngle + 2;
                }

                Values.Children.Add(arc);

                if (i < MainStepCount)
                {
                    //small range
                    var stepSmall = step / MinorStepCount;
                    for (var j = 0; j < MinorStepCount; j++)
                    {
                        Arc arcSmall = new Arc();
                        arcSmall.Margin = new Thickness(16);
                        arcSmall.ArcThickness = 10;
                        arcSmall.ArcThicknessUnit = Microsoft.Expression.Media.UnitType.Pixel;
                        arcSmall.Fill = this.ScaleColor;
                        arcSmall.Stretch = Stretch.None;

                        arcSmall.StartAngle = arc.StartAngle + (stepSmall * j);
                        arcSmall.EndAngle = arcSmall.StartAngle + 1;

                        Values.Children.Add(arcSmall);
                    }
                }
            }

        }
    }
}
