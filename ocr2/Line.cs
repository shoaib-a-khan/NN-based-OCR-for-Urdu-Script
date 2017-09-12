using System;
using System.Drawing;
using System.IO;

namespace ocr2
{
	/// <summary>
	/// Summary description for Line.
	/// </summary>
	/// 

	struct SampleDiacritic
	{
		public byte [,] array;
		public string name;
		public int legArea;
		public int width;
		public int height;

	};
	
	public class Line
	{
		public byte [,] array;
		public System.Drawing.Bitmap lineImage;
		public int baseLine;
		public int height;
		public int width;
		public List ligatureLsit;
		public List diacritics;
		public List D_templates;
		public int maxD_size = 0;
		public int maxD_width = 0;
		public int maxD_height = 0;
		public void initialize(int inpHeight, int inpWidth, int inpBaseLine, byte [,] input)
		{
			
			this.height = inpHeight;
			this.width = inpWidth;
			this.baseLine = inpBaseLine;

			this.ligatureLsit = new List();
			this.D_templates = new List();
			this.diacritics = new List();

			array = (byte[,])input.Clone();

			lineImage = new Bitmap(this.width,this.height);
            
			for(int x=0; x<width; x++)
			{
				for(int y=0; y<height; y++)
				{
					if(this.array[y,x] == 0)
						lineImage.SetPixel(x, y, System.Drawing.Color.Black);
					else
						lineImage.SetPixel( x,y, System.Drawing.Color.Red);
				}//for

			}//for
		}


		public Line(int inpHeight, int inpWidth, int inpBaseLine, byte [,] input)
		{
			//
			// TODO: Add constructor logic here
			//
			initialize( inpHeight,  inpWidth, inpBaseLine, input);

		}//constructor
	
		public Line(Line input)
		{
			this.initialize(input.height, input.width, input.baseLine, input.array);
		}

		public void labeleLegature()
		{
			Legature T_Leg;
			Diacritics T_Diacritic;
			string temp = null;
			byte label = 2;
			int i = 0;
			int j = 0;
			int lmp;//left most point
			int rmp;//right most point
			int tmp;//top most point
			int bmp;//bottom most point
			int legArea = 0;//legatre area i.e. total number of dots forming legature
			int baseBand = 10;
			
			Form2 F = new Form2();//to b deleted if necessory

			loadDiacriticalTemplates();

			//iterate in the base band to seperate the main body legatures in their order of existance
			for(j = this.width-2; j > 0; j--)
			{
				for(i = this.baseLine-baseBand; i <= this.baseLine ; i++)
				{
					if(this.array[i,j] == 0)
					{
						rmp = j;
						lmp = j;
						tmp = i;
						bmp = i;
						legArea = 0;
						_8ConnectedMethod(j, i, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
						temp = null;
						if(legArea <= this.maxD_size && (bmp-tmp) <= this.maxD_height && (rmp-lmp)<= this.maxD_width)
						{
							temp = compareDiacritic( label, rmp, lmp, tmp, bmp);
							if(temp.CompareTo("mad")==0 || temp.CompareTo("hamza")==0)
							{
								T_Leg = new Legature(getmainBody(label,rmp,lmp,tmp,bmp), rmp, lmp, tmp, bmp,legArea);	
								this.ligatureLsit.addObject(T_Leg);
							}//if
							else
							{
								bool _3dFlag = false;
								if(temp.CompareTo("one dot")==0 || temp.CompareTo("two dot")==0)
								{
									_3dFlag = check_and_set_threedots(temp, rmp, lmp, tmp, bmp);
								}//if
								if(_3dFlag == false)
								{
									if(temp.CompareTo("shad")==0)//shad always comes above
										T_Diacritic = new Diacritics(temp, rmp, lmp, tmp, bmp,1);
									else
										T_Diacritic = new Diacritics(temp, rmp, lmp, tmp, bmp,getPosition(label,(rmp-lmp)/2+lmp,(bmp-tmp)/2+tmp));
									this.diacritics.addObject(T_Diacritic);
								}//if
							}//else
						}//if
						else
						{
							T_Leg = new Legature(getmainBody(label,rmp,lmp,tmp,bmp), rmp, lmp, tmp, bmp,legArea);	
							this.ligatureLsit.addObject(T_Leg);
						}//else
/*						Bitmap leg = new System.Drawing.Bitmap(1+rmp-lmp,1+bmp-tmp);
						int w=lmp;
						int h=tmp;
						for(int l=0;l<leg.Width;l++)
						{
							h=tmp;
							for(int m = 0; m<leg.Height;m++)
							{
								if(this.array[h,w]==label)
									leg.SetPixel(l,m,System.Drawing.Color.Black);
								else 
									leg.SetPixel(l,m,System.Drawing.Color.White);
								h++;
							}//for
							w++;
						}//for
						
						F.pictureBox1.Image = leg;
						F.pictureBox1.Show();
						F.textBox1.Text = temp;
						F.ShowDialog();
*/
						label++;
					}//else if
				}//for
			}//for
			
			//iterate in the whole line to seperate diacritical legatures that were missed in above iteration
			for(j = this.width-2; j > 0; j--)
			{
				for(i = 1; i < this.height-1 ; i++)
				{
					if(this.array[i,j] == 0)
					{
						rmp = j;
						lmp = j;
						tmp = i;
						bmp = i;
						legArea = 0;
						_8ConnectedMethod(j, i, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
						temp = null;
						if(legArea <= this.maxD_size && (bmp-tmp) <= this.maxD_height && (rmp-lmp)<= this.maxD_width)
						{
							bool _3dFlag = false;
							temp = compareDiacritic( label, rmp, lmp, tmp, bmp);
							if(temp.CompareTo("one dot")==0 || temp.CompareTo("two dot")==0)
							{
								_3dFlag = check_and_set_threedots( temp, rmp, lmp, tmp, bmp);
							}
							if(_3dFlag == false)
							{
								if(temp.CompareTo("shad")==0)//shad always comes above
									T_Diacritic = new Diacritics(temp, rmp, lmp, tmp, bmp,1);
								else
									T_Diacritic = new Diacritics(temp, rmp, lmp, tmp, bmp,this.getPosition(label,(rmp-lmp)/2+lmp, (bmp-tmp)/2+tmp));
								this.diacritics.addObject(T_Diacritic);
							}//if
							
						}//if
						else
						{
							T_Leg = new Legature(getmainBody(label,rmp,lmp,tmp,bmp), rmp, lmp, tmp, bmp,legArea);	
							this.ligatureLsit.addObject(T_Leg);
						}//else
/*						Bitmap leg = new System.Drawing.Bitmap(1+rmp-lmp,1+bmp-tmp);
						int w=lmp;
						int h=tmp;
						for(int l=0;l<leg.Width;l++)
						{
							h=tmp;
							for(int m = 0; m<leg.Height;m++)
							{
								if(this.array[h,w]==label)
									leg.SetPixel(l,m,System.Drawing.Color.Black);
								else 
									leg.SetPixel(l,m,System.Drawing.Color.White);
								h++;
							}//for
							w++;
						}//for
						
						F.pictureBox1.Image = leg;
						F.pictureBox1.Show();
						F.textBox1.Text = temp;
						F.ShowDialog();
*/
						label++;
					}//else if
				}//for
			}//for

		}//labelLegature

		private void _8ConnectedMethod(int x, int y,byte label,ref int rmp,ref int lmp,ref int tmp,ref int bmp,ref int legArea)
		{
			if(x > 0 && x < this.width-1 && y > 0 && y < this.height-1)
			{
				
				if(this.array[y-1,x-1]==0)
				{
					this.array[y-1,x-1] = label;
					_8ConnectedMethod(x-1, y-1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);

					if(lmp > x-1) lmp = x-1;//to find the left most point
					if(tmp > y-1) tmp = y-1;//to find the top most point
				}
				if(this.array[y-1,x] == 0 )
				{
					this.array[y-1,x] = label;
					_8ConnectedMethod(x, y-1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);

					if(tmp > y-1 ) tmp = y-1;
				}	
				if(this.array[y-1,x+1] == 0)
				{
					this.array[y-1,x+1] = label;
					_8ConnectedMethod(x+1 , y-1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);

					if(rmp < x+1) rmp = x+1;//to find the right most point
					if(tmp > y-1) tmp = y-1;
				}
				if(this.array[y,x+1] == 0)
				{
					this.array[y,x+1] = label;
					_8ConnectedMethod(x+1,y,label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(rmp < x+1) rmp = x+1;
				}
				if( this.array[y+1,x+1] == 0)
				{
					this.array[y+1,x+1] = label;
					_8ConnectedMethod(x+1, y+1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(rmp < x+1) rmp = x+1;
					if(bmp < y+1) bmp = y+1;//to find the bottom most point
				}
				if( this.array[y+1,x] == 0)
				{
					this.array[y+1,x] = label;
					_8ConnectedMethod(x, y+1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(bmp < y+1) bmp = y+1;
				}
				if( this.array[y+1,x-1] == 0)
				{
					this.array[y+1,x-1] = label;
					_8ConnectedMethod(x-1, y+1, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(bmp < y+1) bmp = y+1;
					if(lmp > x-1) lmp = x-1;
				}
				if(this.array[y,x-1] == 0)
				{
					this.array[y,x-1] = label;
					_8ConnectedMethod(x-1, y, label,ref rmp,ref lmp,ref tmp,ref bmp, ref legArea);
					
					if(lmp > x-1) lmp = x-1;
				}
				
				legArea++;
			}//if base case
						
		}//_8ConnectedMethod()

		private string compareDiacritic(int label,int rmp, int lmp,int tmp,int bmp)
		{
			int M_of_Similarity = 0;
			int i=0;
			int j=0;
			int l=0;
			int m=0;

			byte [,] temp = this.getmainBody(label,rmp,lmp,tmp,bmp);//new byte[bmp-tmp+1,rmp-lmp+1];
/*			for(i=0, l=tmp; l<bmp+1; l++,i++)
			{
				for(j=0, m=lmp; m<rmp+1; m++, j++)
				{
					if(array[l,m]==label)
						temp[i,j] = 0;
					else 
						temp[i,j] = 1;
				}//for
			}//for
*/
			int index_of_mostSimilar = -1;//remembers the index of most similar diacritic
			int temp_M_of_Similarity = 0;
			int index = 0;//count for itrator
			Node iterator = this.D_templates.first;
			SampleDiacritic smpl;
			while(iterator != null)
			{
				smpl = (SampleDiacritic)iterator.data;
				temp_M_of_Similarity = 0;
				for(i=0,l=0; i<smpl.height && l<bmp-tmp+1; i++,l++)
				{
					for(j=0,m=0; j<smpl.width && m<rmp-lmp+1; j++,m++)
					{
						if(smpl.array[i,j]==temp[l,m])
							temp_M_of_Similarity++;
						else 
							temp_M_of_Similarity--;
					}//for
				}//for
				int hDiff = smpl.height-(bmp-tmp);
				int wDiff = smpl.width-(rmp-lmp);
				if(hDiff >=0 && wDiff >=0)
				{
					temp_M_of_Similarity = temp_M_of_Similarity - (hDiff*smpl.width + wDiff*(bmp-tmp));
				}
				else if(hDiff <=0 && wDiff <=0)
				{
					hDiff = hDiff *(-1);
					wDiff = wDiff *(-1);
					temp_M_of_Similarity = temp_M_of_Similarity - (hDiff*(rmp-lmp)+wDiff*smpl.height);
				}
				else if(hDiff > 0 && wDiff < 0)
				{
					wDiff = wDiff * (-1);
					temp_M_of_Similarity = temp_M_of_Similarity - (hDiff*smpl.width + wDiff*(bmp-tmp));
				}
				else if(hDiff < 0 && wDiff > 0)
				{
					hDiff = hDiff * (-1);
					temp_M_of_Similarity = temp_M_of_Similarity - (hDiff*(rmp-lmp + wDiff*smpl.height));
				}

				if(M_of_Similarity<temp_M_of_Similarity || index_of_mostSimilar==-1)
				{
					M_of_Similarity = temp_M_of_Similarity;
					index_of_mostSimilar = index;
				}
				iterator = iterator.next;
				index++;
			}//while
			iterator = this.D_templates.first;
			for(i=0;i<index_of_mostSimilar;i++)
				iterator = iterator.next;
			return ((SampleDiacritic)(iterator.data)).name;

		}//compare diacritic()

		public byte[,] getmainBody(int label,int rmp, int lmp,int tmp,int bmp)
		{
			int x=0;
			int y=0;
			int l=0;
			int m=0;
			byte[,] sample = new byte[bmp-tmp+1, rmp-lmp+1];
			for (x=0, l=lmp ;x<rmp-lmp+1;x++, l++)
			{
				for (y=0,  m=tmp; y<bmp-tmp+1; y++,m++)
				{
					if(this.array[m,l]==label)
						sample[y,x] = 0;
					else
						sample[y,x] = 1;
				}//for
			}//for
			return sample;						
		}//getmainBody()

		public bool check_and_set_threedots(string temp,int rmp,int lmp,int tmp,int bmp)
		{
			Node iterator = this.diacritics.first;
			Diacritics dtemp;
			while(iterator != null)
			{
				dtemp = (Diacritics)iterator.data;
				if(dtemp.name.CompareTo("one dot")==0 && temp.CompareTo("two dots")==0)
				{
					if(dtemp.LeftMostPoint >lmp && dtemp.RightMostPoint <rmp)
					{
						if(dtemp.TopMostPoint < tmp)
						{
							dtemp.BottomMostPoint = bmp;
							dtemp.location = 1;
						}
						else if(dtemp.BottomMostPoint > bmp)
						{
							dtemp.TopMostPoint = tmp;
							dtemp.location = -1;
						}
						dtemp.LeftMostPoint = lmp;
						dtemp.RightMostPoint = rmp;
						dtemp.name = "three dots";
						return true;
					}//if
				}//if
				else if(dtemp.name.CompareTo("two dot")==0 && temp.CompareTo("one dot")==0)
				{
					if(dtemp.LeftMostPoint <lmp && dtemp.RightMostPoint > rmp)
					{
						if(dtemp.TopMostPoint < tmp)
						{
							dtemp.BottomMostPoint = bmp;
							dtemp.location = -1;
						}
						else if(dtemp.BottomMostPoint > bmp)
						{
							dtemp.TopMostPoint = tmp;
							dtemp.location = 1;
						}
						dtemp.name = "three dots";
						return true;
					}//if
				}//else
				iterator = iterator.next;
			}//while
			return false;
		}//check_and_set_threedots

		public int getPosition(int label, int centerX,int centerY)
		{
			int i;
			int j;
			for(i=centerY; i>0 && this.array[i,centerX]!= label; i--);//to come out of its own body
			for(i=centerY; i>0 && this.array[i,centerX]!= 1; i--);//to find next body
			for(j=centerY; j<this.height && this.array[i,centerX]!= label; j++);
			for(j=centerY; j<this.height && this.array[j,centerX]!= 1; j++);
			if(centerY-i < j-centerY && this.array[i,centerX]!=1)
				return 1;
			//if(centerY-i > j-centerY && this.array[centerX,j]!=1)
				return -1;
		}

		public void relateDiacriticsToLegatures()
		{
			int center;
			Node itrDc = this.diacritics.first;
			Node itrLeg;// = this.ligatureLsit.first;
			Legature tempLeg;
			Diacritics tempDc;
			while(itrDc != null)
			{
				tempDc = (Diacritics)itrDc.data;
				itrLeg = this.ligatureLsit.first;
				while(itrLeg != null)
				{
					//itrLeg = this.ligatureLsit.first;
					tempLeg = (Legature)itrLeg.data;
					center = (tempDc.RightMostPoint-tempDc.LeftMostPoint)/2+tempDc.LeftMostPoint;
					if(tempLeg.LeftMostPoint < center && tempLeg.RightMostPoint > center)
					{
						tempLeg.addDiacritic(tempDc);
						break;
					}
					itrLeg = itrLeg.next;
				}//while
				itrDc = itrDc.next;
			}//while

			//display all legatures
			itrLeg = this.ligatureLsit.first;
			while(itrLeg != null)
			{
				((Legature)itrLeg.data).show();
				itrLeg = itrLeg.next;
			}//while
		}//relateDiacriticsToLegatures()

        public void loadDiacriticalTemplates()
		{
			SampleDiacritic smpl = new SampleDiacritic();
			FileStream fs = new FileStream("e:\\specialFeatures.txt", System.IO.FileMode.Open);
			StreamReader sr = new StreamReader(fs);
			string line;
			while((smpl.name = sr.ReadLine()) != null)
			{
				smpl.legArea = int.Parse(sr.ReadLine());
				smpl.height = int.Parse(sr.ReadLine());
				smpl.width = int.Parse(sr.ReadLine());
				smpl.array = new byte[smpl.height, smpl.width];
			
				for (int i=0; i<smpl.height;i++)
				{
					line = sr.ReadLine();
					line.ToCharArray();
					for(int j=0;j<smpl.width; j++)
					{
						smpl.array[i,j] = (byte)((int)line[j]-48);
					}
				}//for

				this.D_templates.addObject(smpl);

				if(this.maxD_size < smpl.legArea)
					this.maxD_size = smpl.legArea;
				if(this.maxD_width < smpl.width)
					this.maxD_width = smpl.width;
				if(this.maxD_height < smpl.height)
					this.maxD_height = smpl.height;
			}//while
			sr.Close();
		}//loadDiacriticalTemplates()
		
	}//Line class
}//ocr2
