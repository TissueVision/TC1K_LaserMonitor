using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace TC1K_LaserMonitor
{


    //
    public class laser_Discovery : Laser
    {

        int nObjectivesMax = 32; // 31 is the highest index; 0 or 1 is the lowest

        // constructor only
        public laser_Discovery()
        {
            baudRate = 19200;
            newLineChar = "\r\n";
            this.modelockConfigured = false;
            this.fixedShutterConfigured = true;
            this.minOKPowerConfigured = true;
            this.fixedWavelength = 1040;
            this.tunableWavelengthMin_nm = 660;
            this.tunableWavelengthMax_nm = 1320;
        }



        public override LaserReturnCode setUpComms()
        {
            string heartbeatString;
            if (enableWatchdog)
            {
                heartbeatString = "HB=1";
            }
            else
            {
                heartbeatString = "HB=0";
            }
            string[] allSetupCommands =
            {
                "ECHO=0", // turn off echo mode
                "Prompt=0", // turn off prompt mode
                "EOT=0", // turn off end-of-text character
                "FC", // clear inactive faults
                heartbeatString, // set heartbeat (watchdog)
                "HBR=" + watchdogTimeout_s.ToString() // turn on heartbeat (watchdog)
            };
            LaserReturnCode stepOK;
            for (int n = 0; n < allSetupCommands.Length; n++)
            {
                stepOK = sendCommand(allSetupCommands[n]);
                if (stepOK!=LaserReturnCode.OK)
                {
                    string setupErrorMsg = String.Format("Laser setup failed during command ", allSetupCommands[n]);
                    Rep.Post(setupErrorMsg, repLevel.error, null);
                    return (LaserReturnCode.MiscError);
                }
            }
            return (LaserReturnCode.OK);
        }



        public LaserReturnCode sendCommand(string command)
        {
            if (!commsOK && !initializing)
            {
                Rep.Post("Laser serial port is not OK! Can not send command.", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            if (fakeOut)
            {
                return (LaserReturnCode.OK);
            }
            string response;
            try
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Write(command + newLineChar);
                response = _serialPort.ReadLine();
                if (response=="ECHO=0")
                {
                    // do nothing -- this just handles the case in which echo was on; this occurs when it gets turned off
                }
                else if (response.Contains("Out of range"))
                {
                    Rep.Post("Laser reported a range error!", repLevel.error, response);
                    return (LaserReturnCode.CommError);
                }
                else if (response.Contains("Error, invalid command"))
                {
                    Rep.Post("Laser reported an invalid command!", repLevel.error, response);
                    return (LaserReturnCode.CommError);
                }
                else if (response != "")
                {
                    Rep.Post("Badly-formatted reply from laser!", repLevel.error, response);
                    return (LaserReturnCode.CommError);
                }
            }
            catch (Exception ex)
            {
                Rep.Post("Could not send command to laser!", repLevel.error, null);
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
            string response;
            try
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Write(query + newLineChar);
                response = _serialPort.ReadLine();
                if (response.Contains("Error, invalid command"))
                {
                    Rep.Post("Laser reported an invalid command!", repLevel.error, response);
                    return ("error");
                }
                else
                {
                    return (response);
                }
            }
            catch (Exception excep)
            {
                return ("error");
            }
        }



        // return value is OK if all responses are received OK
        // it's NOT about whether the responses indicate that the laser is ready
        // results are all stored in queried status variables
        public override LaserReturnCode queryStatus(bool calledByUpdateGUI)
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            if (fakeOut)
            {
                lastQueryOK = true;
                physicalKeyIsOn = true;
                pumpLaserIsOn = true;
                currentWavelength = 777;
                tunableShutterIsOpen = true;
                fixedShutterIsOpen = false;
                currentPower = 7.77;
                laserError = false;
                errorCode = "77777";
                checkLaserReady();
                return (LaserReturnCode.OK);
            }

            Lockout.LockoutReturn lockOK = lockout.requestLock(calledByUpdateGUI);
            if (lockOK != Lockout.LockoutReturn.OK)
            {
                return (lockout.lockoutToLaserCode(lockOK));
            }

            string queryResponse;
            bool allResponsesOK = true;
            string trimmedResponse = "";
            try
            {
                _serialPort.DiscardInBuffer();

                // check whether physical key is on
                queryResponse = sendQuery(false,"?K");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    physicalKeyIsOn = (queryResponse == "1");
                }

                // check whether laser (softkey) is on
                queryResponse = sendQuery(false,"?L");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpLaserIsOn = (queryResponse == "1");
                }

                // check current wavelength
                queryResponse = sendQuery(false,"?WV");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    currentWavelength = Convert.ToDouble(queryResponse);
                }

                // check shutters
                // first, tunable shutter
                queryResponse = sendQuery(false,"?SVAR");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    tunableShutterIsOpen = (queryResponse == "1");
                    if (tunableShutterNeededForLaserOK)
                    {
                        if (tunableShutterIsOpen)
                        {
                            nConsecutiveShutterErrors = 0;
                        }
                        else
                        {
                            nShutterErrors++;
                            nConsecutiveShutterErrors++;
                        }
                    }                
                }

                // next, fixed shutter
                queryResponse = sendQuery(false,"?SFIXED");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    fixedShutterIsOpen = (queryResponse == "1");
                    if (fixedShutterNeededForLaserOK)
                    {
                        if (fixedShutterIsOpen)
                        {
                            nConsecutiveShutterErrors = 0;
                        }
                        else
                        {
                            nShutterErrors++;
                            nConsecutiveShutterErrors++;
                        }
                    }
                }

                // read power
                if (fixedShutterIsOpen && !tunableShutterIsOpen)
                {
                    queryResponse = sendQuery(false,"?PFIXED");
                }
                else // if tunable shutter is open, OR both are open
                {
                    queryResponse = sendQuery(false,"?PVAR");
                }
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    currentPower = Convert.ToDouble(queryResponse) / 1000;
                }

                // read error code
                queryResponse = sendQuery(false,"?F"); // get all faults
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    if (queryResponse == "0")
                    {
                        laserError = false;
                    }
                    else
                    {
                        laserError = true;
                        errorCode = queryResponse;
                        queryResponse = sendQuery(false,"?FINFO"); // get text of active fault
                        allResponsesOK = allResponsesOK && (queryResponse != "error");
                        errorCode = errorCode + " / " + queryResponse; // append to error code
                    }
                }

            }
            catch (Exception ex)
            {
                allResponsesOK = false;
            }
            if (allResponsesOK)
            {
                nConsecutiveQueryErrors = 0;
            }
            else
            {
                nQueryErrors++;
                nConsecutiveQueryErrors++;
                string queryErrorMsg = String.Format("{0} consecutive laser query errors", nConsecutiveQueryErrors);
                if (nConsecutiveQueryErrors > optionSettings.maxConsecutiveQueryErrors)
                {
                    setNotReady();
                }

                Rep.Post(queryErrorMsg, repLevel.details, null);
                //gui.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                //{
                //    Rep.Post(queryErrorMsg, repLevel.details, null);
                //}));
            }
            lockout.releaseLock();
            if (allResponsesOK)
            {
                lastQueryOK = true;
                checkLaserReady();
                return (LaserReturnCode.OK);
            }
            else
            {
                lastQueryOK = false;
                return (LaserReturnCode.MiscError);
            }
        }



        // 
        public override LaserReturnCode setWavelength(Double wavelengthToSet)
        {
            if (!commsOK)
            {
                Rep.Post("Laser is not connected, can not set wavelength!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            if ((wavelengthToSet < tunableWavelengthMin_nm) || (wavelengthToSet > tunableWavelengthMax_nm))
            {
                string outOfRangeMsg = "Commanded wavelength is outside of valid range!";
                Rep.Post(outOfRangeMsg, repLevel.error, null);
                Rep.CancelAll(outOfRangeMsg);
                return (LaserReturnCode.InvalidWavelength);
            }
            string lambdaString = String.Format("Tuning laser to new wavelength {0} nm...", wavelengthToSet);
            Rep.Post(lambdaString, repLevel.details, null);
            string tuningCompleteMsg = String.Format("Laser wavelength set to {0}nm", wavelengthToSet);
            if (fakeOut)
            {
                Rep.Post(tuningCompleteMsg, repLevel.details, null);
                return (LaserReturnCode.OK);
            }

            wavelengthIsCurrentlyChanging = true;
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                wavelengthIsCurrentlyChanging = false;
                Rep.Post("Lock timeout while tuning laser!", repLevel.error, null);
                return (LaserReturnCode.CommTimeout);
            }
            gui.laserStabilityAndon.set("Laser is stabilizing", gui.andonSemiOKColor, gui.andonTextDarkColor);
            commandString = "WV=" + wavelengthToSet.ToString();
            LaserReturnCode commandOK = sendCommand(commandString);
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            bool tuningComplete = false;
            while (!tuningComplete)
            {
                System.Threading.Thread.Sleep(optionSettings.laserGUIUpdateInterval_ms);

                string tuningStatus = sendQuery(false,"?TS");
                tuningComplete = (tuningStatus=="0");

                if (stopWatch.ElapsedMilliseconds > setWavelengthTimeout_s * 1000)
                {
                    wavelengthIsCurrentlyChanging = false;
                    Rep.Post("Laser timed out while setting wavelength!", repLevel.error, null);
                    return (LaserReturnCode.MiscError);
                }
            }

            lockout.releaseLock();
            if (commandOK==LaserReturnCode.OK)
            {
                wavelengthIsCurrentlyChanging = false;
                waitForLaserReady(); // this lets power / modelock stabilize
                Rep.Post(tuningCompleteMsg, repLevel.details, null);
                return (commandOK);
            }
            else
            {
                wavelengthIsCurrentlyChanging = false;
                queryStatus(false);
                checkLaserReady();
                Rep.Post("Could not set wavelength!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
            }
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
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                Rep.Post("Lock timeout while setting shutter!", repLevel.error, null);
                return (LaserReturnCode.CommTimeout);
            }
            string shutterTypeString = "";
            string onOffString = "";
            if (shutterType == ShutterType.TunableWavelength)
            {
                shutterTypeString = "SVAR=";
                tunableShutterNeededForLaserOK = openOrClosed;
            }
            else if (shutterType == ShutterType.FixedWavelength)
            {
                shutterTypeString = "SFIXED=";
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
                    lockout.releaseLock();
                    Rep.Post("Pump laser is not on! Can not open shutter.", repLevel.error, null);
                    return LaserReturnCode.PumpNotOn;
                }
            }
            else
            {
                onOffString = "0";
            }
            commandString = shutterTypeString + onOffString;
            LaserReturnCode commandOK = sendCommand(commandString);
            System.Threading.Thread.Sleep(1000);
            lockout.releaseLock();
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



        // for the Discovery NX, turnPumpOnOff is actually the laser on / off soft key;
        // there is no control for the pump laser per se
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
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                Rep.Post("Lock timeout while setting pump!", repLevel.error, null);
                return (LaserReturnCode.CommTimeout);
            }

            if (onOrOff)
            {
                commandString = "LASER=1";
            }
            else
            {
                commandString = "LASER=0";
            }
            LaserReturnCode commandOK = sendCommand(commandString);
            lockout.releaseLock();
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
            LaserReturnCode stepOK;
            stepOK = sendCommand("HB=0");
            if (stepOK == LaserReturnCode.OK)
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
            Rep.Post("Discover laser does not have a shutdown command! Programming error.", repLevel.error, null);
            return (LaserReturnCode.MiscError);
        }



        // 
        public override LaserReturnCode setAlignMode(bool onOff, ShutterType shutter)
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                return (LaserReturnCode.MiscError);
            }
            string whichOutput = null;
            string whichMode = null;
            if (shutter==ShutterType.FixedWavelength)
            {
                whichOutput = "ALIGNFIXED";
            }
            else if (shutter == ShutterType.TunableWavelength)
            {
                whichOutput = "ALIGNVAR";
            }
            if (onOff)
            {
                whichMode = "1";
            }
            else
            {
                whichMode = "0";
            }
            commandString = whichOutput + "=" + whichMode;
            LaserReturnCode commandOK = sendCommand(commandString);
            lockout.releaseLock();
            return (commandOK);
        }



        // 
        public override LaserReturnCode selectObjective(string objectiveName)
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                return (LaserReturnCode.CommTimeout);
            }
            commandString = "GDDCURVEN=" + objectiveName;
            LaserReturnCode commandOK = sendCommand(commandString);
            lockout.releaseLock();
            return (commandOK); // default in case override does not work.
        }



        public override LaserReturnCode pullObjectiveList()
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                return (LaserReturnCode.CommTimeout);
            }
            objectiveList = new List<string>();
            string queryResponse = null;
            for (int iObjective = 0; iObjective < nObjectivesMax; iObjective++)
            {
                string queryString = "?CURVEN:" + iObjective.ToString();
                queryResponse = sendQuery(false, queryString);
                if (queryResponse == "error")
                {
                    return (LaserReturnCode.CommError);
                }
                else if (queryResponse == "BLANK")
                {
                    // do nothing
                }
                else
                {
                    objectiveList.Add(queryResponse);
                }
            }
            lockout.releaseLock();
            return (LaserReturnCode.OK); // default in case override does not work.
        }


    }
}
