using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.CustomExceptions;

namespace WindowsFormsApp1
{
    public class HelperClass
    {
        public List<string> validCommands { get; set; }
        public HelperClass()
        {
            validCommands = new List<string>
            {
                "moveTo",
                "drawTo",
                "rectangle",
                "circle",
                "clear",
                "triangle",
                "green",
                "reset",
                "red",
                "blue",
                "fill"
            };
        }


        public void ValidateSyntax (string command)
        {
            string[] input = command.Split(' ');
            if (input.Length == 0 || !validCommands.Contains(input[0].ToLower()))
            {
                throw new InvalidCommandException("Please enter a valid command");
            }
            #region commented
            //if (!validCommands.Contains(input[0]))
            //{
            //    throw new ArgumentException("Invalid command. Please enter a valid command.");
            //}
            #endregion

            if (input[0] == "drawTo" || input[0] == "moveTo")
            {
                if (input.Length != 3 || !int.TryParse(input[1], out _) || !int.TryParse(input[2], out _))
                {
                    throw new InvalidSyntaxException("Invalid syntax, recheck the command");
                }
            }
        }
    }
}
