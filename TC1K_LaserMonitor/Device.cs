
namespace TC1K_LaserMonitor
{

    public class Device
    {

        public bool fakeOut = false;
        public bool ready = false;
        public bool commsOK = false; // this matters for cases when starting comms is different from setting ready, i.e. it requires additional initialization
        public bool stopOrchestratorOnDeviceFatalError = true;
        public Reporter Rep = new Reporter();


        // constructor only
        public Device()
        {
        }


        public virtual TaskEnd initialize()
        {
            return TaskEnd.GeneralError;
        }



        public virtual void closeAll()
        {
        }


        public virtual TaskEnd stop()
        {
            return TaskEnd.GeneralError;
        }


        // emergency stop includes stop
        public virtual TaskEnd eStop()
        {
            if (commsOK)
            {
                stop();
            }
            setNotReady();
            return TaskEnd.GeneralError;
        }


        public virtual void setReady()
        {
            commsOK = true; // it may be set elsewhere too, but if it's ready then comms must be OK so this saves code
            ready = true;
        }


        public virtual void setNotReady()
        {
            commsOK = false;
            ready = false;
        }


        public void fatalError(string errorMsg, string auxMessage)
        {
            setNotReady();
            if (stopOrchestratorOnDeviceFatalError)
            {
                Rep.CancelAll(errorMsg);
            }
        }








    }


}


