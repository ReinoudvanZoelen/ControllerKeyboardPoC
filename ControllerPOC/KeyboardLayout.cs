using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerPOC
{
    public enum Direction { North, East, South, West }

    class KeyboardLayout
    {
        public static readonly char[] Ned_Freq = new Char[28] { 'e', 'n', 'a', 't', 'i', 'r', 'o', 'd', 's', 'l', 'g', 'v', 'h', 'k', 'm', 'u', 'b', 'p', 'w', 'j', 'z', 'c', 'f', 'x', 'y', 'q', '?', '!' };
        public static readonly char[] Eng_Freq = new Char[28] { 'e', 'a', 'r', 'i', 'o', 't', 'n', 's', 'l', 'c', 'u', 'd', 'p', 'm', 'h', 'g', 'b', 'f', 'y', 'w', 'k', 'v', 'x', 'z', 'j', 'q', '?', '!' };

        private Char[] up = new Char[7];
        private Char[] right = new Char[7];
        private Char[] down = new Char[7];
        private Char[] left = new Char[7];

        private Char[][] fullKeyboard = new Char[4][];

        public KeyboardLayout(char[] layout)
        {
            // Up
            this.up[0] = layout[27];
            this.up[1] = layout[19];
            this.up[2] = layout[11];

            this.up[3] = layout[0];

            this.up[4] = layout[4];
            this.up[5] = layout[12];
            this.up[6] = layout[20];

            // Right
            this.right[0] = layout[21];
            this.right[1] = layout[13];
            this.right[2] = layout[5];

            this.right[3] = layout[1];

            this.right[4] = layout[6];
            this.right[5] = layout[14];
            this.right[6] = layout[22];

            // Down
            this.down[0] = layout[23];
            this.down[1] = layout[15];
            this.down[2] = layout[7];

            this.down[3] = layout[2];

            this.down[4] = layout[8];
            this.down[5] = layout[16];
            this.down[6] = layout[24];

            // Left
            this.left[0] = layout[25];
            this.left[1] = layout[17];
            this.left[2] = layout[9];

            this.left[3] = layout[3];

            this.left[4] = layout[10];
            this.left[5] = layout[18];
            this.left[6] = layout[26];

            // Map them to the fullKeyboard
            this.fullKeyboard[0] = up;
            this.fullKeyboard[1] = right;
            this.fullKeyboard[2] = down;
            this.fullKeyboard[3] = left;
        }

        public Char getKey(Direction direction, int modification)
        {
            if (modification < -3 || modification > 3)
            {
                throw new IndexOutOfRangeException("The modification when searching for a letter needs to be between -3 and +3");
            }

            return fullKeyboard[(int)direction][modification + 3];
        }
    }
}
