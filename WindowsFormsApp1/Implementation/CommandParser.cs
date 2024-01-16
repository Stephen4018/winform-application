﻿using System;
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

        private Graphics graphics;
        private PictureBox pictureBox1;
        private Pen pen;
        public Pen Pen { get { return pen; } }

        public CommandParser()
        {
            pictureBox1 = new PictureBox();
            graphics = pictureBox1.CreateGraphics();
            pen = new Pen(Color.Black);
        }



        public void Clear()
        {
            graphics.Clear(Color.Transparent);
        }

        public void ColorApply(string color)
        {
            if(Enum.TryParse(color, true, out KnownColor knowncolor))
            {
                Pen.Color = Color.FromKnownColor(knowncolor);
            }
        }

        public void DrawCire(int radius)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void DrawToPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DrawTriangle(int side1, int side2)
        {
            throw new NotImplementedException();
        }

        public void FillApply(string toggle)
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
    }
}
