using System;
using System.IO;
using System.Text;

namespace TC1K_LaserMonitor
{

    // 
    public class Andon
    {

        public System.Windows.Forms.Button control;
        //public string andonReadyText = "";
        //public string andonErrorText = "";
        //public string andonNeutralText = "";
        //public System.Drawing.Color andonReadyTextColor = System.Drawing.Color.Black;
        //public System.Drawing.Color andonErrorTextColor = System.Drawing.Color.Gray;
        //public System.Drawing.Color andonNeutralTextColor = System.Drawing.Color.Gray;


        // constructor only
        public Andon()
        {
        }



        public void set(string andonText, System.Drawing.Color backgroundColor, System.Drawing.Color textColor)
        {
            if (control != null)
            {
                control.BackColor = backgroundColor;
                control.ForeColor = textColor;
                control.Text = andonText;
            }
        }


    }
}
