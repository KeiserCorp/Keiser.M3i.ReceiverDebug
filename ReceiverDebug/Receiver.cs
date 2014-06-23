using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Keiser.M3i.ReceiverDebug
{
    class Receiver
    {
        private Thread _Thread;
        private volatile Boolean _KeepWorking;
        private Logger logger;
        public bool running = false;

        public string ipAddress = "";
        public UInt16 ipPort;
        public string apiVersionStr = "";
        public int apiVersionInt;
        public string ipEndPointString = "";
        public List<Rider> riders = new List<Rider>();

        public Receiver(Logger _logger)
        {
            logger = _logger;
        }

        public void start()
        {
            riders.Clear();
            _Thread = new Thread(worker);
            logger.start(riders, apiVersionInt, ipEndPointString);
            _KeepWorking = running = true;
            _Thread.Start();
        }

        public void stop()
        {
            logger.stop();
            _KeepWorking = running = false;
        }

        private void worker()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, ipPort);
            EndPoint remoteEndPoint = (EndPoint)ipEndPoint;
            socket.Bind(ipEndPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(ipAddress)));
            byte[] receivedData = new byte[1024];
            while (_KeepWorking)
            {
                if (socket.Poll(200000, SelectMode.SelectRead))
                {
                    //socket.Receive(receivedData);
                    socket.ReceiveFrom(receivedData, ref remoteEndPoint);
                    ipEndPointString = remoteEndPoint.ToString();
                    switch (apiVersionStr)
                    {
                        case "0.8":
                            apiVersionInt = 8;
                            parser_v08(receivedData);
                            break;
                        case "1.0+":
                            parser_v10p(receivedData);
                            break;
                    }
                    receivedData = new byte[1024];
                }
            }
            socket.Close();
        }

        private struct configSettings_v08
        {
            public bool uuidLong;
            public bool rpmLong;
            public bool hrLong;
            public bool kcalSend;
            public bool clockSend;
            public bool rssiSend;

            public int rpmOffset()
            {
                return (uuidLong) ? 6 : 3;
            }

            public int hrOffset()
            {
                int returnOffset = rpmOffset();
                returnOffset += (rpmLong) ? 2 : 1;
                return returnOffset;
            }

            public int powerOffset()
            {
                int returnOffset = hrOffset();
                returnOffset += (hrLong) ? 2 : 1;
                return returnOffset;
            }

            public int kcalOffset()
            {
                return powerOffset() + 2;
            }

            public int clockOffset()
            {
                int returnOffset = kcalOffset();
                returnOffset += (kcalSend) ? 2 : 0;
                return returnOffset;
            }

            public int rssiOffset()
            {
                int returnOffset = clockOffset();
                returnOffset += (clockSend) ? 2 : 0;
                return returnOffset;
            }
        }

        private struct configSettings_v10
        {
            public bool imperialUnits;
            public bool rssiSend;
            public bool intervalSend;
            public bool versionSend;
            public bool uuidSend;

            public int idOffset()
            {
                return 0;
            }

            public int uuidOffset()
            {
                return 1;
            }

            public int majorOffset()
            {
                return uuidOffset() + ((uuidSend) ? 6 : 0);
            }

            public int minorOffset()
            {
                return majorOffset() + ((versionSend) ? 1 : 0);
            }

            public int rpmOffset()
            {
                return minorOffset() + ((versionSend) ? 1 : 0);
            }

            public int hrOffset()
            {
                return rpmOffset() + 1;
            }

            public int powerOffset()
            {
                return hrOffset() + 1;
            }

            public int intervalOffset()
            {
                return powerOffset() + 2;
            }

            public int kcalOffset()
            {
                return intervalOffset() + ((intervalSend) ? 1 : 0);
            }

            public int clockOffset()
            {
                return kcalOffset() + ((intervalSend) ? 2 : 0);
            }

            public int tripOffset()
            {
                return clockOffset() + ((intervalSend) ? 2 : 0);
            }

            public int rssiOffset()
            {
                return tripOffset() + ((intervalSend) ? 1 : 0);
            }
        }

        private void parser_v08(byte[] receivedData)
        {
            byte configFlags = receivedData[0];
            configSettings_v08 configSettings = getConfigSettings_v08(configFlags);
            int dataSize = sizeOfData_v08(configSettings);
            for (int x = 0; x < Convert.ToUInt16((receivedData.Length - 1) / dataSize); x++)
            {
                int offset = 1 + (x * dataSize);
                if (receivedData[offset + 0] == 0 && receivedData[offset + 1] == 0 && receivedData[offset + 2] == 0)
                    continue;
                byte[] uuid = transferUUID(configSettings, receivedData, offset);
                if (!riders.Exists(y => y.uuidEquals(uuid)))
                {
                    riders.Add(new Rider(uuid));
                }
                Rider rider = riders.Find(y => y.uuidEquals(uuid));
                updateRide_v08(configSettings, receivedData, offset, rider);
            }

        }

        private void parser_v10p(byte[] receivedData)
        {
            byte apiVersion = receivedData[0];
            switch (apiVersion)
            {
                case 10:
                    apiVersionInt = 10;
                    parser_v10(receivedData);
                    break;
            }
        }

        private void parser_v10(byte[] receivedData)
        {
            byte configFlags = receivedData[1];
            configSettings_v10 configSettings = getConfigSettings_v10(configFlags);
            int dataSize = sizeOfData_v10(configSettings);
            for (int x = 0; x < Convert.ToUInt16((receivedData.Length - 2) / dataSize); x++)
            {
                int offset = 2 + (x * dataSize);
                UInt16 id = receivedData[offset];
                if (!riders.Exists(y => y.idEquals(id)))
                {
                    riders.Add(new Rider(id));
                }
                Rider rider = riders.Find(y => y.idEquals(id));
                updateRide_v10(configSettings, receivedData, offset, rider);
            }

        }

        private void updateRide_v08(configSettings_v08 configSettings, byte[] receivedData, int offset, Rider rider)
        {
            UInt16 rpm = Convert.ToUInt16((configSettings.rpmLong) ? twoByteConcat(receivedData[offset + configSettings.rpmOffset()], receivedData[offset + configSettings.rpmOffset() + 1]) / 10 : receivedData[offset + configSettings.rpmOffset()]);
            UInt16 hr = Convert.ToUInt16((configSettings.hrLong) ? twoByteConcat(receivedData[offset + configSettings.hrOffset()], receivedData[offset + configSettings.hrOffset() + 1]) / 10 : receivedData[offset + configSettings.hrOffset()]);
            UInt16 power = twoByteConcat(receivedData[offset + configSettings.powerOffset()], receivedData[offset + configSettings.powerOffset() + 1]);
            UInt16? kcal = null;
            if (configSettings.kcalSend)
            {
                kcal = twoByteConcat(receivedData[offset + configSettings.kcalOffset()], receivedData[offset + configSettings.kcalOffset() + 1]);
            }
            UInt16? clock = null;
            if (configSettings.clockSend)
            {
                clock = twoByteConcat(receivedData[offset + configSettings.clockOffset()], receivedData[offset + configSettings.clockOffset() + 1]);
            }
            Int16? rssi = null;
            if (configSettings.rssiSend)
            {
                rssi = Convert.ToInt16((sbyte)receivedData[offset + configSettings.rssiOffset()]);
            }
            rider.update_v08(rpm, hr, power, kcal, clock, rssi);
        }

        private void updateRide_v10(configSettings_v10 configSettings, byte[] receivedData, int offset, Rider rider)
        {
            byte[] uuid = (configSettings.uuidSend) ? getUUID(receivedData, configSettings.uuidOffset()) : new byte[6];
            UInt16 major =  Convert.ToUInt16((configSettings.versionSend) ? receivedData[offset + configSettings.majorOffset()] : 0);
            UInt16 minor = Convert.ToUInt16((configSettings.versionSend) ? receivedData[offset + configSettings.minorOffset()] : 0);
            UInt16 rpm = Convert.ToUInt16(receivedData[offset + configSettings.rpmOffset()]);
            UInt16 hr = Convert.ToUInt16(receivedData[offset + configSettings.hrOffset()]);
            UInt16 power = Convert.ToUInt16(twoByteConcat(receivedData[offset + configSettings.powerOffset()], receivedData[offset + configSettings.powerOffset() + 1]));
            UInt16 interval = Convert.ToUInt16((configSettings.intervalSend) ? receivedData[offset + configSettings.intervalOffset()] : 0);
            UInt16 kcal = Convert.ToUInt16((configSettings.intervalSend) ? twoByteConcat(receivedData[offset + configSettings.kcalOffset()], receivedData[offset + configSettings.kcalOffset() + 1]) : 0);
            UInt16 clock = Convert.ToUInt16((configSettings.intervalSend) ? twoByteConcat(receivedData[offset + configSettings.clockOffset()], receivedData[offset + configSettings.clockOffset() + 1]) : 0);
            UInt16 trip = Convert.ToUInt16((configSettings.intervalSend) ? twoByteConcat(receivedData[offset + configSettings.tripOffset()], receivedData[offset + configSettings.tripOffset() + 1]) : 0);
            Int16 rssi = Convert.ToInt16((configSettings.rssiSend) ? receivedData[offset + configSettings.rssiOffset()] : 0);
            rider.update_v10(uuid, major, minor, rpm, hr, power, interval, kcal, clock, trip, rssi);
        }

        private UInt16 twoByteConcat(byte lower, byte higher)
        {
            return (UInt16)((higher << 8) | lower);
        }

        private byte[] transferUUID(configSettings_v08 configSettings, byte[] receivedData, int offset)
        {
            byte[] uuid = new byte[6];
            int size = (configSettings.uuidLong) ? 6 : 3;
            for (int x = 0; x < size; x++)
            {
                uuid[x] = receivedData[offset + x];
            }
            return uuid;
        }

        private byte[] getUUID(byte[] receivedData, int offset)
        {
            byte[] uuid = new byte[6];
            int size = 6;
            for (int x = 0; x < size; x++)
            {
                uuid[x] = receivedData[offset + x];
            }
            return uuid;
        }

        private int sizeOfData_v08(configSettings_v08 configSettings)
        {
            int size = 0;
            size += (configSettings.uuidLong) ? 6 : 3;
            size += (configSettings.rpmLong) ? 2 : 1;
            size += (configSettings.hrLong) ? 2 : 1;
            size += 2; //POWER
            size += (configSettings.kcalSend) ? 2 : 0;
            size += (configSettings.clockSend) ? 2 : 0;
            size += (configSettings.rssiSend) ? 1 : 0;
            return size;
        }

        private int sizeOfData_v10(configSettings_v10 configSettings)
        {
            int size = 5; //BikeID, RPM, HR, Power
            size += (configSettings.uuidSend) ? 6 : 0;
            size += (configSettings.versionSend) ? 2 : 0;
            size += (configSettings.intervalSend) ? 7 : 0;
            size += (configSettings.rssiSend) ? 1 : 0;
            return size;
        }

        private configSettings_v08 getConfigSettings_v08(byte configFlags)
        {
            return new configSettings_v08
            {
                uuidLong = (configFlags & 1) != 0,
                rpmLong = (configFlags & 2) != 0,
                hrLong = (configFlags & 4) != 0,
                kcalSend = (configFlags & 8) != 0,
                clockSend = (configFlags & 16) != 0,
                rssiSend = (configFlags & 32) != 0
            };
        }

        private configSettings_v10 getConfigSettings_v10(byte configFlags)
        {
            return new configSettings_v10
            {
                uuidSend = (configFlags & 1) != 0,
                versionSend = (configFlags & 2) != 0,
                intervalSend = (configFlags & 4) != 0,
                rssiSend = (configFlags & 8) != 0,
                imperialUnits = (configFlags & 128) != 0
            };
        }
    }
}
