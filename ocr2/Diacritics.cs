using System;

namespace ocr2
{
	/// <summary>
	/// Summary description for diacratics.
	/// </summary>
	public class Diacritics
	{
		public string name;
		public int RightMostPoint;
		public int LeftMostPoint;
		public int TopMostPoint;
		public int BottomMostPoint;
		public int location;//1 for above and -1 for below
		public Diacritics()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public Diacritics( string inp_name, int rmp,int lmp,int tmp,int bmp, int pos)
		{
			this.name = (string)inp_name.Clone();
			this.RightMostPoint = rmp;
			this.LeftMostPoint = lmp;
			this.TopMostPoint = tmp;
			this.BottomMostPoint = bmp;
			this.location = pos;
		}
	}
}
