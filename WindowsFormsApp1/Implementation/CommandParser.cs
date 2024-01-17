using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Implementation
{
    public class CommandParser :  ICommandParser
    {
        private bool fillshapes;
        private Graphics _graphics;
        private Pen pen;

        public Pen Pen { get { return pen; } }
        private Point penPosition;

        // constructor
        public CommandParser()
        {
            //pictureBox1 = new PictureBox();
            pen = new Pen(Color.Black);
            fillshapes = false;
            penPosition = new Point(10, 10);
        }



        public void Clear(Graphics graphics)
        {
            graphics.Clear(Color.Transparent);
        }

        public void ColorApply(string color)
        {
            if(Enum.TryParse(color, true, out KnownColor knowncolor))
            {
                Pen.Color = Color.FromKnownColor(knowncolor);
            }
            else
            {
                throw new ArgumentException("Invalid color name");
            }
        }

        public void DrawToPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void FillApply(string toggle)
        {
            if(toggle.ToUpper() == "ON")
            {
                fillshapes = true;
            }
            else if(toggle.ToUpper() == "OFF")
            {
                fillshapes = false;

            }
        }

        public void LoadCommandFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void MoveToPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void SaveCommandToFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void DrawCire(Graphics graphics, Point center, int radius)
        {
            if(fillshapes)
            {
                graphics.FillEllipse(new SolidBrush(pen.Color), center.X, center.Y, 2 * radius, 2 * radius);
            }
            else
            {
                graphics.DrawEllipse(pen, center.X, center.Y, 2 * radius, 2 * radius);
            }
        }

        public void DrawRectangle(Graphics graphics, Rectangle rectangle)
        {
            if (fillshapes)
            {
                graphics.FillRectangle(new SolidBrush(pen.Color), 10, 10, rectangle.Width, rectangle.Height );
            }
            else
            {
                graphics.DrawRectangle(pen, 10, 10, rectangle.Width, rectangle.Height);
            }
        }

        public void DrawTriangle(Graphics graphics, Point point1, Point point2, Point point3)
        {
            if(fillshapes)
            {
                graphics.FillPolygon(new SolidBrush(pen.Color), new Point[] { point1, point2, point3 });
            }
            else
            {
                graphics.DrawPolygon(pen, new Point[] { point1, point2, point3 });
            }
        }

    
    }
}
