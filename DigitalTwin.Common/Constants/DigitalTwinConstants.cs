using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Common.Constants
{
    public static class DigitalTwinConstants
    {
        public static string Plant_Load = "Plant Load";

        public static List<string> Categories = new List<string>()
        {
                "Platform",
                "Terminal",
                "OPU",
                "Plant",
                "Trading Arm",
                "Customer"
        };

        public static List<string> Exploration_Extractions = new List<string> { "Platform", "Terminal" };
        public static List<string> Processings = new List<string> { "OPU", "Plant", "Trading Arm" };
        public static List<string> Deliveries = new List<string> { "Customer" };
        public static string ProductCateogry = "Product";
        public static string BusinessCateogry = "Business";
        public static List<string> BusinessTab = new List<string> { "OPU", "Plant" };
        public static string BusinessType = "Downstream";
        public static string KPI_ProductionVolume = "Production Volume";
        public static string CustomerName = "Sales Volume";
        public static string BusinessName = "Throughput";
        public static string OPUName = "OPU";
        public static string PlantName = "Plant";
        public static List<int> BusinessLevels = new List<int> { 2, 4, 5 };
        public static List<string> BusinessLevelName = new List<string> { "OPU", "Plant", "Business" };
        public static string KPI_Feedstock = "Inventory Feedstock";
        public static string Downstream = "Downstream";
    }
}
