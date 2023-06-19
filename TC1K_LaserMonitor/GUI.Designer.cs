namespace TC1K_LaserMonitor
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.setGreenPower = new System.Windows.Forms.Button();
            this.greenPowerNow = new System.Windows.Forms.Label();
            this.greenPowerToSet = new System.Windows.Forms.TextBox();
            this.laserStatusMessage = new System.Windows.Forms.TextBox();
            this.wavelengthToSet = new System.Windows.Forms.TextBox();
            this.check_controlGreenPower = new System.Windows.Forms.CheckBox();
            this.label_greenPowerNow = new System.Windows.Forms.Label();
            this.dispersion_objectiveChoice = new System.Windows.Forms.ComboBox();
            this.button_populateObjectiveList = new System.Windows.Forms.Button();
            this.label_dispersionCorrection = new System.Windows.Forms.Label();
            this.button_chooseObjective = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.pumpLaser2Temperature = new System.Windows.Forms.Label();
            this.pumpLaser2TemperatureLabel = new System.Windows.Forms.Label();
            this.pumpLaser2CurrentLabel = new System.Windows.Forms.Label();
            this.pumpLaser2Current = new System.Windows.Forms.Label();
            this.alignTunableLambda = new System.Windows.Forms.Label();
            this.alignFixedLambda = new System.Windows.Forms.Label();
            this.alignTunableLabel = new System.Windows.Forms.Label();
            this.alignFixedLabel = new System.Windows.Forms.Label();
            this.button_alignOFF_FixedWL = new System.Windows.Forms.Button();
            this.button_align_FixedWL = new System.Windows.Forms.Button();
            this.button_alignOFF_TunableWL = new System.Windows.Forms.Button();
            this.button_align_TunableWL = new System.Windows.Forms.Button();
            this.label162 = new System.Windows.Forms.Label();
            this.fixedLambdaLabel = new System.Windows.Forms.Label();
            this.pumpLaserTemperature = new System.Windows.Forms.Label();
            this.pumpLaserTemperatureLabel = new System.Windows.Forms.Label();
            this.pumpLaserCurrentLabel = new System.Windows.Forms.Label();
            this.pumpLaserHours = new System.Windows.Forms.Label();
            this.pumpLaserHoursLabel = new System.Windows.Forms.Label();
            this.pumpLaserCurrent = new System.Windows.Forms.Label();
            this.laserTypeLabel = new System.Windows.Forms.Label();
            this.label163 = new System.Windows.Forms.Label();
            this.button_disconnectPumpOn = new System.Windows.Forms.Button();
            this.physicalKeyStatus = new System.Windows.Forms.Label();
            this.label164 = new System.Windows.Forms.Label();
            this.warmupPctLabel = new System.Windows.Forms.Label();
            this.pumpLaserOnStatus = new System.Windows.Forms.Label();
            this.readyToCollect = new System.Windows.Forms.Button();
            this.label138 = new System.Windows.Forms.Label();
            this.label141 = new System.Windows.Forms.Label();
            this.fixedWavelengthLabel = new System.Windows.Forms.Label();
            this.button_shutDownLaser = new System.Windows.Forms.Button();
            this.button_closeShutter_TunableWL = new System.Windows.Forms.Button();
            this.shutterStatus_TunableWL = new System.Windows.Forms.Label();
            this.button_openShutter_TunableWL = new System.Windows.Forms.Button();
            this.label148 = new System.Windows.Forms.Label();
            this.errorCode = new System.Windows.Forms.Label();
            this.label147 = new System.Windows.Forms.Label();
            this.power = new System.Windows.Forms.Label();
            this.label145 = new System.Windows.Forms.Label();
            this.modelockStatus = new System.Windows.Forms.Label();
            this.wavelengthStatus = new System.Windows.Forms.Label();
            this.modelockLabel = new System.Windows.Forms.Label();
            this.label140 = new System.Windows.Forms.Label();
            this.blinker = new System.Windows.Forms.Button();
            this.label139 = new System.Windows.Forms.Label();
            this.button_closeShutter_FixedWL = new System.Windows.Forms.Button();
            this.shutterStatus_FixedWL = new System.Windows.Forms.Label();
            this.button_openShutter_FixedWL = new System.Windows.Forms.Button();
            this.button_pumpLaserOff = new System.Windows.Forms.Button();
            this.button_pumpLaserOn = new System.Windows.Forms.Button();
            this.button_setWavelength = new System.Windows.Forms.Button();
            this.warmupStatus = new System.Windows.Forms.Label();
            this.connectionStatus = new System.Windows.Forms.Label();
            this.button_ConnectToLaser = new System.Windows.Forms.Button();
            this.errorAndon = new System.Windows.Forms.Button();
            this.button_clearError = new System.Windows.Forms.Button();
            this.laserStatusAndon = new System.Windows.Forms.Button();
            this.fixedWLshutterAndon = new System.Windows.Forms.Button();
            this.tunableWLshutterAndon = new System.Windows.Forms.Button();
            this.pmtOnOffIndicator = new System.Windows.Forms.Button();
            this.userMessages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // setGreenPower
            // 
            this.setGreenPower.Location = new System.Drawing.Point(704, 584);
            this.setGreenPower.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.setGreenPower.Name = "setGreenPower";
            this.setGreenPower.Size = new System.Drawing.Size(188, 29);
            this.setGreenPower.TabIndex = 148;
            this.setGreenPower.Text = "Set green power (W)";
            this.setGreenPower.UseVisualStyleBackColor = true;
            this.setGreenPower.Click += new System.EventHandler(this.button_setGreenPower);
            // 
            // greenPowerNow
            // 
            this.greenPowerNow.AutoSize = true;
            this.greenPowerNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.greenPowerNow.ForeColor = System.Drawing.SystemColors.Highlight;
            this.greenPowerNow.Location = new System.Drawing.Point(1034, 622);
            this.greenPowerNow.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenPowerNow.Name = "greenPowerNow";
            this.greenPowerNow.Size = new System.Drawing.Size(15, 20);
            this.greenPowerNow.TabIndex = 147;
            this.greenPowerNow.Text = "-";
            // 
            // greenPowerToSet
            // 
            this.greenPowerToSet.Location = new System.Drawing.Point(1008, 585);
            this.greenPowerToSet.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.greenPowerToSet.Name = "greenPowerToSet";
            this.greenPowerToSet.Size = new System.Drawing.Size(72, 24);
            this.greenPowerToSet.TabIndex = 146;
            // 
            // laserStatusMessage
            // 
            this.laserStatusMessage.Location = new System.Drawing.Point(704, 45);
            this.laserStatusMessage.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.laserStatusMessage.Name = "laserStatusMessage";
            this.laserStatusMessage.Size = new System.Drawing.Size(437, 24);
            this.laserStatusMessage.TabIndex = 103;
            // 
            // wavelengthToSet
            // 
            this.wavelengthToSet.Location = new System.Drawing.Point(279, 250);
            this.wavelengthToSet.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.wavelengthToSet.Name = "wavelengthToSet";
            this.wavelengthToSet.Size = new System.Drawing.Size(72, 24);
            this.wavelengthToSet.TabIndex = 94;
            // 
            // check_controlGreenPower
            // 
            this.check_controlGreenPower.AutoSize = true;
            this.check_controlGreenPower.Location = new System.Drawing.Point(704, 546);
            this.check_controlGreenPower.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.check_controlGreenPower.Name = "check_controlGreenPower";
            this.check_controlGreenPower.Size = new System.Drawing.Size(162, 22);
            this.check_controlGreenPower.TabIndex = 145;
            this.check_controlGreenPower.Text = "Control green power";
            this.check_controlGreenPower.UseVisualStyleBackColor = true;
            // 
            // label_greenPowerNow
            // 
            this.label_greenPowerNow.AutoSize = true;
            this.label_greenPowerNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_greenPowerNow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_greenPowerNow.Location = new System.Drawing.Point(700, 622);
            this.label_greenPowerNow.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_greenPowerNow.Name = "label_greenPowerNow";
            this.label_greenPowerNow.Size = new System.Drawing.Size(163, 20);
            this.label_greenPowerNow.TabIndex = 144;
            this.label_greenPowerNow.Text = "Green power now (W)";
            // 
            // dispersion_objectiveChoice
            // 
            this.dispersion_objectiveChoice.FormattingEnabled = true;
            this.dispersion_objectiveChoice.Location = new System.Drawing.Point(851, 434);
            this.dispersion_objectiveChoice.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.dispersion_objectiveChoice.Name = "dispersion_objectiveChoice";
            this.dispersion_objectiveChoice.Size = new System.Drawing.Size(234, 26);
            this.dispersion_objectiveChoice.TabIndex = 143;
            // 
            // button_populateObjectiveList
            // 
            this.button_populateObjectiveList.Location = new System.Drawing.Point(704, 429);
            this.button_populateObjectiveList.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_populateObjectiveList.Name = "button_populateObjectiveList";
            this.button_populateObjectiveList.Size = new System.Drawing.Size(139, 33);
            this.button_populateObjectiveList.TabIndex = 142;
            this.button_populateObjectiveList.Text = "Populate list";
            this.button_populateObjectiveList.UseVisualStyleBackColor = true;
            this.button_populateObjectiveList.Click += new System.EventHandler(this.button_populateObjectiveList_Click);
            // 
            // label_dispersionCorrection
            // 
            this.label_dispersionCorrection.AutoSize = true;
            this.label_dispersionCorrection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_dispersionCorrection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_dispersionCorrection.Location = new System.Drawing.Point(700, 405);
            this.label_dispersionCorrection.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_dispersionCorrection.Name = "label_dispersionCorrection";
            this.label_dispersionCorrection.Size = new System.Drawing.Size(158, 20);
            this.label_dispersionCorrection.TabIndex = 141;
            this.label_dispersionCorrection.Text = "Dispersion correction";
            // 
            // button_chooseObjective
            // 
            this.button_chooseObjective.Location = new System.Drawing.Point(704, 468);
            this.button_chooseObjective.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_chooseObjective.Name = "button_chooseObjective";
            this.button_chooseObjective.Size = new System.Drawing.Size(139, 33);
            this.button_chooseObjective.TabIndex = 140;
            this.button_chooseObjective.Text = "Choose objective";
            this.button_chooseObjective.UseVisualStyleBackColor = true;
            this.button_chooseObjective.Click += new System.EventHandler(this.button_chooseObjective_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(177, 542);
            this.button6.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(301, 29);
            this.button6.TabIndex = 139;
            this.button6.Text = "  Disconnect, close shutter, pump OFF";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button_disconnectPumpOff_Click);
            // 
            // pumpLaser2Temperature
            // 
            this.pumpLaser2Temperature.AutoSize = true;
            this.pumpLaser2Temperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaser2Temperature.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pumpLaser2Temperature.Location = new System.Drawing.Point(1034, 355);
            this.pumpLaser2Temperature.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaser2Temperature.Name = "pumpLaser2Temperature";
            this.pumpLaser2Temperature.Size = new System.Drawing.Size(15, 20);
            this.pumpLaser2Temperature.TabIndex = 138;
            this.pumpLaser2Temperature.Text = "-";
            // 
            // pumpLaser2TemperatureLabel
            // 
            this.pumpLaser2TemperatureLabel.AutoSize = true;
            this.pumpLaser2TemperatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaser2TemperatureLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pumpLaser2TemperatureLabel.Location = new System.Drawing.Point(700, 355);
            this.pumpLaser2TemperatureLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaser2TemperatureLabel.Name = "pumpLaser2TemperatureLabel";
            this.pumpLaser2TemperatureLabel.Size = new System.Drawing.Size(103, 20);
            this.pumpLaser2TemperatureLabel.TabIndex = 137;
            this.pumpLaser2TemperatureLabel.Text = "Pump temp 2";
            // 
            // pumpLaser2CurrentLabel
            // 
            this.pumpLaser2CurrentLabel.AutoSize = true;
            this.pumpLaser2CurrentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaser2CurrentLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pumpLaser2CurrentLabel.Location = new System.Drawing.Point(700, 320);
            this.pumpLaser2CurrentLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaser2CurrentLabel.Name = "pumpLaser2CurrentLabel";
            this.pumpLaser2CurrentLabel.Size = new System.Drawing.Size(117, 20);
            this.pumpLaser2CurrentLabel.TabIndex = 136;
            this.pumpLaser2CurrentLabel.Text = "Pump current 2";
            // 
            // pumpLaser2Current
            // 
            this.pumpLaser2Current.AutoSize = true;
            this.pumpLaser2Current.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaser2Current.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pumpLaser2Current.Location = new System.Drawing.Point(1034, 320);
            this.pumpLaser2Current.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaser2Current.Name = "pumpLaser2Current";
            this.pumpLaser2Current.Size = new System.Drawing.Size(15, 20);
            this.pumpLaser2Current.TabIndex = 135;
            this.pumpLaser2Current.Text = "-";
            // 
            // alignTunableLambda
            // 
            this.alignTunableLambda.AutoSize = true;
            this.alignTunableLambda.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignTunableLambda.ForeColor = System.Drawing.SystemColors.ControlText;
            this.alignTunableLambda.Location = new System.Drawing.Point(1085, 162);
            this.alignTunableLambda.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.alignTunableLambda.Name = "alignTunableLambda";
            this.alignTunableLambda.Size = new System.Drawing.Size(18, 20);
            this.alignTunableLambda.TabIndex = 134;
            this.alignTunableLambda.Text = "l";
            // 
            // alignFixedLambda
            // 
            this.alignFixedLambda.AutoSize = true;
            this.alignFixedLambda.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignFixedLambda.ForeColor = System.Drawing.SystemColors.ControlText;
            this.alignFixedLambda.Location = new System.Drawing.Point(1060, 117);
            this.alignFixedLambda.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.alignFixedLambda.Name = "alignFixedLambda";
            this.alignFixedLambda.Size = new System.Drawing.Size(18, 20);
            this.alignFixedLambda.TabIndex = 133;
            this.alignFixedLambda.Text = "l";
            // 
            // alignTunableLabel
            // 
            this.alignTunableLabel.AutoSize = true;
            this.alignTunableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignTunableLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.alignTunableLabel.Location = new System.Drawing.Point(1005, 162);
            this.alignTunableLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.alignTunableLabel.Name = "alignTunableLabel";
            this.alignTunableLabel.Size = new System.Drawing.Size(83, 20);
            this.alignTunableLabel.TabIndex = 132;
            this.alignTunableLabel.Text = "TUNABLE";
            // 
            // alignFixedLabel
            // 
            this.alignFixedLabel.AutoSize = true;
            this.alignFixedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignFixedLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.alignFixedLabel.Location = new System.Drawing.Point(1005, 117);
            this.alignFixedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.alignFixedLabel.Name = "alignFixedLabel";
            this.alignFixedLabel.Size = new System.Drawing.Size(58, 20);
            this.alignFixedLabel.TabIndex = 131;
            this.alignFixedLabel.Text = "FIXED";
            // 
            // button_alignOFF_FixedWL
            // 
            this.button_alignOFF_FixedWL.Location = new System.Drawing.Point(855, 111);
            this.button_alignOFF_FixedWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_alignOFF_FixedWL.Name = "button_alignOFF_FixedWL";
            this.button_alignOFF_FixedWL.Size = new System.Drawing.Size(139, 33);
            this.button_alignOFF_FixedWL.TabIndex = 130;
            this.button_alignOFF_FixedWL.Text = "Normal mode";
            this.button_alignOFF_FixedWL.UseVisualStyleBackColor = true;
            this.button_alignOFF_FixedWL.Click += new System.EventHandler(this.button_setAlignMode_Click);
            // 
            // button_align_FixedWL
            // 
            this.button_align_FixedWL.Location = new System.Drawing.Point(704, 111);
            this.button_align_FixedWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_align_FixedWL.Name = "button_align_FixedWL";
            this.button_align_FixedWL.Size = new System.Drawing.Size(139, 33);
            this.button_align_FixedWL.TabIndex = 129;
            this.button_align_FixedWL.Text = "Align mode";
            this.button_align_FixedWL.UseVisualStyleBackColor = true;
            this.button_align_FixedWL.Click += new System.EventHandler(this.button_setAlignMode_Click);
            // 
            // button_alignOFF_TunableWL
            // 
            this.button_alignOFF_TunableWL.Location = new System.Drawing.Point(855, 155);
            this.button_alignOFF_TunableWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_alignOFF_TunableWL.Name = "button_alignOFF_TunableWL";
            this.button_alignOFF_TunableWL.Size = new System.Drawing.Size(139, 33);
            this.button_alignOFF_TunableWL.TabIndex = 128;
            this.button_alignOFF_TunableWL.Text = "Normal mode";
            this.button_alignOFF_TunableWL.UseVisualStyleBackColor = true;
            this.button_alignOFF_TunableWL.Click += new System.EventHandler(this.button_setAlignMode_Click);
            // 
            // button_align_TunableWL
            // 
            this.button_align_TunableWL.Location = new System.Drawing.Point(704, 155);
            this.button_align_TunableWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_align_TunableWL.Name = "button_align_TunableWL";
            this.button_align_TunableWL.Size = new System.Drawing.Size(139, 33);
            this.button_align_TunableWL.TabIndex = 127;
            this.button_align_TunableWL.Text = "Align mode";
            this.button_align_TunableWL.UseVisualStyleBackColor = true;
            this.button_align_TunableWL.Click += new System.EventHandler(this.button_setAlignMode_Click);
            // 
            // label162
            // 
            this.label162.AutoSize = true;
            this.label162.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label162.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label162.Location = new System.Drawing.Point(155, 208);
            this.label162.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label162.Name = "label162";
            this.label162.Size = new System.Drawing.Size(18, 20);
            this.label162.TabIndex = 126;
            this.label162.Text = "l";
            // 
            // fixedLambdaLabel
            // 
            this.fixedLambdaLabel.AutoSize = true;
            this.fixedLambdaLabel.Font = new System.Drawing.Font("Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixedLambdaLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fixedLambdaLabel.Location = new System.Drawing.Point(130, 162);
            this.fixedLambdaLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fixedLambdaLabel.Name = "fixedLambdaLabel";
            this.fixedLambdaLabel.Size = new System.Drawing.Size(18, 20);
            this.fixedLambdaLabel.TabIndex = 125;
            this.fixedLambdaLabel.Text = "l";
            // 
            // pumpLaserTemperature
            // 
            this.pumpLaserTemperature.AutoSize = true;
            this.pumpLaserTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserTemperature.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pumpLaserTemperature.Location = new System.Drawing.Point(1034, 245);
            this.pumpLaserTemperature.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserTemperature.Name = "pumpLaserTemperature";
            this.pumpLaserTemperature.Size = new System.Drawing.Size(15, 20);
            this.pumpLaserTemperature.TabIndex = 124;
            this.pumpLaserTemperature.Text = "-";
            // 
            // pumpLaserTemperatureLabel
            // 
            this.pumpLaserTemperatureLabel.AutoSize = true;
            this.pumpLaserTemperatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserTemperatureLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pumpLaserTemperatureLabel.Location = new System.Drawing.Point(700, 245);
            this.pumpLaserTemperatureLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserTemperatureLabel.Name = "pumpLaserTemperatureLabel";
            this.pumpLaserTemperatureLabel.Size = new System.Drawing.Size(90, 20);
            this.pumpLaserTemperatureLabel.TabIndex = 123;
            this.pumpLaserTemperatureLabel.Text = "Pump temp";
            // 
            // pumpLaserCurrentLabel
            // 
            this.pumpLaserCurrentLabel.AutoSize = true;
            this.pumpLaserCurrentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserCurrentLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pumpLaserCurrentLabel.Location = new System.Drawing.Point(700, 208);
            this.pumpLaserCurrentLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserCurrentLabel.Name = "pumpLaserCurrentLabel";
            this.pumpLaserCurrentLabel.Size = new System.Drawing.Size(104, 20);
            this.pumpLaserCurrentLabel.TabIndex = 122;
            this.pumpLaserCurrentLabel.Text = "Pump current";
            // 
            // pumpLaserHours
            // 
            this.pumpLaserHours.AutoSize = true;
            this.pumpLaserHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserHours.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pumpLaserHours.Location = new System.Drawing.Point(1034, 278);
            this.pumpLaserHours.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserHours.Name = "pumpLaserHours";
            this.pumpLaserHours.Size = new System.Drawing.Size(15, 20);
            this.pumpLaserHours.TabIndex = 121;
            this.pumpLaserHours.Text = "-";
            // 
            // pumpLaserHoursLabel
            // 
            this.pumpLaserHoursLabel.AutoSize = true;
            this.pumpLaserHoursLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserHoursLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pumpLaserHoursLabel.Location = new System.Drawing.Point(700, 278);
            this.pumpLaserHoursLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserHoursLabel.Name = "pumpLaserHoursLabel";
            this.pumpLaserHoursLabel.Size = new System.Drawing.Size(94, 20);
            this.pumpLaserHoursLabel.TabIndex = 120;
            this.pumpLaserHoursLabel.Text = "Pump hours";
            // 
            // pumpLaserCurrent
            // 
            this.pumpLaserCurrent.AutoSize = true;
            this.pumpLaserCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserCurrent.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pumpLaserCurrent.Location = new System.Drawing.Point(1034, 208);
            this.pumpLaserCurrent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserCurrent.Name = "pumpLaserCurrent";
            this.pumpLaserCurrent.Size = new System.Drawing.Size(15, 20);
            this.pumpLaserCurrent.TabIndex = 119;
            this.pumpLaserCurrent.Text = "-";
            // 
            // laserTypeLabel
            // 
            this.laserTypeLabel.AutoSize = true;
            this.laserTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laserTypeLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.laserTypeLabel.Location = new System.Drawing.Point(285, 22);
            this.laserTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.laserTypeLabel.Name = "laserTypeLabel";
            this.laserTypeLabel.Size = new System.Drawing.Size(15, 20);
            this.laserTypeLabel.TabIndex = 118;
            this.laserTypeLabel.Text = "-";
            // 
            // label163
            // 
            this.label163.AutoSize = true;
            this.label163.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label163.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label163.Location = new System.Drawing.Point(173, 24);
            this.label163.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label163.Name = "label163";
            this.label163.Size = new System.Drawing.Size(83, 20);
            this.label163.TabIndex = 117;
            this.label163.Text = "Laser type";
            // 
            // button_disconnectPumpOn
            // 
            this.button_disconnectPumpOn.Location = new System.Drawing.Point(177, 584);
            this.button_disconnectPumpOn.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_disconnectPumpOn.Name = "button_disconnectPumpOn";
            this.button_disconnectPumpOn.Size = new System.Drawing.Size(301, 29);
            this.button_disconnectPumpOn.TabIndex = 116;
            this.button_disconnectPumpOn.Text = "  Disconnect, close shutter, pump ON";
            this.button_disconnectPumpOn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_disconnectPumpOn.UseVisualStyleBackColor = true;
            this.button_disconnectPumpOn.Click += new System.EventHandler(this.button_disconnectPumpOn_Click);
            // 
            // physicalKeyStatus
            // 
            this.physicalKeyStatus.AutoSize = true;
            this.physicalKeyStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.physicalKeyStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.physicalKeyStatus.Location = new System.Drawing.Point(507, 338);
            this.physicalKeyStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.physicalKeyStatus.Name = "physicalKeyStatus";
            this.physicalKeyStatus.Size = new System.Drawing.Size(15, 20);
            this.physicalKeyStatus.TabIndex = 115;
            this.physicalKeyStatus.Text = "-";
            // 
            // label164
            // 
            this.label164.AutoSize = true;
            this.label164.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label164.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label164.Location = new System.Drawing.Point(173, 338);
            this.label164.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label164.Name = "label164";
            this.label164.Size = new System.Drawing.Size(75, 20);
            this.label164.TabIndex = 114;
            this.label164.Text = "Key state";
            // 
            // warmupPctLabel
            // 
            this.warmupPctLabel.AutoSize = true;
            this.warmupPctLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warmupPctLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.warmupPctLabel.Location = new System.Drawing.Point(173, 303);
            this.warmupPctLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.warmupPctLabel.Name = "warmupPctLabel";
            this.warmupPctLabel.Size = new System.Drawing.Size(95, 20);
            this.warmupPctLabel.TabIndex = 113;
            this.warmupPctLabel.Text = "Warmup pct";
            // 
            // pumpLaserOnStatus
            // 
            this.pumpLaserOnStatus.AutoSize = true;
            this.pumpLaserOnStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pumpLaserOnStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.pumpLaserOnStatus.Location = new System.Drawing.Point(507, 117);
            this.pumpLaserOnStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pumpLaserOnStatus.Name = "pumpLaserOnStatus";
            this.pumpLaserOnStatus.Size = new System.Drawing.Size(15, 20);
            this.pumpLaserOnStatus.TabIndex = 112;
            this.pumpLaserOnStatus.Text = "-";
            // 
            // readyToCollect
            // 
            this.readyToCollect.Location = new System.Drawing.Point(343, 492);
            this.readyToCollect.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.readyToCollect.Name = "readyToCollect";
            this.readyToCollect.Size = new System.Drawing.Size(15, 16);
            this.readyToCollect.TabIndex = 111;
            this.readyToCollect.UseVisualStyleBackColor = true;
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label138.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label138.Location = new System.Drawing.Point(173, 489);
            this.label138.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(158, 20);
            this.label138.TabIndex = 110;
            this.label138.Text = "Ready to collect data";
            // 
            // label141
            // 
            this.label141.AutoSize = true;
            this.label141.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label141.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label141.Location = new System.Drawing.Point(75, 208);
            this.label141.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label141.Name = "label141";
            this.label141.Size = new System.Drawing.Size(83, 20);
            this.label141.TabIndex = 109;
            this.label141.Text = "TUNABLE";
            // 
            // fixedWavelengthLabel
            // 
            this.fixedWavelengthLabel.AutoSize = true;
            this.fixedWavelengthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixedWavelengthLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fixedWavelengthLabel.Location = new System.Drawing.Point(75, 162);
            this.fixedWavelengthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fixedWavelengthLabel.Name = "fixedWavelengthLabel";
            this.fixedWavelengthLabel.Size = new System.Drawing.Size(58, 20);
            this.fixedWavelengthLabel.TabIndex = 108;
            this.fixedWavelengthLabel.Text = "FIXED";
            // 
            // button_shutDownLaser
            // 
            this.button_shutDownLaser.Location = new System.Drawing.Point(177, 626);
            this.button_shutDownLaser.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_shutDownLaser.Name = "button_shutDownLaser";
            this.button_shutDownLaser.Size = new System.Drawing.Size(301, 29);
            this.button_shutDownLaser.TabIndex = 107;
            this.button_shutDownLaser.Text = "  Disconnect, prepare for hard powerdown";
            this.button_shutDownLaser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_shutDownLaser.UseVisualStyleBackColor = true;
            this.button_shutDownLaser.Click += new System.EventHandler(this.shutDownLaser);
            // 
            // button_closeShutter_TunableWL
            // 
            this.button_closeShutter_TunableWL.Location = new System.Drawing.Point(328, 202);
            this.button_closeShutter_TunableWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_closeShutter_TunableWL.Name = "button_closeShutter_TunableWL";
            this.button_closeShutter_TunableWL.Size = new System.Drawing.Size(139, 33);
            this.button_closeShutter_TunableWL.TabIndex = 106;
            this.button_closeShutter_TunableWL.Text = "CLOSE shutter";
            this.button_closeShutter_TunableWL.UseVisualStyleBackColor = true;
            this.button_closeShutter_TunableWL.Click += new System.EventHandler(this.button_closeShutter_Click);
            // 
            // shutterStatus_TunableWL
            // 
            this.shutterStatus_TunableWL.AutoSize = true;
            this.shutterStatus_TunableWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shutterStatus_TunableWL.ForeColor = System.Drawing.SystemColors.Highlight;
            this.shutterStatus_TunableWL.Location = new System.Drawing.Point(507, 208);
            this.shutterStatus_TunableWL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.shutterStatus_TunableWL.Name = "shutterStatus_TunableWL";
            this.shutterStatus_TunableWL.Size = new System.Drawing.Size(15, 20);
            this.shutterStatus_TunableWL.TabIndex = 105;
            this.shutterStatus_TunableWL.Text = "-";
            // 
            // button_openShutter_TunableWL
            // 
            this.button_openShutter_TunableWL.Location = new System.Drawing.Point(177, 202);
            this.button_openShutter_TunableWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_openShutter_TunableWL.Name = "button_openShutter_TunableWL";
            this.button_openShutter_TunableWL.Size = new System.Drawing.Size(139, 33);
            this.button_openShutter_TunableWL.TabIndex = 104;
            this.button_openShutter_TunableWL.Text = "OPEN shutter";
            this.button_openShutter_TunableWL.UseVisualStyleBackColor = true;
            this.button_openShutter_TunableWL.Click += new System.EventHandler(this.button_openShutter_Click);
            // 
            // label148
            // 
            this.label148.AutoSize = true;
            this.label148.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label148.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label148.Location = new System.Drawing.Point(700, 24);
            this.label148.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label148.Name = "label148";
            this.label148.Size = new System.Drawing.Size(125, 20);
            this.label148.TabIndex = 102;
            this.label148.Text = "Status message";
            // 
            // errorCode
            // 
            this.errorCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorCode.ForeColor = System.Drawing.SystemColors.Highlight;
            this.errorCode.Location = new System.Drawing.Point(507, 442);
            this.errorCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorCode.MaximumSize = new System.Drawing.Size(160, 0);
            this.errorCode.MinimumSize = new System.Drawing.Size(160, 60);
            this.errorCode.Name = "errorCode";
            this.errorCode.Size = new System.Drawing.Size(160, 60);
            this.errorCode.TabIndex = 101;
            this.errorCode.Text = "-";
            // 
            // label147
            // 
            this.label147.AutoSize = true;
            this.label147.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label147.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label147.Location = new System.Drawing.Point(173, 442);
            this.label147.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label147.Name = "label147";
            this.label147.Size = new System.Drawing.Size(83, 20);
            this.label147.TabIndex = 100;
            this.label147.Text = "Error code";
            // 
            // power
            // 
            this.power.AutoSize = true;
            this.power.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.power.ForeColor = System.Drawing.SystemColors.Highlight;
            this.power.Location = new System.Drawing.Point(507, 408);
            this.power.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.power.Name = "power";
            this.power.Size = new System.Drawing.Size(15, 20);
            this.power.TabIndex = 99;
            this.power.Text = "-";
            // 
            // label145
            // 
            this.label145.AutoSize = true;
            this.label145.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label145.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label145.Location = new System.Drawing.Point(173, 408);
            this.label145.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label145.Name = "label145";
            this.label145.Size = new System.Drawing.Size(53, 20);
            this.label145.TabIndex = 98;
            this.label145.Text = "Power";
            // 
            // modelockStatus
            // 
            this.modelockStatus.AutoSize = true;
            this.modelockStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelockStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.modelockStatus.Location = new System.Drawing.Point(507, 373);
            this.modelockStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.modelockStatus.Name = "modelockStatus";
            this.modelockStatus.Size = new System.Drawing.Size(15, 20);
            this.modelockStatus.TabIndex = 97;
            this.modelockStatus.Text = "-";
            // 
            // wavelengthStatus
            // 
            this.wavelengthStatus.AutoSize = true;
            this.wavelengthStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wavelengthStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.wavelengthStatus.Location = new System.Drawing.Point(507, 254);
            this.wavelengthStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.wavelengthStatus.Name = "wavelengthStatus";
            this.wavelengthStatus.Size = new System.Drawing.Size(15, 20);
            this.wavelengthStatus.TabIndex = 96;
            this.wavelengthStatus.Text = "-";
            // 
            // modelockLabel
            // 
            this.modelockLabel.AutoSize = true;
            this.modelockLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelockLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.modelockLabel.Location = new System.Drawing.Point(173, 373);
            this.modelockLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.modelockLabel.Name = "modelockLabel";
            this.modelockLabel.Size = new System.Drawing.Size(77, 20);
            this.modelockLabel.TabIndex = 95;
            this.modelockLabel.Text = "Modelock";
            // 
            // label140
            // 
            this.label140.AutoSize = true;
            this.label140.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label140.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label140.Location = new System.Drawing.Point(173, 252);
            this.label140.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label140.Name = "label140";
            this.label140.Size = new System.Drawing.Size(93, 20);
            this.label140.TabIndex = 93;
            this.label140.Text = "Wavelength";
            // 
            // blinker
            // 
            this.blinker.Location = new System.Drawing.Point(618, 26);
            this.blinker.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.blinker.Name = "blinker";
            this.blinker.Size = new System.Drawing.Size(15, 16);
            this.blinker.TabIndex = 92;
            this.blinker.UseVisualStyleBackColor = true;
            // 
            // label139
            // 
            this.label139.AutoSize = true;
            this.label139.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label139.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label139.Location = new System.Drawing.Point(507, 24);
            this.label139.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label139.Name = "label139";
            this.label139.Size = new System.Drawing.Size(111, 20);
            this.label139.TabIndex = 91;
            this.label139.Text = "Comm active";
            // 
            // button_closeShutter_FixedWL
            // 
            this.button_closeShutter_FixedWL.Location = new System.Drawing.Point(328, 155);
            this.button_closeShutter_FixedWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_closeShutter_FixedWL.Name = "button_closeShutter_FixedWL";
            this.button_closeShutter_FixedWL.Size = new System.Drawing.Size(139, 33);
            this.button_closeShutter_FixedWL.TabIndex = 90;
            this.button_closeShutter_FixedWL.Text = "CLOSE shutter";
            this.button_closeShutter_FixedWL.UseVisualStyleBackColor = true;
            this.button_closeShutter_FixedWL.Click += new System.EventHandler(this.button_closeShutter_Click);
            // 
            // shutterStatus_FixedWL
            // 
            this.shutterStatus_FixedWL.AutoSize = true;
            this.shutterStatus_FixedWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shutterStatus_FixedWL.ForeColor = System.Drawing.SystemColors.Highlight;
            this.shutterStatus_FixedWL.Location = new System.Drawing.Point(507, 162);
            this.shutterStatus_FixedWL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.shutterStatus_FixedWL.Name = "shutterStatus_FixedWL";
            this.shutterStatus_FixedWL.Size = new System.Drawing.Size(15, 20);
            this.shutterStatus_FixedWL.TabIndex = 89;
            this.shutterStatus_FixedWL.Text = "-";
            // 
            // button_openShutter_FixedWL
            // 
            this.button_openShutter_FixedWL.Location = new System.Drawing.Point(177, 155);
            this.button_openShutter_FixedWL.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_openShutter_FixedWL.Name = "button_openShutter_FixedWL";
            this.button_openShutter_FixedWL.Size = new System.Drawing.Size(139, 33);
            this.button_openShutter_FixedWL.TabIndex = 88;
            this.button_openShutter_FixedWL.Text = "OPEN shutter";
            this.button_openShutter_FixedWL.UseVisualStyleBackColor = true;
            this.button_openShutter_FixedWL.Click += new System.EventHandler(this.button_openShutter_Click);
            // 
            // button_pumpLaserOff
            // 
            this.button_pumpLaserOff.Location = new System.Drawing.Point(328, 113);
            this.button_pumpLaserOff.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_pumpLaserOff.Name = "button_pumpLaserOff";
            this.button_pumpLaserOff.Size = new System.Drawing.Size(139, 29);
            this.button_pumpLaserOff.TabIndex = 87;
            this.button_pumpLaserOff.Text = "Pump laser OFF";
            this.button_pumpLaserOff.UseVisualStyleBackColor = true;
            this.button_pumpLaserOff.Click += new System.EventHandler(this.button_pumpOff_Click);
            // 
            // button_pumpLaserOn
            // 
            this.button_pumpLaserOn.Location = new System.Drawing.Point(177, 113);
            this.button_pumpLaserOn.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_pumpLaserOn.Name = "button_pumpLaserOn";
            this.button_pumpLaserOn.Size = new System.Drawing.Size(139, 29);
            this.button_pumpLaserOn.TabIndex = 86;
            this.button_pumpLaserOn.Text = "Pump laser ON";
            this.button_pumpLaserOn.UseVisualStyleBackColor = true;
            this.button_pumpLaserOn.Click += new System.EventHandler(this.button_pumpOn_Click);
            // 
            // button_setWavelength
            // 
            this.button_setWavelength.Location = new System.Drawing.Point(399, 247);
            this.button_setWavelength.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_setWavelength.Name = "button_setWavelength";
            this.button_setWavelength.Size = new System.Drawing.Size(68, 29);
            this.button_setWavelength.TabIndex = 85;
            this.button_setWavelength.Text = "Set";
            this.button_setWavelength.UseVisualStyleBackColor = true;
            this.button_setWavelength.Click += new System.EventHandler(this.button_setWavelength_Click);
            // 
            // warmupStatus
            // 
            this.warmupStatus.AutoSize = true;
            this.warmupStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warmupStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.warmupStatus.Location = new System.Drawing.Point(507, 303);
            this.warmupStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.warmupStatus.Name = "warmupStatus";
            this.warmupStatus.Size = new System.Drawing.Size(15, 20);
            this.warmupStatus.TabIndex = 84;
            this.warmupStatus.Text = "-";
            // 
            // connectionStatus
            // 
            this.connectionStatus.AutoSize = true;
            this.connectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.connectionStatus.Location = new System.Drawing.Point(507, 76);
            this.connectionStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Size = new System.Drawing.Size(126, 20);
            this.connectionStatus.TabIndex = 83;
            this.connectionStatus.Text = "Not connected";
            // 
            // button_ConnectToLaser
            // 
            this.button_ConnectToLaser.Enabled = false;
            this.button_ConnectToLaser.Location = new System.Drawing.Point(177, 70);
            this.button_ConnectToLaser.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button_ConnectToLaser.Name = "button_ConnectToLaser";
            this.button_ConnectToLaser.Size = new System.Drawing.Size(127, 29);
            this.button_ConnectToLaser.TabIndex = 82;
            this.button_ConnectToLaser.Text = "Connect";
            this.button_ConnectToLaser.UseVisualStyleBackColor = true;
            this.button_ConnectToLaser.Click += new System.EventHandler(this.button_connectToLaser_Click);
            // 
            // errorAndon
            // 
            this.errorAndon.BackColor = System.Drawing.Color.White;
            this.errorAndon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorAndon.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.errorAndon.Location = new System.Drawing.Point(179, 689);
            this.errorAndon.Name = "errorAndon";
            this.errorAndon.Size = new System.Drawing.Size(100, 60);
            this.errorAndon.TabIndex = 224;
            this.errorAndon.Text = "Error";
            this.errorAndon.UseVisualStyleBackColor = false;
            // 
            // button_clearError
            // 
            this.button_clearError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_clearError.Location = new System.Drawing.Point(179, 756);
            this.button_clearError.Margin = new System.Windows.Forms.Padding(4);
            this.button_clearError.Name = "button_clearError";
            this.button_clearError.Size = new System.Drawing.Size(100, 26);
            this.button_clearError.TabIndex = 225;
            this.button_clearError.Text = "Clear error";
            this.button_clearError.UseVisualStyleBackColor = true;
            this.button_clearError.Click += new System.EventHandler(this.button_clearError_Click);
            // 
            // laserStatusAndon
            // 
            this.laserStatusAndon.BackColor = System.Drawing.SystemColors.Control;
            this.laserStatusAndon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laserStatusAndon.Location = new System.Drawing.Point(399, 689);
            this.laserStatusAndon.Margin = new System.Windows.Forms.Padding(4);
            this.laserStatusAndon.MaximumSize = new System.Drawing.Size(100, 60);
            this.laserStatusAndon.MinimumSize = new System.Drawing.Size(100, 60);
            this.laserStatusAndon.Name = "laserStatusAndon";
            this.laserStatusAndon.Size = new System.Drawing.Size(100, 60);
            this.laserStatusAndon.TabIndex = 226;
            this.laserStatusAndon.Text = ".....";
            this.laserStatusAndon.UseVisualStyleBackColor = false;
            // 
            // fixedWLshutterAndon
            // 
            this.fixedWLshutterAndon.BackColor = System.Drawing.SystemColors.Control;
            this.fixedWLshutterAndon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixedWLshutterAndon.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fixedWLshutterAndon.Location = new System.Drawing.Point(619, 689);
            this.fixedWLshutterAndon.Margin = new System.Windows.Forms.Padding(4);
            this.fixedWLshutterAndon.MaximumSize = new System.Drawing.Size(100, 60);
            this.fixedWLshutterAndon.MinimumSize = new System.Drawing.Size(100, 60);
            this.fixedWLshutterAndon.Name = "fixedWLshutterAndon";
            this.fixedWLshutterAndon.Size = new System.Drawing.Size(100, 60);
            this.fixedWLshutterAndon.TabIndex = 227;
            this.fixedWLshutterAndon.Text = ".....";
            this.fixedWLshutterAndon.UseVisualStyleBackColor = false;
            // 
            // tunableWLshutterAndon
            // 
            this.tunableWLshutterAndon.BackColor = System.Drawing.SystemColors.Control;
            this.tunableWLshutterAndon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tunableWLshutterAndon.Location = new System.Drawing.Point(509, 689);
            this.tunableWLshutterAndon.Margin = new System.Windows.Forms.Padding(4);
            this.tunableWLshutterAndon.MaximumSize = new System.Drawing.Size(100, 60);
            this.tunableWLshutterAndon.MinimumSize = new System.Drawing.Size(100, 60);
            this.tunableWLshutterAndon.Name = "tunableWLshutterAndon";
            this.tunableWLshutterAndon.Size = new System.Drawing.Size(100, 60);
            this.tunableWLshutterAndon.TabIndex = 228;
            this.tunableWLshutterAndon.Text = ".....";
            this.tunableWLshutterAndon.UseVisualStyleBackColor = false;
            // 
            // pmtOnOffIndicator
            // 
            this.pmtOnOffIndicator.BackColor = System.Drawing.SystemColors.Control;
            this.pmtOnOffIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pmtOnOffIndicator.Location = new System.Drawing.Point(289, 689);
            this.pmtOnOffIndicator.Margin = new System.Windows.Forms.Padding(4);
            this.pmtOnOffIndicator.MaximumSize = new System.Drawing.Size(100, 60);
            this.pmtOnOffIndicator.MinimumSize = new System.Drawing.Size(100, 60);
            this.pmtOnOffIndicator.Name = "pmtOnOffIndicator";
            this.pmtOnOffIndicator.Size = new System.Drawing.Size(100, 60);
            this.pmtOnOffIndicator.TabIndex = 223;
            this.pmtOnOffIndicator.Text = ".....";
            this.pmtOnOffIndicator.UseVisualStyleBackColor = false;
            // 
            // userMessages
            // 
            this.userMessages.BackColor = System.Drawing.SystemColors.MenuBar;
            this.userMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userMessages.FormattingEnabled = true;
            this.userMessages.ItemHeight = 16;
            this.userMessages.Location = new System.Drawing.Point(747, 689);
            this.userMessages.Name = "userMessages";
            this.userMessages.ScrollAlwaysVisible = true;
            this.userMessages.Size = new System.Drawing.Size(469, 84);
            this.userMessages.TabIndex = 229;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 800);
            this.Controls.Add(this.userMessages);
            this.Controls.Add(this.errorAndon);
            this.Controls.Add(this.button_clearError);
            this.Controls.Add(this.laserStatusAndon);
            this.Controls.Add(this.fixedWLshutterAndon);
            this.Controls.Add(this.tunableWLshutterAndon);
            this.Controls.Add(this.pmtOnOffIndicator);
            this.Controls.Add(this.setGreenPower);
            this.Controls.Add(this.greenPowerNow);
            this.Controls.Add(this.greenPowerToSet);
            this.Controls.Add(this.laserStatusMessage);
            this.Controls.Add(this.wavelengthToSet);
            this.Controls.Add(this.check_controlGreenPower);
            this.Controls.Add(this.label_greenPowerNow);
            this.Controls.Add(this.dispersion_objectiveChoice);
            this.Controls.Add(this.button_populateObjectiveList);
            this.Controls.Add(this.label_dispersionCorrection);
            this.Controls.Add(this.button_chooseObjective);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.pumpLaser2Temperature);
            this.Controls.Add(this.pumpLaser2TemperatureLabel);
            this.Controls.Add(this.pumpLaser2CurrentLabel);
            this.Controls.Add(this.pumpLaser2Current);
            this.Controls.Add(this.alignTunableLambda);
            this.Controls.Add(this.alignFixedLambda);
            this.Controls.Add(this.alignTunableLabel);
            this.Controls.Add(this.alignFixedLabel);
            this.Controls.Add(this.button_alignOFF_FixedWL);
            this.Controls.Add(this.button_align_FixedWL);
            this.Controls.Add(this.button_alignOFF_TunableWL);
            this.Controls.Add(this.button_align_TunableWL);
            this.Controls.Add(this.label162);
            this.Controls.Add(this.fixedLambdaLabel);
            this.Controls.Add(this.pumpLaserTemperature);
            this.Controls.Add(this.pumpLaserTemperatureLabel);
            this.Controls.Add(this.pumpLaserCurrentLabel);
            this.Controls.Add(this.pumpLaserHours);
            this.Controls.Add(this.pumpLaserHoursLabel);
            this.Controls.Add(this.pumpLaserCurrent);
            this.Controls.Add(this.laserTypeLabel);
            this.Controls.Add(this.label163);
            this.Controls.Add(this.button_disconnectPumpOn);
            this.Controls.Add(this.physicalKeyStatus);
            this.Controls.Add(this.label164);
            this.Controls.Add(this.warmupPctLabel);
            this.Controls.Add(this.pumpLaserOnStatus);
            this.Controls.Add(this.readyToCollect);
            this.Controls.Add(this.label138);
            this.Controls.Add(this.label141);
            this.Controls.Add(this.fixedWavelengthLabel);
            this.Controls.Add(this.button_shutDownLaser);
            this.Controls.Add(this.button_closeShutter_TunableWL);
            this.Controls.Add(this.shutterStatus_TunableWL);
            this.Controls.Add(this.button_openShutter_TunableWL);
            this.Controls.Add(this.label148);
            this.Controls.Add(this.errorCode);
            this.Controls.Add(this.label147);
            this.Controls.Add(this.power);
            this.Controls.Add(this.label145);
            this.Controls.Add(this.modelockStatus);
            this.Controls.Add(this.wavelengthStatus);
            this.Controls.Add(this.modelockLabel);
            this.Controls.Add(this.label140);
            this.Controls.Add(this.blinker);
            this.Controls.Add(this.label139);
            this.Controls.Add(this.button_closeShutter_FixedWL);
            this.Controls.Add(this.shutterStatus_FixedWL);
            this.Controls.Add(this.button_openShutter_FixedWL);
            this.Controls.Add(this.button_pumpLaserOff);
            this.Controls.Add(this.button_pumpLaserOn);
            this.Controls.Add(this.button_setWavelength);
            this.Controls.Add(this.warmupStatus);
            this.Controls.Add(this.connectionStatus);
            this.Controls.Add(this.button_ConnectToLaser);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GUI";
            this.Text = "Laser Control and Monitor";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setGreenPower;
        private System.Windows.Forms.Label greenPowerNow;
        private System.Windows.Forms.TextBox greenPowerToSet;
        private System.Windows.Forms.TextBox laserStatusMessage;
        private System.Windows.Forms.TextBox wavelengthToSet;
        private System.Windows.Forms.CheckBox check_controlGreenPower;
        private System.Windows.Forms.Label label_greenPowerNow;
        private System.Windows.Forms.ComboBox dispersion_objectiveChoice;
        private System.Windows.Forms.Button button_populateObjectiveList;
        private System.Windows.Forms.Label label_dispersionCorrection;
        private System.Windows.Forms.Button button_chooseObjective;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label pumpLaser2Temperature;
        private System.Windows.Forms.Label pumpLaser2TemperatureLabel;
        private System.Windows.Forms.Label pumpLaser2CurrentLabel;
        private System.Windows.Forms.Label pumpLaser2Current;
        private System.Windows.Forms.Label alignTunableLambda;
        private System.Windows.Forms.Label alignFixedLambda;
        private System.Windows.Forms.Label alignTunableLabel;
        private System.Windows.Forms.Label alignFixedLabel;
        private System.Windows.Forms.Button button_alignOFF_FixedWL;
        private System.Windows.Forms.Button button_align_FixedWL;
        private System.Windows.Forms.Button button_alignOFF_TunableWL;
        private System.Windows.Forms.Button button_align_TunableWL;
        private System.Windows.Forms.Label label162;
        private System.Windows.Forms.Label fixedLambdaLabel;
        private System.Windows.Forms.Label pumpLaserTemperature;
        private System.Windows.Forms.Label pumpLaserTemperatureLabel;
        private System.Windows.Forms.Label pumpLaserCurrentLabel;
        private System.Windows.Forms.Label pumpLaserHours;
        private System.Windows.Forms.Label pumpLaserHoursLabel;
        private System.Windows.Forms.Label pumpLaserCurrent;
        private System.Windows.Forms.Label laserTypeLabel;
        private System.Windows.Forms.Label label163;
        private System.Windows.Forms.Button button_disconnectPumpOn;
        private System.Windows.Forms.Label physicalKeyStatus;
        private System.Windows.Forms.Label label164;
        private System.Windows.Forms.Label warmupPctLabel;
        private System.Windows.Forms.Label pumpLaserOnStatus;
        private System.Windows.Forms.Button readyToCollect;
        private System.Windows.Forms.Label label138;
        private System.Windows.Forms.Label label141;
        private System.Windows.Forms.Label fixedWavelengthLabel;
        private System.Windows.Forms.Button button_shutDownLaser;
        private System.Windows.Forms.Button button_closeShutter_TunableWL;
        private System.Windows.Forms.Label shutterStatus_TunableWL;
        private System.Windows.Forms.Button button_openShutter_TunableWL;
        private System.Windows.Forms.Label label148;
        private System.Windows.Forms.Label errorCode;
        private System.Windows.Forms.Label label147;
        private System.Windows.Forms.Label power;
        private System.Windows.Forms.Label label145;
        private System.Windows.Forms.Label modelockStatus;
        private System.Windows.Forms.Label wavelengthStatus;
        private System.Windows.Forms.Label modelockLabel;
        private System.Windows.Forms.Label label140;
        private System.Windows.Forms.Button blinker;
        private System.Windows.Forms.Label label139;
        private System.Windows.Forms.Button button_closeShutter_FixedWL;
        private System.Windows.Forms.Label shutterStatus_FixedWL;
        private System.Windows.Forms.Button button_openShutter_FixedWL;
        private System.Windows.Forms.Button button_pumpLaserOff;
        private System.Windows.Forms.Button button_pumpLaserOn;
        private System.Windows.Forms.Button button_setWavelength;
        private System.Windows.Forms.Label warmupStatus;
        private System.Windows.Forms.Label connectionStatus;
        private System.Windows.Forms.Button button_ConnectToLaser;
        public System.Windows.Forms.Button errorAndon;
        public System.Windows.Forms.Button button_clearError;
        public System.Windows.Forms.Button laserStatusAndon;
        public System.Windows.Forms.Button fixedWLshutterAndon;
        public System.Windows.Forms.Button tunableWLshutterAndon;
        public System.Windows.Forms.Button pmtOnOffIndicator;
        private System.Windows.Forms.ListBox userMessages;

    }
}

