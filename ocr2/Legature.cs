using System;
using System.Drawing;

namespace ocr2
{
	/// <summary>
	/// Summary description for Legature.
	/// </summary>
	/// 
	public class DcNode
	{
		public string name;
		public int x;
		public DcNode next;
		public DcNode()
		{
			this.next = null;
		}
	};
	public class Legature
	{
		public byte [,] array;
		public int RightMostPoint;
		public int LeftMostPoint;
		public int TopMostPoint;
		public int BottomMostPoint;
		public int legArea;
		DcNode dcList;
		public Legature()
		{
			//
			// TODO: Add constructor logic here
			//
			dcList = null;
		}//Legature
		public Legature(byte [,]inp_array, int rmp,int lmp,int tmp,int bmp, int inp_legArea)
		{
			dcList = null;
			//this.array = (byte[,])inp_array.Clone();
			this.RightMostPoint = rmp;
			this.LeftMostPoint = lmp;
			this.TopMostPoint = tmp;
			this.BottomMostPoint = bmp;
			this.legArea = inp_legArea;

			this.array = new byte[bmp-tmp+1,rmp-lmp+1];
			for(int i=0;i<bmp-tmp+1; i++)
				for(int j=0; j<rmp-lmp+1; j++)
					this.array[i,j] = inp_array[i,j];
			//this.show();

		}//Legature

		public void addDiacritic(Diacritics inpDc)
		{
			DcNode temp = new DcNode();
			temp.name = (string)inpDc.name.Clone();
			if(inpDc.location < 0)
				temp.name = string.Concat(temp.name, " below");
			else
				temp.name = string.Concat(temp.name, " above");
			temp.x = (inpDc.RightMostPoint - inpDc.LeftMostPoint)/2+inpDc.LeftMostPoint;
			
			if(this.dcList == null)
			{
				this.dcList = temp;
			}
			else
			{
				DcNode iterator = this.dcList;
				if(temp.x > iterator.x)
				{
					temp.next = iterator;
					this.dcList = temp;
				}
				else
				{
					while(iterator.next != null)
					{
						if(temp.x > iterator.next.x)
						{
							temp.next = iterator.next;
							iterator.next = temp;
							break;
						}
						iterator = iterator.next;
					}//while
					if(iterator.next == null )
						iterator.next = temp;
				}//else
			}//else
		}//addDiacritic()

		public void show()
		{
			Form2 F = new Form2();
			int width = this.RightMostPoint-this.LeftMostPoint+1;
			int height= this.BottomMostPoint-this.TopMostPoint+1;
			Bitmap bmp = new System.Drawing.Bitmap(width, height);
			for(int i=0; i<height;i++)
			{
				for(int j=0;j<width;j++)
				{
					if(this.array[i,j]==1)
						bmp.SetPixel(j,i,System.Drawing.Color.White);
					else
						bmp.SetPixel(j,i,System.Drawing.Color.Black);
				}//for
			}//for
			F.pictureBox1.Image = bmp;
			F.pictureBox1.Show();
			
			DcNode temp = this.dcList;
			string dots = " ";
			while(temp != null)
			{
				dots = string.Concat(dots, " , ", temp.name);
				temp = temp.next;
			}//while

			F.textBox1.Text = dots;
			F.ShowDialog();
		}//show()
	}//class Legature
}
