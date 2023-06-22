using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;


namespace TC1K_LaserMonitor
{


    //
    public class laser_MaiTai : laser_Spectra
    {


        // constructor only
        public laser_MaiTai()
        {
            this.fixedShutterConfigured = false;
            this.modelockConfigured = true;
            this.minOKPowerConfigured = false;
            //// MaiTai HP:
            //this.tunableWavelengthMin_nm = 710;
            //this.tunableWavelengthMax_nm = 990;
            // MaiTai BB:
            this.tunableWavelengthMin_nm = 690;
            this.tunableWavelengthMax_nm = 1040;
            this.greenPowerConfigured = true;
            this.wavelengthNowReportedWithNM = true;
        }


        public override LaserReturnCode shutDown()
        {
            Rep.Post("MaiTai does not use a shutdown sequence!", repLevel.error, null);
            return (LaserReturnCode.MiscError);
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

            string queryResponse;
            bool allResponsesOK = true;
            string trimmedResponse = "";
            try
            {
                // see whether keyswitch is on
                queryResponse = sendQuery(false,"PLAS:AHIS?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    int physicalKeyOffPosition = queryResponse.IndexOf("120");
                    bool physicalKeyWasOff = (physicalKeyOffPosition != -1);
                    int interlockClearedPosition = queryResponse.IndexOf("116");
                    bool interlockWasCleared = (interlockClearedPosition != -1); // there is a remotely-possible failure mode because there are *other interlock resets that could trip this
                    if (!physicalKeyWasOff)
                    {
                        physicalKeyIsOn = true;
                    }
                    else if (interlockWasCleared && (interlockClearedPosition < physicalKeyOffPosition))
                    {
                        physicalKeyIsOn = true;
                    }
                    else
                    {
                        physicalKeyIsOn = false;
                    }
                }

                // see how warmed up it is
                queryResponse = sendQuery(false,"READ:PCTWARMEDUP?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    trimmedResponse = queryResponse.Substring(0, queryResponse.Length - 1);
                    warmupFraction = Convert.ToDouble(trimmedResponse);
                }

                // check current wavelength
                queryResponse = sendQuery(false, "READ:WAVELENGTH?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    if (wavelengthNowReportedWithNM)
                    {
                        queryResponse = queryResponse.Substring(0, queryResponse.Length - 2); // clip off 'nm' from the end 
                    }
                    currentWavelength = Convert.ToDouble(queryResponse);
                }

                // check shutters
                // first, tunable shutter
                queryResponse = sendQuery(false,"SHUTTER?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    tunableShutterIsOpen = Convert.ToInt16(queryResponse) == 1;
                }

                // check status byte
                queryResponse = sendQuery(false,"*STB?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    byte responseAsByte = Convert.ToByte(queryResponse);
                    // see whether pump laser is on
                    pumpLaserIsOn = ((responseAsByte & 1) == 1); // check whether the 1st bit is high 
                    // check modelock
                    modelocked = ((responseAsByte & 2) == 2); // check whether the 2nd bit is high
                }

                // read power
                queryResponse = sendQuery(false,"READ:POWER?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    trimmedResponse = queryResponse.Substring(0, queryResponse.Length - 1);
                    currentPower = Convert.ToDouble(trimmedResponse);
                }

                // read error code
                queryResponse = sendQuery(false,"PLASER:ERRCODE?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    bool laserReturnsLetterE = false;
                    if (laserReturnsLetterE)
                    {
                        // this was needed for MaiTai HP, but when it was replaced by MaiTai BB in April / May 2022, it resulted in the wrong behavior 
                        errorCode = queryResponse.Substring(0, queryResponse.Length - 1); // this takes an 'e' off the end 
                    }
                    else
                    {
                        errorCode = queryResponse;
                    }

                    laserError = (errorCode != "0");

                    // // this should have worked according to the documentation, but it didn't work that way
                    //string responseAsByteString = Convert.ToString(Convert.ToInt32(queryResponse, 10), 2);
                    //int respLen = responseAsByteString.Length; // response length
                    //if (responseAsByteString.Substring(respLen - 1 - 7, 1) == "1") // if the 8th bit (bit 7) is high, it indicates an error
                    //{
                    //    laserError = true;
                    //}
                    //else
                    //{
                    //    laserError = false;
                    //}
                    //errorCode = queryResponse;

                    // list error codes that we do *not consider to be errors
                    string[] acceptableErrorCodes = { "0", "-211" }; // 0 = no error; -211 = file error  
                    laserError = !Array.Exists(acceptableErrorCodes, element => element == errorCode);
                }

                // read pump current, diode 1
                queryResponse = sendQuery(false,"READ:PLASER:DIODE1:CURRENT?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpCurrent = queryResponse.Substring(0, queryResponse.Length - 1);
                }

                // read pump temperature, diode 2
                queryResponse = sendQuery(false,"READ:PLASER:DIODE1:TEMPERATURE?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpTemperature = queryResponse.Substring(0, queryResponse.Length - 1);
                }

                // no way to read pump hours on

                // read pump current, diode 1
                queryResponse = sendQuery(false,"READ:PLASER:DIODE2:CURRENT?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpCurrent2 = queryResponse.Substring(0, queryResponse.Length - 1);
                }

                // read green power
                queryResponse = sendQuery(false,"PLASER:POWER?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    greenPower = queryResponse;
                }

                // read pump temperature, diode 2
                queryResponse = sendQuery(false,"READ:PLASER:DIODE2:TEMPERATURE?");
                allResponsesOK = allResponsesOK && (queryResponse != "error");
                if (queryResponse != "error")
                {
                    pumpTemperature2 = queryResponse.Substring(0, queryResponse.Length - 1);
                }


            }
            catch (Exception ex)
            {
                allResponsesOK = false;
            }
            if (!allResponsesOK)
            {
                gui.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    Rep.Post("Query error!", repLevel.details, null);
                }));
            }
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
        public override LaserReturnCode setAlignMode(bool onOff, ShutterType shutter)
        {
            Rep.Post("MaiTai does not have an alignment mode!", repLevel.error, null);
            return (LaserReturnCode.MiscError);
        }


        public override LaserReturnCode setGreenPower(bool onOff, double powerToSet)
        {
            if (!commsOK)
            {
                return (LaserReturnCode.CommError);
            }
            LaserReturnCode commandOK;
            if (onOff)
            {
                commandOK = sendCommand("MODE PPOW"); // control pump green power
                if (commandOK == LaserReturnCode.OK)
                {
                    commandOK = sendCommand("PLASER:POWER " + powerToSet.ToString()); // control power to this wattage
                }
            }
            else
            {
                commandOK = sendCommand("MODE POW"); // control output power
            }
            return (commandOK);
        }



    }
}
