using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Gaming.Input;

namespace ControllerPOC
{
    public partial class Form1 : Form
    {
        ControllerManager cm;

        // Timer required to power our infinite loop
        Timer t = new Timer();

        //traysh
        //Char queuedCharacter = '*';



        // Directions is a list of directions that we've hit since our last "return home"
        // This is required to measure if a user has been going clockwise or counterclockwise
        // and to measure if when going North to East we need N-3 or N+1
       public List<Direction> directions = new List<Direction>();

        /// <summary>
        /// Select yout language preferences here
        /// </summary>
        Char[] keyboardLayout = KeyboardLayout.Ned_Freq;


        // The layout we're using, usually Ned_Freq which is Dutch sorted by frequency
        KeyboardLayout kl;

        public Form1()
        {
            InitializeComponent();

            cm = new ControllerManager(this);

            kl = new KeyboardLayout(keyboardLayout);

            // Initiate our infinite loop waiting for input
            t.Tick += loop;
            t.Interval = 1;
            t.Start();

        }

        private void loop(object sender, EventArgs e)
        {
            ShowKeyboardOnUI();


            cm.ReadLeftStick();

            // DEBUG: Add all directions to a list in the UI
            ShowDirections();

            // If the stick is back home and we have some registered directions
            if (cm.isLeftCentered() && directions.Count > 0)
            {
                output();
            }
        }

        private void ShowDirections()
        {
            listbox_Directions.Items.Clear();

            foreach (Direction d in directions)
            {
                listbox_Directions.Items.Add(d);
            }
        }

        private void output()
        {
            // DEBUG: log all of the current directions
            Console.WriteLine("Left stick is back home, printing all received directions...");

            foreach (Direction d in directions)
            {
                Console.WriteLine(d.ToString());
            }

            Console.WriteLine("Done reading all " + directions.Count + " directions.");

            // Filter out any unneeded directions
            FilterDirections();

            // Check the list of directions and verify the modification needed
            int modification = CalculateModification();

            // Get the needed character from our KL
            Char character = kl.getKey(directions[0], modification);

            // Add the character to our output
            label_buttonpressed.Text += character;

            // Clear up the directions
            directions.Clear();
        }

        private int CalculateModification()
        {
            // If we only have 1 item there is no modification
            if (directions.Count == 1)
            {
                // DEBUG
                Console.WriteLine("Only one item in our list, modification set to 0");

                return 0;
            }

            // positive keeps track of out final number and if its positive or not
            bool positive = true;

            // Start from our first direction
            switch (directions[0])
            {
                // Check if the next direction is to its right
                // If its not the comparison will be false and positive will be too
                case Direction.North:
                    positive = (directions[1] == Direction.East);
                    break;
                case Direction.East:
                    positive = (directions[1] == Direction.South);
                    break;
                case Direction.South:
                    positive = (directions[1] == Direction.West);
                    break;
                case Direction.West:
                    positive = (directions[1] == Direction.North);
                    break;
                default: break;
            }

            // If [1] is clockwise from [0] then positive is true and we return a positive number
            // Otherwise return a negative number
            // modify to -1 because {North East} is count 2 but only modifies 1 (+1, Eastwards)
            if (positive)
            {
                // DEBUG
                Console.WriteLine("Final modification set to: " + (directions.Count - 1));

                return (directions.Count - 1);
            }
            else
            {
                // DEBUG
                Console.WriteLine("Final modification set to: " + -(directions.Count - 1));

                return -(directions.Count - 1);
            }
        }

        private void FilterDirections()
        {
            //DEBUG
            Console.WriteLine("Starting the filtering process...");

            // Save the first direction as the start
            // Example: North would be 0
            Direction firstDirection = directions[0];
            int lastFound_FirstDirection_Index = 0;

            for (int i = 0; i < directions.Count; i++)
            {
                if (directions[i] == firstDirection)
                {
                    lastFound_FirstDirection_Index = i;

                    //DEBUG
                    Console.WriteLine("New starting index: " + lastFound_FirstDirection_Index);
                }
            }

            // Go through every item in directions and try to find that same direction again
            // If it's found, we have to start from that point
            // Example: If someone enters {North East North West South} we need to start from the _second_ North since they made a mistake and backtraced
            foreach (Direction d in directions)
            {

            }

            // Create a new list for our filtered directions
            List<Direction> filteredDirections = new List<Direction>();

            // Start at the last found starting point
            // Example: we start at index 2 because that's where we saw North again for the last time
            // New list: {North West South}
            for (int i = lastFound_FirstDirection_Index; i < directions.Count; i++)
            {
                filteredDirections.Add(directions[i]);
            }

            // DEBUG
            Console.WriteLine("Started with " + directions.Count + " items, now shrunk to " + filteredDirections.Count + " items.");

            // Set the list of directions to our filtered list
            if (directions.Count > filteredDirections.Count)
            {
                directions = filteredDirections;

                // DEBUG
                Console.WriteLine("List of directions was overwritten by the filtered list.");
            } //DEBUG 
            else
            {
                Console.WriteLine("List of directions was NOT overwritten.");
            }
        }

        public void directions_Add(Direction direction)
        {
            if (directions.Count == 0)
            {
                directions.Add(direction);
            }

            if (directions[directions.Count - 1] == direction)
            {
                return;
            }

            // For every direction...
            for (int i = 0; i < directions.Count; i++)
            {
                // If our new direction already exists
                if (directions[i] == direction)
                {
                    // DEBUG
                    Console.WriteLine("Starting cleanup of the direction queue");
                    Console.WriteLine("Starting with " + directions.Count + " items.");

                    while (directions.Count > i)
                    {
                        directions.RemoveAt(i);
                    }

                    // DEBUG
                    // Added +1 because at the end the direction is added back so this makes more sense in the output
                    Console.WriteLine("Ended with " + (directions.Count + 1) + " items.");
                }
            }

            directions.Add(direction);

        }

        private void ShowKeyboardOnUI()
        {
            for(int i = 0; i < 28; i++)
            {
                ((Label) Controls.Find("label_" + i,false)[0]).Text = keyboardLayout[i].ToString();
            }
        }
    }
}
