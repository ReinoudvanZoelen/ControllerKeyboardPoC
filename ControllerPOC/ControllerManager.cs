using System;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using Windows.Gaming.Input;

namespace ControllerPOC
{
    class ControllerManager
    {
        private Form1 form1;
        private Gamepad controller;

        // The margin is measured from the origin of the stick and defines when the program will start measuring
        // At 0.5 the first 50% of moment will be ignored, to prevent accidental keystrokes.
        double margin = 0.75;
        double deadzone = 0.5;

        public ControllerManager(Form1 form1)
        {
            this.form1 = form1;

            // Connect the GamepadAdded methods to our own
            Gamepad.GamepadAdded += Gamepad_Added;
            Gamepad.GamepadRemoved += Gamepad_Removed;

            // Runs until we have a controller
            while (controller == null)
            {
                if (Gamepad.Gamepads.Count > 0)
                {
                    // Runs only once but is needed to make sure we have a controller
                    controller = Gamepad.Gamepads.First();
                }
            }
        }


        public void ReadLeftStick()
        {
            if (controller.GetCurrentReading().LeftThumbstickY >= margin)
            {
                // Up
                form1.directions_Add(Direction.North);
            }
            if (controller.GetCurrentReading().LeftThumbstickY <= -margin)
            {
                // Down
                form1.directions_Add(Direction.South);
            }
            if (controller.GetCurrentReading().LeftThumbstickX <= -margin)
            {
                // Left
                form1.directions_Add(Direction.West);
            }
            if (controller.GetCurrentReading().LeftThumbstickX >= margin)
            {
                // Right
                form1.directions_Add(Direction.East);
            }
        }

        public bool isLeftCentered()
        {
            if (controller.GetCurrentReading().LeftThumbstickX >= -deadzone &&
                controller.GetCurrentReading().LeftThumbstickX <= deadzone &&
                controller.GetCurrentReading().LeftThumbstickY >= -deadzone &&
                controller.GetCurrentReading().LeftThumbstickY <= deadzone)
            {
                return true;
            }
            return false;
        }

        private void Gamepad_Added(object sender, Gamepad e)
        {
            Debug.WriteLine("Gamepad was connected");
            form1.BackColor = Color.Green;
        }

        private void Gamepad_Removed(object sender, Gamepad e)
        {
            Debug.WriteLine("Gamepad was removed");
            form1.BackColor = Color.Red;
        }
    }
}
