using System;
using System.Windows.Forms;
using ScottPlot.WinForms;
using ScottPlot;
using System.Net.Quic;
using MathNet.Numerics.Distributions;

namespace RelicCharts
{
    //Warning, if you go beyond this point, you may not return the same man, nor sane...
    //Editing anything is highly discouraged...
    public partial class Form1 : Form
    {
        private TabControl _tabControl;
        private TabPage _tabPage1;
        private TabPage _tabPage2;
        private TabPage _tabPage3;
        private TabPage _tabPage4;
        private TabPage _tabPage5;

        private ComboBox _barBox;
        private ComboBox _pieBox;
        private ComboBox _chiChart;
        private ComboBox _errorChart;

        private string[] _refinements = { "Intact", "Exceptional", "Flawless", "Radiant" };
        private List<Relic> _relics;

        private FormsPlot _pieChart;
        private FormsPlot _barPlot;
        private FormsPlot _chiPlot;
        private FormsPlot _scatterplot;
        private FormsPlot _errorPlot;

        public Form1()
        {
            InitializeComponent();
            _relics = Generator.RelicGenerator();
            _pieChart = pieChart;
            _barPlot = barPlot;
            _chiPlot = chanceSplatter;
            _scatterplot = scatterPlot;
            _errorPlot = errorPlot;

            _tabControl = new TabControl();
            _tabPage1 = new TabPage("Pie Chart");
            _tabPage2 = new TabPage("Bar Plot");
            _tabPage3 = new TabPage("Chi Squared Plot");
            _tabPage4 = new TabPage("Scatter Plot");
            _tabPage5 = new TabPage("Errors");

            _tabPage1.Controls.Add(_pieChart);
            _tabPage2.Controls.Add(_barPlot);
            _tabPage3.Controls.Add(_chiPlot);
            _tabPage4.Controls.Add(_scatterplot);
            _tabPage5.Controls.Add(_errorPlot);

            _tabControl.Controls.Add(_tabPage1);
            _tabControl.Controls.Add(_tabPage2);
            _tabControl.Controls.Add(_tabPage3);
            _tabControl.Controls.Add(_tabPage4);
            _tabControl.Controls.Add(_tabPage5);

            this.Controls.Add(_tabControl);

            _tabControl.Dock = DockStyle.Fill;
            _pieChart.Dock = DockStyle.None;
            _barPlot.Dock = DockStyle.None;

            _barBox = new ComboBox();
            _pieBox = new ComboBox();

            _pieBox.Items.AddRange(_refinements);
            _barBox.Items.AddRange(_refinements);

            _barBox.SelectedIndexChanged += BarBox_SelectedIndexChanged;
            _pieBox.SelectedIndexChanged += PieBox_SelectedIndexChanged;

            _tabPage1.Controls.Add(_pieBox);
            _tabPage2.Controls.Add(_barBox);

            _barBox.Location = new System.Drawing.Point(10, 10);
            _barBox.Width = 150;

            _pieBox.Location = new System.Drawing.Point(10, 10);
            _pieBox.Width = 150;

            UpdatePlots();
        }

        private void BarBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_barBox.SelectedIndex >= 0)
            {
                DropsBarPlot(_relics[_barBox.SelectedIndex], _barPlot);
            }
        }

        private void PieBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_pieBox.SelectedIndex >= 0)
            {
                MostChosenValues(_relics[_pieBox.SelectedIndex], _pieChart);
            }
        }

        private void UpdatePlots()
        {
            MostChosenValues(_relics[0], pieChart);
            DropsBarPlot(_relics[0], barPlot);
            ChiSquaredPlot(chanceSplatter);
            GeneratedValuesPlot(scatterPlot);
            ErrorPlot(errorPlot);
        }

        public static void MostChosenValues(Relic relic, FormsPlot piechart)
        {
            piechart.Reset();
            IPalette palette = new ScottPlot.Palettes.DarkPastel();

            int counter = 0;

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string target = @"TempText";
            string chancesFile = $"{relic.RelicName}_{relic.Refinement}_RolledValues.txt";
            string fullValuePath = Path.Combine(directory, target, chancesFile);

            using (StreamReader readFile = new StreamReader(fullValuePath))
            {
                string ln;
                while ((ln = readFile.ReadLine()) != null)
                {
                    counter++;
                }
                readFile.Close();
            }
                ;


            string[] chanceValues = File.ReadAllLines(fullValuePath);
            var query = chanceValues.GroupBy(x => x).Select(x => new { Item = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);


            List<PieSlice> slices = new List<PieSlice>();
            foreach (var item in query)
            {
                PieSlice slice = new() { Value = item.Count, Label = item.Item };
                slices.Add(slice);
            }

            var pie = piechart.Plot.Add.Pie(slices);

            piechart.Plot.Axes.Frameless();
            piechart.Plot.HideGrid();

            double total = pie.Slices.Select(x => x.Value).Sum();

            for (int i = 0; i < slices.Count; i++)
            {
                pie.Slices[i].FillColor = palette.Colors[i];
                pie.Slices[i].Label = $"{pie.Slices[i].Value}";
                pie.Slices[i].LabelFontSize = 20;
                pie.Slices[i].LabelFontColor = Colors.Black.WithAlpha(.5);
                pie.Slices[i].LegendText = $"{query.ElementAt(i).Item} - {pie.Slices[i].Value} ~ {pie.Slices[i].Value / total:p1}";
            }

            pie.ExplodeFraction = .1;
            pie.SliceLabelDistance = 1.3;

            piechart.Refresh();
        }

        public static void DropsBarPlot(Relic relic, FormsPlot barplot)
        {
            barplot.Reset();
            IPalette palette = new ScottPlot.Palettes.DarkPastel();

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string target = @"TempText";
            string textFilename = $"{relic.RelicName}_{relic.Refinement}_Drops.txt";
            string fullDropPath = Path.Combine(directory, target, textFilename);

            string[] drops = File.ReadAllLines(fullDropPath);
            var query = drops.GroupBy(x => x).Select(x => new { Item = x.Key, Count = x.Count() }).OrderBy(x => x.Count);

            var items = query.Select(x => x.Item).ToArray();
            var counts = query.Select(x => (double)x.Count).ToArray();

            var bars = counts.Select((value, index) => new ScottPlot.Bar { Value = value, Position = index }).ToArray();
            var bigBar = barplot.Plot.Add.Bars(bars);

            foreach (var bar in bigBar.Bars)
            { 
                bar.Label = bar.Value.ToString();
            }
            for (int i = 0; i < bigBar.Bars.Count; i++)
            {
                bigBar.Bars[i].FillColor = palette.Colors[i];
            }

            Tick[] ticks = new Tick[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                ticks[i] = new Tick(i, items[i]);
            }

            barplot.Plot.Title($"Drops for {relic.RelicName} of {relic.Refinement} level");
            barplot.Plot.YLabel("Count");
            barplot.Plot.XLabel("Items");
            barplot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            

            barplot.Refresh();
        }

        public static void ChiSquaredPlot(FormsPlot chiPlot)
        {
            static double dF2(double x) => ChiSquared.PDF(2, x);
            static double dF3(double x) => ChiSquared.PDF(3, x);
            static double dF5(double x) => ChiSquared.PDF(5, x);
            var df2 = chiPlot.Plot.Add.Function(dF2);
            var df3 = chiPlot.Plot.Add.Function(dF3);
            var df5 = chiPlot.Plot.Add.Function(dF5);
            df2.LegendText = "df = 2";
            df3.LegendText = "df = 3";
            df5.LegendText = "df = 5";
            chiPlot.Plot.Axes.SetLimits(0, 20, 0, 0.5);
            chiPlot.Plot.ShowLegend(Edge.Right);
            chiPlot.Refresh();
        }

        public static void GeneratedValuesPlot(FormsPlot scatterplot)
        {
            string target = @"TempText";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string generatedValue = $"GeneratedValues.txt";
            string fullGeneratedPath = Path.Combine(directory, target, generatedValue);

            string[] values = File.ReadAllLines(fullGeneratedPath);
            double[] xs = new double[values.Length];
            double[] ys = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                ys[i] = double.Parse(values[i]);
                xs[i] = i;
            }
            
            var sp = scatterplot.Plot.Add.ScatterLine(xs,ys);
            sp.MarkerSize = 5;
            sp.LineWidth = 0;

            scatterplot.Refresh();
        }

        public static void ErrorPlot(FormsPlot errorplot)
        {
            string target = @"TempText";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string errorName = $"ErrorsForRareItemDrops.txt";
            string errorPath = Path.Combine(directory, target, errorName);

            string[] errors = File.ReadAllLines(errorPath);
            double[] ys = new double[errors.Length];
            double[] xs = new double[errors.Length];

            for (int i = 0; i < errors.Length; i++)
            {
                ys[i] = double.Parse(errors[i]);
                xs[i] = i;
            }

            var sp = errorplot.Plot.Add.ScatterLine(xs,ys);
            sp.MarkerSize = 5;
            sp.LineWidth = 0;

            ScottPlot.Statistics.LinearRegression reg = new(xs, ys);

            Coordinates pt1 = new(xs.First(), reg.GetValue(xs.First()));
            Coordinates pt2 = new(xs.Last(), reg.GetValue(xs.Last()));
            var line = errorplot.Plot.Add.Line(pt1, pt2);
            line.MarkerSize = 0;
            line.LineWidth = 2;
            line.LinePattern = LinePattern.Dashed;

            errorplot.Plot.Title(reg.FormulaWithRSquared);
            errorplot.Refresh();

        }
    }
}


