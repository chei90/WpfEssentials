using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfEssentials.Controls
{
    public class GroupboxScrollViewer : ScrollViewer
    {
        public GroupBox OuterBox
        {
            get { return (GroupBox)GetValue(OuterBoxProperty); }
            set { SetValue(OuterBoxProperty, value); }
        }

        public double HeightOffset
        {
            get { return (double)GetValue(HeightOffsetProperty); }
            set { SetValue(HeightOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeightOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightOffsetProperty =
            DependencyProperty.Register("HeightOffset", typeof(double), typeof(GroupboxScrollViewer), 
                new PropertyMetadata(0.0, OnHeightOffsetChanged));

        // Using a DependencyProperty as the backing store for OuterBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OuterBoxProperty =
            DependencyProperty.Register("OuterBox", typeof(GroupBox), typeof(GroupboxScrollViewer), 
                new PropertyMetadata(null, OnContainerChanged));

        private static void OnContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GroupboxScrollViewer sv)
            {
                if (e.NewValue is GroupBox gb)
                {
                    sv.OuterBox = gb;
                    gb.SizeChanged += sv.UpdateSizeChanged;
                }
            }
        }

        private static void OnHeightOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GroupboxScrollViewer sv)
            {
                sv.HeightOffset = (double)e.NewValue;
            }
        }

        private void UpdateSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newHeight = e.NewSize.Height - HeightOffset;
            newHeight = newHeight < 0 ? 0 : newHeight;

            Height = newHeight;
        }
    }
}
