using System;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace TypeTalker
{
    public partial class Form1 : Form
    {
        private SpeechSynthesizer TextToSpeech;
        private String SayWhat;
        private Random rnd;
        private int JustSounds = 0; // don't hold more than 4 sounds in clue
        
        private string[] numberWord = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        private string lastFourNumbers;

        // this allows keyboard control
        private Utilities.globalKeyboardHook gkh = new Utilities.globalKeyboardHook();

        // GbelowC = 196, A = 220, Asharp = 233, B = 247, C = 262, Csharp = 277, D = 294, Dsharp = 311, E = 330, F = 349, Fsharp = 370,
        // G = 392, Gsharp = 415
        private Int16[] tones = new Int16[] { 196, 220, 233, 247, 262, 277, 294, 311, 330, 349, 370, 392, 415 };

        // WHOLE = 1600, HALF = WHOLE/2, QUARTER = HALF/2, EIGHTH = QUARTER/2, SIXTEENTH = EIGHTH/2
        private Int16[] duration = new short[] { 1600, 800, 400, 200, 100 };

        private string exitPin = "7391";

        private void Speak()
        {
            try
            {
                Application.DoEvents();
                TextToSpeech.SpeakAsyncCancelAll();

                Application.DoEvents();
                TextToSpeech.SpeakAsync(SayWhat);

                Application.DoEvents();
                JustSounds -= 1;

            } catch
            {

            }
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

            //gkh.HookedKeys.Add(Keys.LShiftKey); // shift key
            //gkh.HookedKeys.Add(Keys.RShiftKey);

            gkh.HookedKeys.Add(Keys.LWin);  // windows key
            gkh.HookedKeys.Add(Keys.RWin);  

            gkh.HookedKeys.Add(Keys.Tab); // tab key
            
            gkh.HookedKeys.Add(Keys.Menu); // menu key

        }
        
        public Form1(string cmdArg)
        {
            if(cmdArg != null)
            {
                // in development keep the form small
                if(cmdArg=="DEBUG")
                {
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
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
            //                        a number key                    capital letter                    lowercase letters                
            bool keyIsDisplayable = (keyCode > 47 && keyCode < 58) | (keyCode > 64 && keyCode < 91) | (keyCode > 96 && keyCode < 123);

            lblDisplayPressedKey.Text = "";
            
            DisplayRandomColor();

            if (keyIsDisplayable)
            {
                SayWhat = e.KeyChar.ToString();

                if (keyCode > 47 && keyCode < 58)
                {
                    // added a pin to assist in closing the program
                    if (exitPin.Contains(SayWhat)) { lastFourNumbers += SayWhat; } else { lastFourNumbers = ""; }

                    // if a number is pressed, also spell the word out on screen
                    lblDisplayPressedKey.Text = SayWhat + "\n" + numberWord[int.Parse(SayWhat)];
                }
                else
                {
                    lastFourNumbers = ""; // reset this when a number isn't selected

                    // display what was typed
                    lblDisplayPressedKey.Text = SayWhat;
                }

                if (lastFourNumbers == exitPin)
                {

                    this.Close();  // force the window to close
                }
                else
                {
                    // if the numbers reach a length of 4 and it isn't the 
                    // code to close, then clear it
                    if (lastFourNumbers.Length >= 4) { lastFourNumbers = ""; }
                }
            }
            else
            {
                // check for non-letter and number characters to display
                keyIsDisplayable = OtherDisplayableKeys(keyCode);

                // if the character still can't be spoken and there
                // aren't 3 sounds waiting to be played, play a sound
                // for the non displayable keystroke
                if (JustSounds < 3 && !keyIsDisplayable)
                {
                    JustSounds += 1;
                    PlayRandomSound();
                }
            }

            // say way is in the SayWhat varaible
            if (keyIsDisplayable) {
                Speak(); 
            }
        }

        private bool OtherDisplayableKeys(int keyCode)
        {
            // other keyboard keys that are display and speakable that aren't
            // numbers or letters

            SayWhat = "";

            switch (keyCode)
            {
                case 34:
                    SayWhat = "Quote";
                    lblDisplayPressedKey.Text = "\"";
                    break;

                case 39:
                    SayWhat = "Apostrophe";
                    lblDisplayPressedKey.Text = "'";
                    break;

                case 44:
                    SayWhat = "Comma";
                    lblDisplayPressedKey.Text = ",";
                    break;

                case 46:
                    SayWhat = "Period";
                    lblDisplayPressedKey.Text = ".";
                    break;

                case 47:
                    SayWhat = "Backslash";
                    lblDisplayPressedKey.Text = "/";
                    break;

                case 58:
                    SayWhat = "Colon";
                    lblDisplayPressedKey.Text = ":";
                    break;

                case 59:
                    SayWhat = "Semicolon";
                    lblDisplayPressedKey.Text = ";";
                    break;

                case 63:
                    SayWhat = "Question Mark";
                    lblDisplayPressedKey.Text = "?";
                    break;

                case 92:
                    SayWhat = "Forwardslash";
                    lblDisplayPressedKey.Text = "\\";
                    break;

            }

            return SayWhat.Length > 0;
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
