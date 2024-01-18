using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using static WindowsFormApp1.Form1;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;
using System.CodeDom;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using WindowsFormsApp1;
using WindowsFormsApp1.CustomExceptions;


namespace WindowsFormApp1
{

    public partial class Form1 : Form
    {
        public Action buttonClickAction;
        private PaintEventHandler pictureBox1_Paint;
        private HelperClass helperClass;
        private MenuStrip menuStrip1;
        private Pen pen; //declare the pen
        private Pen blackPen; // set pen color to black
        private Bitmap drawingArea; //bitmap to store the drawing
        private Point penPosition; //Variable to store the current position
        private ToolStripMenuItem LoadToolStripMenuItem;
        private ToolStripMenuItem SaveToolStripMenuItem;

        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private bool fillShapes;
        private Graphics graphics;

        private bool errorDisplayed = false;
        private EventHandler Form1_Load;
        private readonly ICommandParser commandParser;  // interface injected serves a layer of abstraction

        // Constructor
        public Form1(ICommandParser commandParser)
        {
            InitializeComponent();
            this.Load += Form1_Load;

            blackPen = new Pen(Color.Black);
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            button1.Click += new System.EventHandler(this.button1_Click);
            penPosition = new Point(10, 10); //the initial position of the pen
            pen = blackPen;// the initial color of the pen
            textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
            loadToolStripMenuItem = new ToolStripMenuItem("Load");
            saveToolStripMenuItem = new ToolStripMenuItem("Save");
            loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            menuStrip1 = new MenuStrip();
            this.MainMenuStrip = menuStrip1;
            this.Controls.Add(menuStrip1);
            menuStrip1.Items.Add(loadToolStripMenuItem);
            menuStrip1.Items.Add(saveToolStripMenuItem);
            this.Controls.Add(this.pictureBox1);
            //  graphics = Graphics.FromImage(drawingArea);
            drawingArea = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = pictureBox1.CreateGraphics();
            helperClass = new HelperClass();
            Syntax.Click += new System.EventHandler(this.Syntax_Click);
            button1.Click += new System.EventHandler(this.button1_Click);
            this.commandParser = commandParser;
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                commandParser.LoadCommandFromFile(openFileDialog.FileName);
                // Refresh the PictureBox
                pictureBox1.Refresh();
            }
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                commandParser.SaveCommandToFile(saveFileDialog.FileName);
            }
        }

        public void ExecuteCommand()
        {

            try
            {
                string command = textBox1.Text;
                helperClass.ValidateSyntax(command);
                if (pictureBox1.Image == null)
                {
                    pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                }

                graphics = Graphics.FromImage(pictureBox1.Image);

                string[] input = textBox1.Text.ToLower().Split(' ');

                // This clears the picture box before drawing any shape
                commandParser.Clear(graphics);

                if (input[0].ToLower() == "circle" && input.Length == 2 && int.TryParse(input[1], out int radius))
                {
                    
                    commandParser.DrawCire(graphics,penPosition, radius);
                }
                else if (input[0].ToLower() == "rectangle" && input.Length == 3 && int.TryParse(input[1], out int width) && int.TryParse(input[2], out int height))
                {
                   
                    commandParser.DrawRectangle(graphics, width, height);
                }

                else if (input[0].ToLower() == "triangle" && input.Length == 3 && int.TryParse(input[1], out int side1) && int.TryParse(input[2], out int side2))
                {
                    Point point1 = new Point(10, 10 + side2);
                    Point point2 = new Point(10 + side1, 10 + side2);
                    Point point3 = new Point(10 + side1 / 2, 10);

                    commandParser.DrawTriangle(graphics, point1, point2, point3);
                }
                else if (input[0] == "clear")
                {
                    //Clear the graphics area
                    commandParser.Clear(graphics);
                }
                else if (input[0] == "reset")
                {
                    commandParser.Reset();
                }
                else if (input[0].StartsWith("moveTo") && input.Length == 3 && int.TryParse(input[1], out int x) && int.TryParse(input[2], out int y))
                {
                    commandParser.MoveToPosition(x, y);
                }
                else if (input[0].StartsWith("drawTo") && input.Length == 3 && int.TryParse(input[1], out int x2) && int.TryParse(input[2], out int y2))
                {
                    commandParser.DrawToPosition(graphics, x2, y2);
                }
                else if (input[0].ToLower() == "red" || input[0] == "green" || input[0] == "blue")
                {
                    commandParser.ColorApply(input[0].ToLower());
                }
                else if (input[0] == "fill" && input.Length == 2)
                {
                    commandParser.ColorApply(input[1]);
                }
                pictureBox1.Refresh();
                errorDisplayed = false;
            }

            catch (Exception ex)
            {
                if (!errorDisplayed)
                {
                    MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorDisplayed = true;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string command = textBox1.Text;
                ExecuteCommand();
                //button1.PerformClick();
                e.Handled = true;
                
            }
        }

        private void Syntax_Click(object sender, EventArgs e)
        {
            try
            {
                string command = textBox1.Text;
                helperClass.ValidateSyntax(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //A small square at the top left corder of the picturebox
            e.Graphics.FillRectangle(Brushes.Black, penPosition.X, penPosition.Y, 5, 5);
        }
        //private bool fillShapes = false;
       

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteCommand();
            

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Syntax_Click_1(object sender, EventArgs e)
        {

        }
    }

   
}