using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace WinFormsTextRecognising
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        List<RectangleW> RectanglesIdeal = new List<RectangleW>(); 
        List<RectangleW> desiredRectangles = new List<RectangleW>();
        List<RectangleW> RectanglesNotIdeal = new List<RectangleW>();
        List<RectangleW> RotatedPerfectRectangles = new List<RectangleW>(); 
        List<RectangleW> СroppedRectangles = new List<RectangleW>();
        List<FillInTheTable> Table = new List<FillInTheTable>();
        bool one = true;                                                                                                                                                                                          
        Font drawFont = new Font("Times New Roman", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);

        Bitmap bmpD = new Bitmap(1080, 1080);
        int numberPicture = 0;
        //int xAbsolyte = 0;
        //int yAbsolyte = 0;
        //string rightWord = string.Empty;
        //int enterColumn = 0;

        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (!one)
                dataGridView1.Rows.Add();

            //string base64 = ImgToStr(image);
            string fileName = openFileDialog1.FileName;
            FileInfo test = new FileInfo(fileName);
            label3.Text = test.Name;
            Image temp = Image.FromFile(fileName);
            var img = ResizeImage(temp, 1080, 1080);
            var bmp = new Bitmap(1080, 1080);

            foreach(var rec in desiredRectangles)
            {
                var rect = new Rectangle(rec.PositionFigure, new Size(rec.Width, rec.Height));
                Bitmap without_area = CropImage((Bitmap)img, rect);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(without_area, rec.PositionFigure);
                }
            }

            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, ImageFormat.Png);

            string base64 = Convert.ToBase64String(memoryStream.ToArray());

            RectanglesNotIdeal.Clear();
            RectanglesIdeal.Clear();
            RotatedPerfectRectangles.Clear();
            СroppedRectangles.Clear();

            ClearPictureBox();

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
            bodyJson.folderId = "b1gr5t1i5o6d4p2h45rn";
            bodyJson.analyze_specs.Add(analyze);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string json = System.Text.Json.JsonSerializer.Serialize(bodyJson, options);

            string outputJson = await RecognitionRequestAsync(json);

            string textPictureFinal = string.Empty;

            var restored = System.Text.Json.JsonSerializer.Deserialize<AllResults>(outputJson);

            string word = string.Empty;
            int xNeed = 0;
            int yNeed = 0;
            //var blocks = restored.results[0].results[0].textDetection.pages[0].blocks;
            //var lenghtBlock = blocks.Count - 1;
            //var lenghtLine = blocks[lenghtBlock].lines.Count - 1;

            //var relativeX1 = blocks[0].lines[0].words.Min(a => a.boundingBox.vertices[0].x);
            //var relativeY1 = blocks[0].lines[0].words.Min(a => a.boundingBox.vertices[0].y);
            //var relativeY3 = blocks[lenghtBlock].lines[lenghtLine].words.Max(a => Convert.ToInt32(a.boundingBox.vertices[2].y));
            //var relativeX3 = blocks[lenghtBlock].lines[lenghtLine].words.Max(a => Convert.ToInt32(a.boundingBox.vertices[2].x));

            //double side = 1000;
            //double coefficientX = side / Convert.ToDouble(Convert.ToDouble(relativeX3) - Convert.ToDouble(relativeX1));
            //double coefficientY = side / Convert.ToDouble(Convert.ToDouble(relativeY3) - Convert.ToDouble(relativeY1));
            //double coefficientX = 0;
            //double coefficientY = 0;
            //var relativeX1 = 0;
            //var relativeY1 = 0;

            foreach (var a in restored.results)
            {
                foreach (var b in a.results)
                {
                    foreach (var c in b.textDetection.pages)
                    {
                        //var boundingBox = c.blocks[0].boundingBox;
                        //relativeX1 = Convert.ToInt32(boundingBox.vertices[0].x);
                        //relativeY1 = Convert.ToInt32(boundingBox.vertices[0].y);

                        //var relativeX3 = Convert.ToInt32(boundingBox.vertices[2].x);
                        //var relativeY3 = Convert.ToInt32(boundingBox.vertices[2].y);

                        //foreach (var block in c.blocks)
                        //{
                        //    var boundingBoxBlock = block.boundingBox;
                        //    var xLeft = Convert.ToInt32(boundingBoxBlock.vertices[0].x);
                        //    var yLeft = Convert.ToInt32(boundingBoxBlock.vertices[0].y);
                        //    var xRight = Convert.ToInt32(boundingBoxBlock.vertices[2].x);
                        //    var yRight = Convert.ToInt32(boundingBoxBlock.vertices[2].y);

                        //    if (relativeX1 > xLeft)
                        //        relativeX1 = xLeft;
                        //    if (relativeY1 > yLeft)
                        //        relativeY1 = yLeft;
                        //    if (relativeX3 < xRight)
                        //        relativeX3 = xRight;
                        //    if (relativeY3 < yRight)
                        //        relativeY3 = yRight;
                        //}

                        //coefficientX = side / Convert.ToDouble(relativeX3 - relativeX1);
                        //coefficientY = side / Convert.ToDouble(relativeY3 - relativeY1);

                        foreach (var d in c.blocks)
                        {
                            foreach (var q in d.lines)
                            {
                                foreach (var i in q.words)
                                {
                                    var verticesWord = i.boundingBox.vertices;
                                    var x1Word = Convert.ToInt32(verticesWord[0].x);
                                    var y1Word = Convert.ToInt32(verticesWord[0].y);
                                    var x2Word = Convert.ToInt32(verticesWord[2].x);
                                    var y2Word = Convert.ToInt32(verticesWord[2].y);

                                    int widthWord = (int)((double)Math.Abs(x2Word - x1Word) /** coefficientX*/);
                                    int heightWord = (int)((double)Math.Abs(y2Word - y1Word) /** coefficientY*/);
                                    var trueWordCoordinateX = (double)(x1Word /*- Convert.ToInt32(relativeX1)*/) /** coefficientX*/;
                                    var trueWordCoordinateY = (double)(y1Word /*- Convert.ToInt32(relativeY1)*/) /** coefficientY*/;

                                    textPictureFinal += i.text + " ";
                                    word = i.text;

                                    var rectangleWord = new Rectangle((int)trueWordCoordinateX, (int)trueWordCoordinateY, widthWord, heightWord);

                                    var position = new Point((int)trueWordCoordinateX, (int)trueWordCoordinateY);

                                    if (word.Contains("salar"))
                                    {
                                        xNeed = (int)trueWordCoordinateX;
                                        yNeed = (int)trueWordCoordinateY;
                                    }

                                    RectanglesNotIdeal.Add(new RectangleW(position, widthWord, heightWord, word, string.Empty));

                                    DrawRectangleOnPictureBox(Color.Blue, 1, rectangleWord, word, (int)trueWordCoordinateX, (int)trueWordCoordinateY);
                                }
                            }
                        }
                    }
                }
            }


            //ClearPictureBox();

            //var halfCoordinateWordX1 = RectanglesNotIdeal[0].PositionFigure.X;
            //var halfCoordinateWordY1 = RectanglesNotIdeal[0].PositionFigure.Y;
            //int halfCoordinateWordX4 = 0;
            //int halfCoordinateWordY4 = 0;
            //int distance = 0;
            //double cos = 0;
            //double sin = 0;

            //for (var i = 0; i < RectanglesNotIdeal.Count; i++)
            //{
            //    if (RectanglesNotIdeal[i].PositionFigure.X == xNeed && RectanglesNotIdeal[i].PositionFigure.Y == yNeed)
            //    {
            //        halfCoordinateWordX4 = RectanglesNotIdeal[i].PositionFigure.X;
            //        halfCoordinateWordY4 = RectanglesNotIdeal[i].PositionFigure.Y;
            //        distance = halfCoordinateWordY1 - halfCoordinateWordY4;
            //        var BC = (double)halfCoordinateWordX4 - (double)halfCoordinateWordX1;
            //        var AB = Math.Sqrt(Math.Pow(BC, 2) + Math.Pow(distance, 2));
            //        cos = BC / AB;
            //        sin = (double)distance / AB;
            //        break;
            //    }
            //}

            //foreach (var rectangleWord in RectanglesNotIdeal)
            //{
            //    var xWord = rectangleWord.PositionFigure.X;
            //    var yWord = rectangleWord.PositionFigure.Y;
            //    var widthTrue = rectangleWord.Width;
            //    var heightTrue = rectangleWord.Height;
            //    var wordTrue = rectangleWord.Text;

            //    if (distance < 0)
            //        distance = 0;

            //    var trueXWord = (xWord * cos) - (yWord * sin);
            //    var trueYWord = (xWord * sin) + (yWord * cos) - distance;
            //    var sizeRectangleWord = new Size(widthTrue, heightTrue);
            //    var location = new Point((int)trueXWord, (int)trueYWord);
            //    var rectangleTrue = new Rectangle(location, sizeRectangleWord);

            //    DrawRectangleOnPictureBox(Color.Blue, 1, rectangleTrue, wordTrue, (int)trueXWord, (int)trueYWord);

            //    RotatedRectangles.Add(new RectangleW(location, widthTrue, heightTrue, wordTrue, string.Empty));
            //}
            //var minX = desiredRectangles.Min(a => a.PositionFigure.X);
            //var minY = desiredRectangles.Min(a => a.PositionFigure.Y);
            //var maxX = desiredRectangles.Max(a => a.PositionFigure.X);
            //var maxY = desiredRectangles.Max(a => a.PositionFigure.Y);

            //var coefX = side / (double)(maxX - minX);
            //var coefY = side / (double)(maxY - minY);
            //    coefficientX = side / Convert.ToDouble(relativeX3 - relativeX1);
            //coefficientY = side / Convert.ToDouble(relativeY3 - relativeY1);
            foreach (var desired in desiredRectangles)
            {
                var x = (double)(desired.PositionFigure.X/* - minX*/) /** coefX*/;
                var y = (double)(desired.PositionFigure.Y /*- minY*/) /** coefY*/;
                var rectangleNew = new Rectangle((int)x, (int)y, (int)(/*(double)*/desired.Width /** coefX*/), (int)(/*(double)*/desired.Height /** coefX*/));

                putValuesInColumns(rectangleNew, desired.NameColumn, RectanglesNotIdeal, desired.Text);

                DrawRectangleOnPictureBox(Color.Black, 4, rectangleNew, desired.Text, (int)x, (int)y);
            }

            pictureBox1.Image = bmpD;

            //var mass = textPictureFinal.Split(' ');
            //var length = mass.Length;
            //bool bbb = false;
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
            numberPicture++;

            pictureBox1.Refresh();

            if (!string.IsNullOrEmpty(textBox2.Text))
                ColorizeText();

            //button1.Enabled = false;
            one = false;
            watch.Stop();
            label1.Text = "Время выполнения программы в миллисекундах : " + watch.ElapsedMilliseconds + " мс.\r\n" + "Время выполнения программы в секундах : " + watch.Elapsed.Seconds + " с.";
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
        public void putValuesInColumns(Rectangle rectangleTrue, string nameColumn, List<RectangleW> listRectangles, string needWord)
        {
            //int s1 = 0;
            //int s2 = 0;
            //bool first = true;
            var wordA = string.Empty;

            foreach (var rec in listRectangles)
            {
                var size = new Size(rec.Width, rec.Height);
                var rectangleNew = new Rectangle(rec.PositionFigure, size);
                //var wordTrue = rec.Text;

                //var rectangleNewCopy = rectangleNew;

                //rectangleNewCopy.Intersect(rectangleTrue);
                //s2 = rectangleNewCopy.Width * rectangleNewCopy.Height;


                if (rectangleTrue.IntersectsWith(rectangleNew))
                {
                    wordA += rec.Text.ToLower() + " "; 
                }


                //if (nameColumn.Contains("Size"))
                //{
                //    if (rectangleTrue.IntersectsWith(rectangleNew))
                //    {
                //        if (!wordTrue.Contains("KG") && !wordTrue.Contains("kg") && !wordTrue.Contains("Kg") && !wordTrue.Contains("Size") && !wordTrue.Contains("SIZE") && !wordTrue.Contains("LB") 
                //            && !wordTrue.Contains("Lb") && !wordTrue.Contains("lb") && !wordTrue.Contains("Net") && !wordTrue.Contains(",") && !wordTrue.Contains("WT") && !wordTrue.Contains("Premium"))
                //            wordA += wordTrue + " ";
                //    }
                //}
                //else if (nameColumn.Contains("Quality"))
                //{
                //    if (rectangleTrue.IntersectsWith(rectangleNew))
                //    {
                //        wordA += wordTrue + " ";
                //    }
                //}
                //else if (nameColumn.Contains("BestBefore"))
                //{
                //    if (rectangleTrue.IntersectsWith(rectangleNew))
                //    {
                //        if (!wordTrue.Contains("Best") && !wordTrue.Contains("Before"))
                //            wordA += wordTrue + " ";
                //    }
                //}
                //else if (nameColumn.Contains("Net"))
                //{
                //    if (rectangleTrue.IntersectsWith(rectangleNew))
                //    {
                //        wordA += wordTrue + " ";
                //    }
                //}
                //else if (nameColumn.Contains("Elaboration"))
                //{
                //    if (rectangleTrue.IntersectsWith(rectangleNew))
                //    {
                //        wordA += wordTrue + " ";
                //    }
                //}
                //else if (nameColumn.Contains("Plant"))
                //{
                //    if (rectangleTrue.IntersectsWith(rectangleNew))
                //    {
                //        if (!wordTrue.Contains("/"))
                //            wordA += wordTrue + " ";
                //    }
                //}
                //else
                //{
                //    if (s2 > 0 && first)
                //    {
                //        s1 = rectangleNewCopy.Width * rectangleNewCopy.Height;
                //        wordA = wordTrue;
                //        first = false;
                //    }
                //    else if (s2 > s1)
                //    {
                //        wordA = wordTrue;
                //    }
                //}
            }

            var wordB = string.Empty;
            var WORD = wordA.Split(' ');

            if (nameColumn.Contains("Size"))
            {
                for (var word = 0; word < WORD.Length; word++)
                {
                    if (WORD[word].Contains("-"))
                    {
                        if (WORD[word].Length == 1)
                            wordB = WORD[word - 1] + WORD[word] + WORD[word + 1];
                        else
                            wordB = WORD[word];
                    }
                }
            }
            else if (nameColumn.Contains("Net"))
            {
                for (var word = 0; word < WORD.Length; word++)
                {
                    if (WORD[word].Contains(",") || WORD[word].Contains("."))
                    {
                        if (WORD[word].Length == 5)
                            wordB = WORD[word];
                        else
                            wordB = WORD[word] + WORD[word + 1];
                    }
                    else if (string.IsNullOrEmpty(wordB) && (WORD[word].Contains("KG") || WORD[word].Contains("kg")))
                    {
                        if (WORD[word].Contains(".") || WORD[word].Contains(","))
                            wordB = WORD[word - 1];
                        else
                            wordB = WORD[word - 1];
                    }
                }
            }
            else if (nameColumn.Contains("Elaboration"))
            {
                for (var word = 0; word < WORD.Length; word++)
                {
                    var needWordSplit = needWord.Split('/');

                    if (needWordSplit.Length == 1)
                        needWordSplit = needWord.Split('.');

                    if (needWordSplit.Length == 1)
                        needWordSplit = needWord.Split('-');

                    if (WORD[word].Contains(needWordSplit[2]))
                    {
                        if (WORD[word].Length == 10)
                            wordB = WORD[word];
                        else if (WORD[word].Length == 4 && (WORD[word - 1].Length == 2 || WORD[word - 1].Length == 3))
                            wordB = WORD[word - 2] + "." + WORD[word - 1] + "." + WORD[word];
                        else if (WORD[word].Length == 4 && (WORD[word - 1].Length == 5 || WORD[word - 1].Length == 6))
                            wordB = WORD[word - 1] + "." + WORD[word];
                        else if (WORD[word].Length == 7)
                            wordB = WORD[word - 1] + "." + WORD[word];
                    }
                }
            }
            else if (nameColumn.Contains("Best"))
            {
                for (var word = 0; word < WORD.Length; word++)
                {
                    var needWordSplit = needWord.Split('/');

                    if (needWordSplit.Length == 1)
                        needWordSplit = needWord.Split('.');

                    if (needWordSplit.Length == 1)
                        needWordSplit = needWord.Split('-');

                    if (WORD[word].Contains(needWordSplit[2]))
                    {
                        if (WORD[word].Length == 10)
                            wordB = WORD[word];
                        else if (WORD[word].Length == 4 && (WORD[word - 1].Length == 2 || WORD[word - 1].Length == 3))
                            wordB = WORD[word - 2] + "." + WORD[word - 1] + "." + WORD[word];
                        else if (WORD[word].Length == 4 && (WORD[word - 1].Length == 5 || WORD[word - 1].Length == 6))
                            wordB = WORD[word - 1] + "." + WORD[word];
                        else if (WORD[word].Length == 7)
                            wordB = WORD[word - 1] + "." + WORD[word];
                    }
                }
            }
            else
            {
                for (var word = 0; word < WORD.Length; word++)
                {
                    if (WORD[word].Contains(needWord))
                    {
                        wordB = WORD[word];
                    }
                }
            }

            Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));

            //if (nameColumn.Contains("BestBefore"))
            //{
            //    for (var word = 0; word < WORD.Length; word++)
            //    {
            //        if (WORD[word].Contains("/"))
            //        {
            //            var splitWord = WORD[word].Split('/');
            //            int Num;
            //            bool isNum = int.TryParse(splitWord[0], out Num);
            //            if (isNum)
            //                wordB = WORD[word];
            //        }
            //        else if (WORD[word].Contains("."))
            //        {
            //            var splitWord = WORD[word].Split('.');
            //            int Num;
            //            bool isNum = int.TryParse(splitWord[0], out Num);
            //            if (isNum)
            //                wordB = WORD[word];
            //        }
            //    }

            //    Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));
            //}
            //else if (nameColumn.Contains("Net"))
            //{
            //    for (var word = 0; word < WORD.Length; word++)
            //    {
            //        if (WORD[word].Contains("kg") || WORD[word].Contains("KG") && WORD[word].Contains("Kg"))
            //        {
            //            if (WORD[word - 1].Length == 5)
            //                break;
            //            else
            //                wordB += WORD[word - 1];
            //        }
            //        else if (WORD[word].Contains(","))
            //        {
            //            if (WORD[word].Length > 5)
            //                break;
            //            else
            //                wordB += WORD[word];
            //        }

            //        //double Num;
            //        //bool isNum = double.TryParse(WORD[word], out Num);
            //        //if (isNum)
            //        //    wordB = $"{Num}";
            //    }

            //    Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));
            //}
            //else if (nameColumn.Contains("Quality"))
            //{
            //    for (var word = 0; word < WORD.Length; word++)
            //    {
            //        if (WORD[word].Contains("Quality"))
            //            wordB = WORD[word + 1];
            //    }

            //    Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));
            //}
            //else if (nameColumn.Contains("Elaboration"))
            //{
            //    for (var word = 0; word < WORD.Length; word++)
            //    {
            //        if (WORD[word].Contains("N°") || WORD[word].Contains("№") || WORD[word].Contains("N*"))
            //            wordB = WORD[word - 1];
            //    }

            //    Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));
            //}
            //else if (nameColumn.Contains("Plant"))
            //{
            //    for (var word = 0; word < WORD.Length; word++)
            //    {
            //        if (WORD[word].Contains("PLANT"))
            //            wordB = WORD[word + 1];
            //    }

            //    Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));
            //}
            //else if (nameColumn.Contains("Size"))
            //{
            //    bool second = false;
            //    for (var word = 0; word < WORD.Length; word++)
            //    {
            //        int Num;
            //        bool isNum = int.TryParse(WORD[word], out Num);

            //        if (isNum)
            //        {
            //            wordB += Num + " ";

            //            if (second)
            //                break;

            //            second = true;
            //        }
            //        else if (WORD[word].Contains("-"))
            //            wordB += WORD[word] + " ";
            //    }

            //    Table.Add(new FillInTheTable(nameColumn, wordB, numberPicture));
            //}
            //else
            //    Table.Add(new FillInTheTable(nameColumn, wordA, numberPicture));

            foreach (var cell in Table)
            {
                dataGridView1.Rows[cell.Index].Cells[cell.ColumnName].Value = cell.Word;
            }
        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        public string ImgToStr(Image Img)
        {
            MemoryStream Memostr = new MemoryStream();
            Img.Save(Memostr, ImageFormat.Jpeg);
            byte[] arrayimg = Memostr.ToArray();

            return Convert.ToBase64String(arrayimg);
        }

        public async Task<string> RecognitionRequestAsync (string json)
        {
            var key = "AQVN1_mRA_MyD1VTUxC30Gn_D2r3aYtfGQ4O_4ud";

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

        private void ClearPictureBox()
        {
            using (Graphics g = Graphics.FromImage(bmpD))
            {
                g.Clear(Color.White);
            }
        }

        private void DrawRectangleOnPictureBox(Color col, int pencSize, Rectangle rectangle, string text, int x, int y)
        {
            var pen = new Pen(col, pencSize);
            using (Graphics g = Graphics.FromImage(bmpD))
            {
                g.DrawRectangle(pen, rectangle);
                g.DrawString(text, drawFont, drawBrush, x, y);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                ColorizeText();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //List<RectangleW> recTrueList;

            //    recTrueList = RectanglesNotIdeal;
           
            //if (turnedAround)
            //    recTrueList = RotatedPerfectRectangles;

            //if (cut)
            //    recTrueList = СroppedRectangles;

            //for (int i = 0; i < recTrueList.Count; i++)
            //{
            //    var widthRec = recTrueList[i].Width;
            //    var heightRec = recTrueList[i].Height;
            //    var sizeRec = new Size(widthRec, heightRec);
            //    var rectangle = new Rectangle(recTrueList[i].PositionFigure, sizeRec);

            //    if (rectangle.Contains(e.X, e.Y))
            //    {
            //        if (!checkBox2.Checked && !checkBox3.Checked)
            //        {
            //            xAbsolyte = recTrueList[i].PositionFigure.X;
            //            yAbsolyte = recTrueList[i].PositionFigure.Y;
            //            rightWord = recTrueList[i].Text;
            //            var location = new Point(xAbsolyte, yAbsolyte);
            //            var columnName = dataGridView1.Columns[enterColumn].Name;
            //            desiredRectangles.Add(new RectangleW(location, widthRec, heightRec, rightWord, columnName));

            //            dataGridView1.Rows[0].Cells[enterColumn].Value = rightWord;
            //            break;
            //        }
            //        else if (checkBox2.Checked && !checkBox3.Checked)
            //        {
            //            if (count < 2)
            //            {
            //                massRec[count] = new RectangleW(recTrueList[i].PositionFigure, widthRec, heightRec, recTrueList[i].Text, string.Empty);

            //                ClearPictureBox();

            //                foreach (var po in recTrueList)
            //                {
            //                    var widthRecPo = po.Width;
            //                    var heightRecPo = po.Height;
            //                    var sizeRecPo = new Size(widthRecPo, heightRecPo);
            //                    var rectanglePo = new Rectangle(po.PositionFigure, sizeRecPo);
            //                    if (massRec[0].Text == po.Text || (massRec[1] != null && massRec[1].Text == po.Text))
            //                    {
            //                        DrawRectangleOnPictureBox(Color.Red, 1, rectanglePo, po.Text, po.PositionFigure.X, po.PositionFigure.Y);
            //                    }
            //                    else
            //                    {
            //                        DrawRectangleOnPictureBox(Color.Blue, 1, rectanglePo, po.Text, po.PositionFigure.X, po.PositionFigure.Y);
            //                    }
            //                }

            //                if (count == 1)
            //                    button3.Enabled = true;

            //                pictureBox1.Refresh();
            //                count++;
            //                break;
            //            }
            //            else
            //            {
            //                massRec[0] = null;
            //                massRec[1] = null;
            //                count = 0;

            //                ClearPictureBox();

            //                foreach (var t in recTrueList)
            //                {
            //                    var widthRecPo = t.Width;
            //                    var heightRecPo = t.Height;
            //                    var sizeRecPo = new Size(widthRecPo, heightRecPo);
            //                    var rectanglePo = new Rectangle(t.PositionFigure, sizeRecPo);
            //                    DrawRectangleOnPictureBox(Color.Blue, 1, rectanglePo, t.Text, t.PositionFigure.X, t.PositionFigure.Y);
            //                }
            //            }
            //        }
            //        else if (checkBox3.Checked && !checkBox2.Checked)
            //        {
            //            recDelete = new RectangleW(recTrueList[i].PositionFigure, widthRec, heightRec, recTrueList[i].Text, string.Empty);

            //            ClearPictureBox();

            //            foreach (var po in recTrueList)
            //            {
            //                var widthRecPo = po.Width;
            //                var heightRecPo = po.Height;
            //                var sizeRecPo = new Size(widthRecPo, heightRecPo);
            //                var rectanglePo = new Rectangle(po.PositionFigure, sizeRecPo);
            //                if (recDelete.Text == po.Text)
            //                {
            //                    DrawRectangleOnPictureBox(Color.Green, 2, rectanglePo, po.Text, po.PositionFigure.X, po.PositionFigure.Y);
            //                }
            //                else
            //                {
            //                    DrawRectangleOnPictureBox(Color.Blue, 1, rectanglePo, po.Text, po.PositionFigure.X, po.PositionFigure.Y);
            //                }
            //            }

            //            if (recDelete != null)
            //                button4.Enabled = true;

            //            pictureBox1.Refresh();
            //            break;
            //        }
            //    }
            //}
        }

        public Image ResizeImage(Image img, decimal maxWidth, decimal maxHeight)
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

            return img = (Image)(new Bitmap(img, new Size((int)resizeWidth, (int)resizeHeight)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //button3.Enabled = false;
            //button4.Enabled = false;
            //button1.Enabled = false;
            button7.Enabled = false;
            //checkBox2.Enabled = false;
            //checkBox2.Enabled = false;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //enterColumn = e.ColumnIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ClearPictureBox();

            //var halfCoordinateWordX1 = massRec[0].PositionFigure.X;
            //var halfCoordinateWordY1 = massRec[0].PositionFigure.Y;
            //int halfCoordinateWordX4 = massRec[1].PositionFigure.X;
            //int halfCoordinateWordY4 = massRec[1].PositionFigure.Y;

            //int distance = 0;
            //double cos = 0;
            //double sin = 0;

            //distance = halfCoordinateWordY1 - halfCoordinateWordY4;
            //var BC = (double)halfCoordinateWordX4 - (double)halfCoordinateWordX1;
            //var AB = Math.Sqrt(Math.Pow(BC, 2) + Math.Pow(distance, 2));
            //cos = BC / AB;
            //sin = (double)distance / AB;

            //foreach (var rec in RectanglesIdeal)
            //{
            //    var xWord = rec.PositionFigure.X;
            //    var yWord = rec.PositionFigure.Y;
            //    var widthTrue = rec.Width;
            //    var heightTrue = rec.Height;
            //    var wordTrue = rec.Text;

            //    //if (distance < 0)
            //    //    distance = 0;

            //    var trueXWord = (xWord * cos) - (yWord * sin);
            //    var trueYWord = (xWord * sin) + (yWord * cos) /*- distance*/;
            //    var sizeRectangleWord = new Size(widthTrue, heightTrue);
            //    var location = new Point((int)trueXWord, (int)trueYWord);
            //    var rectangleTrue = new Rectangle(location, sizeRectangleWord);

            //    DrawRectangleOnPictureBox(Color.Blue, 1, rectangleTrue, wordTrue, (int)trueXWord, (int)trueYWord);

            //    RotatedPerfectRectangles.Add(new RectangleW(location, widthTrue, heightTrue, wordTrue, string.Empty));
            //    pictureBox1.Refresh();
            //    //button3.Enabled = false;
            //    //checkBox2.Checked = false;
            //}
            //turnedAround = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked)
            //    checkBox3.Enabled = false;
            //else
            //    checkBox3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //ClearPictureBox();

            //List<RectangleW> recsNeedList;

            //    recsNeedList = RectanglesNotIdeal;

            //if (RotatedPerfectRectangles.Count > 0)
            //    recsNeedList = RotatedPerfectRectangles;

            //recsNeedList.RemoveAll(a => a.PositionFigure.Y >= recDelete.PositionFigure.Y + recDelete.Height);
            //var relativeX1 = recsNeedList.Min(a => a.PositionFigure.X);
            //var relativeY1 = recsNeedList.Min(a => a.PositionFigure.Y);
            ////var relativeY1 = recsNeedList.FirstOrDefault(a => a.PositionFigure.X == relativeX1).PositionFigure.Y;

            //var relativeY3 = recsNeedList.Max(a => a.PositionFigure.Y);
            //var heightY3 = recsNeedList.FirstOrDefault(a => a.PositionFigure.Y == relativeY3).Height;
            //var relativeX3 = recsNeedList.Max(a => a.PositionFigure.X);
            //var weightX3 = recsNeedList.FirstOrDefault(a => a.PositionFigure.X == relativeX3).Width;
            ////var relativeX3 = recsNeedList.FirstOrDefault(a => a.PositionFigure.Y == relativeY3).PositionFigure.X;

            //double side = 1000;
            //var coefficientXWord = side / (double)(relativeX3 + weightX3 - relativeX1);
            //var coefficientYWord = side / (double)(relativeY3 + heightY3 - relativeY1);

            //foreach (var rec in recsNeedList)
            //{
            //    var xWord = rec.PositionFigure.X;
            //    var yWord = rec.PositionFigure.Y;
            //    var widthTrue = rec.Width;
            //    var heightTrue = rec.Height;
            //    var wordTrue = rec.Text;

            //    int widthWord = (int)((double)widthTrue * coefficientXWord);
            //    int heightWord = (int)((double)heightTrue * coefficientYWord);
            //    var trueWordCoordinateX = (double)(xWord - relativeX1) * coefficientXWord;
            //    var trueWordCoordinateY = (double)(yWord - relativeY1) * coefficientYWord;

            //    var position = new Point((int)trueWordCoordinateX, (int)trueWordCoordinateY);
            //    var rectangleWord = new Rectangle((int)trueWordCoordinateX, (int)trueWordCoordinateY, widthWord, heightWord);

            //    if (rec.Text == recDelete.Text)
            //    {
            //        DrawRectangleOnPictureBox(Color.Blue, 1, rectangleWord, wordTrue, (int)trueWordCoordinateX, (int)trueWordCoordinateY);

            //        СroppedRectangles.Add(new RectangleW(position, widthWord, heightWord, wordTrue, string.Empty));
            //        break;
            //    }

            //    DrawRectangleOnPictureBox(Color.Blue, 1, rectangleWord, wordTrue, (int)trueWordCoordinateX, (int)trueWordCoordinateY);

            //    СroppedRectangles.Add(new RectangleW(position, widthWord, heightWord, wordTrue, string.Empty));
            //}

            //if (desiredRectangles.Count > 0)
            //{
            //    foreach (var desired in desiredRectangles)
            //    {
            //        var x = desired.PositionFigure.X;
            //        var y = desired.PositionFigure.Y;
            //        var rectangleNew = new Rectangle(x, y, desired.Width, desired.Height);

            //        putValuesInColumns(rectangleNew, desired.NameColumn, СroppedRectangles);

            //        DrawRectangleOnPictureBox(Color.Black, 4, rectangleNew, desired.Text, x, y);
            //    }
            //}

            //cut = true;
            //pictureBox1.Refresh();
            ////checkBox3.Checked = false;
            ////button4.Enabled = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox3.Checked)
            //    checkBox2.Enabled = false;
            //else
            //    checkBox2.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            desiredRectangles.Clear();
            dataGridView1.Rows.Clear();
        }

        Image image;
        private void button6_Click(object sender, EventArgs e)
        {
            ClearPictureBox();

            var formPictureTrue = new FormPicturesTrue();
            var dialogResult = formPictureTrue.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                desiredRectangles.Clear();
                image = formPictureTrue.NeedAPicture;
                button1.Enabled = true;
                label2.Text = $"Идеальная картинка: {formPictureTrue.NamePicture}";

                if (formPictureTrue.ListHighlightedRectangles.Count > 0)
                    desiredRectangles = formPictureTrue.ListHighlightedRectangles;

                //if (formPictureTrue.ArbitraryArea.Count > 0)
                //{
                //    //AreaRandom = formPictureTrue.ArbitraryArea;
                //    MinPoints = formPictureTrue.MinPoints;
                //}
                foreach(var rec in desiredRectangles)
                {
                    var rectangle = new Rectangle(rec.PositionFigure.X, rec.PositionFigure.Y, rec.Width, rec.Height);
                    DrawRectangleOnPictureBox(Color.Red, 1, rectangle, rec.NameColumn, rec.PositionFigure.X, rec.PositionFigure.Y);
                }

                //foreach(var area in AreaRandom)
                //{
                //    using (Graphics g = Graphics.FromImage(bmpD))
                //    {
                //        g.DrawPolygon(new Pen(Color.Red), area.Value.ToArray());
                //        foreach(var point in MinPoints)
                //        {
                //            if (area.Key == point.Key)
                //            {
                //                g.DrawString(area.Key, drawFont, drawBrush, point.Value.X, point.Value.Y);
                //                break;
                //            }
                //        }
                //    }
                //}
                pictureBox1.Image = bmpD;
                button7.Enabled = true;
            }
            else
                Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            XmlSerializer writer = new XmlSerializer(typeof(RectangleW[]));
            if (File.Exists("Settings.xml"))
            {
                File.Delete("Settings.xml");
                using (FileStream fs = new FileStream("Settings.xml", FileMode.OpenOrCreate))
                {
                    writer.Serialize(fs, desiredRectangles.ToArray());
                }
            }
            else
            {
                using (FileStream fs = new FileStream("Settings.xml", FileMode.OpenOrCreate))
                {
                    writer.Serialize(fs, desiredRectangles.ToArray());
                }
            }
            //FileStream file = File.Create(path);

            //writer.Serialize(file, desiredRectangles.ToArray());
            //file.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ClearPictureBox();

            XmlSerializer serializer = new XmlSerializer(typeof(RectangleW[]));

            using (StreamReader reader = new StreamReader("Settings.xml"))
            {
                var rectangles = (RectangleW[])serializer.Deserialize(reader);
                foreach(var rec in rectangles)
                {
                    desiredRectangles.Add(rec);
                    var desiredRectangle = new Rectangle(rec.PositionFigure, new Size(rec.Width, rec.Height));
                    DrawRectangleOnPictureBox(Color.Red, 1, desiredRectangle, rec.NameColumn, rec.PositionFigure.X, rec.PositionFigure.Y);
                }
            }

            pictureBox1.Image = bmpD;
            button1.Enabled = true;
        }

        //FileSystemWatcher watcher = new FileSystemWatcher();

        //public void Check()
        //{
        //    if (args.Length != 2)
        //    {
        //        using (var fbd = new FolderBrowserDialog())
        //        {
        //            DialogResult result = fbd.ShowDialog();

        //            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        //            {
        //                args[0] = fbd.SelectedPath;
        //            }
        //            else
        //                return;
        //        }
        //    }

        //    watcher.Path = args[0];

        //    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        //    watcher.Filter = "*.jpeg";

        //    watcher.Created += new FileSystemEventHandler(OnCreatedAsync);
        //    watcher.EnableRaisingEvents = true;
        //}

        public async void OnCreatedAsync(object source, FileSystemEventArgs e)
        {
            Image temp = Image.FromFile(e.FullPath);
            var img = ResizeImage(temp, 1080, 1080);
            string base64 = ImgToStr(img);

            TextDetectionConfig textDetectionConfig = new TextDetectionConfig();
            textDetectionConfig.languageCodes.Add("*");
            textDetectionConfig.model = "passport";

            Feature f = new Feature();
            f.type = "TEXT_DETECTION";
            f.text_detection_config = textDetectionConfig;

            AnalyzeSpec analyze = new AnalyzeSpec();
            analyze.content = base64;
            analyze.features.Add(f);
            analyze.mime_type = "image";

            TextRecognising bodyJson = new TextRecognising();
            bodyJson.folderId = "b1gr5t1i5o6d4p2h45rn";
            bodyJson.analyze_specs.Add(analyze);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string json = System.Text.Json.JsonSerializer.Serialize(bodyJson, options);

            string outputJson = await RecognitionRequestAsync(json);

            var restored = System.Text.Json.JsonSerializer.Deserialize<AllResults>(outputJson);

            foreach (var results in restored.results)
            {
                foreach(var result in results.results)
                {
                    foreach(var pages in result.textDetection.pages)
                    {
                        foreach (var entity in pages.entities)
                        {
                            if (entity.name == "name")
                            {
                                dataGridView2.Rows[0].Cells["ColumnName"].Value = entity.text;
                            }
                            else if (entity.name == "middle_name")
                            {
                                dataGridView2.Rows[0].Cells["ColumnMiddleName"].Value = entity.text;
                            }
                            else if (entity.name == "surname")
                            {
                                dataGridView2.Rows[0].Cells["ColumnSurname"].Value = entity.text;
                            }
                            else if (entity.name.Contains("gender"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnGender"].Value = entity.text;
                            }
                            else if (entity.name.Contains("citizenship"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnCitizenship"].Value = entity.text;
                            }
                            else if (entity.name.Contains("birth_date"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnBirthDate"].Value = entity.text;
                            }
                            else if (entity.name.Contains("birth_place"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnBirthPlace"].Value = entity.text;
                            }
                            else if (entity.name.Contains("number"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnNumber"].Value = entity.text;
                            }
                            else if (entity.name.Contains("issue_date"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnIssueDate"].Value = entity.text;
                            }
                            else if (entity.name.Contains("birth_place"))
                            {
                                dataGridView2.Rows[0].Cells["ColumnBirthPlace"].Value = entity.text;
                            }
                        }
                    }
                }  
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private System.IO.FileSystemWatcher m_Watcher;
        private bool m_bIsWatching;
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (m_bIsWatching)
            {
                m_bIsWatching = false;
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                button3.BackColor = Color.LightSkyBlue;
                button3.Text = "Start Watching";
            }
            else
            {
                m_bIsWatching = true;
                button3.BackColor = Color.Red;
                button3.Text = "Stop Watching";
                m_Watcher = new System.IO.FileSystemWatcher();

                if (File.Exists("Path.txt"))
                {
                    using (StreamReader reader = new StreamReader("Path.txt"))
                    {
                        var path = reader.ReadToEnd();
                        var l = path.Length;
                        m_Watcher.Path = path.Remove(l - 2);
                    }
                }
                else
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        DialogResult result = fbd.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {
                            m_Watcher.Path = fbd.SelectedPath + "\\";
                        }
                        else
                            return;
                    }

                    using (StreamWriter writer = new StreamWriter("Path.txt", false))
                    {
                        writer.WriteLine(m_Watcher.Path);
                    }
                }

                m_Watcher.Filter = "*.";

                m_Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                m_Watcher.Created += new FileSystemEventHandler(OnCreatedAsync);

                m_Watcher.EnableRaisingEvents = true;
            }
        }
    }

    public class Entity
    {
        public string name { get; set; }
        public string text { get; set; }
    }

    public class Passport
    {
        public List<Entity> entities { get; set; }
    }

    public class RectangleW
    {
        public int Width;
        public int Height;
        public string Text;
        public Point PositionFigure;
        public string NameColumn;

        public RectangleW(Point position, int width, int height, string text, string nameColumn)
        {
            PositionFigure = position;
            Width = width;
            Height = height;
            Text = text;
            NameColumn = nameColumn;
        }
        public RectangleW()
        {

        }
    }

    public class FillInTheTable
    {
        public string ColumnName;
        public string Word;
        public int Index;

        public FillInTheTable(string columnName, string word, int index)
        {
            ColumnName = columnName;
            Word = word;
            Index = index;
        }
    }
}
