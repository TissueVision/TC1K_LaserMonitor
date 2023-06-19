
namespace TC1K_LaserMonitor
{

    public class optionSettings
    {

        // Show tabs
        public bool showAuxButtons { get; set; }
        public bool showManyScansTab { get; set; }
        public bool showEng1Tab { get; set; }
        public bool showSurfaceFindingTab { get; set; }
        public bool showNovoUItabs { get; set; }
        public bool showLegacyTabs { get; set; }
        public bool showSASPtab { get; set; }


        // Misc
        public int yShakeoffDist_mm { get; set; } // when not in section capture mode, it shakes the slice off of the vibratome by this dist (in stage units)
        public int pifocUpdateInterval_ms { get; set; }
        public bool enableTextLog { get; set; }
        public bool findSurfaceForEachWavelength { get; set; } // this only matters if useMultipleWavelengthScan     
        public int ChannelForSurfaceFinding { get; set; }
        public double pifocZDefault_um { get; set; }
        public bool largeSampleForToronto { get; set; } // special feature for SickKids Toronto
        public bool useDropDispenser { get; set; }
        public int nDropsDispense { get; set; }
        public int nDropsAspirate { get; set; }
        public bool drySlide { get; set; }
        public int slideDryingTime_s { get; set; }
        public string connectOKcolor { get; set; }
        public string connectErrorColor { get; set; }
        public string laserLogFilename { get; set; }
        public string flatFieldFolder { get; set; }
        public int auxCamExposure { get; set; }
        public bool logTileTiming { get; set; }
        public bool reportVivaceComms { get; set; }
        public bool attemptMultipleTransfers { get; set; }
        public bool reportGripperDetails { get; set; }
        public bool reportStageDetails { get; set; }



        // Laser
        public bool enableLaserWatchdog { get; set; }
        public bool useIntegratedLaserGUI { get; set; } // enables tab page with laser GUI
        public bool monitorLaser { get; set; } // pauses if the laser reports error conditions
        public int laserGUIUpdateInterval_ms { get; set; } // integrated laser GUI
        public double laserMinOKPower_W { get; set; }
        public int maxQueryErrors { get; set; } // integrated laser GUI
        public int maxConsecutiveQueryErrors { get; set; } // integrated laser GUI
        public int maxShutterErrors { get; set; } // integrated laser GUI
        public int maxConsecutiveShutterErrors { get; set; } // integrated laser GUI
        public int maxModelockErrors { get; set; } // integrated laser GUI
        public int maxConsecutiveModelockErrors { get; set; } // integrated laser GUI


        // Slice capture options
        public bool sliceCaptureConcurrently { get; set; }
        public bool useDiagSlideCam { get; set; }
        public bool useDiagBeltCam { get; set; }
        public int beltCamExposure { get; set; }
        public int slideCamExposure { get; set; }
        public int xZeroPoint_px { get; set; }
        public double maxDistFromCenter_mm { get; set; }
        public int nPixelsToErodeDilate { get; set; }
        public int blowoffTime_ms { get; set; }
        public double maxSliceSearchTime_s { get; set; }
        public double maxMarkerAngle { get; set; }
        public double minSepRatio { get; set; }
        public double maxSepRatio { get; set; }
        public double minMarkerAreaRatio { get; set; }
        public double maxMarkerAreaRatio { get; set; }
        public bool pauseOnSCfailure { get; set; }
        public short pauseOnNConsecutiveFailures { get; set; }
        public double pauseOnCumuFailureFraction { get; set; } // in mm/s at edge, but it may be miscalibrated
        public short pauseOnMinCumuFailures { get; set; }
        public int nTidyUpAbove { get; set; }
        public int nTidyUpBelow { get; set; }
        public bool tidyUpReturned { get; set; }
        public double tidyUpOffset { get; set; } // end of tidy up, relative to nominal slide-in-slot position (negative)
        public bool everyOtherSlide { get; set; }
        public double robotSlowdownFactor { get; set; }
        public bool suppressHCclosure { get; set; }
        public int blowoffDelayLF_ms { get; set; }
        public bool blowoffLF { get; set; }
        public int detectZoneXMin_px { get; set; }
        public int detectZoneXMax_px { get; set; }
        public int detectZoneYMin_px { get; set; }
        public int detectZoneYMax_px { get; set; }
        public double overscoop_mm { get; set; }
        public double underscoop_mm { get; set; }
        public double slideReturnGapY_mm { get; set; } // postive number.  make it more negative to have slide hang out further
        public double slideReturnGapZ_mm { get; set; }
        public double transferUpSpeed_mmps { get; set; } // robot speed when transferring slide from belt to slide, moving up
        public double transferDownSpeed_mmps { get; set; } // robot speed when transferring slide from belt to slide, moving down
        public double displaceBeltDist_mm { get; set; } // how many mm to displace the belt during transfer, from its equilibrium point
        public double waypointToBeltDist_mm { get; set; } // how far the waypoint is from the belt
        public double aspirationToBeltDist_mm { get; set; }
        public CameraColorChannel markerDetectionChannel { get; set; }
        public bool removeSliceSlowlyFromBath { get; set; }
        public double removeSlowSpeed_mmps { get; set; }
        public double removeSlowAccel_mmpss { get; set; }
        public double removeSlowDist_mm { get; set; }



        // trinamic actuator speeds / accels
        public double hcSpeed_dps { get; set; }
        public double hcAccel_dpss { get; set; }
        public double hcHomingSpeed_dps { get; set; }
        public double zStageSpeed_mmps { get; set; }
        public double zStageAccel_mmpss { get; set; }
        public double immLensSpeed_dps { get; set; }
        public double immLensAccel_dpss { get; set; }
        public double beltAccel2SpeedRatio { get; set; }


        // PI stage parameters
        public int x_P_val { get; set; }
        public int x_I_val { get; set; }
        public int x_D_val { get; set; }
        public int x_I_limit_val { get; set; }
        public double x_SettlingWindow_um { get; set; }
        public double x_SettlingTime_s { get; set; }
        public double x_speed_mmps { get; set; }
        public double x_accel_mmpss { get; set; }
        public int y_P_val { get; set; }
        public int y_I_val { get; set; }
        public int y_D_val { get; set; }
        public int y_I_limit_val { get; set; }
        public double y_SettlingWindow_um { get; set; }
        public double y_SettlingTime_s { get; set; }
        public double y_speed_mmps { get; set; }
        public double y_accel_mmpss { get; set; }



        // Fake-outs
        public bool fakeOutXstage { get; set; }
        public bool fakeOutYstage { get; set; }
        public bool fakeOutZstage { get; set; }
        public bool fakeOutVivace { get; set; }
        public bool fakeOutTrinamic { get; set; }
        public bool fakeOutLCVR { get; set; }
        public bool fakeOutVibratome { get; set; }
        public bool fakeOutPiezo { get; set; }
        public bool fakeOutPMTs { get; set; }
        public bool fakeOutRobot { get; set; }
        public bool fakeOutIO { get; set; }
        public bool fakeOutBeltCam { get; set; }
        public bool fakeOutSlideCam { get; set; }
        public bool fakeOutLaser { get; set; } // in this case, laser should be set to DiscoveryNX
        public bool fakeOut2aryDriver { get; set; }
        public bool fakeOutNikonZ7 { get; set; }
        public bool fakeOutGripper { get; set; }
        public bool fakeOutImmLens { get; set; }
        public bool fakeOutAuxCam { get; set; }
        public bool fakeOutBelt { get; set; }
        public bool fakeOutHC1 { get; set; }
        public bool fakeOutHC2 { get; set; }
        public bool fakeOutMonitorCam { get; set; }
        public bool fakeOutSASPcam { get; set; }
        public bool fakeOutAllHardware { get; set; }
        public bool uniqueNoiseForVivaceFakeout { get; set; }
        public bool disregardMissingSettingsFiles { get; set; } // this includes protocol, too

    }

    public enum CameraColorChannel
    {
        Red = 1,
        Green = 2,
        Blue = 3
    }


}
