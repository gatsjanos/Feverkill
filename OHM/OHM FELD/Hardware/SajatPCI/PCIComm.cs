using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Hardware.SajatPCI
{
    class PCIComm
    {
        /* Power management index/data registers */
        ushort PM_INDEX = 0xcd6;
        ushort PM_DATA = 0xcd7;
        ushort PM2_INDEX = 0xcd0;
        ushort PM2_DATA = 0xcd1;

        /* Fan Register Definitions */
        byte FAN_0_OFFSET = 0x00;
        byte FAN_1_OFFSET = 0x10;
        byte FAN_2_OFFSET = 0x20;
        byte FAN_3_OFFSET = 0x30;
        byte FAN_4_OFFSET = 0x40;


        /* FanXInputControl Definitions */
        static byte FAN_INPUT_INTERNAL_DIODE = 0;

        /* FanXControl Definitions */
        static byte FAN_POLARITY_HIGH = (1 << 2);


        /* FanXFreq Definitions */
        /* Typically, fans run at 25KHz */
        static byte FREQ_25KHZ = 0x1;

        ushort FAN_REGISTER_COUNT = 15;

        byte[] fan0_config_vals = { FAN_INPUT_INTERNAL_DIODE, FAN_POLARITY_HIGH, FREQ_25KHZ, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        static void pmio_write_index(ushort port_base, byte reg, byte value)
        {
            Ring0.WriteIoPort(port_base, reg);
            Ring0.WriteIoPort(port_base + (uint)1, value);
            //outb(reg, port_base);
            //outb(value, port_base + 1);
        }
        static byte pmio_read_index(ushort port_base, byte reg)
        {
            Ring0.WriteIoPort(port_base, reg);
            return Ring0.ReadIoPort(port_base + (uint)1);
            //outb(reg, port_base);
            //return inb(port_base + 1);
        }
        void pm2_iowrite(byte reg, byte value)
        {
            pmio_write_index(PM2_INDEX, reg, value);
        }
        byte pm2_ioread(byte reg)
        {
            return pmio_read_index(PM2_INDEX, reg);
        }
        void pm_iowrite(byte reg, byte value)
        {
            pmio_write_index(PM_INDEX, reg, value);
        }
        byte pm_ioread(byte reg)
        {
            return pmio_read_index(PM_INDEX, reg);
        }
        public static List<string> PCILog = new List<string>();
        public static List<PCIDevice> FoundPCIDevices = new List<PCIDevice>();
        static ISettings Settings = null;
        public static void checkAllBuses(ISettings settings, string logstring = "|")
        {
            Settings = settings;
            checkAllBuses(logstring);
        }

        public static void checkAllBuses(string logstring = "|")
        {
            PCILog = new List<string>();
            FoundPCIDevices = new List<PCIDevice>();

            byte function;
            byte bus;

            byte headerType = getHeaderType(0, 0, 0);
            if ((headerType & 0x80) == 0)
            {
                PCILog.Add(logstring + "Single PCI host controller");
                /* Single PCI host controller */
                FoundPCIDevices.Add(new PCIDevice(0, 0, 0, Settings, -1, -1, 0, "Single PCI host controller >0:0:0"));
                checkBus(0, logstring + 0 + "B---->");
            }
            else
            {
                PCILog.Add(logstring + "Multiple PCI host controller");
                /* Multiple PCI host controllers */
                for (function = 0; function < 8; function++)
                {
                    if (getVendorID(0, 0, function) == 0xFFFF)
                        break;
                    bus = function;

                    FoundPCIDevices.Add(new PCIDevice(0, 0, function, Settings, -1, -1, 0, "Multiple PCI host controller >0:0:" + function));
                    checkBus(bus, logstring + bus + "B---->");
                }
            }
        }
        static void checkBus(byte bus, string logstring)
        {
            PCILog.Add(logstring + "Checking bus " + bus);
            byte device;

            for (device = 0; device < 32; device++)
            {
                checkDevice(bus, device, logstring + device + "D---->");
            }
        }
        static void checkDevice(byte bus, byte device, string logstring)
        {
            PCILog.Add(logstring + "Checking device " + device);

            byte function = 0;

            uint vendorID = getVendorID(bus, device, function);
            if (vendorID == 0xFFFF)
            {
                PCILog.Add(logstring + "Device doesn't exist");
                return;        // Device doesn't exist
            }

            checkFunction(bus, device, function, logstring + function + "F---->");
            byte headerType = getHeaderType(bus, device, function);
            if ((headerType & 0x80) != 0)
            {
                PCILog.Add(logstring + "Multi-function device");
                /* It is a multi-function device, so check remaining functions */
                for (function = 1; function < 8; function++)
                {
                    if (getVendorID(bus, device, function) != 0xFFFF)
                    {
                        checkFunction(bus, device, function, logstring + function + "F---->");
                    }
                }
            }
            else
            {
                PCILog.Add(logstring + "Single-function device");
            }
        }
        static void checkFunction(byte bus, byte device, byte function, string logstring)
        {
            PCILog.Add(logstring + "Checking function " + function);
            byte baseClass;
            byte subClass;
            byte secondaryBus;

            uint vendorID = getVendorID(bus, device, function);
            uint deviceID = getDeviceID(bus, device, function);

            baseClass = getBaseClass(bus, device, function);
            subClass = getSubClass(bus, device, function);

            PCILog.Add(logstring + "ID " + bus + ":" + device + ":" + function + " =>\tBCl: 0x" + baseClass.ToString("x") + "\tSCl: 0x" + subClass.ToString("x") + "\tVEN: 0x" + vendorID.ToString("x") + "\tDEV: 0x" + deviceID.ToString("x"));
            FoundPCIDevices.Add(new PCIDevice(bus, device, function, Settings, vendorID, deviceID, GetControlsNumber(vendorID, deviceID), GetAlapNev(baseClass, subClass) + " >" + bus.ToString("d3") + ":" + device.ToString("d2") + ":" + function.ToString("d1") + "     >>VEN: 0x" + vendorID.ToString("X4") + " DEV: 0x" + deviceID.ToString("X4") + "        =>BCl: 0x" + baseClass.ToString("X2") + " SCl: 0x" + subClass.ToString("X2")) { BaseClass = baseClass, SubClass = subClass });

            if ((baseClass == 0x06) && (subClass == 0x04))
            {
                secondaryBus = getSecondaryBus(bus, device, function);
                checkBus(secondaryBus, logstring + secondaryBus + "B---->");
            }
        }
        static uint getVendorID(byte bus, byte device, byte function)
        {
            uint ki = 0xFFFFFFFF;
            Ring0.ReadPciConfig(Ring0.GetPciAddress(bus, device, function), 0, out ki);
            return ki & 0xFFFF;
        }
        static uint getDeviceID(byte bus, byte device, byte function)
        {
            uint ki = 0xFFFFFFFF;
            Ring0.ReadPciConfig(Ring0.GetPciAddress(bus, device, function), 0, out ki);
            return (ki & 0xFFFF0000) >> 16;
        }
        static byte getHeaderType(byte bus, byte device, byte function)
        {
            uint ki = 0xFFFFFFFF;
            Ring0.ReadPciConfig(Ring0.GetPciAddress(bus, device, function), 0x0C, out ki);
            return (byte)((ki & 0x00FF0000) >> 16);
        }
        static byte getBaseClass(byte bus, byte device, byte function)
        {
            uint ki = 0xFFFFFFFF;
            Ring0.ReadPciConfig(Ring0.GetPciAddress(bus, device, function), 0x08, out ki);
            return (byte)((ki & 0xFF000000) >> 24);
        }
        static byte getSubClass(byte bus, byte device, byte function)
        {
            uint ki = 0xFFFFFFFF;
            Ring0.ReadPciConfig(Ring0.GetPciAddress(bus, device, function), 0x08, out ki);
            return (byte)((ki & 0x00FF0000) >> 16);
        }
        static byte getSecondaryBus(byte bus, byte device, byte function)
        {
            uint ki = 0xFFFFFFFF;
            Ring0.ReadPciConfig(Ring0.GetPciAddress(bus, device, function), 0x18, out ki);
            return (byte)((ki & 0x0000FF00) >> 8);
        }
        static string GetAlapNev(uint baseClass, uint subClass)
        {
            if (baseClass == 0x06 && subClass == 0x01)
                return "ISA Bridge";
            if (baseClass == 0x06 && subClass == 0x04)
                return "PCI-to-PCI Bridge";
            if (baseClass == 0x06 && subClass == 0x09)
                return "Semi-transparent PCI-to-PCI Bridge";
            if (baseClass == 0x0c && subClass == 0x05)
                return "System Management Bus";

            return "PCI Device";
        }
        static int GetControlsNumber(uint vendorID, uint deviceID)
        {
            if (vendorID == 0x1002 && deviceID == 0x4385)
                return 5;

            return 0;
        }
    }
}