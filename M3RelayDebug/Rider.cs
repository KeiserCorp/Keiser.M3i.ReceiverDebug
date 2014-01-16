using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace M3RelayDebug
{
    class Rider
    {
        public UInt16? rpm, hr, power, kcal, clock;
        public Int16? rssi;
        public byte[] uuid = new byte[6];
        public int updates;
        public Stopwatch timeFromStart, timeFromUpdate;
        public TimeSpan elapsedAtLastUpdate;

        public Rider(byte[] idArray)
        {
            uuid = idArray;
            rpm = hr = power = kcal = clock = null;
            rssi = null;
            updates = 0;
            timeFromStart = Stopwatch.StartNew();
            timeFromUpdate = Stopwatch.StartNew();
        }

        public bool uuidEquals(byte[] otherUuid)
        {
            return (
                otherUuid[0] == uuid[0] &&
                otherUuid[1] == uuid[1] &&
                otherUuid[2] == uuid[2] &&
                otherUuid[3] == uuid[3] &&
                otherUuid[4] == uuid[4] &&
                otherUuid[5] == uuid[5]
                );
        }

        public void stop()
        {
            timeFromStart.Stop();
            timeFromUpdate.Stop();
        }

        public void update(UInt16 _rpm, UInt16 _hr, UInt16 _power, UInt16? _kcal = null, UInt16? _clock = null, Int16? _rssi = null)
        {
            rpm = _rpm;
            hr = _hr;
            power = _power;
            kcal = _kcal;
            clock = _clock;
            rssi = _rssi;
            updates++;
            elapsedAtLastUpdate = timeFromStart.Elapsed;
            timeFromUpdate.Reset();
            timeFromUpdate.Start();
        }

        public string timeSinceUpdate()
        {
            int elapsed = timeFromUpdate.Elapsed.Seconds;
            return (elapsed > 3) ? elapsed.ToString() : "";
        }

        public string getUuidString()
        {
            string uuidString = "";
            int count = 0;
            foreach (Byte segment in uuid)
            {
                uuidString += string.Format("{0:X2}", segment);
                if (count++ < 5)
                    uuidString += ":";
            }
            return uuidString;
        }

        public string getString()
        {
            return string.Format("{0,3:} {1,3:} {2,4:} {3,4:} {4,5:} {5,3} {6,17} {7,3}", rpm, hr, power, kcal, clock, rssi, getUuidString(), timeSinceUpdate());
        }
    }
}
