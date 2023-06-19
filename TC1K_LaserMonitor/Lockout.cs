using System;
using System.Collections.Generic;


namespace TC1K_LaserMonitor
{

    // 
    public class Lockout
    {
        public string name = "no-name device";
        public bool locked = false;
        public Reporter Rep = new Reporter();
        public List<DateTime> buttonLockRequestTimes = new List<DateTime>();
        public int lockTimeout_ms = 5000;
        public int pollingInterval_ms = 200;
        public bool latestCommandOnly = false;

        public enum LockoutReturn
        {
            OK, Error, CommTimeout, BumpedFromLock
        }


        // constructor only
        public Lockout()
        {
        }



        public LockoutReturn requestLock(bool calledByUpdateGUI)
        {
            //if (!fakeOut)
            //{
            //    if (!_serialPort.IsOpen)
            //    {
            //        return (LaserReturnCode.CommError);
            //    }
            //}

            // if this request is coming from updating the gui, and there is a button request in line, it doesn't even get in line to wait
            bool buttonsAreWaiting = (buttonLockRequestTimes.Count > 0);
            if (calledByUpdateGUI && ( buttonsAreWaiting || locked)) // GUI only updates if nothing else is waiting and it's not locked
            {
                return (LockoutReturn.BumpedFromLock);
            }

            // if there are already locks waiting, overwrite if it is newer
            DateTime thisButtonRequest = DateTime.Now;
            if (latestCommandOnly && buttonsAreWaiting)
            {
                // if a command is waiting but not yet executed, bump it out of the way
                if ((buttonLockRequestTimes.Count > 1))
                {
                    Rep.Post("Two buttons at once?!?! this wasn't supposed to happen!", repLevel.error, null);
                }
                if (thisButtonRequest < buttonLockRequestTimes[0])
                {
                    //Rep.Post("It got bumped for arriving later!", repLevel.error, null);
                    return (LockoutReturn.BumpedFromLock);
                }
                buttonLockRequestTimes.Clear();
            }

            // if it's not locked, then take the lock
            if (!locked)
            {
                locked = true;
                return (LockoutReturn.OK);
            }

            // wait until lock is available
            buttonLockRequestTimes.Add(thisButtonRequest);
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            while (locked)
            {
                System.Threading.Thread.Sleep(pollingInterval_ms);
                if (!buttonLockRequestTimes.Exists(x => x == thisButtonRequest)) // if it has been cancelled by another request
                {
                    return (LockoutReturn.BumpedFromLock);
                }
                if (stopWatch.ElapsedMilliseconds > (lockTimeout_ms))
                {
                    buttonLockRequestTimes.Remove(thisButtonRequest);
                    Rep.Post("Timeout while requesting lockout for " + name, repLevel.details, null);
                    return (LockoutReturn.CommTimeout);
                }
            }
            // if it succeeded
            locked = true;
            buttonLockRequestTimes.Remove(thisButtonRequest);
            return (LockoutReturn.OK);
        }


        public void releaseLock()
        {
            locked = false;
        }


        // in some cases, to keep the code from breaking, the lockout needs to return a *laser code, instead
        public LaserReturnCode lockoutToLaserCode(LockoutReturn lockoutCode)
        {
            if (lockoutCode == LockoutReturn.OK)
            {
                return LaserReturnCode.OK;
            }
            else if (lockoutCode == LockoutReturn.Error)
            {
                return LaserReturnCode.MiscError;
            }
            else if (lockoutCode == LockoutReturn.CommTimeout)
            {
                return LaserReturnCode.CommTimeout;
            }
            else if (lockoutCode == LockoutReturn.BumpedFromLock)
            {
                return LaserReturnCode.BumpedFromLock;
            }
            else
            {
                Rep.Post("The programmer didn't handle a case in lockoutToLaserCode!",repLevel.error,null);
                return LaserReturnCode.MiscError;
            }
        }


    }
}
