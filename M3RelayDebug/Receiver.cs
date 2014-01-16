using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace M3RelayDebug
{
    class Receiver
    {
        private Thread _Thread;
        private volatile Boolean _KeepWorking;
        private Logger logger;
        public bool running = false;

        public string ipAddress = "";
        public UInt16 ipPort;
        public List<Rider> riders = new List<Rider>();

        public Receiver(Logger _logger)
        {
            logger = _logger;
        }

        public void start()
        {
            riders.Clear();
            _Thread = new Thread(worker);
            logger.start(riders);
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
            socket.Bind(ipEndPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(ipAddress)));
            byte[] receivedData = new byte[1024];
            while (_KeepWorking)
            {
                if (socket.Poll(200000, SelectMode.SelectRead))
                {
                    socket.Receive(receivedData);
                    parser(receivedData);
                    receivedData = new byte[1024];
                }
            }
            socket.Close();
        }

        private struct configSettings
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

        private void parser(byte[] receivedData)
        {
            byte configFlags = receivedData[0];
            configSettings configSettings = getConfigSettings(configFlags);
            int dataSize = sizeOfData(configSettings);
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
                updateRide(configSettings, receivedData, offset, rider);
            }

        }

        private void updateRide(configSettings configSettings, byte[] receivedData, int offset, Rider rider)
        {
            UInt16 rpm = Convert.ToUInt16((configSettings.rpmLong) ? twoByteConcat(receivedData[offset + configSettings.rpmOffset()], receivedData[offset + configSettings.rpmOffset() + 1]) / 10 : receivedData[offset + configSettings.rpmOffset()]);
            UInt16 hr = Convert.ToUInt16((configSettings.hrLong) ? twoByteConcat(receivedData[offset + configSettings.hrOffset()], receivedData[offset + configSettings.hrOffset() + 1]) / 10 : receivedData[offset + configSettings.hrOffset()]);
            UInt16 power =twoByteConcat(receivedData[offset + configSettings.powerOffset()], receivedData[offset + configSettings.powerOffset() + 1]);
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
            rider.update(rpm, hr, power, kcal, clock, rssi);
        }

        private UInt16 twoByteConcat(byte lower, byte higher)
        {
            return (UInt16)((higher << 8) | lower);
        }

        private byte[] transferUUID(configSettings configSettings, byte[] receivedData, int offset)
        {
            byte[] uuid = new byte[6];
            int size = (configSettings.uuidLong) ? 6 : 3;
            for (int x = 0; x < size; x++)
            {
                uuid[x] = receivedData[offset + x];
            }
            return uuid;
        }

        private int sizeOfData(configSettings configSettings)
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

        private configSettings getConfigSettings(byte configFlags)
        {
            return new configSettings
            {
                uuidLong = (configFlags & 1) != 0,
                rpmLong = (configFlags & 2) != 0,
                hrLong = (configFlags & 4) != 0,
                kcalSend = (configFlags & 8) != 0,
                clockSend = (configFlags & 16) != 0,
                rssiSend = (configFlags & 32) != 0
            };
        }
    }
}
