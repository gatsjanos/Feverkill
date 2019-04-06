using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Hardware.SajatPCI
{
    //public PCIDeviceGroup(byte bus, byte device, byte function, ISettings settings) : base("PCIDevice " + bus + ":" + device + ":" + function, new Identifier("PCIdev", bus.ToString(), device.ToString(), function.ToString()), settings )
    //    {

    //}
    class PCIDeviceGroup : IGroup
    {
        //private readonly PCIDevice[] pcidevices;
        List<PCIMain> pcimains = new List<PCIMain>();

        public PCIDeviceGroup(ISettings settings)
        {
            pcimains.Add(new PCIMain(settings, "PCI Devices", new Identifier("PCIMain")));
        }

        public void Close()
        {
            foreach (PCIMain item in pcimains)
                item.Close();
        }

        public string GetReport()
        {
            return null;
        }

        public IHardware[] Hardware
        {

            get
            {
                //List<PCIDevice> lst = new List<PCIDevice>();
                //foreach (var item in pcidevices)
                //{
                //    lst.AddRange(GetPCIList(item));
                //}

                //return lst.ToArray();
                return pcimains.ToArray();
            }

        }
        public IHardware[] SubHardware
        {
            get { return pcimains.ToArray(); }

        }
        public void Traverse(IVisitor visitor)
        {
            foreach (IHardware hardware in pcimains)
                hardware.Accept(visitor);
        }

        List<PCIDevice> GetPCIList(PCIDevice dev)
        {
            List<PCIDevice> lst = new List<PCIDevice>();
            lst.Add(dev);

            if (dev?.Hardware?.Count() != 0)
            {
                foreach (var item in dev.Hardware)
                {
                    lst.AddRange(GetPCIList(item as PCIDevice));
                }
            }

            return lst;
        }
    }
}