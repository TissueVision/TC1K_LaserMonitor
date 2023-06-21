using System;
using System.Collections.Generic;
using System.IO.Ports;


namespace TC1K_LaserMonitor
{

    public enum LaserReturnCode
    {
        OK, CommError, MiscError,
        ComPortNotInitialized, Placeholder,
        CommTimeout, NotWarmedUp, PumpNotOn,
        InvalidWavelength, BumpedFromLock
    }


    public enum TaskEnd
    {
        OK = 1,
        Timeout = 2,
        GeneralError = 3,
        UserStop = 4,
        CommError = 5,
        NotInitialized = 6,
        OutOfRange = 7,
        Placeholder = 8,
        Cancel = 9

    }

    //
    public class Laser : Device
    {
        // objects and state
        public string comID;
        public GUI gui;
        public SerialPort _serialPort;
        public Lockout lockout;
        public Int32 baudRate;
        public bool initializing = false;
        public bool pumpLaserIsOn = false;
        public string errorCode = "";
        public bool laserError = false;
        public string commandString = "";
        public List<string> currentQueryErrorMessages;
        public optionSettings optionSettings;

        // queried status variables
        public bool lastQueryOK = false;
        public bool stable = false; // this means that it's ready, *except that it does not count shutter open
        public double currentPower = 0;
        public bool tunableShutterIsOpen = false;
        public bool fixedShutterIsOpen = false;
        public bool physicalKeyIsOn = false;
        public double currentWavelength = 0;
        public double fixedWavelength = 0;
        public string pumpCurrent = "";
        public string pumpTemperature = "";
        public string pumpHours = "";
        public string pumpCurrent2 = "";
        public string pumpTemperature2 = "";
        public List<string> objectiveList = new List<string> {""};
        public string greenPower = "";

        // configuration
        public bool fixedShutterConfigured = false;
        public bool modelockConfigured = false;
        public bool minOKPowerConfigured = false;
        public bool fixedShutterNeededForLaserOK = false;
        public bool tunableShutterNeededForLaserOK = true;
        public double tunableWavelengthMin_nm = 0;
        public double tunableWavelengthMax_nm = 0;
        public bool greenPowerConfigured = false;
        public bool manyScanMode = false;
        public bool wavelengthIsCurrentlyChanging = false;

        // intermittent errors
        public int nQueryErrors = 0;
        public int nConsecutiveQueryErrors = 0;
        public int nShutterErrors = 0;
        public int nConsecutiveShutterErrors = 0;
        public int nModelockErrors = 0;
        public int nConsecutiveModelockErrors = 0;

        // misc params
        public int serialTimeout_ms = 500;
        public int setWavelengthTimeout_s = 120;
        public int laserStabilizationTime_s = 3;
        public bool enableWatchdog = true;
        public int watchdogTimeout_s = 5; // if the laser does not not see serial-port activity for this time, it turns off for safety
        public string newLineChar = null; // the character(s) that indicate the end of a line read from the serial port
        public string serialNum = "SN not implemented";
        public double relativeHumidity = -1; // this is not implemented yet!
        public double baselineLaserPower = 0;

        // Spectra lasers
        public double warmupFraction = 0;
        public bool modelocked = false;

        public enum LaserType
        {
            MaiTai, InsightX3, DiscoveryNX
        }

        // constructor only
        public Laser()
        {
        }


        public override TaskEnd initialize()
        {
            initializing = true;
            commsOK = false;
            laserError = false;
            resetErrorCounts();
            lockout = new Lockout();
            lockout.name = "Laser";
            lockout.Rep = Rep;
            if (fakeOut)
            {
                commsOK = true;
                Rep.Post("Laser communication is initialized by FAKEOUT.", repLevel.details, null);
                return (TaskEnd.OK);
            }
            try
            {
                Rep.Post("Initializing communication with laser...", repLevel.details, null);
                // initialize serial port
                if (!(_serialPort == null))
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                        _serialPort.Dispose();
                    }
                }
                if (comID == null)
                {
                    commsOK = false;
                    setNotReady();
                    Rep.Post("COM port for laser is not assigned!", repLevel.error, null);
                    return (TaskEnd.CommError);
                }
                _serialPort = new SerialPort(comID, baudRate, Parity.None, 8, StopBits.One);
                _serialPort.NewLine = newLineChar;
                _serialPort.Open();
                _serialPort.ReadTimeout = serialTimeout_ms;
                _serialPort.WriteTimeout = serialTimeout_ms;
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
            }
            catch(Exception ex)
            {
                commsOK = false;
                setNotReady();
                Rep.Post("Could not open laser serial port!", repLevel.error, null);
                return (TaskEnd.CommError);
            }

            // send various setup commands here, depending on which type of laser
            var setUpCommsOK = this.setUpComms();
            if (setUpCommsOK != LaserReturnCode.OK)
            {
                commsOK = false;
                setNotReady();
                Rep.Post("Invalid reply from laser!", repLevel.error, null);
                return (TaskEnd.GeneralError);
            }
            commsOK = true;
            initializing = false;
            Rep.Post("Laser communication is initialized.", repLevel.details, null);
            lockout.releaseLock();
            LaserReturnCode laserTask = turnPumpOnOff(true);
            //if (laserTask != LaserReturnCode.OK)
            //{
            //    return (TaskEnd.GeneralError);
            //}
            return (TaskEnd.OK);
        }



        public void checkLaserReady()
        {
            currentQueryErrorMessages = new List<string>();
            bool readyNow = true;
            if (!commsOK)
            {
                readyNow = false;
                currentQueryErrorMessages.Add("Laser communication is not ready!");
            }
            if (!lastQueryOK)
            {
                readyNow = false;
                currentQueryErrorMessages.Add("Communication to laser failed!");
            }
            if (!pumpLaserIsOn)
            {
                readyNow = false;
                currentQueryErrorMessages.Add("Pump laser is not on!");
            }
            //if (!manyScanMode) // in multiple wavelength scan mode, it will set these as needed
            //{
            //    if (tunableShutterNeededForLaserOK && !tunableShutterIsOpen)
            //    {
            //        readyNow = false;
            //        currentQueryErrorMessages.Add("Tunable-wavelength shutter is not open!");
            //    }
            //    if (fixedShutterNeededForLaserOK && !fixedShutterIsOpen)
            //    {
            //        readyNow = false;
            //        currentQueryErrorMessages.Add("Fixed-wavelength shutter is not open!");
            //    }
            //    if (!tunableShutterIsOpen && !fixedShutterIsOpen)
            //    {
            //        readyNow = false;
            //        currentQueryErrorMessages.Add("No shutter is open!");
            //    }
            //}
            if (laserError)
            {
                readyNow = false;
                currentQueryErrorMessages.Add("Laser replied with error code " + errorCode);
            }
            if (minOKPowerConfigured)
            {
                if (currentPower < optionSettings.laserMinOKPower_W)
                {
                    readyNow = false;
                    string powerMsg = String.Format("Laser power is {0}W, below minimum power {1}W!", currentPower, optionSettings.laserMinOKPower_W);
                    currentQueryErrorMessages.Add(powerMsg);
                }
            }
            if (modelockConfigured)
            {
                if (!modelocked)
                {
                    readyNow = false;
                    currentQueryErrorMessages.Add("Laser is not modelocked!");
                }
            }
            if (wavelengthIsCurrentlyChanging)
            {
                readyNow = false;
                currentQueryErrorMessages.Add("Wavelength is currently changing!");
            }

            //// check stability
            //if (wavelengthIsCurrentlyChanging)
            //{
            //    stable = false;
            //}
            //else
            //{ 
            //    if (modelockConfigured)
            //    {
            //        stable = modelocked;
            //    }
            //    else
            //    {
            //        stable = (currentPower >= optionSettings.laserMinOKPower_W);
            //    }            
            //}
            stable = true; // it's always stable cause I'm taking out this feature

            // update state and indicator, if there has been a change
            if (ready && !readyNow)
            {
                ready = false;
            }
            if (!ready && readyNow)
            {
                setReady();
            }
        }



        public virtual LaserReturnCode setUpComms()
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }



        // intermittent errors do NOT cause the laser to go into a not-ready state, but the run may be cancelled because of them
        public bool checkTooManyIntermittentErrors()
        {
            bool tooManyErrors = false;
            string errorMsg;
            if (!ready)
            {
                errorMsg = "Laser is not ready!";
                Rep.Post(errorMsg, repLevel.error, null);
                tooManyErrors = true;
            }
            if (nQueryErrors > optionSettings.maxQueryErrors)
            {
                errorMsg = String.Format("{0} query errors! exceeds limit of {1}", nQueryErrors, optionSettings.maxQueryErrors);
                Rep.Post(errorMsg, repLevel.error, null);
                tooManyErrors = true;
            }
            if (nConsecutiveQueryErrors > optionSettings.maxConsecutiveQueryErrors)
            {
                errorMsg = String.Format("{0} consecutive query errors! exceeds limit of {1}", nConsecutiveQueryErrors, optionSettings.maxConsecutiveQueryErrors);
                Rep.Post(errorMsg, repLevel.error, null);
                tooManyErrors = true;
            }
            if (nShutterErrors > optionSettings.maxShutterErrors)
            {
                errorMsg = String.Format("{0} shutter errors! exceeds limit of {1}", nShutterErrors, optionSettings.maxShutterErrors);
                Rep.Post(errorMsg, repLevel.error, null);
                tooManyErrors = true;
            }
            if (nConsecutiveShutterErrors > optionSettings.maxConsecutiveShutterErrors)
            {
                errorMsg = String.Format("{0} consecutive shutter errors! exceeds limit of {1}", nConsecutiveShutterErrors, optionSettings.maxConsecutiveShutterErrors);
                Rep.Post(errorMsg, repLevel.error, null);
                tooManyErrors = true;
            }
            if (modelockConfigured)
            {
                if (nModelockErrors > optionSettings.maxModelockErrors)
                {
                    errorMsg = String.Format("{0} modelock errors! exceeds limit of {1}", nModelockErrors, optionSettings.maxModelockErrors);
                    Rep.Post(errorMsg, repLevel.error, null);
                    tooManyErrors = true;
                }
                if (nConsecutiveModelockErrors > optionSettings.maxConsecutiveModelockErrors)
                {
                    errorMsg = String.Format("{0} consecutive modelock errors! exceeds limit of {1}", nConsecutiveModelockErrors, optionSettings.maxConsecutiveModelockErrors);
                    Rep.Post(errorMsg, repLevel.error, null);
                    tooManyErrors = true;
                }
            }
            return(tooManyErrors);
        }



        // 
        public void resetErrorCounts()
        {
            nQueryErrors = 0;
            nConsecutiveQueryErrors = 0;
            nShutterErrors = 0;
            nConsecutiveShutterErrors = 0;
            nModelockErrors = 0;
            nConsecutiveModelockErrors = 0;
        }


        public LaserReturnCode waitForLaserReady()
        {
            bool stabilized = false;
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            while (!stabilized)
            {
                queryStatus(false);
                checkLaserReady();
                // this is kind of a strange way to do this....
                // I restart the stopwatch whenever it's *not ready, so I'm relying on the stopwatch
                // getting regularly reset whenever there is a query
                // in principle it could fail if there was some long unanticipated delay
                if (ready)
                {
                    if (stopWatch.ElapsedMilliseconds > laserStabilizationTime_s * 1000)
                    {
                        stabilized = true;
                    }
                }
                else
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                }
                System.Threading.Thread.Sleep(500);
            }
            return (LaserReturnCode.OK);
        }


        // return code is OK if all responses are received OK, false if they are not received
        // it's NOT about whether the responses indicate that the laser is ready
        // results are all stored in queried status variables
        public virtual LaserReturnCode queryStatus(bool calledByUpdateGUI)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }


        public virtual LaserReturnCode setWavelength(Double wavelengthToSet)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }


        // bool input is true for open, false for closed
        public virtual LaserReturnCode setShutter(bool openOrClosed, ShutterType shutterType)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }
        public enum ShutterType
        {
            NotSet, FixedWavelength, TunableWavelength
        }


        public override TaskEnd stop()
        {
            if (commsOK)
            {
                setShutter(false, Laser.ShutterType.FixedWavelength);
                setShutter(false, Laser.ShutterType.TunableWavelength);
            }
            return (TaskEnd.OK);
        }


        // bool input is true for on, false for off
        public virtual LaserReturnCode turnPumpOnOff(bool onOrOff)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }


        public virtual LaserReturnCode startWarmup()
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work
        }


        // 
        public virtual LaserReturnCode readyCloseApp()
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        // bool is true if OK, false if error
        public virtual LaserReturnCode shutDown()
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        // 
        public virtual LaserReturnCode setAlignMode(bool onOff, ShutterType shutter)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        // 
        public virtual LaserReturnCode selectObjective(string objectiveName)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        public virtual LaserReturnCode pullObjectiveList()
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        public virtual LaserReturnCode setGreenPower(bool onOff, double powerToSet)
        {
            return (LaserReturnCode.Placeholder); // default in case override does not work.
        }


        public override void closeAll()
        {
            if (!(_serialPort == null))
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                _serialPort.Dispose();
            }
        }


        // pass it a power reading, and it tells you whether or not it's within the allowed fluctuation band 
        public bool checkLaserFluctuationsOK(double thisPower_W) 
        {
            bool fluctuationsOK = false;
            try 
            {

                bool tooLow = thisPower_W < optionSettings.laserPowerMinFrac * baselineLaserPower;
                bool tooHigh = thisPower_W > optionSettings.laserPowerMaxFrac * baselineLaserPower;
                if (tooLow || tooHigh)
                {
                    fluctuationsOK = false;
                }
                else
                {
                    fluctuationsOK = true;
                }
            } 
            catch (Exception ex) 
            {
                Rep.Post("An exception occurred while checking laser fluctuations!", repLevel.error, null);
            } 
            return (fluctuationsOK); 
        } 
 
 
 
        public LaserReturnCode collectBaselinePower() 
        {
            if (!stable)
            {
                Rep.Post("Laser is not yet ready to collect baseline power!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
            }

            string baselineString = String.Format("Collecting baseline laser power...", baselineLaserPower);
            Rep.Post(baselineString, repLevel.details, null);

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch(); 
            stopWatch.Start(); 
            double powerRunningTotal = 0; 
            double nPowerPoints = 0; 
            int nMaxQueryAttempts = 5; 
            while (stopWatch.Elapsed.TotalSeconds < optionSettings.T_sample_s) 
            { 
                bool querySucceeded = false; 
                for (int jnd = 0; jnd < nMaxQueryAttempts; jnd++) 
                { 
                    var queryOK = queryStatus(false); 
                    if (queryOK == LaserReturnCode.OK) 
                    { 
                        querySucceeded = true; 
                        break; 
                    } 
                } 
                if (querySucceeded) 
                { 
                    powerRunningTotal = powerRunningTotal + currentPower; 
                    nPowerPoints = nPowerPoints + 1; 
                } 
                else 
                { 
                    Rep.Post("Failure while collecting baseline laser!", repLevel.error, null);
                    return (LaserReturnCode.CommError); 
                } 
            }
            baselineLaserPower = powerRunningTotal / nPowerPoints;
            baselineString = String.Format("Baseline laser power established at {0:F2} W", baselineLaserPower);
            Rep.Post(baselineString, repLevel.details, null);

            return (LaserReturnCode.OK); 
        } 






    }
}
