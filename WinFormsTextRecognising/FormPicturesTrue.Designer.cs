
namespace WinFormsTextRecognising
{
    partial class FormPicturesTrue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelNameImage = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.QualityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElaborationDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BestBeforeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetWeightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlantColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LotColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PiecesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNeedWord = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(2, 120);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(625, 482);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(1332, 41);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(425, 314);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Обновить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(83, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(164, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelNameImage
            // 
            this.labelNameImage.AutoSize = true;
            this.labelNameImage.Location = new System.Drawing.Point(288, 21);
            this.labelNameImage.Name = "labelNameImage";
            this.labelNameImage.Size = new System.Drawing.Size(0, 13);
            this.labelNameImage.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QualityColumn,
            this.SizeColumn,
            this.ElaborationDateColumn,
            this.BestBeforeColumn,
            this.NetWeightColumn,
            this.PlantColumn,
            this.LotColumn,
            this.PiecesColumn});
            this.dataGridView1.Location = new System.Drawing.Point(2, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(845, 50);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // QualityColumn
            // 
            this.QualityColumn.HeaderText = "Quality";
            this.QualityColumn.Name = "QualityColumn";
            // 
            // SizeColumn
            // 
            this.SizeColumn.HeaderText = "Size";
            this.SizeColumn.Name = "SizeColumn";
            // 
            // ElaborationDateColumn
            // 
            this.ElaborationDateColumn.HeaderText = "ElaborationDate";
            this.ElaborationDateColumn.Name = "ElaborationDateColumn";
            // 
            // BestBeforeColumn
            // 
            this.BestBeforeColumn.HeaderText = "BestBefore";
            this.BestBeforeColumn.Name = "BestBeforeColumn";
            // 
            // NetWeightColumn
            // 
            this.NetWeightColumn.HeaderText = "NetWeight";
            this.NetWeightColumn.Name = "NetWeightColumn";
            // 
            // PlantColumn
            // 
            this.PlantColumn.HeaderText = "Plant";
            this.PlantColumn.Name = "PlantColumn";
            // 
            // LotColumn
            // 
            this.LotColumn.HeaderText = "Lot";
            this.LotColumn.Name = "LotColumn";
            // 
            // PiecesColumn
            // 
            this.PiecesColumn.HeaderText = "Pieces";
            this.PiecesColumn.Name = "PiecesColumn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(616, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Нужное значение:";
            // 
            // tbNeedWord
            // 
            this.tbNeedWord.Location = new System.Drawing.Point(721, 18);
            this.tbNeedWord.Name = "tbNeedWord";
            this.tbNeedWord.Size = new System.Drawing.Size(136, 20);
            this.tbNeedWord.TabIndex = 10;
            // 
            // FormPicturesTrue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1769, 881);
            this.Controls.Add(this.tbNeedWord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelNameImage);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormPicturesTrue";
            this.Text = "FormPicturesTrue";
            this.Load += new System.EventHandler(this.FormPicturesTrue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelNameImage;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElaborationDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BestBeforeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetWeightColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlantColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LotColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PiecesColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNeedWord;
    }
}