namespace RelicCharts
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            barPlot = new ScottPlot.WinForms.FormsPlot();
            pieChart = new ScottPlot.WinForms.FormsPlot();
            chanceSplatter = new ScottPlot.WinForms.FormsPlot();
            scatterPlot = new ScottPlot.WinForms.FormsPlot();
            errorPlot = new ScottPlot.WinForms.FormsPlot();
            SuspendLayout();
            // 
            // barPlot
            // 
            barPlot.DisplayScale = 1F;
            barPlot.Location = new Point(12, 52);
            barPlot.Name = "barPlot";
            barPlot.Size = new Size(1058, 572);
            barPlot.TabIndex = 1;
            // 
            // pieChart
            // 
            pieChart.DisplayScale = 1F;
            pieChart.Location = new Point(12, 52);
            pieChart.Name = "pieChart";
            pieChart.Size = new Size(1058, 578);
            pieChart.TabIndex = 0;
            // 
            // chanceSplatter
            // 
            chanceSplatter.DisplayScale = 1F;
            chanceSplatter.Location = new Point(12, 52);
            chanceSplatter.Name = "chanceSplatter";
            chanceSplatter.Size = new Size(1058, 572);
            chanceSplatter.TabIndex = 2;
            // 
            // scatterPlot
            // 
            scatterPlot.DisplayScale = 1F;
            scatterPlot.Location = new Point(12, 43);
            scatterPlot.Name = "scatterPlot";
            scatterPlot.Size = new Size(1058, 572);
            scatterPlot.TabIndex = 3;
            // 
            // errorPlot
            // 
            errorPlot.DisplayScale = 1F;
            errorPlot.Location = new Point(12, 52);
            errorPlot.Name = "errorPlot";
            errorPlot.Size = new Size(1058, 563);
            errorPlot.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1082, 652);
            Controls.Add(errorPlot);
            Controls.Add(scatterPlot);
            Controls.Add(barPlot);
            Controls.Add(pieChart);
            Controls.Add(chanceSplatter);
            Name = "Form1";
            Text = "Relic Statistics";
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot barPlot;
        private ScottPlot.WinForms.FormsPlot pieChart;
        private ScottPlot.WinForms.FormsPlot chanceSplatter;
        private ScottPlot.WinForms.FormsPlot scatterPlot;
        private ScottPlot.WinForms.FormsPlot errorPlot;
    }
}
