using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace ocr2
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class master_ocr : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button load;
		private System.Windows.Forms.Button process;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button exit;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		
		//       		
		//user defined variable
		//

		private ImageFile bmp_file ;// loaded file that is to be processed

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public master_ocr()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			
			this.bmp_file = new ImageFile(); 
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.load = new System.Windows.Forms.Button();
			this.process = new System.Windows.Forms.Button();
			this.exit = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// load
			// 
			this.load.Location = new System.Drawing.Point(72, 32);
			this.load.Name = "load";
			this.load.TabIndex = 0;
			this.load.Text = "load";
			this.load.Click += new System.EventHandler(this.load_Click);
			// 
			// process
			// 
			this.process.Location = new System.Drawing.Point(192, 32);
			this.process.Name = "process";
			this.process.TabIndex = 1;
			this.process.Text = "process";
			this.process.Click += new System.EventHandler(this.process_Click);
			// 
			// exit
			// 
			this.exit.Location = new System.Drawing.Point(304, 32);
			this.exit.Name = "exit";
			this.exit.TabIndex = 2;
			this.exit.Text = "exit";
			this.exit.Click += new System.EventHandler(this.exit_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pictureBox1.Location = new System.Drawing.Point(24, 88);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(192, 248);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// master_ocr
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 373);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.exit);
			this.Controls.Add(this.process);
			this.Controls.Add(this.load);
			this.Name = "master_ocr";
			this.Text = "master ocr";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new master_ocr());
		}

		private void load_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog1.ShowDialog();
			if(this.openFileDialog1.FileName.Length > 3)
			{
				this.bmp_file.loadImageFile(this.openFileDialog1.FileName); 
				this.pictureBox1.Image = bmp_file.m_File;
				this.pictureBox1.Show();
			}
		}

		private void exit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void process_Click(object sender, System.EventArgs e)
		{
			if(!this.bmp_file.isloded)
			{
				System.Windows.Forms.MessageBox.Show("No file has been loaded Yet. Kindly first load the (monocrone) *.bmp file" );
				return;
			}
			bmp_file.binarization();
			bmp_file.segmentColoumns();
//			this.pictureBox1.Image = bmp_file.col1.colImage;
//			this.pictureBox1.Show();
//
//			bmp_file.col1.segmentLines();
//			Line line ;
//			line = new Line((Line)bmp_file.col1.lineList.first.data);
//			this.pictureBox1.Image = /*bmp_file.col1.tempLine.lineImage;//*/line.lineImage;
//			this.pictureBox1.Show();

			/*
			for (int i = 0; i < 2; i++)
			{
				for(int j=0;j<2;j++)
				{
					this.dataGrid1.SetBounds(0,0,2,2);
				}//Console.WriteLine("value at row {0} , {1} is {2}", i, j, test[i,j]);
			}*/

		}

						
	}
}
