using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Hardware.SajatPCI
{
    class PCIMain : IHardware
    {
        private string customName;
        List<PCIDevice> pcidevices = new List<PCIDevice>();
        public PCIMain(ISettings settings, string name, Identifier identif)
        {
            Identifier = identif;

            this.customName = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);

            PCIComm.checkAllBuses(settings);
            PCIComm.FoundPCIDevices.Reverse();
            if (Seged.MindenPCIEszkBetolt)
                pcidevices.AddRange(PCIComm.FoundPCIDevices);
            else
                foreach (var item in PCIComm.FoundPCIDevices)
                {
                    if (item.controls.Count > 0 || item.fans.Count > 0)
                        pcidevices.Add(item);
                }

            foreach (var item in pcidevices)
            {
                item.Parent = this;
            }

            //pcidevices.Add(new PCIDevice(1, 0, 2, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 3, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 4, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 5, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 6, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 7, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 8, settings));
            //pcidevices.Add(new PCIDevice(1, 0, 9, settings));
        }
        public HardwareType HardwareType
        {
            get
            {
                return HardwareType.PCIMain;
            }
        }

        public Identifier Identifier
        {
            get;
        }

        public string Name
        {
            get
            {
                return customName;
            }

            set
            {
                customName = value;
            }
        }

        internal void Close()
        {

        }

        IHardware _parent = null;
        public IHardware Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public ISensor[] Sensors
        {
            get { return new ISensor[0]; }
        }

        public IHardware[] SubHardware
        {
            get { return pcidevices.ToArray(); }
        }

        public event SensorEventHandler SensorAdded;
        public event SensorEventHandler SensorRemoved;

        public string GetReport()
        {
            return customName;
        }

        public void Accept(IVisitor visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            visitor.VisitHardware(this);
        }

        public void Traverse(IVisitor visitor)
        {
            foreach (IHardware hardware in pcidevices)
                hardware.Accept(visitor);
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
