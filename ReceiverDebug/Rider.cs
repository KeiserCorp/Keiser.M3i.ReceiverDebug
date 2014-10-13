using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Keiser.M3i.ReceiverDebug
{
    class Rider
    {
        // API Independent
        public int updates;
        public Stopwatch timeFromStart, timeFromUpdate;
        public TimeSpan elapsedAtLastUpdate;

        // API Versions: 1.0, 0.8
        public UInt16? rpm;
        public UInt16? hr;
        public UInt16? power;
        public UInt16? kcal;
        public UInt16? clock;
        public Int16? rssi;
        public byte[] uuid = new byte[6];

        // API Versions: 1.0
        public UInt16? major;
        public UInt16? minor;
        public UInt16? id;
        public UInt16? interval;
        public UInt16? trip;
        // API Versions: 1.1
        public UInt16? gear;


        public Rider(byte[] idArray)
        {
            uuid = idArray;
            rpm = hr = power = kcal = clock = gear = null;
            rssi = null;
            updates = 0;
            timeFromStart = Stopwatch.StartNew();
            timeFromUpdate = Stopwatch.StartNew();
        }

        public Rider(UInt16 id)
        {
            this.id = id;
            rpm = hr = power = kcal = clock = gear = null;
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

        public bool idEquals(UInt16 otherId)
        {
            return (
                otherId == id
                );
        }

        public void stop()
        {
            timeFromStart.Stop();
            timeFromUpdate.Stop();
        }

        public void update_v08(UInt16 _rpm, UInt16 _hr, UInt16 _power, UInt16? _kcal = null, UInt16? _clock = null, Int16? _rssi = null)
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

        public void update_v10(byte[] _uuid, UInt16 _major, UInt16 _minor, UInt16 _rpm, UInt16 _hr, UInt16 _power, UInt16 _interval, UInt16 _kcal, UInt16 _clock, UInt16 _trip, Int16 _rssi, UInt16 _gear)
        {
            uuid = _uuid;
            major = _major;
            minor = _minor;
            rpm = _rpm;
            hr = _hr;
            power = _power;
            interval = _interval;
            kcal = _kcal;
            clock = _clock;
            trip = _trip;
            rssi = _rssi;
            gear = _gear;
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

        public string getString_v08()
        {
            return string.Format("{0,3:} {1,3:} {2,4:} {3,4:} {4,5:} {5,3} {6,17} {7,3}", rpm, hr, power, kcal, clock, rssi, getUuidString(), timeSinceUpdate());
        }

        public string getString_v10()
        {
            return string.Format("{0,3:} {1,4:} {2,3:} {3,4:} {4,4:} {5,6:} {6,5:} {7,4:} {8,4} {9,2:} {10,2:} {11,18} {12,3}", id, rpm, hr, power, interval, clock, kcal, trip, rssi, major, gear, getUuidString(), timeSinceUpdate());
        }
    }
}
