using System;
using System.IO;
using System.Drawing;

namespace save_template
{
	/// <summary>
	/// Summary description for template.
	/// </summary>
	public class Template
	{
		int [,] array;
		int [,] sample ;
		int S_width;
		int S_Height;
		int legArea;
		string name;

		public Template(System.Drawing.Bitmap image)
		{
			//
			// TODO: Add constructor logic here
			//
			
			array = new int[image.Height, image.Width];
			
			for(int i=0; i<image.Height;i++)
			{
				for(int j=0;j<image.Width;j++)
				{
					if(image.GetPixel(j,i).R < 240)
						array[i,j] = 0;
					else 
						array[i,j] = 1;
				}
			}

			toArray( image.Width, image.Height);
		}

		private void toArray( int Width, int Height)
		{
			Form2 dlg = new Form2();

			int rmp;
			int lmp;
			int tmp;
			int bmp;
			int legArea=0;
			for(int j = Width-2; j > 0; j--)
			{
				for(int i = 1; i < Height-1 ; i++)
				{
					if(array[i,j] == 0)
					{
						rmp = j;
						lmp = j;
						tmp = i;
						bmp = i;
						legArea = 0;
						_8ConnectedMethod(Width, Height,j, i, 2,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
						this.sample = new int[bmp-tmp+1, rmp-lmp+1];
						this.S_Height = bmp-tmp+1;
						this.S_width = rmp-lmp+1;
						this.legArea = legArea;

						for (int x=0, l=lmp ;x<rmp-lmp+1;x++, l++)
						{
							for (int y=0,  m=tmp; y<bmp-tmp+1; y++,m++)
							{
								if(array[m,l]==2)
									this.sample[y,x] = 0;
								else
									this.sample[y,x] = 1;
								
							}//for
						}//for
						dlg.display_bitmap(this.sample,this.S_Height,this.S_width);
						dlg.ShowDialog();
						this.name = dlg.textBox1.Text;
						this.saveToFile();
					}//if
				}//for
			}//for
		}//toArray

		private void _8ConnectedMethod(int width, int height ,int x, int y,byte label,ref int rmp,ref int lmp,ref int tmp,ref int bmp,ref int legArea)
		{
			if(x > 0 && x < width-1 && y > 0 && y < height-1)
			{
				
				if(array[y-1,x-1]==0)
				{
					array[y-1,x-1] = label;
					_8ConnectedMethod(width, height,x-1, y-1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);

					if(lmp > x-1) lmp = x-1;//to find the left most point
					if(tmp > y-1) tmp = y-1;//to find the top most point
				}
				if(array[y-1,x] == 0 )
				{
					array[y-1,x] = label;
					_8ConnectedMethod(width, height,x, y-1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);

					if(tmp > y-1 ) tmp = y-1;
				}	
				if(array[y-1,x+1] == 0)
				{
					array[y-1,x+1] = label;
					_8ConnectedMethod(width, height, x+1 , y-1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);

					if(rmp < x+1) rmp = x+1;//to find the right most point
					if(tmp > y-1) tmp = y-1;
				}
				if(array[y,x+1] == 0)
				{
					array[y,x+1] = label;
					_8ConnectedMethod( width, height,x+1,y,label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(rmp < x+1) rmp = x+1;
				}
				if( array[y+1,x+1] == 0)
				{
					array[y+1,x+1] = label;
					_8ConnectedMethod(width, height,x+1, y+1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(rmp < x+1) rmp = x+1;
					if(bmp < y+1) bmp = y+1;//to find the bottom most point
				}
				if( array[y+1,x] == 0)
				{
					array[y+1,x] = label;
					_8ConnectedMethod(width, height,x, y+1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(bmp < y+1) bmp = y+1;
				}
				if( array[y+1,x-1] == 0)
				{
					array[y+1,x-1] = label;
					_8ConnectedMethod(width, height,x-1, y+1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(bmp < y+1) bmp = y+1;
					if(lmp > x-1) lmp = x-1;
				}
				if(array[y,x-1] == 0)
				{
					array[y,x-1] = label;
					_8ConnectedMethod(width, height,x-1, y, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(lmp > x-1) lmp = x-1;
				}
				
				legArea++;
			}//if base case
						
		}//_8ConnectedMethod()

		public void saveToFile()
		{
			string fileName = "e:\\specialFeatures.txt";
			 FileStream fs = new FileStream(fileName,FileMode.Append, FileAccess.Write, FileShare.None);
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine(this.name);
			sw.WriteLine(this.legArea);
			sw.WriteLine(this.S_Height);
			sw.WriteLine(this.S_width);

			for(int i=0; i<this.S_Height;i++)
			{
				for (int j=0; j<this.S_width; j++)
				{	
					sw.Write(this.sample[i,j]);
					//sw.Write(" ");
				}
				sw.Write(sw.NewLine);
			}

			sw.Flush();
			sw.Close();


		}
	}
}
