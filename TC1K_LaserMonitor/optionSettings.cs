
namespace TC1K_LaserMonitor
{

    public class optionSettings
    {
        // laser type
        public string laserPort { get; set; }
        public string laserType { get; set; } // options are: MaiTai, InsightX3, DiscoveryNX
        public bool fakeOutLaser { get; set; }

        // misc
        public int wavelength { get; set; }
        public int laserGUIUpdateInterval_ms { get; set; } // integrated laser GUI
        public bool enableLaserWatchdog { get; set; }

        // fluctuation monitoring
        public int T_sample_s { get; set; }
        public double laserPowerMinFrac { get; set; }
        public double laserPowerMaxFrac { get; set; }
        public bool terminateOrchestratorOnDrift { get; set; }
        public bool checkForDrift { get; set; }
        public int maxModelockErrors { get; set; } // integrated laser GUI
        public double maxModelockLossTime_s { get; set; }
    }


}
