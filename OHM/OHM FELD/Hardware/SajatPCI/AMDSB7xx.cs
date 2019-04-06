using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Hardware.SajatPCI
{
    class AMDSB7xx : IPCISouthBridgeControl
    {
        public AMDSB7xx()
        {
            controls = new float?[FANCONTROL_OFFSETS.Length];
        }
        public byte[] FANCONTROL_OFFSETS
        {
            get
            {
                return new byte[] { 0x00, 0x10, 0x20, 0x30, 0x40 };
            }
        }
        public byte[] FANRPM_OFFSETS
        {
            get
            {
                return new byte[] { 0x69, 0x6E, 0x73, 0x78, 0x7D };
            }
        }

        public ushort FAN_REGISTER_COUNT
        {
            get
            {
                return 15;
            }
        }

        public ushort PM2_INDEX
        {
            get
            {
                return 0xcd0;
            }
        }

        public ushort PM2_DATA
        {
            get
            {
                return 0xcd1;
            }
        }

        public byte pm2_ioread(byte reg)
        {
            return pmio_read_index(PM2_INDEX, reg);
        }

        public void pm2_iowrite(byte reg, byte value)
        {
            pmio_write_index(PM2_INDEX, reg, value);
        }

        public void pmio_write_index(ushort port_base, byte reg, byte value)
        {
            Ring0.WriteIoPort(port_base, reg);
            Ring0.WriteIoPort(port_base + (uint)1, value);
            //outb(reg, port_base);
            //outb(value, port_base + 1);
        }

        public byte pmio_read_index(ushort port_base, byte reg)
        {
            Ring0.WriteIoPort(port_base, reg);
            return Ring0.ReadIoPort(port_base + (uint)1);
            //outb(reg, port_base);
            //return inb(port_base + 1);
        }

        private bool[] restoreDefaultFanControlRequired = new bool[6];
        private byte[] initialFanControlMode = new byte[6];
        private byte[] initialFanPwmCommand = new byte[6];

        private void SaveDefaultFanControl(int index)
        {
            if (!restoreDefaultFanControlRequired[index])
            {
                //initialFanControlMode[index] = ReadByte(FAN_CONTROL_MODE_REG[index]);
                //initialFanPwmCommand[index] = ReadByte(FAN_PWM_COMMAND_REG[index]);
                restoreDefaultFanControlRequired[index] = true;
            }
        }

        private void RestoreDefaultFanControl(int index)
        {
            if (restoreDefaultFanControlRequired[index])
            {
                // WriteByte(FAN_CONTROL_MODE_REG[index], initialFanControlMode[index]);
                // WriteByte(FAN_PWM_COMMAND_REG[index], initialFanPwmCommand[index]);
                restoreDefaultFanControlRequired[index] = false;
            }
        }

        public void SetControl(int index, byte? value)
        {
            if (index < 0 || index >= FANCONTROL_OFFSETS.Length)
                throw new ArgumentOutOfRangeException("index");

            if (!Ring0.WaitIsaBusMutex(10))
                return;

            if (value.HasValue)
            {
                SaveDefaultFanControl(index);

                pm2_iowrite((byte)(FANCONTROL_OFFSETS[index] + 3), value.Value);
            }
            else
            {
                RestoreDefaultFanControl(index);
            }

            Ring0.ReleaseIsaBusMutex();

            controls[index] = value;
        }
        private readonly float?[] controls = new float?[0];
        public float?[] Controls { get { return controls; } }
        public float? ReadGivenFanSpeed(int index)
        {
            if (index < FANCONTROL_OFFSETS.Length)
            {
                return pm2_ioread((byte)(FANCONTROL_OFFSETS[index] + 3));
            }
            else
                return null;
        }

        private readonly float?[] fans = new float?[0];
        public float?[] Fans
        {
            get
            {
                return fans;
            }
        }
        public float? ReadFanRPM(int index)
        {
            if (index < FANRPM_OFFSETS.Length)
            {
                byte Hi = pm2_ioread((byte)(FANRPM_OFFSETS[index] + 1));
                byte Lo = pm2_ioread(FANRPM_OFFSETS[index]);
                if (Hi == 0xFF && Lo == 0xFF)
                    return 0;

                return 678000 / (float)((Hi << 8) | Lo);
            }
            return null;
        }
    }
}
