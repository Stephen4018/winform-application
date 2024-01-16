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

        // constructor
        public CommandParser()
        {
            //pictureBox1 = new PictureBox();
            pen = new Pen(Color.Black);
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

        public void FillApply(bool toggle)
        {
            throw new NotImplementedException();
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
            
        }

        public void DrawRectangle(Graphics graphics, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public void DrawTriangle(Graphics graphics, Point point1, Point point2, Point point3)
        {
            throw new NotImplementedException();
        }

    
    }
}
