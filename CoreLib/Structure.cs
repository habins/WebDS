using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public class Joints
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public string LeftTop { get; set; }
        public string LeftBottom { get; set; }
        public string RightTop { get; set; }
        public string RightBottom { get; set; }
        public string Default { get; set; }
        public string OptionList { get; set; }
    }
    public class Part
    {
        public string StockNo{get;set;}
        public string Description { get; set; }
        public string PackNum { get; set; }
        public string FormatCode { get; set; }
        public string Wastage { get; set; }
        public string Weight { get; set; }
        public string FullSize { get; set; }
        public string InteriaY { get; set; }
        public string InteriaZ { get; set; }
        public string ShortDescription { get; set; }
    }
    public class Rule
    {
        public string ID { get; set; }
        public string Level { get; set; }
        public string DesignElement { get; set; }
        public string AsmPosition { get; set; }
        public string LengthAdjust { get; set; }
        public string Quantity { get; set; }
        public string Clearance { get; set; }
        public string Text { get; set; }
        public string StockNo { get; set; }
        public string Condition { get; set; }
        public string AssemblingPoints { get; set; }
        public string Ruletype { get; set; }
    }

    public class AsmPair
    {
        public string StockNO{ get; set; }
        public string MJValue{ get; set; }
        public string MJManual{ get; set; }
        public string GValue{ get; set; }
        public string GManual{ get; set; }
        public string SValue{ get; set; }
        public string SManual{ get; set; }
        public string UValue{ get; set; }
        public string UManual{ get; set; }
    }

    public class Vendor
    {
        public string ID{ get; set; }
        public string VendorDescr{ get; set; }
        public string Description{ get; set; }
        public string SystemType{ get; set; }
        public string DataVersion{ get; set; }
        public string VendorID{ get; set; }
    }

    public class Combo
    {
        public string ID { get; set; }
        public string Description { get; set; }
    }
}
