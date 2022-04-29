using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTextRecognising
{
    public partial class FormPicturesTrue : Form
    {
        public Image NeedAPicture;
        public string NamePicture = string.Empty;
        public Bitmap AllPicure = new Bitmap(500, 500);
        public List<RectangleW> ListHighlightedRectangles = new List<RectangleW>();
        public Dictionary<string, List<Point>> ArbitraryArea = new Dictionary<string, List<Point>>();
        public Dictionary<string, Point> MinPoints = new Dictionary<string,Point>();
        private string columnName = string.Empty;
        private Image image;
        private List<Image> listImages = new List<Image>();
        //private List<Point> Points = null;
        //private bool Drawing = false;
        private Point start;
        private Point end;

        public FormPicturesTrue()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            var img = ResizeImage(Image.FromFile(fileName), 1080, 1080);
            image = img;

            pictureBox1.Image = image;
            pictureBox2.Image = null;
            NeedAPicture = null;
            FileInfo test = new FileInfo(fileName);
            ArbitraryArea.Clear();
            ListHighlightedRectangles.Clear();
            MinPoints.Clear();
            dataGridView1.Rows.Clear();

            using (Graphics g = Graphics.FromImage(AllPicure))
            {
                g.Clear(Color.White);
            }

            NamePicture = test.Name;
            labelNameImage.Text = test.Name;
        }

        private Image ResizeImage(Image img, decimal maxWidth, decimal maxHeight)
        {
            decimal srcWidth = img.Width;
            decimal srcHeight = img.Height;

            decimal resizeWidth = srcWidth;
            decimal resizeHeight = srcHeight;

            decimal aspect = resizeWidth / resizeHeight;

            if (resizeWidth > maxWidth)
            {
                resizeWidth = maxWidth;
                resizeHeight = resizeWidth / aspect;
            }
            if (resizeHeight > maxHeight)
            {
                aspect = resizeWidth / resizeHeight;
                resizeHeight = maxHeight;
                resizeWidth = resizeHeight * aspect;
            }

            return (Image)(new Bitmap(img, new Size((int)resizeWidth, (int)resizeHeight)));
        }

        //private Bitmap MakeImageWithAreaArbitrarySelection(Bitmap source_bm, List<Point> points)
        //{
        //    Bitmap bm = new Bitmap(source_bm.Width, source_bm.Height);

        //    using (Graphics gr = Graphics.FromImage(bm))
        //    {
        //        gr.Clear(Color.Transparent);

        //        using (Brush brush = new TextureBrush(source_bm))
        //        {
        //            gr.FillPolygon(brush, points.ToArray());
        //        }
        //    }

        //    return bm;
        //}

        public Bitmap MakeImageWithAreaRectangle(Bitmap source_bm, Rectangle rectangle)
        {
            Bitmap bm = new Bitmap(source_bm.Width, source_bm.Height);

            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.Clear(Color.Transparent);

                using (Brush brush = new TextureBrush(source_bm))
                {
                    gr.FillRectangle(brush, rectangle);
                }
            }

            return bm;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //if (!checkBox1.Checked)
            //{
            if (!string.IsNullOrEmpty(tbNeedWord.Text) || columnName.Contains("Net"))
            {
                start = end = e.Location;
                pictureBox1.Invalidate();
            }
            //}
            //else
            //{
            //    Points = new List<Point>();
            //    Drawing = true;
            //}
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!checkBox1.Checked)
            //{
            if (!string.IsNullOrEmpty(tbNeedWord.Text) || columnName.Contains("Net"))
            {
                if (e.Button == MouseButtons.Left)
                {
                    end = e.Location;
                    pictureBox1.Invalidate();
                }
            }
            //}
            //else
            //{
            //    if (!Drawing) return;
            //    Points.Add(new Point(e.X, e.Y));
            //    pictureBox1.Invalidate();
            //}
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //if (!checkBox1.Checked)
            //{
            if (!string.IsNullOrEmpty(tbNeedWord.Text) || columnName.Contains("Net"))
            {
                if (start != end)
                {
                    var text = tbNeedWord.Text.ToLower();
                    using (Graphics g = Graphics.FromImage(AllPicure))
                    {
                        g.Clear(Color.White);
                    }

                    var rec = PointsToRect(start, end);
                    Bitmap without_area = MakeImageWithAreaRectangle((Bitmap)pictureBox1.Image, rec);
                    var xMin = Math.Min(start.X, end.X);
                    var yMin = Math.Min(start.Y, end.Y);

                    var size = new Size(without_area.Width / 4, without_area.Height / 4);
                    Rectangle compressionRectangle = new Rectangle(xMin / 4, yMin / 4, size.Width, size.Height);

                    if (listImages.Count != 0 && ListHighlightedRectangles.Count != 0)
                    {
                        for (var i = 0; i <= ListHighlightedRectangles.Count - 1; i++)
                        {
                            if (ListHighlightedRectangles[i].NameColumn.Contains(columnName))
                            {
                                ListHighlightedRectangles.RemoveAt(i);
                                listImages.RemoveAt(i);
                            }
                        }
                        listImages.Add(without_area);
                        ListHighlightedRectangles.Add(new RectangleW(new Point(xMin, yMin), rec.Width, rec.Height, text, columnName));
                    }
                    else
                    {
                        listImages.Add(without_area);
                        ListHighlightedRectangles.Add(new RectangleW(new Point(xMin, yMin), rec.Width, rec.Height, text, columnName));
                    }

                    foreach (var img in listImages)
                    {
                        using (Graphics g = Graphics.FromImage(AllPicure))
                        {
                            g.DrawImage(img, compressionRectangle);
                        }
                    }

                    start = end;
                    pictureBox2.Image = AllPicure;
                    pictureBox2.Refresh();

                    if (columnName.Contains("Net"))
                        dataGridView1.Rows[0].Cells[columnName].Value = "+";
                    else
                        dataGridView1.Rows[0].Cells[columnName].Value = text;

                    tbNeedWord.Text = string.Empty;
                }
            }   
            //}
            //else
            //{
            //    Drawing = false;
            //    if (Points == null) return;

            //    Bitmap without_area_bitmap = MakeImageWithAreaArbitrarySelection((Bitmap)pictureBox1.Image, Points);
            //    var minX = Points[0].X;
            //    var minY = Points[0].Y;

            //    foreach (var point in Points)
            //    {
            //        if (point.X < minX)
            //            minX = point.X;

            //        if (point.Y < minY)
            //            minY = point.Y;
            //    }

            //    Rectangle compressionRectangle = new Rectangle(minX / 4, minY / 4, without_area_bitmap.Width / 4, without_area_bitmap.Height / 4);

            //    using (Graphics g = Graphics.FromImage(AllPicure))
            //    {
            //        g.DrawImage(without_area_bitmap, compressionRectangle);
            //    }

            //    if (ArbitraryArea.ContainsKey(columnName))
            //    {
            //        ArbitraryArea[columnName] = Points;
            //        MinPoints[columnName] = new Point(minX, minY);
            //    }
            //    else
            //    {
            //        ArbitraryArea.Add(columnName, Points);
            //        MinPoints.Add(columnName, new Point(minX, minY));
            //    }

            //    dataGridView1.Rows[0].Cells[columnName].Value = "+";
            //    NeedAPicture = MakeSampleImage(AllPicure);
            //    pictureBox2.Image = NeedAPicture;
            //}
        }

        //private Bitmap Cut(Bitmap img, Rectangle cutRect)
        //{
        //    if (cutRect.Width == 0 || cutRect.Height == 0) return null;

        //    var rect = new Rectangle(0, 0, cutRect.Width, cutRect.Height);

        //    var path = new GraphicsPath();
        //    path.AddRectangle(rect); 

        //    var res = new Bitmap(rect.Width, rect.Height);
        //    using (var gr = Graphics.FromImage(res))
        //    {
        //        gr.SetClip(path, CombineMode.Intersect);
        //        gr.DrawImage(img, -cutRect.Left, -cutRect.Top);
        //    }

        //    return res;
        //}

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //if (!checkBox1.Checked)
            //{
            if (image != null)
            {
                e.Graphics.DrawImage(image, Point.Empty);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.Red, 2) { DashStyle = DashStyle.Dot })
                {
                    e.Graphics.DrawRectangle(pen, PointsToRect(start, end));
                }
            }
            //}
            //else
            //{
            //if ((Points != null) && (Points.Count > 1))
            //{
            //    using (Pen dashed_pen = new Pen(Color.Black))
            //    {
            //        dashed_pen.DashPattern = new float[] { 5, 5 };
            //        e.Graphics.DrawLines(Pens.Red, Points.ToArray());
            //        e.Graphics.DrawLines(dashed_pen, Points.ToArray());
            //    }
            //}
            //}
        }

        private Rectangle PointsToRect(Point p1, Point p2)
        {
            return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }

        //private Bitmap MakeSampleImage(Bitmap bitmap)
        //{
        //    const int box_wid = 20;
        //    const int box_hgt = 20;

        //    Bitmap bm = new Bitmap(bitmap.Width, bitmap.Height);
        //    using (Graphics gr = Graphics.FromImage(bm))
        //    {
        //        gr.Clear(Color.White);
        //        int num_rows = bm.Height / box_hgt;
        //        int num_cols = bm.Width / box_wid;
        //        for (int row = 0; row < num_rows; row++)
        //        {
        //            int y = row * box_hgt;
        //            for (int col = 0; col < num_cols; col++)
        //            {
        //                int x = 2 * col * box_wid;
        //                if (row % 2 == 1) x += box_wid;
        //                gr.FillRectangle(Brushes.LightBlue,
        //                    x, y, box_wid, box_hgt);
        //            }
        //        }

        //        gr.DrawImageUnscaled(bitmap, 0, 0);
        //    }
        //    return bm;
        //}

        
        private void FormPicturesTrue_Load(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            FileInfo test = new FileInfo(fileName);
            var img = ResizeImage(Image.FromFile(fileName), 1080, 1080);

            NamePicture = test.Name;
            image = img;
            pictureBox1.Image = image;
            labelNameImage.Text = test.Name;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            columnName = dataGridView1.Columns[e.ColumnIndex].Name;
        }
    }
}
