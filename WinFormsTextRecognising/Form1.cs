using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTextRecognising
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<RectangleW> RectanglesWord = new List<RectangleW>(); // фигуры
        List<RectangleW> desiredRectangles = new List<RectangleW>();
        List<RectangleW> RectanglesWordsList = new List<RectangleW>();
        Bitmap bmp = new Bitmap(650, 500);
        int index = 0;
        int numberPicture = 0;
        double coefficientX = 0;
        double coefficientY = 0;
        int xAbsolyte = 0;
        int yAbsolyte = 0;
        int widthAbsolyte = 0;
        int heightAbsolyte = 0;
        string rightWord = string.Empty;
        int enterColumn = 0;

        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            string fileName = openFileDialog1.FileName;
            string base64 = Convert.ToBase64String(File.ReadAllBytes(fileName));

            numberPicture++;

            Bitmap image = new Bitmap(fileName);
            pictureBox1.Size = image.Size;

            Bitmap bmpD = new Bitmap(900, 900);

            using (Graphics g = Graphics.FromImage(bmpD))
            {
                g.Clear(Color.White);
            }

            TextDetectionConfig textDetectionConfig = new TextDetectionConfig();
            textDetectionConfig.languageCodes.Add("*");

            Feature f = new Feature();
            f.type = "TEXT_DETECTION";
            f.text_detection_config = textDetectionConfig;

            AnalyzeSpec analyze = new AnalyzeSpec();
            analyze.content = base64;
            analyze.features.Add(f);
            analyze.mime_type = "image";

            TextRecognising bodyJson = new TextRecognising();
            bodyJson.folderId = "b1gekd8j9cnaes2150fl";
            bodyJson.analyze_specs.Add(analyze);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string json = System.Text.Json.JsonSerializer.Serialize(bodyJson, options);

            string outputJson = await RecognitionRequestAsync(json);

            string textPictureFinal = string.Empty;

            var restored = System.Text.Json.JsonSerializer.Deserialize<AllResults>(outputJson);

            Font drawFont = new Font("Times New Roman", 11);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            string word = string.Empty;
            int xNeed = 0;
            int yNeed = 0;

            foreach (var a in restored.results)
            {
                foreach (var b in a.results)
                {
                    foreach (var c in b.textDetection.pages)
                    {
                        var boundingBox = c.blocks[0].boundingBox;
                        var relativeX1 = Convert.ToInt32(boundingBox.vertices[0].x);
                        var relativeY1 = Convert.ToInt32(boundingBox.vertices[0].y);

                        var relativeX3 = Convert.ToInt32(boundingBox.vertices[2].x);
                        var relativeY3 = Convert.ToInt32(boundingBox.vertices[2].y);

                        foreach (var block in c.blocks)
                        {
                            var boundingBoxBlock = block.boundingBox;
                            var xLeft = Convert.ToInt32(boundingBoxBlock.vertices[0].x);
                            var yLeft = Convert.ToInt32(boundingBoxBlock.vertices[0].y);
                            var xRight = Convert.ToInt32(boundingBoxBlock.vertices[2].x);
                            var yRight = Convert.ToInt32(boundingBoxBlock.vertices[2].y);

                            if (relativeX1 > xLeft)
                                relativeX1 = xLeft;
                            if (relativeY1 > yLeft)
                                relativeY1 = yLeft;
                            if (relativeX3 < xRight)
                                relativeX3 = xRight;
                            if (relativeY3 < yRight)
                                relativeY3 = yRight;
                        }

                        double side = 900;
                        coefficientX = side / Convert.ToDouble(relativeX3 - relativeX1);
                        coefficientY = side / Convert.ToDouble(relativeY3 - relativeY1);

                        foreach (var d in c.blocks)
                        {                            
                            var textblock = string.Empty;
                            //var vertices = d.boundingBox.vertices;
                            //var x1 = Convert.ToInt32(vertices[0].x);
                            //var y1 = Convert.ToInt32(vertices[0].y);
                            //var x2 = Convert.ToInt32(vertices[2].x);
                            //var y2 = Convert.ToInt32(vertices[2].y);

                            //int width = (int)((double)Math.Abs(x2 - x1) * coefficientX);
                            //int height = (int)((double)Math.Abs(y2 - y1) * coefficientY);
                            //var trueCoordinateX = (double)(x1 - relativeX1) * coefficientX; 
                            //var trueCoordinateY = (double)(y1 - relativeY1) * coefficientY;
                            //var rectangle = new Rectangle((int)trueCoordinateX, (int)trueCoordinateY, width, height);

                            //using (Graphics g = Graphics.FromImage(bmpD))
                            //{
                            //    g.DrawRectangle(Pens.Red, rectangle);
                            //}
                            
                            foreach (var q in d.lines)
                            {
                                foreach (var i in q.words)
                                {
                                    var verticesWord = i.boundingBox.vertices;
                                    var x1Word = Convert.ToInt32(verticesWord[0].x);
                                    var y1Word = Convert.ToInt32(verticesWord[0].y);
                                    var x2Word = Convert.ToInt32(verticesWord[2].x);
                                    var y2Word = Convert.ToInt32(verticesWord[2].y);

                                    int widthWord = (int)((double)Math.Abs(x2Word - x1Word) * coefficientX);
                                    int heightWord = (int)((double)Math.Abs(y2Word - y1Word) * coefficientY);
                                    var trueWordCoordinateX = (double)(x1Word - relativeX1) * coefficientX;
                                    var trueWordCoordinateY = (double)(y1Word - relativeY1) * coefficientY;

                                    textPictureFinal += i.text + " ";
                                    textblock += i.text + " ";
                                    word = i.text;

                                    var rectangleWord = new Rectangle((int)trueWordCoordinateX, (int)trueWordCoordinateY, widthWord, heightWord);

                                    var position = new Point((int)trueWordCoordinateX, (int)trueWordCoordinateY);

                                    if (word.Contains("salar"))
                                    {
                                        xNeed = (int)trueWordCoordinateX;
                                        yNeed = (int)trueWordCoordinateY;
                                    }
                                    //if (firstB)
                                    //{
                                    //    xNeed = (int)trueWordCoordinateX;
                                    //    yNeed = (int)trueWordCoordinateY;
                                    //    firstB = false;
                                    //}
                                    //else if (xNeed < trueWordCoordinateX && yNeed > trueWordCoordinateY)
                                    //{
                                    //    xNeed = (int)trueWordCoordinateX;
                                    //    yNeed = (int)trueWordCoordinateY;
                                    //}

                                    if (checkBox1.Checked)
                                        RectanglesWord.Add(new RectangleW(Color.Blue, position, widthWord, heightWord, word, string.Empty));
                                    else
                                        RectanglesWordsList.Add(new RectangleW(Color.Blue, position, widthWord, heightWord, word, string.Empty));

                                    using (Graphics g = Graphics.FromImage(bmpD))
                                    {
                                        g.DrawRectangle(Pens.Blue, rectangleWord);
                                        g.DrawString(word, drawFont, drawBrush, (int)trueWordCoordinateX, (int)trueWordCoordinateY);
                                    }
                                }
                            }

                            //if (Rectangles.Count > 0 && !checkBox1.Checked)
                            //{
                                //var rectangleAbsolute = Rectangles[index];
                                //var siz = new Size(rectangleAbsolute.Width, rectangleAbsolute.Height);
                                //var recAbsolute = new Rectangle(rectangleAbsolute.PositionFigure, siz);

                                //textBox1.Text = textblock;
                                //using (Graphics g = Graphics.FromImage(bmpD))
                                //{
                                //    var pen = new Pen(Color.Black, 4);
                                //    g.DrawRectangle(pen, rectangle);
                                //}

                                //foreach (var pyk in RectanglesWordsList)
                                //{
                                //    var recGold = new Rectangle(pyk.PositionFigure.X, pyk.PositionFigure.Y, pyk.Width, pyk.Height);
                                //    if (rectangle.IntersectsWith(recGold))
                                //    {
                                //        using (Graphics g = Graphics.FromImage(bmpD))
                                //        {
                                //            var pen = new Pen(Color.Gold, 4);
                                //            g.DrawRectangle(pen, recGold);
                                //            g.DrawString(pyk.Text, drawFont, drawBrush, pyk.PositionFigure.X, pyk.PositionFigure.Y);
                                //        }
                                //    }
                                //}

                            //}
                        }
                    }
                }
            }

            if (!checkBox1.Checked)
            {
                using (Graphics g = Graphics.FromImage(bmpD))
                {
                    g.Clear(Color.White);
                }

                //var rectangleAbsolute = RectanglesWord[index];
                //var siz = new Size(rectangleAbsolute.Width, rectangleAbsolute.Height);
                //var recAbsolute = new Rectangle(rectangleAbsolute.PositionFigure, siz);

                RectanglesWordsList.ToArray();

                var halfCoordinateWordX1 = RectanglesWordsList[0].PositionFigure.X /*+ (RectanglesWordsList[0].Width / 2)*/;
                var halfCoordinateWordY1 = RectanglesWordsList[0].PositionFigure.Y /*+ (RectanglesWordsList[0].Height / 2)*/;
                int halfCoordinateWordX4 = 0;
                int halfCoordinateWordY4 = 0;
                int distance = 0;
                double cos = 0;
                double sin = 0;
                var wordA = string.Empty;

                for (var i = 0; i < RectanglesWordsList.Count; i++)
                {
                    if (RectanglesWordsList[i].PositionFigure.X == xNeed && RectanglesWordsList[i].PositionFigure.Y == yNeed)
                    {
                        halfCoordinateWordX4 = RectanglesWordsList[i].PositionFigure.X /*+ (RectanglesWordsList[1].Width / 2)*/;
                        halfCoordinateWordY4 = RectanglesWordsList[i].PositionFigure.Y /*+ (RectanglesWordsList[i].Height / 2)*/;
                        distance = halfCoordinateWordY1 - halfCoordinateWordY4;
                        var BC = (double)halfCoordinateWordX4 - (double)halfCoordinateWordX1;
                        var AB = Math.Sqrt(Math.Pow(BC, 2) + Math.Pow(distance, 2));
                        cos = BC / AB;
                        sin = (double)distance / AB;
                        //using (Graphics g = Graphics.FromImage(bmpD))
                        //{
                        //    var pen = new Pen(Color.Red, 2);
                        //    g.DrawLine(pen, halfCoordinateWordX1, halfCoordinateWordY1, halfCoordinateWordX4, halfCoordinateWordY4);
                        //}
                        break;
                    }
                }

                foreach (var desired in desiredRectangles)
                {
                    var x = desired.PositionFigure.X;
                    var y = desired.PositionFigure.Y;
                    var rectangleNew = new Rectangle(x, y, desired.Width, desired.Height);
                    putValuesInColumns(rectangleNew, desired.NameColumn);

                    using (Graphics g = Graphics.FromImage(bmpD))
                    {
                        var pen = new Pen(Color.Black, 4);
                        //g.DrawRectangle(pen, recAbsolute);
                        g.DrawRectangle(pen, rectangleNew);
                        g.DrawString(desired.Text, drawFont, drawBrush, x, y);
                    }
                }

                foreach (var rectangleWord in RectanglesWordsList)
                {
                    var xWord = rectangleWord.PositionFigure.X;
                    var yWord = rectangleWord.PositionFigure.Y;
                    var widthTrue = rectangleWord.Width;
                    var heightTrue = rectangleWord.Height;
                    var wordTrue = rectangleWord.Text;

                    if (distance < 0)
                        distance = 0;

                    var trueXWord = (xWord * cos) - (yWord * sin);
                    var trueYWord = (xWord * sin) + (yWord * cos) - distance;
                    var sizeRectangleWord = new Size(widthTrue, heightTrue);
                    var location = new Point((int)trueXWord, (int)trueYWord);
                    var rectangleTrue = new Rectangle(location, sizeRectangleWord);
                    using (Graphics g = Graphics.FromImage(bmpD))
                    {
                        g.DrawRectangle(Pens.Blue, rectangleTrue);
                        g.DrawString(wordTrue, drawFont, drawBrush, (int)trueXWord, (int)trueYWord);
                    }

                    //foreach(var u in desiredRectangles)
                    //{
                    //    var size = new Size(u.Width, u.Height);
                    //    var rectangleNew = new Rectangle(u.PositionFigure, size);

                    //    var rectangleNewCopy = rectangleNew;
                    //    rectangleNewCopy.Intersect(rectangleTrue);
                    //    s2 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                    //    if (s2 > 0 && first)
                    //    {
                    //        s1 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                    //    }
                    //}

                    //var rectangleNew = new Rectangle(u.PositionFigure, size);

                    //var rectangleNewCopy = rectangleNew;

                    //rectangleNewCopy.Intersect(rectangleTrue);
                    //s2 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                    //if (s2 > 0 && first)
                    //{
                    //    s1 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                    //    wordA = wordTrue;
                    //    first = false;
                    //}
                    //else if (s2 > s1)
                    //{
                    //    wordA = wordTrue;
                    //}

                }

                var rectanglePictureBox1 = new Rectangle(0, 0, widthAbsolyte, heightAbsolyte);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var pen = new Pen(Color.Black, 4);
                    g.DrawRectangle(pen, rectanglePictureBox1);
                    g.DrawString(rightWord, drawFont, drawBrush, 0, 0);
                }

                textBox1.Text = wordA;
            }

            pictureBox1.Image = bmpD;

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            var mass = textPictureFinal.Split(' ');
            var length = mass.Length;
            bool bbb = false;
            //for (var i = 0; i < length; i++)
            //{
            //    if (!bbb && (mass[i] == "KG" || mass[i] == "Kg" || mass[i] == "kg"))
            //    {
            //        dataGridView1.Rows[0].Cells["ColumnQuality"].Value = mass[i - 2];
            //        bbb = true;
            //    }
            //    else if (bbb && (mass[i] == "KG" || mass[i] == "Kg" || mass[i] == "kg"))
            //    {
            //        dataGridView1.Rows[0].Cells["ColumnSize"].Value = mass[i - 1];
            //        bbb = false;
            //    }
            //    else if (mass[length - i - 1] == "KG" || mass[length - i - 1] == "Kg" || mass[length - i - 1] == "kg")//NetWeight
            //        dataGridView1.Rows[0].Cells["ColumnNetWeight"].Value = mass[length - i - 2];
            //    else if (mass[i] == "Pieces")
            //        dataGridView1.Rows[0].Cells["ColumnPieces"].Value = mass[i + 1];
            //    else if (mass[i] == "Plant")
            //        dataGridView1.Rows[0].Cells["ColumnPlant"].Value = mass[i + 1];
            //    else if (mass[i] == "Lot")
            //        dataGridView1.Rows[0].Cells["ColumnLot"].Value = mass[i + 5];
            //    else if (mass[i] == "Before")
            //        dataGridView1.Rows[0].Cells["ColumnBestBefore"].Value = mass[i + 5];
            //    else if (mass[i] == "Date")
            //        dataGridView1.Rows[0].Cells["ColumnElaborationDate"].Value = mass[i + 3];
            //    //else if (mass[i] == "For")
            //    //    dataGridView1.Rows[0].Cells["ColumnPackedFor"].Value = mass[i + 1];
            //}

            richTextBox1.Text = textPictureFinal;

            if (!string.IsNullOrEmpty(textBox2.Text))
                ColorizeText();

            watch.Stop();
            label1.Text = "Время выполнения программы в миллисекундах : " + watch.ElapsedMilliseconds + " мс.\r\n" + "Время выполнения программы в секундах : " + watch.Elapsed.Seconds + " с.";
            checkBox1.Checked = false;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            string fileName = openFileDialog1.FileName;
            Image temp = Image.FromFile(fileName);
            string base64 = Convert.ToBase64String(File.ReadAllBytes(fileName));

            var feature = new Feature();
            feature.type = "FACE_DETECTION";

            var analyze = new AnalyzeSpec();
            analyze.content = base64;
            analyze.features.Add(feature);

            var face = new TextRecognising();
            face.folderId = "b1gekd8j9cnaes2150fl";
            face.analyze_specs.Add(analyze);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string json = System.Text.Json.JsonSerializer.Serialize(face, options);
            string outputJson = await RecognitionRequestAsync(json);

            var restored = System.Text.Json.JsonSerializer.Deserialize<AllResults>(outputJson);
            List<BoundingBox> boundingBoxes = new List<BoundingBox>();
            int x1 = 0;
            int x3 = 0;
            int y1 = 0;
            int y3 = 0;

            foreach (var a in restored.results)
            {
                foreach (var b in a.results)
                {
                    foreach (var c in b.faceDetection.faces)
                    {
                        boundingBoxes.Add(c.boundingBox);
                    }
                }
            }

            foreach (var facePicture in boundingBoxes)
            {
                x1 = Convert.ToInt32(facePicture.vertices[0].x);
                y1 = Convert.ToInt32(facePicture.vertices[0].y);

                x3 = Convert.ToInt32(facePicture.vertices[2].x);
                y3 = Convert.ToInt32(facePicture.vertices[2].y);

                int width = x3 - x1;
                int height = y3 - y1;

                var rectangle = new Rectangle(x1, y1, width, height);
                var bmp = new Bitmap(temp, temp.Width, temp.Height);
                var src = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(src))
                {
                    g.DrawImage(bmp, 0, 0, rectangle, GraphicsUnit.Pixel);
                }

                SaveFileDialog fileOpslaan = new SaveFileDialog();
                fileOpslaan.Filter = "Bitmap Afbeelding (*.bmp)|*.bmp";
                if (fileOpslaan.ShowDialog() == DialogResult.OK)
                    src.Save(fileOpslaan.FileName);
            }

            watch.Stop();
            label1.Text = "Время выполнения программы в миллисекундах : " + watch.ElapsedMilliseconds + " мс.\r\n" + "Время выполнения программы в секундах : " + watch.Elapsed.Seconds + " c.";
        }
        public void putValuesInColumns(Rectangle rectangleTrue, string nameColumn)
        {
            int s1 = 0;
            int s2 = 0;
            bool first = true;
            var wordA = string.Empty;
            foreach (var u in RectanglesWordsList)
            {
                var size = new Size(u.Width, u.Height);
                var rectangleNew = new Rectangle(u.PositionFigure, size);
                var wordTrue = u.Text;

                var rectangleNewCopy = rectangleNew;

                rectangleNewCopy.Intersect(rectangleTrue);
                s2 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                if (s2 > 0 && first)
                {
                    s1 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                    wordA = wordTrue;
                    first = false;
                }
                else if (s2 > s1)
                {
                    wordA = wordTrue;
                }
            }
            dataGridView1.Rows[numberPicture].Cells[nameColumn].Value = wordA;
        }

        public async Task<string> RecognitionRequestAsync (string json)
        {
            var key = "AQVNw2T9vIw25dzuPCdmHOQ3nyQKhulzcZOveoN-";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://vision.api.cloud.yandex.net/vision/v1/batchAnalyze");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Api-Key " + key;

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string outputJson = string.Empty;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    outputJson = reader.ReadToEnd();
                }
            }
            response.Close();

            return outputJson;
        }

        public void ColorizeText()
        {
            int koll = richTextBox1.Text.Length;
            int koll2 = textBox2.Text.Length;

            char[] mass = new char[koll];
            char[] mass2 = new char[koll2];
            char[] mass3 = new char[koll2];

            for (int i = 0; i < koll; i++)
            {
                mass[i] = richTextBox1.Text[i];
            }

            for (int i = 0; i < koll2; i++)
            {
                mass2[i] = textBox2.Text[i];
            }

            for (int i = 0; i < koll; i++)
            {
                int pos = i;
                for (int k = 0; k < koll2; k++)
                {
                    if (mass[pos] == mass2[k])
                    {
                        mass3[k] = mass2[k];
                        pos++;
                    }
                    else
                    {
                        Array.Clear(mass3, 0, koll2);
                        break;
                    }
                }

                if (mass2.SequenceEqual(mass3))
                {
                    richTextBox1.Select(i, koll2);
                    richTextBox1.SelectionBackColor = Color.Yellow;
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                ColorizeText();
            }
        }

        RectangleW[] massRec = new RectangleW[2];
        int count = 0;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Font drawFontWord = new Font("Times New Roman", 12);
            SolidBrush drawBrushWord = new SolidBrush(Color.Black);

            for (int i = 0; i < RectanglesWord.Count; i++)
            {
                var widthRec = RectanglesWord[i].Width;
                var heightRec = RectanglesWord[i].Height;
                var sizeRec = new Size(widthRec, heightRec);
                var rectangle = new Rectangle(RectanglesWord[i].PositionFigure, sizeRec);

                if (rectangle.Contains(e.X, e.Y))
                {
                    if (!checkBox2.Checked)
                    {
                        //textBox1.Text = RectanglesWord[i].Text;
                        index = i;
                        xAbsolyte = RectanglesWord[i].PositionFigure.X;
                        yAbsolyte = RectanglesWord[i].PositionFigure.Y;
                        widthAbsolyte = widthRec;
                        heightAbsolyte = heightRec;
                        var newRectangle = new Rectangle(0, 0, widthAbsolyte, heightAbsolyte);
                        rightWord = RectanglesWord[i].Text;
                        var location = new Point(xAbsolyte, yAbsolyte);
                        var yt = dataGridView1.Columns[enterColumn].Name;
                        desiredRectangles.Add(new RectangleW(Color.Blue, location, widthRec, heightRec, RectanglesWord[i].Text, yt));

                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.Clear(Color.White);
                            g.DrawRectangle(Pens.Blue, newRectangle);
                            g.DrawString(rightWord, drawFontWord, drawBrushWord, 0, 0);
                        }

                        dataGridView1.Rows[numberPicture].Cells[enterColumn].Value = RectanglesWord[i].Text;
                        break;
                    }
                    else
                    {
                        if (count < 2)
                        {
                            massRec[count] = new RectangleW(Color.Blue, RectanglesWord[i].PositionFigure, widthRec, heightRec, RectanglesWord[i].Text, string.Empty);
                            count++;
                            break;
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            enterColumn = e.ColumnIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RectanglesWord.Count; i++)
            {
                var widthRec = RectanglesWord[i].Width;
                var heightRec = RectanglesWord[i].Height;
                var sizeRec = new Size(widthRec, heightRec);
                var rectangle = new Rectangle(RectanglesWord[i].PositionFigure, sizeRec);

                var halfCoordinateWordX1 = RectanglesWordsList[0].PositionFigure.X /*+ (RectanglesWordsList[0].Width / 2)*/;
                var halfCoordinateWordY1 = RectanglesWordsList[0].PositionFigure.Y /*+ (RectanglesWordsList[0].Height / 2)*/;
                int halfCoordinateWordX4 = 0;
                int halfCoordinateWordY4 = 0;
                int distance = 0;
                double cos = 0;
                double sin = 0;

                if (RectanglesWord[i].PositionFigure.X == massRec[0].PositionFigure.X && RectanglesWord[i].PositionFigure.Y == massRec[0].PositionFigure.Y)
                {
                    halfCoordinateWordX4 = RectanglesWordsList[i].PositionFigure.X /*+ (RectanglesWordsList[1].Width / 2)*/;
                    halfCoordinateWordY4 = RectanglesWordsList[i].PositionFigure.Y /*+ (RectanglesWordsList[i].Height / 2)*/;
                    distance = halfCoordinateWordY1 - halfCoordinateWordY4;
                    var BC = (double)halfCoordinateWordX4 - (double)halfCoordinateWordX1;
                    var AB = Math.Sqrt(Math.Pow(BC, 2) + Math.Pow(distance, 2));
                    cos = BC / AB;
                    sin = (double)distance / AB;
                    //using (Graphics g = Graphics.FromImage(bmpD))
                    //{
                    //    var pen = new Pen(Color.Red, 2);
                    //    g.DrawLine(pen, halfCoordinateWordX1, halfCoordinateWordY1, halfCoordinateWordX4, halfCoordinateWordY4);
                    //}
                    break;
                }
            }
        }
    }

    class RectangleW
    {
        public int Width;
        public int Height;
        public string Text;
        public Color ColorFigure;
        public Point PositionFigure;
        public string NameColumn;

        public RectangleW(Color color, Point position, int width, int height, string text, string nameColumn)
        {
            ColorFigure = color;
            PositionFigure = position;
            Width = width;
            Height = height;
            Text = text;
            NameColumn = nameColumn;
        }
    }
}
