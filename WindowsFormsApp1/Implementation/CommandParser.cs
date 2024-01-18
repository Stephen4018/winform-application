using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            string convert = color.ToLower();
            if(Enum.TryParse(convert, true, out KnownColor knowncolor))
            {
                Pen.Color = Color.FromKnownColor(knowncolor);
            }
            else
            {
                throw new ArgumentException("Invalid color name");
            }
        }

        public void DrawToPosition(Graphics graphics, int x, int y)
        {
            Point point = new Point(x, y);
            graphics.DrawLine(pen, penPosition, point);
            penPosition = point;
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
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
                        if (parts[0] == "penPosition" && parts.Length == 3 &&
                            int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                        {
                            penPosition = new Point(x, y);
                        }
                        else if (parts[0] == "penColor" && parts.Length == 2)
                        {
                            pen.Color = Color.FromName(parts[1]);
                        }
                        else if (parts[0] == "FillShapes" && parts.Length == 2 && bool.TryParse(parts[1], out bool fill))
                        {
                            fillshapes = fill;
                        }
                    }
                }
            }
        }

        public void MoveToPosition(int x, int y)
        {
            penPosition = new Point(x, y); 
        }

        public void Reset()
        {
            penPosition = new Point(10, 10);
        }

        public void SaveCommandToFile(string fileName)
        {
            using(StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine($"PenPosition {penPosition.X} {penPosition.Y}");
                sw.WriteLine($"PenColor {pen.Color.Name}");
                sw.WriteLine($"Fillshapes {fillshapes}");
            }
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

        public void DrawRectangle(Graphics graphics, int width, int height)
        {
            if (fillshapes)
            {
                graphics.FillRectangle(new SolidBrush(pen.Color), 10, 10, width, height );
            }
            else
            {
                graphics.DrawRectangle(pen, 10, 10, width, height);
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
