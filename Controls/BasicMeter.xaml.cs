using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace OdbCommunicator.Controls
{
    public partial class BasicMeter : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperty

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(BasicMeter), new PropertyMetadata(0.0, ValuePropertyChange));
        private static void ValuePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.Value = (Double) e.NewValue;
        }
        
        public static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(Double), typeof(BasicMeter), new PropertyMetadata(0.0, MinValuePropertyChange));
        private static void MinValuePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.MinValue = (Double)e.NewValue;
        }
        
        public static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(Double), typeof(BasicMeter), new PropertyMetadata(100.0, MaxValuePropertyChange));
        private static void MaxValuePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.MaxValue = (Double)e.NewValue;
        }

        public static DependencyProperty BarColorProperty = DependencyProperty.Register("BarColor", typeof(Brush), typeof(BasicMeter), new PropertyMetadata(new SolidColorBrush(Colors.Red), BarColorPropertyChange));
        private static void BarColorPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.BarColor = (Brush)e.NewValue;
        }

        public static DependencyProperty ScaleColorProperty = DependencyProperty.Register("ScaleColor", typeof(Brush), typeof(BasicMeter), new PropertyMetadata(new SolidColorBrush(Colors.White), ScaleColorPropertyChange));
        private static void ScaleColorPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.ScaleColor = (Brush)e.NewValue;
        }

        public static DependencyProperty MainStepCountProperty = DependencyProperty.Register("MainStepCount", typeof(Int32), typeof(BasicMeter), new PropertyMetadata(6, MainStepCountPropertyChange));
        private static void MainStepCountPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.MainStepCount = (Int32)e.NewValue;
        }

        public static DependencyProperty MinorStepCountProperty = DependencyProperty.Register("MinorStepCount", typeof(Int32), typeof(BasicMeter), new PropertyMetadata(6, MinorStepCountPropertyChange));
        private static void MinorStepCountPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.MinorStepCount = (Int32)e.NewValue;
        }

        public static DependencyProperty ValueRoundProperty = DependencyProperty.Register("ValueRound", typeof(Int32), typeof(BasicMeter), new PropertyMetadata(0, ValueRoundPropertyChange));
        private static void ValueRoundPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.ValueRound = (Int32)e.NewValue;
        }

        public static DependencyProperty ValueFontSizeProperty = DependencyProperty.Register("ValueFontSize", typeof(Double), typeof(BasicMeter), new PropertyMetadata(20.0, ValueFontSizePropertyChange));
        private static void ValueFontSizePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.ValueFontSize = (Double)e.NewValue;
        }

        public static DependencyProperty ValueTresholdProperty = DependencyProperty.Register("ValueTreshold", typeof(Double), typeof(BasicMeter), new PropertyMetadata(15.0, ValueTresholdPropertyChange));
        private static void ValueTresholdPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicMeter control = d as BasicMeter;
            control.ValueTreshold = (Double)e.NewValue;
        }

        #endregion

        #region Property

        /// <summary>
        /// ValueTreshold
        /// </summary>
        public Double ValueTreshold
        {
            get
            {
                return (Double)GetValue(ValueTresholdProperty);
            }
            set
            {
                SetValue(ValueTresholdProperty, value);
                UpdateSize();
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
                UpdateSize();
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
                UpdateSize();
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
                UpdateSize();
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
                UpdateSize();
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
                GenerateMainScale();
                RaisePropertyChanged();
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
                Meter.Background = value;
                RaisePropertyChanged();
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
                ResolveHeight();
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
                UpdateSize();
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
                UpdateSize();
            }
        }

        #endregion

        /// <summary>
        /// Create bacis meter
        /// </summary>
        public BasicMeter()
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
            ResolveHeight();
            GenerateMainScale();
            GenerateValues();
        }

        /// <summary>
        /// Resolve height
        /// </summary>
        private void ResolveHeight()
        {
            var height = this.ActualHeight;
            var scale = MaxValue - MinValue;
            var percent = (Value - MinValue) / scale;
            Meter.Height = height * percent;
        }

        /// <summary>
        /// Generate values
        /// </summary>
        private void GenerateValues()
        {
            var scale = MaxValue - MinValue;
            var step = (this.ActualHeight / MainStepCount);
            ValueContainer.Children.Clear();
            for (var i = MainStepCount; i >= 0; i--)
            {
                TextBlock value = new TextBlock();
                value.TextAlignment = TextAlignment.Left;
                value.FontSize = ValueFontSize;
                value.Height = Math.Max(step, 0);
                value.Margin = new Thickness(0, -ValueTreshold, 0, ValueTreshold);

                Double textValue = MinValue + ((scale / MainStepCount) * i);
                if (i == MainStepCount)
                {
                    value.Margin = new Thickness(0, -ValueTreshold / 2, 0, ValueTreshold / 2);
                    value.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    textValue = MaxValue;
                } 
                else if (i == 1) 
                {
                    value.Margin = new Thickness(0, -ValueTreshold, 0, ValueTreshold / 2);
                    value.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                } 
                else if (i == 0)
                {
                    value.Margin = new Thickness(0, -ValueTreshold, 0, ValueTreshold / 2);
                    value.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    textValue = MinValue;
                }

                value.Text = Math.Round(textValue, ValueRound).ToString();
                ValueContainer.Children.Add(value);
            }
        }

        /// <summary>
        /// Generate scale
        /// </summary>
        private void GenerateMainScale()
        {
            var height = this.ActualHeight;
            ScaleContainer.Children.Clear();
            for (var i = 0; i < MainStepCount; i++) 
            {
                Double partHeight;
                Thickness partBorder;
                if (i == 0)
                {
                    partHeight = Math.Max((height / MainStepCount), 0);
                    partBorder = new Thickness(0, 2, 0, 2);
                }
                else
                {
                    partHeight = Math.Max((height / MainStepCount), 0);
                    partBorder = new Thickness(0, 0, 0, 2);
                }

                Border b = new Border();
                b.Width = 20;
                b.Height = partHeight;
                b.BorderThickness = partBorder;
                b.BorderBrush = ScaleColor;
                b.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Grid grid = new Grid();
                grid.Height = partHeight;
                b.Child = grid;

                GenerateSecondaryScale(grid);
                ScaleContainer.Children.Add(b);
            }
        }

        /// <summary>
        /// Generate secondary scale
        /// </summary>
        /// <param name="grid"></param>
        private void GenerateSecondaryScale(Grid grid)
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Orientation.Vertical;
            stck.Height = grid.Height;

            var height = grid.Height;
            for (var i = 0; i < MinorStepCount; i++)
            {
                Double partHeight;
                Thickness partBorder;
                if (i == MinorStepCount - 1)
                {
                    partHeight = Math.Max((height / MinorStepCount), 0);
                    partBorder = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    partHeight = Math.Max((height / MinorStepCount), 0);
                    partBorder = new Thickness(0, 0, 0, 1);
                }

                Border c = new Border();
                c.Width = 12;
                c.Height = partHeight;
                c.BorderThickness = partBorder;
                c.BorderBrush = ScaleColor;
                c.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                stck.Children.Add(c);
            }

            grid.Children.Add(stck);
        }

        #region PROPERTY CHANGE

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On property change
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            });
        }

        /// <summary>
        /// Raise proeprty change
        /// </summary>
        /// <param name="caller"></param>
        public void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(caller));
                }
            });
        }

        #endregion
    }
}
