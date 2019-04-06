using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHardwareMonitor.Hardware.SajatPCI
{
    class PCIDevice : Hardware
    {
        List<PCIDevice> pcidevices = new List<PCIDevice>();
        private readonly ISettings Settings;
        private string customName;

        public readonly List<Sensor> fans = new List<Sensor>();
        public readonly List<Sensor> controls = new List<Sensor>();

        IPCISouthBridgeControl PCISBCtrl = new AMDSB7xx();
        public long VendorID = 0xFFFF, DeviceID = 0xFFFF, BaseClass = 0xFFFF, SubClass = 0xFFFF;

        public event SensorEventHandler SensorAdded;
        public event SensorEventHandler SensorRemoved;

        short Bus, Device, Function;
        public PCIDevice(short bus, short device, short function, ISettings settings, long vendorID = -1, long deviceID = -1, int ControlsNumber = 0, string name = "{<nothinggiven19981001>}") : base("PCIDevice " + bus + ":" + device + ":" + function, new Identifier("PCIdev", bus.ToString(), device.ToString(), function.ToString()), settings)
        {
            Bus = bus;
            Device = device;
            Function = function;
            Settings = settings;

            if (vendorID != -1)
                VendorID = vendorID;
            if (deviceID != -1)
                DeviceID = deviceID;

            if (name == "{<nothinggiven19981001>}")
            {
                this.customName = settings.GetValue(new Identifier(Identifier, "name").ToString(), "PCIDevice " + bus + ":" + device + ":" + function);
            }
            else
            {
                this.customName = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);
            }
            base.Name = name;

            if (ControlsNumber > 0)
            {
                if (PCISouthbridges.GetSBModel(vendorID, deviceID, out PCISBCtrl))
                {
                    List<Ctrl> c = new List<Ctrl>();
                    List<Fan> f = new List<Fan>();

                    for (int i = 0; i < PCISBCtrl.FANCONTROL_OFFSETS.Length; i++)
                        c.Add(new Ctrl("Fan #" + (i + 1), i));

                    for (int i = 0; i < PCISBCtrl.FANRPM_OFFSETS.Length; i++)
                        f.Add(new Fan("Fan #" + (i + 1), i));

                    CreateControlSensors(settings, c);
                    CreateFanSensors(settings, f);
                }
            }
        }
        private void CreateControlSensors(ISettings settings, IList<Ctrl> c)
        {
            foreach (Ctrl ctrl in c)
            {
                int index = ctrl.Index;
                if (index < PCISBCtrl.FANCONTROL_OFFSETS.Length)
                {
                    Sensor sensor = new Sensor(ctrl.Name, index, SensorType.Control, this, settings);
                    Control control = new Control(sensor, settings, 0, 100);
                    control.ControlModeChanged += (cc) =>
                    {
                        switch (cc.ControlMode)
                        {
                            case ControlMode.Undefined:
                                return;
                            case ControlMode.Alapert:
                                PCISBCtrl.SetControl(index, null);
                                controls[index].Value = null;
                                break;
                            case ControlMode.Kezi:
                                PCISBCtrl.SetControl(index, (byte)(cc.SoftwareValue * 2.55));
                                controls[index].Value = control.SoftwareValue;
                                break;
                            default:
                                return;
                        }
                    };
                    control.SoftwareControlValueChanged += (cc) =>
                    {
                        if (cc.ControlMode == ControlMode.Kezi)
                        {
                            PCISBCtrl.SetControl(index, (byte)(cc.SoftwareValue * 2.55));
                            controls[index].Value = control.SoftwareValue;
                        }
                    };

                    switch (control.ControlMode)
                    {
                        case ControlMode.Undefined:
                            break;
                        case ControlMode.Alapert:
                            PCISBCtrl.SetControl(index, null);
                            controls[index].Value = null;
                            break;
                        case ControlMode.Kezi:
                            PCISBCtrl.SetControl(index, (byte)(control.SoftwareValue * 2.55));
                            controls[index].Value = control.SoftwareValue;
                            break;
                        default:
                            break;
                    }

                    sensor.Control = control;
                    controls.Add(sensor);
                    ActivateSensor(sensor);
                }
            }
        }
        private void CreateFanSensors(ISettings settings, IList<Fan> f)
        {
            foreach (Fan fan in f)
            {
                if (fan.Index < PCISBCtrl.FANRPM_OFFSETS.Length)
                {
                    Sensor sensor = new Sensor(fan.Name, fan.Index, SensorType.Fan, this, settings);
                    fans.Add(sensor);
                    ActivateSensor(sensor);
                }
            }
        }
        public override HardwareType HardwareType
        {
            get
            {
                return HardwareType.PCIDevice;
            }
        }
        //public ISensor[] Sensors
        //{
        //    get { return new ISensor[0]; }
        //}
        //public void Close()
        //{
        //    foreach (PCIDevice item in pcidevices)
        //        item.Close();
        //}

        //public string GetReport()
        //{
        //    //string ki = "";

        //    //foreach (var item in PCIComm.PCILog)
        //    //{
        //    //    ki += item + "\n";
        //    //}
        //    return customName;
        //}

        //public void Accept(IVisitor visitor)
        //{
        //    //throw new NotImplementedException();
        //}

        //public void Traverse(IVisitor visitor)
        //{
        //    //throw new NotImplementedException();
        //}

        public override void Update()
        {
            foreach (Sensor sensor in controls)
            {
                float? value = PCISBCtrl.ReadGivenFanSpeed(sensor.Index);
                if (value.HasValue)
                {
                    sensor.Value = value / (float)2.55;
                }
            }

            foreach (Sensor sensor in fans)
            {
                float? value = PCISBCtrl.ReadFanRPM(sensor.Index);
                if (value.HasValue)
                {
                    sensor.Value = value;
                    // if (value.Value > 0)
                    // ActivateSensor(sensor);
                }
            }
        }

        public IHardware[] Hardware
        {
            get { return pcidevices.ToArray(); }

        }

        //public string Name
        //{
        //    get
        //    {
        //        return customName;
        //    }

        //    set
        //    {
        //        //if (!string.IsNullOrEmpty(value))

        //        customName = value;

        //        //Settings.SetValue(new Identifier(Identifier, "name").ToString(), customName);
        //    }
        //}

        //public Identifier Identifier
        //{
        //    get
        //    {
        //        return new Identifier("PCIDevice_" + Bus + ":" + Device + ":" + Function);
        //    }
        //}

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
        private class Ctrl
        {
            public readonly string Name;
            public readonly int Index;

            public Ctrl(string name, int index)
            {
                this.Name = name;
                this.Index = index;
            }
        }
        private class Fan
        {
            public readonly string Name;
            public readonly int Index;

            public Fan(string name, int index)
            {
                this.Name = name;
                this.Index = index;
            }
        }
    }
}
