using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public interface ICommandParser
    {
        void LoadCommandFromFile(string fileName);
        void SaveCommandToFile(string fileName);

        void DrawCire(int radius);
        void DrawRectangle(int width, int height);
        void DrawTriangle(int side1, int side2);
        void Clear();
        void Reset();
        void MoveToPosition(int x, int y);
        void DrawToPosition(int x, int y);
        void ColorApply(string color);

        void FillApply(string toggle);
    }
}
