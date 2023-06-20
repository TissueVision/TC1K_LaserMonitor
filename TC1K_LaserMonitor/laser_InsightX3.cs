using System;


namespace TC1K_LaserMonitor
{

    //
    public class laser_InsightX3: laser_Spectra
    {

        // constructor only
        public laser_InsightX3()
        {
            this.fixedShutterConfigured = true;
            this.modelockConfigured = true;
            this.minOKPowerConfigured = true;
            this.fixedWavelength = 1045;
            this.tunableWavelengthMin_nm = 680;
            this.tunableWavelengthMax_nm = 1300;
            this.wavelengthNowReportedWithNM = false;
        }


        public override LaserReturnCode shutDown()
        {
            if (!commsOK)
            {
                Rep.Post("Laser is not connected, can not start soft shutdown!", repLevel.error, null);
                return (LaserReturnCode.CommError);
            }
            string shutdownOKMsg = "Soft shutdown is complete. Turn off power switch on laser unit.";
            if (fakeOut)
            {
                Rep.Post(shutdownOKMsg, repLevel.details, null);
                return (LaserReturnCode.OK);
            }
            var lockTask = lockout.requestLock(false);
            if (lockTask != Lockout.LockoutReturn.OK)
            {
                Rep.Post("Lock timeout while starting soft shutdown!", repLevel.error, null);
                return (LaserReturnCode.CommTimeout);
            }

            Rep.Post("Laser is beginning soft shutdown.  Please wait...", repLevel.error, null);
            string commandString = "SHUTDOWN";
            LaserReturnCode commandOK = sendCommand(commandString);
            lockout.releaseLock();
            if (commandOK == LaserReturnCode.OK)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                Rep.Post(shutdownOKMsg, repLevel.details, null);
                return (LaserReturnCode.OK);
            }
            else
            {
                Rep.Post("Could not send soft shutdown command!", repLevel.error, null);
                return (LaserReturnCode.MiscError);
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
            Lockout.LockoutReturn lockOK =  lockout.requestLock(calledByUpdateGUI);
            if (lockOK != Lockout.LockoutReturn.OK)
            {
                return (lockout.lockoutToLaserCode(lockOK));
            }

            string queryResponse;
            bool allResponsesOK = true;
            try
            {

                _serialPort.DiscardInBuffer();

                // see how warmed up it is
                queryResponse = sendQuery(false,"READ:PCTWARMEDUP?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    warmupFraction = Convert.ToDouble(queryResponse);
                }

                // check current wavelength
                queryResponse = sendQuery(false,"READ:WAVELENGTH?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    if (wavelengthNowReportedWithNM)
                    {
                        queryResponse = queryResponse.Substring(0, queryResponse.Length - 2); // clip off 'nm' from the end 
                    }
                    currentWavelength = Convert.ToDouble(queryResponse);
                }

                // read power
                queryResponse = sendQuery(false,"READ:POWER?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    currentPower = Convert.ToDouble(queryResponse);
                }

                // read pump current
                queryResponse = sendQuery(false,"READ:PLASER:DIODE1:CURRENT?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpCurrent = queryResponse + "A";
                }

                // read pump temperature
                queryResponse = sendQuery(false,"READ:PLASER:DIODE1:TEMPERATURE?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpTemperature = queryResponse;
                }

                // read pump hours on
                queryResponse = sendQuery(false,"READ:PLASER:DIODE1:HOURS?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpHours = queryResponse;
                }

                // check status byte
                queryResponse = sendQuery(false,"*STB?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    string responseAsByteString = Convert.ToString(Convert.ToInt32(queryResponse, 10), 2);
                    int respLen = responseAsByteString.Length; // response length
                    // check whether pump laser is on
                    pumpLaserIsOn = (responseAsByteString.Substring(respLen - 1 - 0, 1) == "1"); // if the 1st bit (bit 0) is high, it indicates an error
                    // check modelock
                    modelocked = (responseAsByteString.Substring(respLen - 1 - 1, 1) == "1"); // check whether the 2nd bit (bit 1) is high 
                    if (modelocked)
                    {
                        nConsecutiveModelockErrors = 0;
                    }
                    else
                    {
                        nModelockErrors++;
                        nConsecutiveModelockErrors++;
                    }
                    // check keyswitch
                    physicalKeyIsOn = (responseAsByteString.Substring(respLen - 1 - 10, 1) == "0"); // if the 11th bit (bit 10) is low, the key is on
                    // check for error
                    if (responseAsByteString.Substring(respLen - 1 - 15, 1) == "1") // if the 16th bit (bit 15) is high, it indicates an error
                    {
                        // find out what the error is
                        laserError = true;
                        queryResponse = sendQuery(false,"READ:AHISTORY?");
                        allResponsesOK = allResponsesOK && (queryResponse != "error");
                        errorCode = queryResponse;
                    }
                    else 
                    {
                        laserError = false;
                    }
                    // check shutters
                    tunableShutterIsOpen = (responseAsByteString.Substring(respLen - 1 - 2, 1) == "1");
                    fixedShutterIsOpen = (responseAsByteString.Substring(respLen - 1 - 3, 1) == "1");
                    //if (fixedShutterNeededForLaserOK)
                    //{
                    //    if (fixedShutterConfigured && fixedShutterIsOpen)
                    //    {
                    //        nConsecutiveShutterErrors = 0;
                    //    }
                    //    else
                    //    {
                    //        nShutterErrors++;
                    //        nConsecutiveShutterErrors++;
                    //    }
                    //}
                    //if (tunableShutterNeededForLaserOK)
                    //{
                    //    if (tunableShutterIsOpen)
                    //    {
                    //        nConsecutiveShutterErrors = 0;
                    //    }
                    //    else
                    //    {
                    //        nShutterErrors++;
                    //        nConsecutiveShutterErrors++;
                    //    }
                    //}
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
                gui.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    Rep.Post(queryErrorMsg, repLevel.details, null);
                }));
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


        // shutter type does not matter for Insight
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
            string commandString = null;
            if (onOff)
            {
                commandString = "MODE ALIGN";
            }
            else
            {
                commandString = "MODE RUN";
            }
            LaserReturnCode commandOK = sendCommand(commandString);
            lockout.releaseLock();
            return (commandOK);
        }



    }
}
