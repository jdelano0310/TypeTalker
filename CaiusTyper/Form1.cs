using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace CaiusTyper
{
    public partial class Form1 : Form
    {
        private SpeechSynthesizer TextToSpeech;
        private String SayWhat;
        private Random rnd;
        private int JustSounds = 0; // don't hold more than 4 sounds in clue
        private string[] numberWord = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        private string lastFourNumbers;

        private Utilities.globalKeyboardHook gkh = new Utilities.globalKeyboardHook();

        // GbelowC = 196, A = 220, Asharp = 233, B = 247, C = 262, Csharp = 277, D = 294, Dsharp = 311, E = 330, F = 349, Fsharp = 370,
        // G = 392, Gsharp = 415
        private Int16[] tones = new Int16[] { 196, 220, 233, 247, 262, 277, 294, 311, 330, 349, 370, 392, 415 };

        // WHOLE = 1600, HALF = WHOLE/2, QUARTER = HALF/2, EIGHTH = QUARTER/2, SIXTEENTH = EIGHTH/2
        private Int16[] duration = new short[] { 1600, 800, 400, 200, 100 };

        private void Speak()
        {
            Application.DoEvents();
            TextToSpeech.SpeakAsyncCancelAll();
            Application.DoEvents();
            TextToSpeech.SpeakAsync(SayWhat);
            Application.DoEvents();
            JustSounds -= 1;
        }


        private void DisplayRandomColor()
        {
            Application.DoEvents();
            rnd = new Random();
            this.BackColor = Color.FromArgb(255, rnd.Next(255), rnd.Next(255), rnd.Next(255));
            Application.DoEvents();
        }

        private void PlayRandomSound()
        {
            // plays a tone when a key is pressed that doesn't have an associated letter
            Int32 toneIdx = 0;
            Int32 durationIdx = 0;

            Application.DoEvents();
            rnd = new Random();
            toneIdx = rnd.Next(12);
            durationIdx = rnd.Next(4);

            Console.Beep(tones[toneIdx], duration[durationIdx]);

        }

        private void IgnoreTheseKeys()
        {

            // these keys will be ignored while the program is running
            gkh.HookedKeys.Add(Keys.LMenu); // alt key
            gkh.HookedKeys.Add(Keys.RMenu);

            gkh.HookedKeys.Add(Keys.LShiftKey); // shift key
            gkh.HookedKeys.Add(Keys.RShiftKey);

            gkh.HookedKeys.Add(Keys.LWin);  // windows key
            gkh.HookedKeys.Add(Keys.RWin);  

            gkh.HookedKeys.Add(Keys.Tab); // tab key
            
            gkh.HookedKeys.Add(Keys.Menu); // menu key

        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TextToSpeech = new SpeechSynthesizer
            {
                Rate = -2
            };

            IgnoreTheseKeys();

            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);

        }

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            lblDisplayPressedKey.Text = "";
            e.Handled = true;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            lblDisplayPressedKey.Text = "";
            e.Handled = true;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            Int32 keyCode = Convert.ToInt32(e.KeyChar);
            bool keyIsDisplayable = (keyCode > 47 && keyCode < 58) | (keyCode > 64 && keyCode < 91) | (keyCode > 96 && keyCode < 123);

            DisplayRandomColor();

            if (keyIsDisplayable)
            {
                SayWhat = e.KeyChar.ToString();

                if (keyCode > 47 && keyCode < 58)
                {
                    // added a pin to force close the program
                    lastFourNumbers += SayWhat;

                    // if a number is pressed, also spell the word out on screen
                    lblDisplayPressedKey.Text = SayWhat + "\n(" + numberWord[int.Parse(SayWhat)] + ")";
                }
                else
                {
                    lastFourNumbers = "";

                    // speak what was typed
                    lblDisplayPressedKey.Text = SayWhat;
                }
                Speak();

                if (lastFourNumbers == "7391")
                {
                    this.Close();  // force the window to close
                }
                else
                {
                    if (lastFourNumbers.Length == 4) { lastFourNumbers = ""; }
                }
            }
            else
            {
                if (JustSounds < 3)
                {
                    JustSounds += 1;
                    lblDisplayPressedKey.Text = "";
                    PlayRandomSound();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                TextToSpeech.Dispose();
                gkh.unhook();
            }
            catch {

            }

        }

        private void lblDisplayPressedKey_MouseDown(object sender, MouseEventArgs e)
        {
            lblDisplayPressedKey.Text = "";
            SayWhat = "mouse click";

            DisplayRandomColor();
            Speak();
        }

    }
}
