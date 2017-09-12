using System;
using System.Drawing;

namespace ocr2
{
	/// <summary>
	/// Summary description for Coloumn.
	/// </summary>
	public class Coloumn
	{
		public System.Drawing.Bitmap colImage;
		public byte[,] array;
		public int height;
		public int width;
		public List lineList;//list of lines

		public Coloumn( int inpWidth, int inpHeight, byte[,] input)
		{
			//
			// TODO: Add constructor logic here
			//
			this.lineList = new List();
			this.height = inpHeight;
			this.width = inpWidth;
			
			//array = new byte[height, width];
			this.array = (byte[,])input.Clone();
			
			colImage = new Bitmap(width, height);
			
			for(int x=0; x<width; x++)
			{
				for(int y=0; y<height; y++)
				{
					if(this.array[y,x] == 0)
						colImage.SetPixel(x, y, System.Drawing.Color.Black);
					else
						colImage.SetPixel( x,y, System.Drawing.Color.Red);
				}//for

			}//for
		}//cunstructor

		public void segmentLines()
		{
			Line tempLine;
			byte[,] tempLineArray ;
			int [,] lineWidth = new int [153,3];
			int index = 0;
			
			bool isblack = false;//isblack = true if line contains black pixel
			bool wasPrevBlack = false;
			int blackCount =0;
			int baseLine = 0;
			
			for(int y =0 ; y<this.colImage.Height; y++)
			{
				isblack = false;
				blackCount = 0;
				for(int x = 0; x<this.colImage.Width; x++)
				{
					if(array[y,x] == 0)
					{
						blackCount ++;
						if(blackCount > 2)
							isblack = true;
					}
				}//for
				if(blackCount > 2 && blackCount > baseLine )//finding baseline
				{
					baseLine = blackCount;
					lineWidth[index,2] = y;
				}
				if(isblack && !wasPrevBlack)//finding the top of text line
				{
					lineWidth[index,0] = y;
				}
				else if(!isblack && wasPrevBlack)//defining the base of a text line
				{
					lineWidth[index, 1] = y;
					index++;
					baseLine = 0; 
				}

				wasPrevBlack = isblack;
			}//for
			for(int i=0; i < index; i++)
			{
				tempLineArray = new byte[lineWidth[i,1] - lineWidth[i,0]+1,this.width ];
				int ty = lineWidth[i,0];
				
				for( int y=0  ; ty < lineWidth[i,1]; y++, ty++)
					for(int x=0; x<this.width; x++)				
						tempLineArray[y,x] = this.array[ty,x];
				
				tempLine = new Line(lineWidth[i,1] - lineWidth[i,0]+1,this.width , lineWidth[i,2] - lineWidth[i,0], tempLineArray);
				
				this.lineList.addObject(tempLine);
		
			}//for
			
		}//segmentLines()

		public void legature_segmentation()
		{
			
			Node temp = this.lineList.first;
			while(temp != null)
			{
				((Line)temp.data).labeleLegature();
				((Line)temp.data).relateDiacriticsToLegatures();
				temp = temp.next;
			}
		}
	}//class coloumn
}
