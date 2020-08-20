using System;
using System.Collections.Generic;
using System.Text;
using F4SharedMem;
using System.Threading;
using System.Globalization;
namespace netSocket
{
    class Falcon
    {
        static Reader _sharedMemReader = new Reader();
        static FlightData _lastFlightData;
        static private FlightData ReadSharedMem()
        {
            return _lastFlightData = _sharedMemReader.GetCurrentData();
        }
    
        public double sendPitch()
        {  
            while (true)
            {
                Thread.Sleep(50);
                if (ReadSharedMem() != null)
                {
                   return _lastFlightData.pitch;
                    
                }
            }

        }
        public double sendRoll()
        {
            while (true)
            {
                Thread.Sleep(50);
                if (ReadSharedMem() != null)
                {
                    return -1 * _lastFlightData.roll;
                    
                }
            }

        }
        public double sendYaw()
        {
            while (true)
            {
                Thread.Sleep(50);
                if (ReadSharedMem() != null)
                {
                  return _lastFlightData.yaw;
                    
                }
            }

        }
    }
}
