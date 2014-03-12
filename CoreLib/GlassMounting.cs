using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace CoreLib
{
    public class GlassMounting
    {
        public struct GlazingRow
        {
            public string Lowerlimit;
            public string Upperlimit;
            public string Bead;
            public string FSGasket;
            public string BGasket;
        }
        private int index = 0;
        public string profileList;
        public ArrayList GlassRow = new ArrayList();
        public void AddItem(GlassMounting.GlazingRow glassM)
        {
            this.GlassRow.Add(glassM);
        }
        public void AddItem(string strUpperLimit, string strLowerLimit, string strBead, string strFSGasket, string strBGasket)
        {
            GlassMounting.GlazingRow glazingRow = default(GlassMounting.GlazingRow);
            glazingRow.Lowerlimit = strLowerLimit;
            glazingRow.Upperlimit = strUpperLimit;
            glazingRow.Bead = strBead;
            glazingRow.FSGasket = strFSGasket;
            glazingRow.BGasket = strBGasket;
            this.GlassRow.Add(glazingRow);
        }
        public void ClearGlass()
        {
            this.GlassRow.Clear();
        }
        public void DelItem(GlassMounting.GlazingRow glassM)
        {
            this.GlassRow.Remove(glassM);
        }
        public GlassMounting.GlazingRow[] GetRows()
        {
            GlassMounting.GlazingRow[] array = new GlassMounting.GlazingRow[this.GlassRow.Count];
            for (int i = 0; i < this.GlassRow.Count; i++)
            {
                array[i] = (GlassMounting.GlazingRow)this.GlassRow[i];
            }
            return array;
        }
    }
}
