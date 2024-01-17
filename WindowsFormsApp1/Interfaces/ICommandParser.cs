using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public interface ICommandParser
    {
        void LoadCommandFromFile(string fileName);
        void SaveCommandToFile(string fileName);

        void DrawCire(Graphics graphics, Point center, int radius);
        void DrawRectangle(Graphics graphics, int width, int height);
        void DrawTriangle(Graphics graphics, Point point1, Point point2, Point point3);
        void Clear(Graphics graphics);
        void Reset();
        void MoveToPosition(int x, int y);
        void DrawToPosition(int x, int y);
        void ColorApply(string color);

        void FillApply(string fill);
    }
}
