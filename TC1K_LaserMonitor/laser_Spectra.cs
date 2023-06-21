using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;


namespace TC1K_LaserMonitor
{


    //
    public class laser_Spectra : Laser
    {

        string spectra_identifier = "Spectra"; // verifies that it has connected OK
        int allLinesWaitTime_ms = 500; // when using 'sendQuery' with 'allLines' true, how long to wait before reading all lines?
        public bool wavelengthNowReportedWithNM = false; // whether a 'READ:WAVELENGTH?' command replies '800' or '800nm'.  Newer MaiTai (Spring 2022ish) has this

        // constructor only
        public laser_Spectra()
        {
            baudRate = 115200;
            newLineChar = "\n";
        }


        public override LaserReturnCode setUpComms()
        {
            string queryResponse;
            string trimmedResponse;
            // check identifier, make sure it's the Spectra laser
            queryResponse = sendQuery(false, "*IDN?");
            queryResponse = sendQuery(false, "*IDN?"); // for some reason, you need to do it twice in cases where it has disconnected
            if (queryResponse == "error")
            {
                Rep.Post("Bad reply from laser; initialization failed!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            trimmedResponse = queryResponse.Substring(0, spectra_identifier.Length);
            if (trimmedResponse != spectra_identifier)
            {
                Rep.Post("Laser did not identify itself; initialization failed!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            if (enableWatchdog)
            {
                commandString = "TIMER:WATCHDOG " + watchdogTimeout_s.ToString();
            }
            else
            {
                commandString = "TIMER:WATCHDOG 0";
            }
            LaserReturnCode commandOK = sendCommand(commandString);
            if (commandOK != LaserReturnCode.OK)
            {
                Rep.Post("Could not set watchdog; initialization failed!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }

            // MaiTai needs to have a warmup command sent
            Thread.Sleep(500); // added for TC1K laser monitor
            if (typeof(laser_Spectra).IsInstanceOfType(this))
            {
                queryResponse = sendQuery(false,"READ:PCTWARMEDUP?"); // see how warmed up it is
                trimmedResponse = queryResponse.Replace("%", "0");
                warmupFraction = Convert.ToDouble(trimmedResponse); // added for TC1K laser monitor
                if (queryResponse == "error")
                {
                    Rep.Post("Error reading warmup fraction! Initialization failed!", repLevel.error, null);
                    return (LaserReturnCode.MiscError);
                }
                else if (Convert.ToDouble(trimmedResponse) == 0) // it has to be zero
                {
                    commandOK = sendCommand("ON");
                    return (commandOK);
                }
                // otherwise, it's already warming!
            }
            //if it made it thru OK...
            return (LaserReturnCode.OK);
        }


        public LaserReturnCode sendCommand( string command)
        {
            if (fakeOut)
            {
                return (LaserReturnCode.OK);
            }
            if (!commsOK && !initializing)
            {
                Rep.Post("Laser serial port is not OK! Can not send command.", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            try
            {
                _serialPort.DiscardOutBuffer();
                _serialPort.Write(command + newLineChar);
            }
            catch
            {
                fatalError("Could not send command to laser!",null);
                return (LaserReturnCode.CommError);
            }
            return (LaserReturnCode.OK);
        }


        // string is 'error' if there is an error
        // otherwise, it's the reply from the query
        public string sendQuery(bool allLines, string query)
        {
            if (!commsOK && !initializing)
            {
                Rep.Post("Laser serial port is not OK! Can not send query.", repLevel.error, null);
                return ("error");
            }
            string response = "";
            try
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Write(query + newLineChar);
                if (allLines)
                {
                    Thread.Sleep(allLinesWaitTime_ms); // NOTE, this could introduce long delays! it's a synchronous waiting
                    int totalBytes = _serialPort.BytesToRead;
                    byte[] buffer = new byte[totalBytes];
                    _serialPort.Read(buffer, 0, totalBytes);
                    response = Encoding.UTF8.GetString(buffer, 0, totalBytes);
                }
                else // if just one line
                {
                    response = _serialPort.ReadLine();
                }
                if (response.Length == 0)
                {
                    // don't have an error message, cause it happens regularly and is OK
                    return ("error");
                }
            }
            catch(Exception excep)
            {
                // Don't post this, because it happens all the time for non-important reasons
                // Rep.Post("Exception while querying laser.", repLevel.error, excep.Message);
                return ("error");
            }
            return (response);
        }


        // return value is OK if all responses are received OK
        // it's NOT about whether the responses indicate that the laser is ready
        // results are all stored in queried status variables
        public override LaserReturnCode queryStatus(bool calledByUpdateGUI)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }


        // 
        public override LaserReturnCode setWavelength(Double wavelengthToSet)
        {
            if (!commsOK)
            {
                Rep.Post("Laser is not connected, can not set wavelength!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            if ( (wavelengthToSet < tunableWavelengthMin_nm) || (wavelengthToSet > tunableWavelengthMax_nm) )
            {
                string outOfRangeMsg = "Commanded wavelength is outside of valid range!";
                Rep.Post(outOfRangeMsg, repLevel.error, null);
                Rep.CancelAll(outOfRangeMsg);
                return (LaserReturnCode.InvalidWavelength);
            }
            string lambdaString = String.Format("Tuning laser to new wavelength {0} nm...", wavelengthToSet);
            Rep.Post(lambdaString, repLevel.details, null);
            string tuningCompleteMsg = String.Format("Laser wavelength set to {0} nm", wavelengthToSet);
            if (fakeOut)
            {
                Rep.Post(tuningCompleteMsg, repLevel.details, null);
                return (LaserReturnCode.OK);
            }

            wavelengthIsCurrentlyChanging = true;
            commandString = "WAV " + wavelengthToSet.ToString();
            LaserReturnCode commandOK = sendCommand(commandString);

            // I'm not sure why this is needed but I'm trying to keep it from hanging
            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(1000); 
            System.Windows.Forms.Application.DoEvents();

            // wait for confirmation 
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            //bool setWavelengthComplete = false;
            //while (!setWavelengthComplete)
            //{
            //    System.Threading.Thread.Sleep(optionSettings.laserGUIUpdateInterval_ms);

            //    // get the current wavelength
            //    lockTask = lockout.requestLock(false);
            //    if (lockTask != Lockout.LockoutReturn.OK)
            //    {
            //        continue;
            //    }
            //    string wavelengthNowResponse = sendQuery(false, "READ:WAVELENGTH?");
            //    lockout.releaseLock();

            //    // compare with target wavelength
            //    double wavelengthNow = 0;
            //    if (wavelengthNowResponse != "error")
            //    {
            //        if (wavelengthNowReportedWithNM)
            //        {
            //            wavelengthNowResponse = wavelengthNowResponse.Substring(0, wavelengthNowResponse.Length - 2); // clip off 'nm' from the end 
            //        }
            //        wavelengthNow = Convert.ToDouble(wavelengthNowResponse);
            //    }
            //    if (wavelengthNow == wavelengthToSet)
            //    {
            //        wavelengthIsCurrentlyChanging = false;
            //        waitForLaserReady(); // this lets power / modelock stabilize
            //        thisExit = LaserReturnCode.OK;
            //        setWavelengthComplete = true;
            //        Rep.Post(tuningCompleteMsg, repLevel.details, null);
            //    }
            //    else if (stopWatch.ElapsedMilliseconds > setWavelengthTimeout_s * 1000)
            //    {
            //        wavelengthIsCurrentlyChanging = false;
            //        thisExit = LaserReturnCode.MiscError;
            //        setWavelengthComplete = true;
            //        Rep.Post("Laser timed out while setting wavelength!", repLevel.details, null);
            //    }
            //}
            queryStatus(false);
            checkLaserReady();
            return (LaserReturnCode.OK);
        }


        // bool input is true for open, false for closed
        public override LaserReturnCode setShutter(bool openOrClosed, ShutterType shutterType)
        {
            if (!commsOK)
            {
                Rep.Post("Laser is not connected, can not set shutter!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            if (fakeOut)
            {
                Rep.Post("Shutter set.", repLevel.details, null);
                return (LaserReturnCode.OK);
            }
            string shutterTypeString = "";
            string onOffString = "";
            if (shutterType == ShutterType.TunableWavelength)
            {
                shutterTypeString = "SHUTTER";
                tunableShutterNeededForLaserOK = openOrClosed;
            }
            else if (shutterType == ShutterType.FixedWavelength)
            {
                shutterTypeString = "IRSHUTTER";
                fixedShutterNeededForLaserOK = openOrClosed;
            }
            else
            {
                Rep.Post("Invalid shutter type!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
            }
            if (openOrClosed)
            {
                onOffString = "1";
                if (!pumpLaserIsOn)
                {
                    Rep.Post("Pump laser is not on! Can not open shutter.", repLevel.error, null);
                    return LaserReturnCode.PumpNotOn;
                }
            }
            else
            {
                onOffString = "0";
            }
            commandString = shutterTypeString + " " + onOffString;
            LaserReturnCode commandOK = sendCommand(commandString);
            System.Threading.Thread.Sleep(1000);
            if (commandOK == LaserReturnCode.OK)
            {
                Rep.Post("Shutter is set!", repLevel.details, null);
                return (LaserReturnCode.OK);
            }
            else
            {
                Rep.Post("Could not send shutter command!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
            }
        }



        // bool input is true for on, false for off
        public override LaserReturnCode turnPumpOnOff(bool onOrOff)
        {
            if (!commsOK)
            {
                Rep.Post("Laser is not connected, can not set pump!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            if (fakeOut)
            {
                Rep.Post("Pump set.", repLevel.details, null);
                return (LaserReturnCode.OK);
            }

            string commandString;
            if (onOrOff)
            {
                if (warmupFraction != 100)
                {
                    Rep.Post("Pump laser can not be turned on until system is 100% warmed up!", repLevel.error, null);
                    return (LaserReturnCode.NotWarmedUp);
                }
                commandString = "ON";
            }
            else
            {
                commandString = "OFF";
            }
            LaserReturnCode commandOK = sendCommand(commandString);
            if (commandOK == LaserReturnCode.OK)
            {
                Rep.Post("Pump is set!", repLevel.details, null);
                pumpLaserIsOn = onOrOff;
                return (LaserReturnCode.OK);
            }
            else
            {
                Rep.Post("Could not send pump command!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
            }
        }



        public override LaserReturnCode readyCloseApp()
        {
            commandString = "TIMER:WATCHDOG 0";
            LaserReturnCode commandOK = sendCommand(commandString);
            if (commandOK == LaserReturnCode.OK)
            {
                Rep.Post("Laser watchdog is disconnected!", repLevel.details, null);
                return (LaserReturnCode.OK);
            }
            else
            {
                Rep.Post("Could not disable watchdog!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
            }
        }



        public override LaserReturnCode shutDown()
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }

        // 
        public override LaserReturnCode setAlignMode(bool onOff, ShutterType shutter)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        // this does not apply to MaiTai unless MaiTai is configured with DeepSee
        public override LaserReturnCode selectObjective(string objectiveName)
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            commandString = "OBJECTIVE:SELECT " + objectiveName;
            LaserReturnCode commandOK = sendCommand(commandString);
            if (commandOK != LaserReturnCode.OK)
            {
                Rep.Post("Error while selecting objective!", repLevel.error, null);
                return (commandOK);
            }
            return (LaserReturnCode.OK);
        }


        // this does not apply to MaiTai unless MaiTai is configured with DeepSee
        public override LaserReturnCode pullObjectiveList()
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            string queryResponse = sendQuery(true,"OBJECTIVE:LIST?");
            if (queryResponse == "error")
            {
                Rep.Post("Could not get objective list!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            else
            {
                // figure out how to interpret the string and make it the objective list
                objectiveList = new List<string>();
                string[] objectiveArray = queryResponse.Split('\n');
                for (int ii=0; ii<objectiveArray.Length; ii++)
                {
                    string thisObjective = objectiveArray[ii];
                    if (thisObjective=="**END**")
                    {
                        break;
                    }
                    objectiveList.Add(thisObjective);
                }
            }
            return (LaserReturnCode.OK); // default in case override does not work.
        }


        public override LaserReturnCode setGreenPower(bool onOff, double powerToSet)
        {
            return (LaserReturnCode.Placeholder);
        }


    }
}
