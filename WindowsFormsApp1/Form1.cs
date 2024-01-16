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
        //private Graphics graphics;
        //private bool errorDisplayed;

        // Constructor
        public Form1()
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
        }

        private void LoadFromFile(string fileName)
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
                            fillShapes = fill;
                        }
                    }
                }

            }


        }
        private void SaveToFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                //save current state of program
                sw.WriteLine($"PenPosition {penPosition.X} {penPosition.Y}");
                sw.WriteLine($"PenColor {pen.Color.Name}");
                sw.WriteLine($"FillShapes {fillShapes}");
            }
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFromFile(openFileDialog.FileName);
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
                SaveToFile(saveFileDialog.FileName);
            }
        }



        public void ExecuteCommand(string command)
        {
            try
            {
                helperClass.ValidateSyntax(command);
                string[] input = command.Split(' ');
                //buttonClickAction?.Invoke();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string command = textBox1.Text;
                ExecuteCommand(command);
                button1.PerformClick();
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
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class SyntaxChecker
        {
            private HashSet<string> validCommands; //list of valid commands
            public SyntaxChecker()
            {
                validCommands = new HashSet<string>
            {
                "moveTo",
                "drawTo",
                "Rectangle",
                "Circle",
                "clear",
                "Triangle",
                "green",
                "reset",
                "red",
                "blue",
                "fill"
            };
            }
            public void CheckSyntax(string command)
            {
                string[] input = command.Split(' ');
                if (input.Length == 0)
                {
                    throw new ArgumentException("Empty command. Please enter a valid command.");
                }
                if (!validCommands.Contains(input[0]))
                {
                    throw new ArgumentException("Invalid command. Please enter a valid command.");
                }
                switch (input[0])
                {
                    case "drawTo":
                        if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
                        {
                            throw new ArgumentException("Invalid syntax for drawTo command.");
                        }
                        break;

                    case "moveTo":
                        if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
                        {
                            throw new ArgumentException("Invalid syntax for moveTo command.");
                        }
                        break;
                }
            }
        }


        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //A small square at the top left corder of the picturebox
            e.Graphics.FillRectangle(Brushes.Black, penPosition.X, penPosition.Y, 5, 5);
        }
        //private bool fillShapes = false;
        private Graphics graphics;

        private bool errorDisplayed = false;
        private EventHandler Form1_Load;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string command = textBox1.Text;
                syntaxChecker.CheckSyntax(command);
                if (pictureBox1.Image == null)
                {
                    pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                }

                graphics = Graphics.FromImage(pictureBox1.Image);

                string[] input = textBox1.Text.Split(' ');
                if (input[0] == "Circle" && input.Length == 2 && int.TryParse(input[1], out int radius))
                {
                    if (fillShapes)
                    {
                        graphics.FillEllipse(pen.Brush, 10, 10, radius * 2, radius * 2);
                    }
                    else
                    {
                        graphics.DrawEllipse(pen, 10, 10, radius * 2, radius * 2);
                    }
                }
                else if (input[0] == "Rectangle" && input.Length == 3 && int.TryParse(input[1], out int width) && int.TryParse(input[2], out int height))
                {
                    if (fillShapes)
                    {
                        graphics.FillRectangle(pen.Brush, 10, 10, width, height);

                    }
                    else
                    {
                        graphics.DrawRectangle(pen, 10, 10, width, height);
                    }
                }

                else if (input[0] == "Triangle" && input.Length == 3 && int.TryParse(input[1], out int side1) && int.TryParse(input[2], out int side2))
                {
                    if (fillShapes)
                    {
                        Point point1 = new Point(10, 10 + side2);
                        Point point2 = new Point(10 + side1, 10 + side2);
                        Point point3 = new Point(10 + side1 / 2, 10);

                        Point[] trianglePoints = { point1, point2, point3 };

                        graphics.FillPolygon(pen.Brush, trianglePoints);
                    }
                    else
                    {
                        Point point1 = new Point(10, 10 + side2);
                        Point point2 = new Point(10 + side1, 10 + side2);
                        Point point3 = new Point(10 + side1 / 2, 10);

                        Point[] trianglePoints = { point1, point2, point3 };

                        graphics.DrawPolygon(pen, trianglePoints);
                    }
                }
                else if (input[0] == "clear")
                {
                    //Clear the graphics area
                    graphics.Clear(Color.Transparent);
                }
                else if (input[0] == "reset")
                {
                    penPosition = new Point(10, 10);
                }
                else if (input[0].StartsWith("moveTo") && input.Length == 3 && int.TryParse(input[1], out int x) && int.TryParse(input[2], out int y))
                {
                    penPosition = new Point(x, y);
                }
                else if (input[0].StartsWith("drawTo") && input.Length == 3 && int.TryParse(input[1], out int x2) && int.TryParse(input[2], out int y2))
                {
                    Point endPoint = new Point(x2, y2);
                    graphics.DrawLine(pen, penPosition, endPoint);
                    penPosition = endPoint; // update the pen position
                }
                else if (input[0] == "red")
                {
                    pen.Color = Color.Red;
                }
                else if (input[0] == "green")
                {
                    pen.Color = Color.Green;
                }
                else if (input[0] == "blue")
                {
                    pen.Color = Color.Blue;
                }
                else if (input[0] == "fill" && input.Length == 2)
                {
                    if (input[1] == "on")
                    {
                        fillShapes = true;
                    }
                    else if (input[1] == "off")
                    {
                        fillShapes = false;
                    }
                }
                pictureBox1.Refresh();
                errorDisplayed = false;
            }

            catch (ArgumentException ex)
            {
                if (!errorDisplayed)
                {
                    MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorDisplayed = true;
                }
            }

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Syntax_Click_1(object sender, EventArgs e)
        {

        }
    }

    #region old code
    //public partial class Form1 : Form
    //{

    //    private bool circleDrawn = false;
    //    private int circleDiameter = 0; 
    //    private string lastMessageBoxContent;

    //    public Action buttonClickAction;
    //    private PaintEventHandler pictureBox1_Paint;
    //    private SyntaxChecker syntaxChecker; 
    //    private MenuStrip menuStrip1;
    //    private Pen pen; 
    //    private Pen blackPen; 
    //    private Bitmap drawingArea; 
    //    private Point penPosition;
    //    private ToolStripMenuItem loadToolStripMenuItem;
    //    private ToolStripMenuItem saveToolStripMenuItem;
    //    private bool fillShapes;



    //    private void LoadFromFile(string fileName)
    //    {
    //        if (File.Exists(fileName))
    //        {
    //            using (StreamReader sr = new StreamReader(fileName))
    //            {
    //                string line;
    //                while ((line = sr.ReadLine()) != null)
    //                {
    //                    string[] parts = line.Split(' ');
    //                    if (parts[0] == "penPosition" && parts.Length == 3 &&
    //                        int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
    //                    {
    //                        penPosition = new Point(x, y);
    //                    }
    //                    else if (parts[0] == "penColor" && parts.Length == 2)
    //                    {
    //                        pen.Color = Color.FromName(parts[1]);
    //                    }
    //                    else if (parts[0] == "FillShapes" && parts.Length == 2 && bool.TryParse(parts[1], out bool fill))
    //                    {
    //                        fillShapes = fill;
    //                    }
    //                }
    //            }

    //        }


    //    }
    //    /// <summary>
    //    /// Saves drawing related to file
    //    /// </summary>
    //    /// <param name="fileName"> The file name were the settings would be saved</param>
    //    private void SaveToFile(string fileName)
    //    {
    //        using (StreamWriter sw = new StreamWriter(fileName))
    //        {
    //            //save current state of program
    //            sw.WriteLine($"PenPosition {penPosition.X} {penPosition.Y}");
    //            sw.WriteLine($"PenColor {pen.Color.Name}");
    //            sw.WriteLine($"FillShapes {fillShapes}");
    //        }
    //    }

    //    /// <summary>
    //    /// Handles the click for load, which allows the loading of drawing settings from a file
    //    /// </summary>
    //    /// <param name="sender">The event tthat triggered the event</param>
    //    /// <param name="e"> an EventArgs object which contains event data</param>
    //    private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        //create and configure an open dialog file
    //        OpenFileDialog openFileDialog = new OpenFileDialog();
    //        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
    //        openFileDialog.RestoreDirectory = true;


    //        //performs check to confirm if a file is selected by a user
    //        if (openFileDialog.ShowDialog() == DialogResult.OK)
    //        {
    //            LoadFromFile(openFileDialog.FileName);
    //            // Refresh the PictureBox to reflect the loaded setiings
    //            pictureBox1.Refresh();
    //        }
    //    }

    //    /// <summary>
    //    /// Handles the click for load, which allows the saving of drawing settings from a file
    //    /// </summary>
    //    /// <param name="sender">The event tthat triggered the event</param>
    //    /// <param name="e">an EventArgs object which contains event data</param>
    //    private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        SaveFileDialog saveFileDialog = new SaveFileDialog();
    //        saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
    //        saveFileDialog.RestoreDirectory = true;

    //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
    //        {
    //            SaveToFile(saveFileDialog.FileName);
    //        }
    //    }


    //    public void ExecuteCommand(string command)
    //    {
    //        try
    //        {
    //            syntaxChecker.CheckSyntax(command); //check the syntax of the command
    //            string[] input = command.Split(' ');
    //            buttonClickAction?.Invoke();
    //            // If the command is executed successfully, reset the error message
    //            ShowMessageBox(""); // Empty string or null to reset the error message

    //            switch (input[0])
    //            {
    //                case "moveTo":
    //                    if (input[0].StartsWith("moveTo") && input.Length == 3 && int.TryParse(input[1], out int x) && int.TryParse(input[2], out int y))
    //                    {
    //                        penPosition = new Point(x, y);
    //                    }
    //                    else
    //                    {
    //                        throw new ArgumentException("Invalid syntax for moveTo command.");
    //                    }
    //                    break;




    //                case "drawTo":
    //                    if (input.Length == 3 && int.TryParse(input[1], out int x2) && int.TryParse(input[2], out int y2))
    //                    {
    //                        DrawToPosition(x2, y2);
    //                    }
    //                    else
    //                    {
    //                        throw new ArgumentException("Invalid syntax for drawTo command.");
    //                    }
    //                    break;

    //                case "clear":
    //                    ClearGraphicsArea();
    //                    break;
    //                case "reset":
    //                    ResetPenPosition();
    //                    break;

    //                case "Circle":
    //                    if (input[0] == "Circle" && input.Length == 2 && int.TryParse(input[1], out int radius))
    //                    {
    //                        DrawCircle(radius);
    //                    }
    //                    else
    //                    {
    //                        throw new ArgumentException("Invalid syntax for Circle command.");
    //                    }
    //                    break;

    //                case "Rectangle":
    //                    if (input[0] == "Rectangle" && input.Length == 3 && int.TryParse(input[1], out int width) && int.TryParse(input[2], out int height))
    //                    {
    //                        DrawRectangle(width, height);
    //                    }
    //                    else
    //                    {
    //                        throw new ArgumentException("Invalid syntax for Rectangleo command.");
    //                    }
    //                    break;

    //                case "Triangle":
    //                    if (input[0] == "Triangle" && input.Length == 3 && int.TryParse(input[1], out int side1) && int.TryParse(input[2], out int side2))
    //                    {
    //                        DrawTriangle(side1, side2);
    //                    }
    //                    else
    //                    {
    //                        throw new ArgumentException("Invalid syntax for drawTo command.");
    //                    }
    //                    break;

    //                case "blue":
    //                    SetPenColor(Color.Blue);
    //                    break;

    //                case "green":
    //                    SetPenColor(Color.Green);
    //                    break;

    //                case "red":
    //                    SetPenColor(Color.Red);
    //                    break;
    //            }

    //        }

    //        catch (ArgumentException ex)
    //        {
    //            // If there's an exception, set the error message
    //            ShowMessageBox(ex.Message);
    //            //MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            //throw ex;
    //        }
    //    }

    //    /// <summary>
    //    /// Handles the keypress event for textbox1, focusing on the enter key
    //    /// </summary>
    //    /// <param name="sender"> The object that triggered the event  </param>
    //    /// <param name="e"></param>
    //    private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
    //    {
    //        if (e.KeyChar == (char)Keys.Enter)
    //        {
    //            string command = textBox1.Text;
    //            ExecuteCommand(command);
    //            button1.PerformClick();
    //            e.Handled = true;
    //        }
    //    }
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="Form1"/> class.
    //    /// </summary>
    //    /// <remarks>
    //    /// This Constructor sets up the initial configuration for the from1 instance, including initializing components, attaching event handlers.
    //    /// Setting up the drawing area, configuring the new strip and initialzing the syntax clearer
    //    /// </remarks>

    //    public Form1()
    //    {
    //        //initialize components using the designer generated code
    //        InitializeComponent();
    //        //Attach the Form1_Load eventhandler
    //        this.Load += Form1_Load;
    //        //Create a black pen for drawing
    //        blackPen = new Pen(Color.Black);
    //        //Attach the pictureBox1_Paint event handler to the paint event of the picture box
    //        pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
    //        //Attach the button1_Click event handler to the Click event of button1
    //        button1.Click += new System.EventHandler(this.button1_Click);
    //        //the initial position of the pen
    //        penPosition = new Point(10, 10);
    //        // the initial color of the pen
    //        pen = blackPen;
    //        //attach the textbox1nevent handler to the keypress event of textBox1
    //        textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
    //        //Initialize toomStripMenuItems for "Load" and "Save"
    //        loadToolStripMenuItem = new ToolStripMenuItem("Load");
    //        saveToolStripMenuItem = new ToolStripMenuItem("Save");
    //        //Attach the LoadTooStripMenuItem_Click event handler to the click event of LoadTooStripMenuItem
    //        loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
    //        //Attach the SaveTooStripMenuItem_Click event handler to the click event of SaveTooStripMenuItem
    //        saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
    //        //Create and configure the MenuStrip
    //        menuStrip1 = new MenuStrip();
    //        this.MainMenuStrip = menuStrip1;
    //        this.Controls.Add(menuStrip1);
    //        //Add tooStripMenu Items to the menuStrip
    //        menuStrip1.Items.Add(loadToolStripMenuItem);
    //        menuStrip1.Items.Add(saveToolStripMenuItem);
    //        //Add the pictureBox to the form! contorls
    //        this.Controls.Add(this.pictureBox1);
    //        //Initialize the drawing area as a Bitmap with the sixe of the PictureBox
    //        drawingArea = new Bitmap(pictureBox1.Width, pictureBox1.Height);
    //        //Creates a Graphics object for drawing on pictureBox
    //        graphics = pictureBox1.CreateGraphics();
    //        //Initialize the syntax checker
    //        syntaxChecker = new SyntaxChecker();
    //        //Attach the Syntax_Click event handler to the click event of the syntax button
    //        Syntax.Click += new System.EventHandler(this.Syntax_Click);
    //        //Attach the Button1_Click event handler to the click event of the Run button
    //        button1.Click += new System.EventHandler(this.button1_Click);
    //    }
    //    /// <summary>
    //    /// Handles the click event for the syntax button, checking the syntax command in TextBox1
    //    /// </summary>
    //    /// <param name="sender"> The object that Triggered the event</param>
    //    /// <param name="e"> an EventArgs object containing the event data</param>
    //    private void Syntax_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            string command = textBox1.Text;
    //            syntaxChecker.CheckSyntax(command);
    //        }
    //        catch (ArgumentException ex)
    //        {
    //            MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //    }
    //    /// <summary>
    //    /// Handles the valid commands in the textBox
    //    /// </summary>
    //    public class SyntaxChecker
    //    {
    //        private HashSet<string> validCommands; //list of valid commands
    //        public SyntaxChecker()
    //        {
    //            validCommands = new HashSet<string>
    //        {
    //            "moveTo",
    //            "drawTo",
    //            "Rectangle",
    //            "Circle",
    //            "clear",
    //            "Triangle",
    //            "green",
    //            "reset",
    //            "red",
    //            "blue",
    //            "fill"
    //        };
    //        }
    //        /// <summary>
    //        /// Handles the syntax of the provided drawing command 
    //        /// </summary>
    //        /// <param name="command"> the drawing coomand to be validated</param>
    //        /// <exception cref="ArgumentException">The Ereor message which will be thrown when an invalid command is entered </exception>
    //        public void CheckSyntax(string command)
    //        {
    //            string[] input = command.Split(' ');
    //            if (input.Length == 0)
    //            {
    //                throw new ArgumentException("Empty command. Please enter a valid command.");
    //            }
    //            if (!validCommands.Contains(input[0]))
    //            {
    //                throw new ArgumentException("Invalid command. Please enter a valid command.");
    //            }
    //            switch (input[0])
    //            {
    //                case "drawTo":
    //                    if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
    //                    {
    //                        throw new ArgumentException("Invalid syntax for drawTo command.");
    //                    }
    //                    break;

    //                case "moveTo":
    //                    if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
    //                    {
    //                        throw new ArgumentException("Invalid syntax for moveTo command.");
    //                    }
    //                    break;
    //            }
    //        }

    //    }
    //    private void PictureBox1_Paint(object sender, PaintEventArgs e)
    //    {
    //        //A small square at the top left corder of the picturebox
    //        e.Graphics.FillRectangle(Brushes.Black, penPosition.X, penPosition.Y, 5, 5);
    //    }
    //    /// <summary>
    //    /// The Graphics object used for the drawing on the PictureBox
    //    /// </summary>
    //    private Graphics graphics;
    //    /// <summary>
    //    /// inidicates if the error message has been thrown
    //    /// </summary>
    //    private bool errorDisplayed = false;
    //    /// <summary>
    //    /// Event handler for the form1 load event
    //    /// </summary>
    //    private EventHandler Form1_Load;



    //    /// <summary>
    //    /// Handles the Click event for the button1 control.
    //    /// Calls the <see cref="DoSave"/> method to process and save the drawing command.
    //    /// </summary>
    //    /// <param name="sender">The object that triggered the event.</param>
    //    /// <param name="e">An EventArgs object containing event data.</param>
    //    private void button1_Click(object sender, EventArgs e)
    //    {
    //        //Call the doSave method to process and save the drawing command
    //        DoSave();
    //    }
    //    public void DoSave()
    //    {
    //        /// <summary>
    //        /// Processes and saves the drawing command entered in the textBox1 control.
    //        /// </summary>
    //        /// <remarks>
    //        /// This method extracts the drawing command from textBox1, checks its syntax using <see cref="SyntaxChecker.CheckSyntax"/>,
    //        /// and executes the corresponding drawing operation. Supported commands include "Circle," "Rectangle," "Triangle," "clear," "reset,"
    //        /// "moveTo," "drawTo," "red," "green," "blue," and "fill."
    //        /// If an ArgumentException is caught during processing, it is handled by the <see cref="HandleArgumentException"/> method.
    //        /// </remarks>

    //        {
    //            try
    //            {
    //                //extract the command from textBox1
    //                string command = textBox1.Text;
    //                //Check the syntax of the command
    //                syntaxChecker.CheckSyntax(command);
    //                //Initialize the drawing area if it is null

    //                if (pictureBox1.Image == null)
    //                {
    //                    pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
    //                }

    //                graphics = Graphics.FromImage(pictureBox1.Image);

    //                string[] input = textBox1.Text.Split(' ');
    //                if (input[0] == "Circle" && input.Length == 2 && int.TryParse(input[1], out int radius))
    //                {
    //                    DrawCircle(radius);
    //                }
    //                else if (input[0] == "Rectangle" && input.Length == 3 && int.TryParse(input[1], out int width) && int.TryParse(input[2], out int height))
    //                {
    //                    DrawRectangle(width, height);
    //                }

    //                else if (input[0] == "Triangle" && input.Length == 3 && int.TryParse(input[1], out int side1) && int.TryParse(input[2], out int side2))
    //                {
    //                    DrawTriangle(side1, side2);
    //                }
    //                else if (input[0] == "clear")
    //                {
    //                    ClearGraphicsArea();
    //                }
    //                else if (input[0] == "reset")
    //                {
    //                    ResetPenPosition();
    //                }
    //                else if (input[0].StartsWith("moveTo") && input.Length == 3 && int.TryParse(input[1], out int x) && int.TryParse(input[2], out int y))
    //                {
    //                    MoveToPosition(x, y);
    //                }
    //                else if (input[0].StartsWith("drawTo") && input.Length == 3 && int.TryParse(input[1], out int x2) && int.TryParse(input[2], out int y2))
    //                {
    //                    DrawToPosition(x2, y2);
    //                }
    //                else if (input[0] == "red")
    //                {
    //                    SetPenColor(Color.Red);
    //                }
    //                else if (input[0] == "green")
    //                {
    //                    SetPenColor(Color.Green);
    //                }
    //                else if (input[0] == "blue")
    //                {
    //                    SetPenColor(Color.Blue);
    //                }
    //                else if (input[0] == "fill" && input.Length == 2)
    //                {
    //                    SetFillShapes(input[1]);
    //                }
    //                pictureBox1.Refresh();
    //                errorDisplayed = false;
    //            }

    //            catch (ArgumentException ex)
    //            {

    //                HandleArgumentException(ex);

    //            }


    //        }
    //    }
    //    /// <summary>
    //    /// Draws a Circle
    //    /// </summary>
    //    /// <param name="radius"> The Radius of Circle to be Drawn</param>
    //    public void DrawCircle(int radius)
    //    {
    //        try
    //        {
    //            if (fillShapes)
    //            {
    //                graphics.FillEllipse(pen.Brush, 10, 10, radius * 2, radius * 2);
    //            }
    //            else
    //            {
    //                graphics.DrawEllipse(pen, 10, 10, radius * 2, radius * 2);
    //            }
    //            circleDrawn = true;
    //            circleDiameter = radius * 2;
    //            MessageBox.Show("Circle drawn successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //        }

    //    }/// <summary>
    //     /// Checks if Circle is drawn
    //     /// </summary>
    //     /// <returns><c>true</c> if a circle has been drawn; otherwise, <c>false</c>.</returns>
    //    public bool IsCircleDrawn()
    //    {
    //        return circleDrawn;
    //    }
    //    /// <summary>
    //    /// Gets the diameter of the circle drawn
    //    /// </summary>
    //    /// <returns><c>true</c> if diameter is valid; otherwise, <c>false</c>.</returns>
    //    public int GetCircleDiameter()
    //    {
    //        return circleDiameter;
    //    }

    //    // Other methods and properties in your Form1 class





    //    /// <summary>
    //    /// Draws a Rectangle
    //    /// </summary>
    //    /// <param name="width">Width of the retangle drawn</param>
    //    /// <param name="height">Height of the retangle drawn</param>

    //    private void DrawRectangle(int width, int height)
    //    {
    //        if (fillShapes)
    //        {
    //            graphics.FillRectangle(pen.Brush, 10, 10, width, height);

    //        }
    //        else
    //        {
    //            graphics.DrawRectangle(pen, 10, 10, width, height);
    //        }
    //    }

    //    /// <summary>
    //    /// Draws a Triangle
    //    /// </summary>
    //    /// <param name="side1"> side 1 of the triangle drawn</param>
    //    /// <param name="side2">side 1 of the triangle drawn</param>
    //    private void DrawTriangle(int side1, int side2)
    //    {
    //        if (fillShapes)
    //        {
    //            Point point1 = new Point(10, 10 + side2);
    //            Point point2 = new Point(10 + side1, 10 + side2);
    //            Point point3 = new Point(10 + side1 / 2, 10);

    //            Point[] trianglePoints = { point1, point2, point3 };

    //            graphics.FillPolygon(pen.Brush, trianglePoints);
    //        }
    //        else
    //        {
    //            Point point1 = new Point(10, 10 + side2);
    //            Point point2 = new Point(10 + side1, 10 + side2);
    //            Point point3 = new Point(10 + side1 / 2, 10);

    //            Point[] trianglePoints = { point1, point2, point3 };

    //            graphics.DrawPolygon(pen, trianglePoints);
    //        }
    //    }

    //    /// <summary>
    //    /// Clears the graphic area
    //    /// </summary>
    //    private void ClearGraphicsArea()
    //    {
    //        //Clear the graphics area
    //        graphics.Clear(Color.Transparent);
    //    }

    //    /// <summary>
    //    /// Resets the initial penPosition
    //    /// </summary>
    //    private void ResetPenPosition()
    //    {
    //        penPosition = new Point(10, 10);
    //    }

    //    /// <summary>
    //    /// Move pen position
    //    /// </summary>
    //    /// <param name="x">move to point x</param>
    //    /// <param name="y">move to point y</param>
    //    private void MoveToPosition(int x, int y)
    //    {
    //        penPosition = new Point(x, y);
    //    }

    //    /// <summary>
    //    /// Draw to penPosition
    //    /// </summary>
    //    /// <param name="x2">Postion x2</param>
    //    /// <param name="y2"> position y2</param>
    //    private void DrawToPosition(int x2, int y2)
    //    {
    //        Point endPoint = new Point(x2, y2);
    //        graphics.DrawLine(pen, penPosition, endPoint);
    //        penPosition = endPoint; // update the pen position
    //    }

    //    /// <summary>
    //    /// sets pen color 
    //    /// </summary>
    //    /// <param name="color"> name of the color the pen will be set to</param>
    //    private void SetPenColor(Color color)
    //    {
    //        pen.Color = color;
    //    }

    //    /// <summary>
    //    /// Sets fill shape to on and off
    //    /// </summary>
    //    /// <param name="input"> input to set fill on and off</param>
    //    private void SetFillShapes(string input)
    //    {
    //        if (input == "on")
    //        {
    //            fillShapes = true;
    //        }
    //        else if (input == "off")
    //        {
    //            fillShapes = false;
    //        }
    //    }

    //    /// <summary>
    //    /// Handles an ArgumentException by displaying an error message in a MessageBox if it hasn't been displayed before.
    //    /// </summary>
    //    /// <param name="ex">The ArgumentException to handle.</param>
    //    private void HandleArgumentException(ArgumentException ex)
    //    {
    //        if (!errorDisplayed)
    //        {
    //            MessageBox.Show(ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            errorDisplayed = true;
    //        }
    //    }
    //    /// <summary>
    //    /// Handles the TextChanged event for the richTextBox1 control.
    //    /// </summary>

    //    private void richTextBox1_TextChanged(object sender, EventArgs e)
    //    {

    //    }
    //    /// <summary>
    //    /// Gets the content of last Message
    //    /// </summary>
    //    /// <returns> A string representing the content of the last displayed message</returns>
    //    public string GetLastMessageBoxContent()
    //    {
    //        return lastMessageBoxContent;
    //    }

    //    /// <summary>
    //    /// Shows the message box
    //    /// </summary>
    //    /// <param name="message"> Simulate showing a message box and store the content</param>
    //    public void ShowMessageBox(string message)
    //    {
    //        // Simulate showing a message box and store the content
    //        lastMessageBoxContent = message;
    //    }

    //    /// <summary>
    //    /// Gets the current penPoistion on the drawing surface
    //    /// </summary>
    //    /// <returns>The point representing the current positon</returns>
    //    public Point GetPenPosition()
    //    {
    //        return penPosition;
    //    }

    //    /// <summary>
    //    /// Gets the current penColor on the drawing surface
    //    /// </summary>
    //    /// <returns>the color of the pen</returns>
    //    public Color GetPenColor()
    //    {
    //        return pen.Color;
    //    }

    //    /// <summary>
    //    /// Retrieves the current fill shapes status.
    //    /// </summary>
    //    /// <returns>
    //    /// <c>true</c> if fill shapes are enabled; otherwise, <c>false</c>.
    //    public bool GetFillShapes()
    //    {
    //        return fillShapes;
    //    }



    //}
    #endregion 
}