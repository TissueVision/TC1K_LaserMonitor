using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace TC1K_LaserMonitor
{

    public enum repLevel
    { 
        logOnly = 1,
        error = 2,
        eng = 3,
        details = 4,
        stageMoves = 5,
        popUp = 6,
        narrative = 7,
        vivaceComms = 8, // for troubleshooting log
        gripperDetails = 9, // for troubleshooting log
        stageDetails = 10 // for troubleshooting log
    }

    // 
    public class Reporter
    {

        public System.Windows.Forms.ListBox[] messageCtrl = new System.Windows.Forms.ListBox[0];
        public Andon errorAndon = new Andon();
        public int nMessagesMax = 200;
        public int nMessagesToTrim = 100;
        public bool logEnabled;
        public string logPath;
        public bool cancelAction = false;
        public bool useCallerPrefix = false;
        public string errorPrefix = "ERROR! ";
        public string cancelAllMessage = "";


        // constructor only
        public Reporter()
        {
        }


        public void initialize()
        {
            ClearConsole();
            setErrorAndon(false);
        }


        // 
        public void Post(string message, repLevel level, string descrip)
        {
            // add prefix for error
            if (level == repLevel.error)
            {
                message = errorPrefix + message;
                setErrorAndon(true);
            }

            // add prefix for calling object
            if (useCallerPrefix)
            {
                string callerFullFileName = new System.Diagnostics.StackFrame(1, true).GetFileName();
                string callerName = System.IO.Path.GetFileNameWithoutExtension(callerFullFileName);
                message = callerName + ": " + message;
            }

            // this is only visible if you are debugging in VS
            Console.WriteLine(message);

            // log it
            List<repLevel> levelsToLog = new List<repLevel>(new repLevel[] { repLevel.details, repLevel.error, repLevel.logOnly, 
                repLevel.narrative, repLevel.popUp });

            if (levelsToLog.Contains(level))
            {
                LogText(message);
            }

            // handle according to report level
            if (level == repLevel.stageMoves)
            {
                return;
            }

            // for all message controls, display on status message, with message control as list box:
            List<repLevel> levelsToDisplay = new List<repLevel>(new repLevel[] { repLevel.narrative, repLevel.error, repLevel.popUp, repLevel.details });

            if (levelsToDisplay.Contains(level))
            {
                for (int ind = 0; ind < messageCtrl.Length; ind++)
                {
                    System.Windows.Forms.ListBox thisCtrl = messageCtrl[ind];
                    if (thisCtrl.Items.Count > nMessagesMax)
                    {
                        for (int ii = 0; ii < nMessagesToTrim; ii++)
                        {
                            thisCtrl.Items.RemoveAt(0);
                        }
                    }
                    try 
                    {
                        thisCtrl.Items.Add(message);
                        thisCtrl.SetSelected(thisCtrl.Items.Count - 1, true);
                        thisCtrl.SetSelected(thisCtrl.Items.Count - 1, false);
                    }
                    catch(Exception ex)
                    { 
                        // put a breakpoint here to catch a bug, which I think occurs when message is null
                    }
                }
            }

            if (level == repLevel.popUp)
            {
                System.Windows.Forms.MessageBox.Show(message);
                return;
            }

        }


        public void ClearConsole()
        {
            for (int ind = 0; ind < messageCtrl.Length; ind++)
            {
                System.Windows.Forms.ListBox thisCtrl = messageCtrl[ind];
                thisCtrl.Items.Clear();
            }
        }



        public void LogText(string textToLog)
        {
            if (!logEnabled)
            {
                return;
            }
            if (logPath == null)
            {
                Post("Log path is not set! Can not log.", repLevel.error, null);
                return;
            }

            try
            {
                string fullString = createNowString(true) + "  " + textToLog;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@logPath, true))
                {
                    file.WriteLine(fullString);
                    file.Close();
                    file.Dispose();
                }
            }
            catch
            {
                logEnabled = false; // without this, it will cycle infinitely
                Post("Logging text failed!", repLevel.error, null);
            }
        }


        public string createNowString(bool includeMillisecond)
        {
            DateTime now = DateTime.Now;
            string nowString = String.Format("{0}-{1:00}-{2:00}_{3:00}h{4:00}m{5:00}s", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            if (includeMillisecond)
            {
                nowString = nowString + String.Format("{0:000}ms", now.Millisecond);
            }
            return (nowString);
        }





        public void CancelAll(string errorMessage)
        {
            cancelAction = true;
            cancelAllMessage = errorMessage;
            setErrorAndon(true);

            // tell user
            string popupMsg = "Canceling action! " + cancelAllMessage;
            Post(popupMsg, repLevel.error, null);
            var result = System.Windows.Forms.MessageBox.Show(popupMsg);
        }



        public void ClearCancelAll()
        {
            if (cancelAction)
            {
                cancelAction = false;
                Post("Error is cleared.", repLevel.details, null);
            }
            cancelAllMessage = "";
            setErrorAndon(false);
        }


        // true makes it show an error
        public void setErrorAndon(bool errorOn)
        {
            if (errorOn)
            {
                errorAndon.set("Error", System.Drawing.Color.White, System.Drawing.Color.Red);
            }
            else
            {
                errorAndon.set("Error", System.Drawing.Color.White, System.Drawing.Color.WhiteSmoke);
            }
        }




    }
}
