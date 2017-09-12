using System;
using System.Drawing;

namespace ocr2
{
	/// <summary>
	/// Summary description for ImageFile.
	/// </summary>
	public class ImageFile
	{

		public System.Drawing.Bitmap org_File;
		public System.Drawing.Bitmap m_File;
		public byte [,] array;
		public bool isloded = false;
		public Coloumn col1;//a list of coloumn is to placed at this place

		

		public ImageFile()
		{
			//
			// TODO: Add constructor logic here
			//
			
		}

        public void loadImageFile(string file_name)
		{
			this.isloded = true;
			this.org_File = new Bitmap(file_name);//initializing the file
			this. m_File = this.org_File;//making a backup copy of original bitmap file
		}

		public void binarization()
		{
			array = new byte [this.m_File.Height,this.m_File.Width];
			int threshHold = 240;
			System.Drawing.Color imageColor;
			for(int y=0; y< this.m_File.Height ; y++)
			{
				for(int x=0; x<this.m_File.Width ; x++)
				{
					imageColor = this.m_File.GetPixel(x,y);
					if(imageColor.R < threshHold && imageColor.R == imageColor.G && imageColor.R == imageColor.B)
					{
						this.array[y,x] = (byte)0 ;
					}//if
					else
					{
						this.array[y,x] = (byte)1 ;
					}//else
				}//for
			}//for
		}//binarization
        
		public void segmentColoumns()//segment the page into coloumns
		{
			//
			//write code to segment page into coloumns
			//
			
			this.col1 = new Coloumn(this.m_File.Width, this.m_File.Height, array);	
			this.col1.segmentLines();
			this.col1.legature_segmentation();
		} //segmentColoumn
	}
}
