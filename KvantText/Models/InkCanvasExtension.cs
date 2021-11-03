using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace KvantText.Models
{
    public static class InkCanvasExtension
    {
        public static bool GetIsSelectionEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectionEnabled);
        }

        public static void SetIsSelectionEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectionEnabled, value);
        }

            public static readonly DependencyProperty IsSelectionEnabled =
            DependencyProperty.RegisterAttached("IsSelectionEnabled",
            typeof(bool), typeof(InkCanvasExtension),
            new UIPropertyMetadata(false, OnIsSelectionEnabled));


        private static void OnIsSelectionEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            InkCanvas ic = sender as InkCanvas;
            if (ic != null)
            {
                bool isEnabled = (bool)e.NewValue;
                if (isEnabled)
                {
                    ic.SelectionChanged += OnSelectionChanged;
                }
                else
                {
                    ic.SelectionChanged -= OnSelectionChanged;
                }
            }
        }

        private static void OnSelectionChanged(object sender, EventArgs e)
        {
            InkCanvas ic = sender as InkCanvas;
            StrokeCollection selectedStrokes = ic.GetSelectedStrokes();
            SetTheSelectedStrokes(ic, selectedStrokes);
        }

        public static StrokeCollection GetTheSelectedStrokes(DependencyObject obj)
        {
            return (StrokeCollection)obj.GetValue(TheSelectedStrokes);
        }

        public static void SetTheSelectedStrokes(DependencyObject obj, StrokeCollection value)
        {
            obj.SetValue(TheSelectedStrokes, value);
        }

        public static readonly DependencyProperty TheSelectedStrokes =
                DependencyProperty.RegisterAttached("TheSelectedStrokes",
                typeof(StrokeCollection), typeof(InkCanvasExtension),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }
}
