using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net;

namespace Control_PC
{
    internal class DeviceItem
    {
        public string DeviceName { get; set; }
        public BluetoothAddress DeviceAdress { get; set; }

        public DeviceItem(string deviceName, BluetoothAddress deviceAdress)
        {
            DeviceName = deviceName;
            DeviceAdress = deviceAdress;
        }

    }
}
