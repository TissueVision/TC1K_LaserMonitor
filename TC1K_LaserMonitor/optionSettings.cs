
namespace TC1K_LaserMonitor
{

    public class optionSettings
    {
        // laser type
        public string laserPort { get; set; }
        public string laserType { get; set; } // options are: MaiTai, InsightX3, DiscoveryNX
        public bool fakeOutLaser { get; set; }

        // gui vals
        public int wavelength { get; set; }

        // fluctuation monitoring
        public int T_sample_s { get; set; }
        public double laserPowerMinFrac { get; set; }
        public double laserPowerMaxFrac { get; set; }
        public bool terminateOrchestratorOnDrift { get; set; }

        // misc monitoring
        public bool enableLaserWatchdog { get; set; }
        public int laserGUIUpdateInterval_ms { get; set; } // integrated laser GUI
        public double laserMinOKPower_W { get; set; }
        public int maxQueryErrors { get; set; } // integrated laser GUI
        public int maxConsecutiveQueryErrors { get; set; } // integrated laser GUI
        public int maxShutterErrors { get; set; } // integrated laser GUI
        public int maxConsecutiveShutterErrors { get; set; } // integrated laser GUI
        public int maxModelockErrors { get; set; } // integrated laser GUI
        public int maxConsecutiveModelockErrors { get; set; } // integrated laser GUI
    }


}
