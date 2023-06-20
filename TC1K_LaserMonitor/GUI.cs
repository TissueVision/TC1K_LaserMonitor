using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TC1K_LaserMonitor
{
    public partial class GUI : Form
    {
        // major objects
        Laser laser = new Laser();
        public Reporter Rep = new Reporter();
        public optionSettings optionSettings = new optionSettings();
        public SettingsFileHandler settingsFileHandler = new SettingsFileHandler();

        // timer
        public System.Timers.Timer laserGUIupdateTimer = new System.Timers.Timer(); // triggers updating the laser
        public double laserGUIUpdateInterval_ms = 1000;
        bool blinkerState = false;

        // misc
        bool queryInProgress = false;
        bool baselineCollected = false;

        // andons
        public Andon fixedShutterAndon = new Andon();
        public Andon tunableShutterAndon = new Andon();
        public Andon laserStabilityAndon = new Andon();
        public Andon monitorOKAndonObj = new Andon();
        public Color andonOKColor = Color.LimeGreen;
        public Color andonSemiOKColor = Color.Yellow;
        public Color andonErrorColor = Color.GhostWhite;
        public Color andonTextDarkColor = Color.Gray;
        public Color andonTextLightColor = Color.White;

        // values
        string optionSettingsPath = @"C:\TissueVision\LaserMonitor\optionSettings.csv";
        string localLogDirectory = @"C:\TissueVision\LaserMonitor\Logs";



        public GUI()
        {
            InitializeComponent();
        }




        private void GUI_Load(object sender, EventArgs e)
        {

            Rep.errorAndon.control = errorAndon;
            Rep.messageCtrl = userMessages;
            Rep.initialize();

            // create local log
            if (!System.IO.Directory.Exists(localLogDirectory))
            {
                Directory.CreateDirectory(localLogDirectory);
            }
            Rep.logPath = Path.Combine(localLogDirectory, "LocalLog_" + createNowString(false)) + ".txt";
            Rep.logEnabled = true;

            // load settings file, etc.
            settingsFileHandler.Rep = Rep;
            bool thisFileLoadedOK;
            optionSettings = (optionSettings)settingsFileHandler.loadSettingsFile(out thisFileLoadedOK, optionSettingsPath, optionSettings);
            optionSettingsBindingSource.DataSource = optionSettings;
            optionSettingsBindingSource.ResetBindings(false);
            if (!thisFileLoadedOK)
            {
                Rep.Post("Failed to load settings file! Initialization failed!", repLevel.error, null);
                return;
            }

            // set andons
            fixedShutterAndon.control = fixedWLshutterAndon;
            tunableShutterAndon.control = tunableWLshutterAndon;
            laserStabilityAndon.control = laserStatusAndon;
            monitorOKAndonObj.control = monitorOKAndon;
            updateMonitorAndon();

            // configure the laser
            if ( (optionSettings.laserType == "MaiTai") || optionSettings.fakeOutLaser)
            {
                laser = new laser_MaiTai();
            }
            else if (optionSettings.laserType == "DiscoveryNX")
            {
                laser = new laser_Discovery();
            }
            else if (optionSettings.laserType == "InsightX3")
            {
                laser = new laser_InsightX3();
            }
            else
            {
                Rep.Post("Invalid laser type " + optionSettings.laserType, repLevel.error, null);
                return;
            }
            laser.gui = this;
            laser.Rep = Rep;
            laser.optionSettings = optionSettings;
            laser.fakeOut = optionSettings.fakeOutLaser;
            laser.comID = optionSettings.laserPort;
            laser.enableWatchdog = optionSettings.enableLaserWatchdog;
            setLaserButtonVisibilities(optionSettings.laserType);
            System.Threading.Thread.Sleep(optionSettings.laserGUIUpdateInterval_ms);

        }



        public void button_connectToLaser_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            laser.closeAll();
            System.Threading.Thread.Sleep((int)laserGUIUpdateInterval_ms);
            if (laser.initialize() == TaskEnd.OK)
            {
                // create a timer
                laserGUIupdateTimer = new System.Timers.Timer(laserGUIUpdateInterval_ms);
                laserGUIupdateTimer.Elapsed += updateLaserStatus; // Hook up the Elapsed event for the timer.
                laserGUIupdateTimer.AutoReset = true;
                laserGUIupdateTimer.Enabled = true;
                queryInProgress = false;
                //collectBaseline_Click(null, null); // this kind of flips out if the power is close to 0.
            }
            else
            {
                laserGUIupdateTimer.Enabled = false;
                queryInProgress = false;
            }
        }


        private void button_setWavelength_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; } 

            double wavelength;
            try
            {
                wavelength = Convert.ToDouble(wavelengthToSet.Text);
                var laserTask = laser.setWavelength(wavelength);
            }
            catch
            {
                Rep.Post("Wavelength setting failed!", repLevel.error, null);
            }

            queryInProgress = false;
        }


        private void button_openShutter_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            Laser.ShutterType shutterType = Laser.ShutterType.NotSet;
            if (((Button)sender).Name == "button_openShutter_FixedWL")
            {
                shutterType = Laser.ShutterType.FixedWavelength;
            }
            else if (((Button)sender).Name == "button_openShutter_TunableWL")
            {
                shutterType = Laser.ShutterType.TunableWavelength;
            }
            var shutterTask = laser.setShutter(true, shutterType);

            queryInProgress = false;
        }


        private void button_closeShutter_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            Laser.ShutterType shutterType = Laser.ShutterType.NotSet;
            if (((Button)sender).Name == "button_closeShutter_FixedWL")
            {
                shutterType = Laser.ShutterType.FixedWavelength;
            }
            else if (((Button)sender).Name == "button_closeShutter_TunableWL")
            {
                shutterType = Laser.ShutterType.TunableWavelength;
            }
            var shutterTask = laser.setShutter(false, shutterType);

            queryInProgress = false;
        }


        private void button_pumpOn_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            Rep.Post("Turning pump on...", repLevel.details, null);
            var laserTask = laser.turnPumpOnOff(true);
            if (laserTask == LaserReturnCode.OK)
            {
                Rep.Post("Pump laser is turned on.", repLevel.details, null);
            }
            else if (laserTask == LaserReturnCode.CommError)
            {
                Rep.Post("Communication error!  Could not turn on pump laser.", repLevel.error, null);
            }
            else if (laserTask == LaserReturnCode.NotWarmedUp)
            {
                Rep.Post("System is not warmed up!  Can not turn on pump laser yet.", repLevel.error, null);
            }
            else
            {
                Rep.Post("Error while turning on pump laser!", repLevel.error, null);
            }

            queryInProgress = false;
        }


        private void button_pumpOff_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            var laserTask = laser.turnPumpOnOff(false);

            queryInProgress = false;
        }


        private void shutDownLaser(object sender, EventArgs e) // only InsightX3 needs this
        {
            if (getQueryLock() == false) { return; }

            var laserTask = laser.shutDown();
            laserGUIupdateTimer.Enabled = false;
            laser.readyCloseApp();
            laser._serialPort.Close();
            laser.commsOK = false;

            queryInProgress = false;
        }


        private void button_disconnectPumpOn_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            // close shutter(s)
            if (laser.tunableShutterIsOpen)
            {
                laser.setShutter(false, Laser.ShutterType.TunableWavelength);
            }
            if (laser.fixedShutterIsOpen)
            {
                laser.setShutter(false, Laser.ShutterType.FixedWavelength);
            }
            var readyToClose = laser.readyCloseApp();
            try
            {
                laser._serialPort.Close();
                laser.commsOK = false;
            }
            catch
            { }
            if (readyToClose == LaserReturnCode.OK)
            {
                Rep.Post("OK to close App", repLevel.details, null);
            }

            queryInProgress = false;
        }


        private void button_disconnectPumpOff_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            // turn off pump
            laser.turnPumpOnOff(false);
            // call other function which does the rest
            button_disconnectPumpOn_Click(null, null);

            queryInProgress = false;
        }


        private void button_setAlignMode_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            bool onOff = false;
            Laser.ShutterType output = Laser.ShutterType.NotSet;
            if (((Button)sender).Name == "button_align_FixedWL")
            {
                output = Laser.ShutterType.FixedWavelength;
                onOff = true;
            }
            else if (((Button)sender).Name == "button_alignOFF_FixedWL")
            {
                output = Laser.ShutterType.FixedWavelength;
                onOff = false;
            }
            else if (((Button)sender).Name == "button_align_TunableWL")
            {
                output = Laser.ShutterType.TunableWavelength;
                onOff = true;
            }
            else if (((Button)sender).Name == "button_alignOFF_TunableWL")
            {
                output = Laser.ShutterType.TunableWavelength;
                onOff = false;
            }
            var setAlignTask = laser.setAlignMode(onOff, output);
            string laserMessage;
            if (setAlignTask == LaserReturnCode.OK)
            {
                string outputName = "";
                string modeName = "";
                if (output == Laser.ShutterType.TunableWavelength)
                {
                    outputName = "Tunable";
                }
                else if (output == Laser.ShutterType.FixedWavelength)
                {
                    outputName = "Fixed";
                }
                if (onOff)
                {
                    modeName = "alignment mode. Power falls gradually.";
                }
                else
                {
                    modeName = "normal mode. Power rises gradually.";
                }
                laserMessage = outputName + " is in " + modeName;
            }
            else
            {
                laserMessage = "Error while setting alignment mode!";
            }
            Rep.Post(laserMessage, repLevel.details, null);

            queryInProgress = false;
        }


        private void button_chooseObjective_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            Rep.Post("Setting objective...", repLevel.details, null);
            if (!laser.commsOK)
            {
                Rep.Post("Laser is not initialized.  Activating objective failed!", repLevel.error, null);
            }
            else if (dispersion_objectiveChoice.SelectedItem == null)
            {
                Rep.Post("No objective was selected!  Activating objective failed!", repLevel.error, null);
            }
            else
            {
                laser.selectObjective(dispersion_objectiveChoice.SelectedItem.ToString());
                string activeString = "Objective " + dispersion_objectiveChoice.SelectedItem.ToString() + " is active";
                Rep.Post(activeString, repLevel.details, null);
            }

            queryInProgress = false;
        }


        private void button_populateObjectiveList_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            Rep.Post("Getting objective list...", repLevel.details, null);
            if (!laser.commsOK)
            {
                Rep.Post("Laser is not initialized.  Could not populate objective list!", repLevel.error, null);
            }
            else
            {
                var pullObjTask = laser.pullObjectiveList();
                if (pullObjTask != LaserReturnCode.OK)
                {
                    Rep.Post("Error while pulling objective list!", repLevel.error, null);
                }
                else
                {
                    dispersion_objectiveChoice.Items.Clear();
                    foreach (string thisChoice in laser.objectiveList)
                    {
                        dispersion_objectiveChoice.Items.Add(thisChoice);
                    }
                    dispersion_objectiveChoice.SelectedIndex = 0;
                    Rep.Post("Objective list is ready.", repLevel.details, null);
                }
            }

            queryInProgress = false;
        }


        private void button_setGreenPower(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            Rep.Post("Setting green power", repLevel.details, null);
            try
            {
                bool greenPowerOn = check_controlGreenPower.Checked;
                if (greenPowerToSet.Text == "")
                {
                    greenPowerToSet.Text = laser.greenPower.Substring(0, laser.greenPower.Length - 1);
                }
                double newGreenPower = Convert.ToDouble(this.greenPowerToSet.Text);
                var setGreenTask = laser.setGreenPower(greenPowerOn, newGreenPower);
                if (setGreenTask == LaserReturnCode.OK)
                {
                    if (greenPowerOn)
                    {
                        Rep.Post("Green power set to " + newGreenPower.ToString(), repLevel.details, null);
                    }
                    else
                    {
                        Rep.Post("Green power mode cancelled", repLevel.details, null);
                    }
                }
                else
                {
                    Rep.Post("Could not send green power command!", repLevel.error, null);
                }
            }
            catch
            {
                Rep.Post("Misc error while setting green power!", repLevel.error, null);
            }

            queryInProgress = false;
        }



        private void button_clearError_Click(object sender, EventArgs e)
        {
            Rep.ClearCancelAll();
        }

        
        public void setLaserButtonVisibilities(string laserType)
        {
            if (laserType == "MaiTai")
            {
                laserTypeLabel.Text = "Spectra-Physics MaiTai";
                button_closeShutter_FixedWL.Visible = false;
                button_openShutter_FixedWL.Visible = false;
                shutterStatus_FixedWL.Visible = false;
                button_shutDownLaser.Visible = false;
                pumpLaserHours.Visible = false;
                pumpLaserHoursLabel.Visible = false;
                button_align_FixedWL.Visible = false;
                button_alignOFF_FixedWL.Visible = false;
                button_align_TunableWL.Visible = false;
                button_alignOFF_TunableWL.Visible = false;
                button_chooseObjective.Visible = false;
                button_populateObjectiveList.Visible = false;
                dispersion_objectiveChoice.Visible = false;
                label_dispersionCorrection.Visible = false;
            }
            else if (laserType == "InsightX3")
            {
                laserTypeLabel.Text = "Spectra-Physics InsightX3";
                pumpLaser2Current.Visible = false;
                pumpLaser2CurrentLabel.Visible = false;
                pumpLaser2Temperature.Visible = false;
                pumpLaser2TemperatureLabel.Visible = false;
                button_align_FixedWL.Visible = false;
                button_alignOFF_FixedWL.Visible = false;
                check_controlGreenPower.Visible = false;
                label_greenPowerNow.Visible = false;
                greenPowerNow.Visible = false;
                greenPowerToSet.Visible = false;
                setGreenPower.Visible = false;
            }
            else if (laserType == "DiscoveryNX")
            {
                laserTypeLabel.Text = "Coherent Discovery NX";
                modelockLabel.Visible = false;
                modelockStatus.Visible = false;
                warmupStatus.Visible = false;
                warmupPctLabel.Visible = false;
                button_shutDownLaser.Visible = false;
                pumpLaserCurrent.Visible = false;
                pumpLaserCurrentLabel.Visible = false;
                pumpLaserTemperature.Visible = false;
                pumpLaserTemperatureLabel.Visible = false;
                pumpLaserHours.Visible = false;
                pumpLaserHoursLabel.Visible = false;
                pumpLaser2Current.Visible = false;
                pumpLaser2CurrentLabel.Visible = false;
                pumpLaser2Temperature.Visible = false;
                pumpLaser2TemperatureLabel.Visible = false;
                check_controlGreenPower.Visible = false;
                label_greenPowerNow.Visible = false;
                greenPowerNow.Visible = false;
                greenPowerToSet.Visible = false;
                setGreenPower.Visible = false;
            }
            else
            {
                Rep.Post("Invalid laser type " + laserType, repLevel.error, null);
            }

        }

        
        private void timerTest_Click(object sender, EventArgs e)
        {
            int thisButtonActionID = DateTime.Now.Second;
            Rep.Post("Button pressed... " + thisButtonActionID.ToString(), repLevel.details, null);

            if (getQueryLock() == false) { return; }

            int delay_ms = 2000;
            System.Threading.Thread.Sleep(delay_ms);
            Rep.Post("Button action done! " + thisButtonActionID.ToString(), repLevel.details, null);

            queryInProgress = false;

        }


        private bool getQueryLock()
        {
            // wait for query to be available, or timeout
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int timeout_ms = 5000;
            while (true)
            {
                if (queryInProgress == false)
                {
                    queryInProgress = true;
                    //Rep.Post("Obtained query lock...", repLevel.details, null);
                    return(true);
                }
                System.Windows.Forms.Application.DoEvents();
                if (sw.ElapsedMilliseconds > timeout_ms)
                {
                    Rep.Post("Query lock timed out!", repLevel.error, null);
                    return (false);
                }
            }
        }
        

        public void testTimerUpdate(object sender, EventArgs e)
        {
            if (queryInProgress)
            {
                return;
            }
            queryInProgress = true;


            int timeSecs = DateTime.Now.Second;
            this.Invoke(new MethodInvoker(delegate()
            {
                string timerStr = "timer action start. " + timeSecs.ToString();
                timerTestLabel.Text = timerStr;
                Rep.Post(timerStr, repLevel.details, null);
            }));


            int delay_ms = 2000;
            System.Threading.Thread.Sleep(delay_ms);
            queryInProgress = false;


            this.Invoke(new MethodInvoker(delegate()
            {
                string timerStr = "timer action done. " + timeSecs.ToString();
                timerTestLabel.Text = timerStr;
                Rep.Post(timerStr, repLevel.details, null);
            }));


        }
        

        public void updateLaserStatus(object sender, EventArgs e)
        {
            if (queryInProgress)
            {
                return;
            }
            queryInProgress = true;

            try
            {
                var queryTask = laser.queryStatus(true);
                if (queryTask == LaserReturnCode.BumpedFromLock)
                {
                    queryInProgress = false;
                    return;
                }
                else if ((queryTask == LaserReturnCode.OK)) // this means that the values are valid, NOT that the laser is ready to use
                {

                    this.Invoke(new MethodInvoker(delegate()
                    {
                        if (laser.fixedShutterIsOpen)
                        {
                            shutterStatus_FixedWL.Text = "Open";
                            fixedShutterAndon.set("Fixed WL shutter OPEN", andonOKColor, andonTextLightColor);
                        }
                        else
                        {
                            shutterStatus_FixedWL.Text = "Closed";
                            fixedShutterAndon.set("Fixed WL shutter CLOSED", andonErrorColor, andonTextDarkColor);
                        }
                        if (laser.tunableShutterIsOpen)
                        {
                            shutterStatus_TunableWL.Text = "Open";
                            tunableShutterAndon.set("Tunable WL shutter OPEN", andonOKColor, andonTextLightColor);
                        }
                        else
                        {
                            shutterStatus_TunableWL.Text = "Closed";
                            tunableShutterAndon.set("Tunable WL shutter CLOSED", andonErrorColor, andonTextDarkColor);
                        }
                        if (laser.pumpLaserIsOn)
                        {
                            pumpLaserOnStatus.Text = "On";
                        }
                        else
                        {
                            pumpLaserOnStatus.Text = "Off";
                        }
                        if (laser.physicalKeyIsOn)
                        {
                            physicalKeyStatus.Text = "On";
                        }
                        else
                        {
                            physicalKeyStatus.Text = "Off";
                        }
                        if (laser.commsOK)
                        {
                            connectionStatus.Text = "Connected OK";
                        }
                        else
                        {
                            connectionStatus.Text = "Not connected";
                        }
                        wavelengthStatus.Text = Convert.ToString(laser.currentWavelength) + "nm";
                        if (laser.modelocked) // if modelocking is not applicable to the laser, it doesn't matter because the label is hidden
                        {
                            modelockStatus.Text = "Yes";
                        }
                        else
                        {
                            modelockStatus.Text = "No";
                        }
                        power.Text = laser.currentPower.ToString("0.000000") + "W";
                        warmupStatus.Text = Convert.ToString(laser.warmupFraction); // if warmup is not applicable to the laser, it doesn't matter because the label is hidden
                        if (laser.laserError)
                        {
                            errorCode.Text = Convert.ToString(laser.errorCode);
                        }
                        else
                        {
                            errorCode.Text = "";
                        }
                        pumpLaserCurrent.Text = laser.pumpCurrent;
                        pumpLaserTemperature.Text = Convert.ToString(laser.pumpTemperature);
                        pumpLaserHours.Text = Convert.ToString(laser.pumpHours) + " h";
                        pumpLaser2Current.Text = laser.pumpCurrent2;
                        pumpLaser2Temperature.Text = Convert.ToString(laser.pumpTemperature);
                        greenPowerNow.Text = laser.greenPower;

                        // heartbeat to show that it's updating
                        if (blinkerState)
                        {
                            blinker.BackColor = Color.Transparent;
                            blinkerState = false;
                        }
                        else
                        {
                            blinker.BackColor = Color.MediumBlue;
                            blinkerState = true;
                        }

                        // update andons
                        if (laser.pumpLaserIsOn)
                        {
                            if (laser.stable)
                            {
                                readyToCollect.BackColor = Color.MediumBlue;
                                laserStabilityAndon.set("Laser is STABLE", andonOKColor, andonTextLightColor);
                            }
                            else
                            {
                                readyToCollect.BackColor = Color.Transparent;
                                laserStabilityAndon.set("Laser is stabilizing", andonSemiOKColor, andonTextDarkColor);
                            }
                        }
                        else
                        {
                            readyToCollect.BackColor = Color.Transparent;
                            laserStabilityAndon.set("Laser pump is off", andonErrorColor, andonTextDarkColor);
                        }

                        // monitor laser power (the whole point of this app!)
                        if (baselineCollected)
                        {
                            bool powerOK = laser.checkLaserFluctuationsOK(laser.currentPower);
                            if (!powerOK)
                            {
                                string powerString = String.Format("Laser power {0}W exceeds range of {1}-{2}x {3}W!", laser.currentPower,
                                    optionSettings.laserPowerMinFrac, optionSettings.laserPowerMaxFrac, laser.baselineLaserPower);
                                Rep.Post(powerString, repLevel.error, null);
                                if (optionSettings.terminateOrchestratorOnDrift)
                                {
                                    terminateOrchestrator();
                                }
                            }
                        }

                    }));

                }
                else // including if it gets bumped from the lockout
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        connectionStatus.Text = "-";
                        wavelengthStatus.Text = "-";
                        modelockStatus.Text = "-";
                        power.Text = "-";
                        warmupStatus.Text = "-";
                        errorCode.Text = "-";
                        pumpLaserCurrent.Text = "-";
                        pumpLaserTemperature.Text = "-";
                        pumpLaserHours.Text = "-";
                        pumpLaserOnStatus.Text = "-";
                        physicalKeyStatus.Text = "-";
                        shutterStatus_FixedWL.Text = "-";
                        shutterStatus_TunableWL.Text = "-";
                        tunableWLshutterAndon.Text = "-";
                        fixedWLshutterAndon.Text = "-";
                        laserStatusAndon.Text = "-";
                        tunableWLshutterAndon.BackColor = andonErrorColor;
                        fixedWLshutterAndon.BackColor = andonErrorColor;
                        laserStatusAndon.BackColor = andonErrorColor;
                    }));
                }
            }
            catch (Exception ex)
            {
            }

            queryInProgress = false;
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


        private void collectBaseline_Click(object sender, EventArgs e)
        {
            if (getQueryLock() == false) { return; }

            var baselineOK = laser.collectBaselinePower();
            if (baselineOK == LaserReturnCode.OK)
            {
                baseline_W.Text = String.Format("{0:F2}", laser.baselineLaserPower);
                baselineCollected = true;
            }
            else
            {
                baseline_W.Text = "invalid";
                baselineCollected = false;
            }

            queryInProgress = false;
            updateMonitorAndon();
        }
        

        private void updateMonitorAndon()
        {
            if (baselineCollected && optionSettings.terminateOrchestratorOnDrift)
            {
                monitorOKAndonObj.set("Monitor is ON", andonOKColor, andonTextLightColor);
            }
            else
            {
                monitorOKAndonObj.set("Monitor is off", andonErrorColor, andonTextDarkColor);
            }
        }

        
        private void terminateOrchestrator()
        {
            // Get all processes running on the local computer.
            System.Diagnostics.Process[] localAll = System.Diagnostics.Process.GetProcesses();

            // Get all instances of DssHost running on the local computer.
            // This will return an empty array if DssHost isn't running.
            string procName = "DssHost";
            System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcessesByName(procName);
            if (localByName.Length==0)
            {
                Rep.Post("Could not get DssHost process! Could not terminate orchestrator!", repLevel.error, null);
                return;
            }
            else
            {
                if (localByName.Length > 1)
                {
                    Rep.Post("Multiple DssHost processes were found! " + localByName.Length.ToString(), repLevel.error, null);
                    return;
                }
                try
                {
                    localByName[0].Kill();
                    Rep.Post("Orchestrator was terminated!", repLevel.details, null);
                }
                catch (Exception ex)
                {
                    Rep.Post("Exception while attempting to terminate Orchestrator!", repLevel.error, ex.Message);
                }
            }
        }


        private void terminateOrchestratorOnDrift_CheckedChanged(object sender, EventArgs e)
        {
            optionSettings.terminateOrchestratorOnDrift = terminateOrchestratorOnDrift.Checked;

            // the value of optionSettings.terminateOrchestratorOnDrift gets updated automatically, then this affects the andon
            updateMonitorAndon();
        }



    }
}
