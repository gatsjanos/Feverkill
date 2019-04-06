/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2013 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

//TODO: !-!-!-!-!-! MÓDOSÍTOTT OpenSource KÓD 9
//Ebben a .cs-ben sok helyen
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public class PlotPanel : UserControl
    {

        private readonly PersistentSettings settings;
        private readonly UnitManager unitManager;

        private readonly PlotView plot;
        private readonly PlotModel model;
        private readonly TimeSpanAxis timeAxis = new TimeSpanAxis();
        private readonly SortedDictionary<SensorType, LinearAxis> axes =
          new SortedDictionary<SensorType, LinearAxis>();

        private UserOption stackedAxes;
        private TextBox textBox1;
        private DateTime now;

        public PlotPanel(PersistentSettings settings, UnitManager unitManager)
        {
            this.settings = settings;
            this.unitManager = unitManager;

            this.model = CreatePlotModel();

            this.plot = new PlotView();
            this.plot.Dock = DockStyle.Fill;
            this.plot.Model = model;
            this.plot.BackColor = Color.White;
            this.plot.ContextMenu = CreateMenu();

            plot.Model.LegendBorder = OxyColor.FromRgb(0, 0, 0);
            plot.Model.LegendBorderThickness = 10;

            plot.Controller = new PlotController();

            plot.Controller.UnbindMouseDown(OxyMouseButton.Right, OxyModifierKeys.None, 1);
            plot.Controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.HoverTrack);
            plot.Controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);


            UpdateAxesPosition();

            InitializeComponent();

            this.SuspendLayout();
            this.Controls.Add(plot);
            this.ResumeLayout(true);
        }

        public void SetCurrentSettings()
        {
            settings.SetValue("plotPanel.MinTimeSpan", (float)timeAxis.ActualMinimum);
            settings.SetValue("plotPanel.MaxTimeSpan", (float)timeAxis.ActualMinimum);

            foreach (var axis in axes.Values)
            {
                settings.SetValue("plotPanel.Min" + axis.Key, (float)axis.ActualMinimum);
                settings.SetValue("plotPanel.Max" + axis.Key, (float)axis.ActualMinimum);
            }
        }

        private ContextMenu CreateMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem stackedAxesMenuItem = new MenuItem(((Program.KONFNyelv == "hun") ? "Eltérő típusok külön grafikonra" : Eszk.GetNyelvSzo("PLOTPANConMenuKulongraf")));
            stackedAxes = new UserOption("stackedAxes", true,
              stackedAxesMenuItem, settings);
            stackedAxes.Changed += (sender, e) =>
            {
                UpdateAxesPosition();
                InvalidatePlot();
            };
            menu.MenuItems.Add(stackedAxesMenuItem);

            MenuItem timeWindow = new MenuItem(((Program.KONFNyelv == "hun") ? "Időtengely intervallum" : Eszk.GetNyelvSzo("PLOTPANConMenuIdobontas")));
            MenuItem[] timeWindowMenuItems =
              {
          new MenuItem("Auto",
            (s, e) => { timeAxis.Zoom(0, double.NaN); InvalidatePlot(); }),
          new MenuItem("1 sec",
            (s, e) => { timeAxis.Zoom(0, 1); InvalidatePlot(); }),
          new MenuItem("2 sec",
            (s, e) => { timeAxis.Zoom(0, 2); InvalidatePlot(); }),
          new MenuItem("5 sec",
            (s, e) => { timeAxis.Zoom(0, 5); InvalidatePlot(); }),
          new MenuItem("10 sec",
            (s, e) => { timeAxis.Zoom(0, 10); InvalidatePlot(); }),
          new MenuItem("20 sec",
            (s, e) => { timeAxis.Zoom(0, 20); InvalidatePlot(); }),
          new MenuItem("30 sec",
            (s, e) => { timeAxis.Zoom(0, 30); InvalidatePlot(); }),
          new MenuItem("1 min",
            (s, e) => { timeAxis.Zoom(0, 1 * 60); InvalidatePlot(); }),
          new MenuItem("2 min",
            (s, e) => { timeAxis.Zoom(0, 2 * 60); InvalidatePlot(); }),
          new MenuItem("5 min",
            (s, e) => { timeAxis.Zoom(0, 5 * 60); InvalidatePlot(); }),
          new MenuItem("10 min",
            (s, e) => { timeAxis.Zoom(0, 10 * 60); InvalidatePlot(); }),
          new MenuItem("20 min",
            (s, e) => { timeAxis.Zoom(0, 20 * 60); InvalidatePlot(); }),
          new MenuItem("30 min",
            (s, e) => { timeAxis.Zoom(0, 30 * 60); InvalidatePlot(); }),
          new MenuItem("45 min",
            (s, e) => { timeAxis.Zoom(0, 45 * 60); InvalidatePlot(); }),
          new MenuItem("1 h",
            (s, e) => { timeAxis.Zoom(0, 60 * 60); InvalidatePlot(); }),
          new MenuItem("1.5 h",
            (s, e) => { timeAxis.Zoom(0, 1.5 * 60 * 60); InvalidatePlot(); }),
          new MenuItem("2 h",
            (s, e) => { timeAxis.Zoom(0, 2 * 60 * 60); InvalidatePlot(); }),
          new MenuItem("3 h",
            (s, e) => { timeAxis.Zoom(0, 3 * 60 * 60); InvalidatePlot(); }),
          new MenuItem("6 h",
            (s, e) => { timeAxis.Zoom(0, 6 * 60 * 60); InvalidatePlot(); }),
          new MenuItem("12 h",
            (s, e) => { timeAxis.Zoom(0, 12 * 60 * 60); InvalidatePlot(); }),
          new MenuItem("24 h",
            (s, e) => { timeAxis.Zoom(0, 24 * 60 * 60); InvalidatePlot(); })};

            foreach (MenuItem mi in timeWindowMenuItems)
                timeWindow.MenuItems.Add(mi);
            menu.MenuItems.Add(timeWindow);

            MenuItem IranyitasMenuItem = new MenuItem("Controls (help)",(s,e) => { MessageBox.Show("-Drag with mouse on plot: to move view\n-Drag with mouse on axes: to move axis interval\n-Scroll with mouse on plot: to zoom view\n-Scroll with mouse on axes: to zoom axis interval\n-Hold right mouse button over a specific graph: to display accurate values\n-Press Key 'A': to set view for every selected graphs","Controls (help)", MessageBoxButtons.OK, MessageBoxIcon.Information); } );
            menu.MenuItems.Add(IranyitasMenuItem);
            return menu;
        }

        private PlotModel CreatePlotModel()
        {
            timeAxis.Position = AxisPosition.Bottom;
            timeAxis.MajorGridlineStyle = LineStyle.Solid;
            timeAxis.MajorGridlineThickness = 1;
            timeAxis.MajorGridlineColor = OxyColor.FromRgb(192, 192, 192);
            timeAxis.MinorGridlineStyle = LineStyle.Solid;
            timeAxis.MinorGridlineThickness = 1;
            timeAxis.MinorGridlineColor = OxyColor.FromRgb(232, 232, 232);
            timeAxis.StartPosition = 1;
            timeAxis.EndPosition = 0;
            timeAxis.MinimumPadding = 0;
            timeAxis.MaximumPadding = 0;
            timeAxis.AbsoluteMinimum = 0;
            timeAxis.Minimum = 0;
            timeAxis.AbsoluteMaximum = 24 * 60 * 60;
            timeAxis.Zoom(
              settings.GetValue("plotPanel.MinTimeSpan", 0.0f),
              settings.GetValue("plotPanel.MaxTimeSpan", 10.0f * 60));
            timeAxis.StringFormat = "h:mm";

            var units = new Dictionary<SensorType, string>();
            units.Add(SensorType.Voltage, "V");
            units.Add(SensorType.Clock, "MHz");
            units.Add(SensorType.Temperature, "°C");
            units.Add(SensorType.Load, "%");
            units.Add(SensorType.Fan, "RPM");
            units.Add(SensorType.Flow, "L/h");
            units.Add(SensorType.Control, "%");
            units.Add(SensorType.Level, "%");
            units.Add(SensorType.Factor, "1");
            units.Add(SensorType.Power, "W");
            units.Add(SensorType.Data, "GB");

            foreach (SensorType type in Enum.GetValues(typeof(SensorType)))
            {
                var axis = new LinearAxis();
                axis.Position = AxisPosition.Left;
                axis.MajorGridlineStyle = LineStyle.Solid;
                axis.MajorGridlineThickness = 1;
                axis.MajorGridlineColor = timeAxis.MajorGridlineColor;
                axis.MinorGridlineStyle = LineStyle.Solid;
                axis.MinorGridlineThickness = 1;
                axis.MinorGridlineColor = timeAxis.MinorGridlineColor;
                axis.AxislineStyle = LineStyle.Solid;
                axis.Title = type.ToString();
                axis.Key = type.ToString();

                axis.Zoom(
                  settings.GetValue("plotPanel.Min" + axis.Key, float.NaN),
                  settings.GetValue("plotPanel.Max" + axis.Key, float.NaN));


                if (units.ContainsKey(type))
                    axis.Unit = units[type];
                axes.Add(type, axis);
            }

            var model = new PlotModel();
            model.Axes.Add(timeAxis);
            foreach (var axis in axes.Values)
                model.Axes.Add(axis);
            // model.PlotMargins = new OxyThickness(0);
            model.IsLegendVisible = false;

            return model;
        }

        public void SetSensors(List<ISensor> sensors,
          IDictionary<ISensor, Color> colors)
        {
            this.model.Series.Clear();

            ListSet<SensorType> types = new ListSet<SensorType>();

            foreach (ISensor sensor in sensors)
            {
                var series = new LineSeries();
                if (sensor.SensorType == SensorType.Temperature)
                {
                    series.ItemsSource = sensor.Values.Select(value => new DataPoint((now - value.Time).TotalSeconds, unitManager.TemperatureUnit == TemperatureUnit.Celsius ? value.Value : UnitManager.CelsiusToFahrenheit(value.Value).Value));
                }
                else
                {
                    series.ItemsSource = sensor.Values.Select(value => new DataPoint((now - value.Time).TotalSeconds, value.Value));
                }
                series.Color = colors[sensor].ToOxyColor();
                series.StrokeThickness = 1;
                series.YAxisKey = axes[sensor.SensorType].Key;
                series.Title = sensor.Hardware.Name + " " + sensor.Name;
                this.model.Series.Add(series);

                types.Add(sensor.SensorType);
            }


            foreach (var pair in axes.Reverse())
            {
                var axis = pair.Value;
                var type = pair.Key;
                axis.IsAxisVisible = types.Contains(type);
            }

            if (model.Series.Count == 0)
            {
                textBox1.Visible = true;
            }
            else
            {
                textBox1.Visible = false;
            }

            UpdateAxesPosition();
            InvalidatePlot();
        }

        private void UpdateAxesPosition()
        {
            if (stackedAxes.Value)
            {

                var count = axes.Values.Count(axis => axis.IsAxisVisible);
                var start = 0.0;

                foreach (var pair in axes.Reverse())
                {
                    var axis = pair.Value;
                    var type = pair.Key;
                    axis.StartPosition = start;
                    var delta = axis.IsAxisVisible ? 1.0 / count : 0;
                    start += delta;
                    axis.EndPosition = start;
                    axis.PositionTier = 0;
                    axis.MajorGridlineStyle = LineStyle.Solid;
                    axis.MinorGridlineStyle = LineStyle.Solid;
                }
            }
            else
            {
                var tier = 0;

                foreach (var pair in axes.Reverse())
                {
                    var axis = pair.Value;
                    var type = pair.Key;
                    if (axis.IsAxisVisible)
                    {
                        axis.StartPosition = 0;
                        axis.EndPosition = 1;
                        axis.PositionTier = tier;
                        tier++;
                    }
                    else
                    {
                        axis.StartPosition = 0;
                        axis.EndPosition = 0;
                        axis.PositionTier = 0;
                    }
                    axis.MajorGridlineStyle = LineStyle.None;
                    axis.MinorGridlineStyle = LineStyle.None;
                }
            }

        }

        public void InvalidatePlot()
        {
            this.now = DateTime.UtcNow;

            foreach (var pair in axes)
            {
                var axis = pair.Value;
                var type = pair.Key;
                if (type == SensorType.Temperature)
                    axis.Unit = unitManager.TemperatureUnit == TemperatureUnit.Celsius ?
                    "°C" : "°F";
            }

            this.plot.InvalidatePlot(true);
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Segoe Print", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(750, 488);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Select checkboxes in Advanced Window to display values here!\r\n(Try right click on" +
    " plot for more!)";
            // 
            // PlotPanel
            // 
            this.Controls.Add(this.textBox1);
            this.Name = "PlotPanel";
            this.Size = new System.Drawing.Size(750, 488);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
