using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TC1K_LaserMonitor
{
    public partial class GUI : Form
    {
        public Reporter Rep = new Reporter();
        public optionSettings optionSettings = new optionSettings();

        public System.Timers.Timer laserGUIupdateTimer = new System.Timers.Timer(); // triggers updating the laser
        public double laserGUIUpdateInterval_ms = 1000;
        bool blinkerState = false;
        Laser laser = new Laser();

        // andons
        public Andon fixedShutterAndon = new Andon();
        public Andon tunableShutterAndon = new Andon();
        public Andon laserStabilityAndon = new Andon();
        public Color andonOKColor = Color.LimeGreen;
        public Color andonSemiOKColor = Color.Yellow;
        public Color andonErrorColor = Color.GhostWhite;
        public Color andonTextDarkColor = Color.Gray;
        public Color andonTextLightColor = Color.White;




        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            // load settings file, etc.


            laser.gui = this;
            laser.optionSettings = optionSettings;
        }




        public void updateLaserStatus(object sender, EventArgs e)
        {
            try
            {
                var queryTask = laser.queryStatus(true);
                if (queryTask == LaserReturnCode.BumpedFromLock)
                {
                    return;
                }
                else if (queryTask == LaserReturnCode.OK) // this means that the values are valid, NOT that the laser is ready to use
                {

                    this.Invoke(new MethodInvoker(delegate ()
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
                    }));

                }
                else // including if it gets bumped from the lockout
                {
                    this.Invoke(new MethodInvoker(delegate ()
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
            catch
            {
            }
        }


        public void setLaserButtonVisibilities(string laserType)
        {
            if (laserType == "MaiTai")
            {
                laserTypeLabel.Text = "Spectra-Physics MaiTai";
                button_closeShutter_FixedWL.Visible = false;
                button_openShutter_FixedWL.Visible = false;
                shutterStatus_FixedWL.Visible = false;
                fixedWavelengthLabel.Visible = false;
                fixedLambdaLabel.Visible = false;
                button_shutDownLaser.Visible = false;
                pumpLaserHours.Visible = false;
                pumpLaserHoursLabel.Visible = false;
                button_align_FixedWL.Visible = false;
                button_alignOFF_FixedWL.Visible = false;
                alignFixedLabel.Visible = false;
                alignFixedLambda.Visible = false;
                button_align_TunableWL.Visible = false;
                button_alignOFF_TunableWL.Visible = false;
                alignTunableLabel.Visible = false;
                alignTunableLambda.Visible = false;
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
                alignFixedLabel.Visible = false;
                alignFixedLambda.Visible = false;
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



        public void button_connectToLaser_Click(object sender, EventArgs e)
        {
            laserGUIupdateTimer.Enabled = false;
            System.Threading.Thread.Sleep((int)laserGUIUpdateInterval_ms);
            laser.enableWatchdog = optionSettings.enableLaserWatchdog;
            laser.fakeOut = optionSettings.fakeOutLaser || optionSettings.fakeOutAllHardware;
            laser.Rep = Rep;
            if (laser.initialize() == TaskEnd.OK)
            {
                // Create a timer
                laserGUIupdateTimer = new System.Timers.Timer(laserGUIUpdateInterval_ms);
                laserGUIupdateTimer.Elapsed += updateLaserStatus; // Hook up the Elapsed event for the timer. 
                laserGUIupdateTimer.AutoReset = true;
                laserGUIupdateTimer.Enabled = true;
            }
            else
            {
                laserGUIupdateTimer.Enabled = false;
            }
        }



        private void button_setWavelength_Click(object sender, EventArgs e)
        {
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
            return;
        }


        private void button_openShutter_Click(object sender, EventArgs e)
        {
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
        }


        private void button_closeShutter_Click(object sender, EventArgs e)
        {
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
        }


        private void button_pumpOn_Click(object sender, EventArgs e)
        {
            laserStatusMessage.Text = "Turning pump on...";
            var laserTask = laser.turnPumpOnOff(true);
            if (laserTask == LaserReturnCode.OK)
            {
                laserStatusMessage.Text = "Pump laser is turned on.";
            }
            else if (laserTask == LaserReturnCode.CommError)
            {
                laserStatusMessage.Text = "Communication error!  Could not turn on pump laser.";
            }
            else if (laserTask == LaserReturnCode.NotWarmedUp)
            {
                laserStatusMessage.Text = "System is not warmed up!  Can not turn on pump laser yet.";
            }
            else
            {
                laserStatusMessage.Text = "Error while turning on pump laser!";
            }
            return;
        }


        private void button_pumpOff_Click(object sender, EventArgs e)
        {
            var laserTask = laser.turnPumpOnOff(false);
        }


        private void shutDownLaser(object sender, EventArgs e) // only InsightX3 needs this
        {
            var laserTask = laser.shutDown();
            laserGUIupdateTimer.Enabled = false;
            laser.readyCloseApp();
            laser._serialPort.Close();
            laser.commsOK = false;
            return;
        }


        private void button_disconnectPumpOn_Click(object sender, EventArgs e)
        {
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
        }


        private void button_disconnectPumpOff_Click(object sender, EventArgs e)
        {
            // turn off pump
            laser.turnPumpOnOff(false);
            // call other function which does the rest
            button_disconnectPumpOn_Click(null, null);
            return;
        }


        private void button_setAlignMode_Click(object sender, EventArgs e)
        {
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
            return;
        }


        private void button_chooseObjective_Click(object sender, EventArgs e)
        {
            laserStatusMessage.Text = "Setting objective...";
            if (!laser.commsOK)
            {
                laserStatusMessage.Text = "Laser is not initialized.  Activating objective failed!";
            }
            else if (dispersion_objectiveChoice.SelectedItem == null)
            {
                laserStatusMessage.Text = "No objective was selected!  Activating objective failed!";
            }
            else
            {
                laser.selectObjective(dispersion_objectiveChoice.SelectedItem.ToString());
                laserStatusMessage.Text = "Objective " + dispersion_objectiveChoice.SelectedItem.ToString() + " is active";
            }
        }


        private void button_populateObjectiveList_Click(object sender, EventArgs e)
        {
            laserStatusMessage.Text = "Getting objective list...";
            if (!laser.commsOK)
            {
                laserStatusMessage.Text = "Laser is not initialized.  Could not populate objective list!";
            }
            else
            {
                var pullObjTask = laser.pullObjectiveList();
                if (pullObjTask != LaserReturnCode.OK)
                {
                    laserStatusMessage.Text = "Error while pulling objective list!";
                }
                else
                {
                    dispersion_objectiveChoice.Items.Clear();
                    foreach (string thisChoice in laser.objectiveList)
                    {
                        dispersion_objectiveChoice.Items.Add(thisChoice);
                    }
                    dispersion_objectiveChoice.SelectedIndex = 0;
                    laserStatusMessage.Text = "Objective list is ready.";
                }
            }
        }


        private void button_setGreenPower(object sender, EventArgs e)
        {
            laserStatusMessage.Text = "Setting green power";
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
                        laserStatusMessage.Text = "Green power set to " + newGreenPower.ToString();
                    }
                    else
                    {
                        laserStatusMessage.Text = "Green power mode cancelled";
                    }
                }
                else
                {
                    laserStatusMessage.Text = "Could not send green power command!";
                }
            }
            catch
            {
                laserStatusMessage.Text = "Misc error while setting green power!";
            }
            return;
        }















    }
}
