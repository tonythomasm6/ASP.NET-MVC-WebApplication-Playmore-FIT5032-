using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayMore_V5._0.Models
{
    public class ChartPoint
    {
        public ChartPoint(String Label, int X, int Y)
        {
            this.Label = Label;
            this.X = X;
            this.Y = Y;
        }

        
        public string Label { get; set; }
        
        public int X { get; set; }

        public int Y { get; set; }
    }
}