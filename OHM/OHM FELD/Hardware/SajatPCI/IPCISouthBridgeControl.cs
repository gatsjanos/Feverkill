using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Hardware.SajatPCI
{
    interface IPCISouthBridgeControl
    {
        ushort PM2_INDEX { get; }
        ushort PM2_DATA { get; }
        byte[] FANCONTROL_OFFSETS { get; }
        byte[] FANRPM_OFFSETS { get; }
        ushort FAN_REGISTER_COUNT { get; }
        void pm2_iowrite(byte reg, byte value);
        byte pm2_ioread(byte reg);
        void pmio_write_index(ushort port_base, byte reg, byte value);
        byte pmio_read_index(ushort port_base, byte reg);
        void SetControl(int index, byte? value);
        float?[] Controls { get; }
        float?[] Fans { get; }
        float? ReadFanRPM(int index);
        float? ReadGivenFanSpeed(int index);
    }

    class PCISouthbridges
    {
        public static bool GetSBModel(long vendorID, long deviceID, out IPCISouthBridgeControl obj)
        {
            if (vendorID == 0x1002 && deviceID == 0x4385)
            {
                obj = new AMDSB7xx();
                return true;
            }

            obj = new AMDSB7xx();
            return false;
        }
    }
}
