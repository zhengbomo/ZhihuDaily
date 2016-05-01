using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Shagu.Controls
{
    public class UniversalWrapPanel : Panel
    {

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
          DependencyProperty.Register("Orientation",
          typeof(Orientation), typeof(UniversalWrapPanel), null);

        public UniversalWrapPanel()
        {
            // default orientation
            Orientation = Orientation.Vertical;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Point point = new Point(0, 0);

            if (Orientation == Orientation.Horizontal)
            {
                double largestHeight = 0.0;
                double maxWidth = 0;

                for (int c = 0; c < Children.Count; c++)
                {
                    UIElement child = Children[c];
                    child.Measure(availableSize);

                    if ((point.X + child.DesiredSize.Width) > availableSize.Width)
                    {
                        point.X = 0;
                        point.Y = point.Y + largestHeight;
                        largestHeight = 0.0;
                    }
                    point.X += child.DesiredSize.Width;

                    // Tallest item in the row
                    largestHeight = Math.Max(largestHeight, child.DesiredSize.Height);

                    // Furthest right edge
                    maxWidth = Math.Max(maxWidth, point.X);
                }
                return new Size(maxWidth, point.Y + largestHeight);
            }
            else
            {
                double largestWidth = 0.0;
                double maxHeight = 0;

                // Loop invariant:
                // at top of loop, point is top-left of where this child will try to go
                // point.Y should never be 0 at the top except when c = 0
                for (int c = 0; c < Children.Count; c++)
                {
                    UIElement child = Children[c];
                    child.Measure(availableSize);

                    if ((point.Y + child.DesiredSize.Height) > availableSize.Height)
                    {
                        point.Y = 0;
                        point.X = point.X + largestWidth;
                        largestWidth = 0.0;
                    }
                    point.Y += child.DesiredSize.Height;

                    // Widest item in the column
                    largestWidth = Math.Max(largestWidth, child.DesiredSize.Width);

                    // Furthest bottom edge
                    maxHeight = Math.Max(maxHeight, point.Y);
                }
                return new Size(point.X + largestWidth, maxHeight);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point point = new Point(0, 0);
            int i = 0;

            if (Orientation == Orientation.Horizontal)
            {
                double largestHeight = 0.0;

                foreach (UIElement child in Children)
                {
                    child.Arrange(new Rect(point, new Point(point.X +
                      child.DesiredSize.Width, point.Y + child.DesiredSize.Height)));

                    if (child.DesiredSize.Height > largestHeight)
                        largestHeight = child.DesiredSize.Height;

                    point.X = point.X + child.DesiredSize.Width;

                    if ((i + 1) < Children.Count)
                    {
                        if ((point.X + Children[i + 1].DesiredSize.Width) > finalSize.Width)
                        {
                            point.X = 0;
                            point.Y = point.Y + largestHeight;
                            largestHeight = 0.0;
                        }
                    }
                    i++;
                }
            }
            else
            {
                double largestWidth = 0.0;

                foreach (UIElement child in Children)
                {
                    child.Arrange(new Rect(point, new Point(point.X +
                      child.DesiredSize.Width, point.Y + child.DesiredSize.Height)));

                    if (child.DesiredSize.Width > largestWidth)
                        largestWidth = child.DesiredSize.Width;

                    point.Y = point.Y + child.DesiredSize.Height;

                    if ((i + 1) < Children.Count)
                    {
                        if ((point.Y + Children[i + 1].DesiredSize.Height) > finalSize.Height)
                        {
                            point.Y = 0;
                            point.X = point.X + largestWidth;
                            largestWidth = 0.0;
                        }
                    }

                    i++;
                }
            }

            return base.ArrangeOverride(finalSize);
        }
    }
}