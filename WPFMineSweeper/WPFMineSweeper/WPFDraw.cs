using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

class WPFDraw
{
    private Canvas canvas;
    public WPFDraw(Canvas canvas_)
    {
        canvas = canvas_;
    }

    public void DrawRectangle(double Top, double Left, double XSize, double YSize, Brush color)
    {
        Rectangle rect = getRectangle(XSize, YSize, color);

        rect.SetValue(Canvas.TopProperty, Top);
        rect.SetValue(Canvas.LeftProperty, Left);

        canvas.Children.Add(rect);
    }
    public void DrawEllipse(double Top, double Left, double XSize, double YSize, Brush color)
    {
        Ellipse elips = GetEllipse(XSize, YSize, color);

        elips.SetValue(Canvas.TopProperty, Top);
        elips.SetValue(Canvas.LeftProperty, Left);

        canvas.Children.Add(elips);
    }
    public void DrawLine(double x1, double y1, double x2, double y2, Brush color)
    {
        Line line = getLine(x1, y1, x2, y2,color);

        //line.SetValue(Canvas.TopProperty, y1);
        //line.SetValue(Canvas.LeftProperty, x1);

        canvas.Children.Add(line);
    }
    public void DrawGrid(int rows, int columns,Brush color)
    {
        double width = canvas.ActualWidth;
        double height = canvas.ActualHeight;
        double rowHeight = height / rows;
        double columnwidth = width / columns;

        for(int i = 0; i < rows+1; i++)
        {
            DrawLine(0, i * rowHeight, width, i * rowHeight, color);
        }
        for (int i = 0; i < columns + 1; i++)
        {
            DrawLine(i * columnwidth, 0, i * columnwidth, height, color);
        }
    }
    public void DrawText(double x1,double y1, double height, double width,string text,Color col)
    {
        TextBlock textBlock = new TextBlock();
        textBlock.Text = text;
        textBlock.FontSize = height/2;
        textBlock.Foreground = new SolidColorBrush(col);
        Canvas.SetLeft(textBlock, x1 + (width/3));
        Canvas.SetTop(textBlock, y1);
        canvas.Children.Add(textBlock);
    }

    public void Clear()
    {
        canvas.Children.Clear();
    }

    private Rectangle getRectangle(double XSize, double YSize,Brush color)
    {
        Rectangle returnRectangle = new Rectangle();

        returnRectangle.Fill = color;
        returnRectangle.Height = YSize;
        returnRectangle.Width = XSize;

        return returnRectangle;
    }
    private Ellipse GetEllipse(double height, double width, Brush color)
    {
        Ellipse returnEllipse = new Ellipse();

        returnEllipse.Fill = color;
        returnEllipse.Height = height;
        returnEllipse.Width = width;

        return returnEllipse;
    }
    private Line getLine(double x1, double y1, double x2, double y2,Brush color)
    {
        Line returnLine = new Line();

        returnLine.X1 = x1;
        returnLine.X2 = x2;
        returnLine.Y1 = y1;
        returnLine.Y2 = y2;
        returnLine.Stroke = color;
        returnLine.StrokeThickness = 2;

        return returnLine;
    }
}
