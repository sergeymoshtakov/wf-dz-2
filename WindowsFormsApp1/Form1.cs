using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DateTime startTime;
        private Color[] colors;
        private int currentColorIndex;
        private static int numberOfClicks;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1000;
            startTime = DateTime.Now;

            timer2.Interval = 20000;

            colors = new Color[]
            {
                Color.Black,
                Color.Red,
                Color.Yellow,
                Color.Cyan,
                Color.Blue,
                Color.Pink,
                Color.White
            };

            currentColorIndex = 0;

            numberOfClicks = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int millisecondsElapsed = (int)(DateTime.Now - startTime).TotalMilliseconds;
            this.Text = Convert.ToString(millisecondsElapsed);

            DateTime newYear = new DateTime(DateTime.Now.Year + 1, 1, 1);
            DateTime exam = new DateTime(2023, 6, 13);
            TimeSpan timeLeftToNewYear = newYear - DateTime.Now;
            TimeSpan timeLeftToExam = exam - DateTime.Now;
            UpdateLabel1(timeLeftToNewYear);
            UpdateLabel2(timeLeftToExam);

            Color currentColor = this.BackColor;
            Color nextColor = colors[currentColorIndex];
            AnimateBackgroundColor(currentColor, nextColor);
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }

        private void UpdateLabel1(TimeSpan timeLeft)
        {
            label1.Text = string.Format($"До нового года: {timeLeft.Days} дней, {timeLeft.Hours}:{timeLeft.Minutes}:{timeLeft.Seconds}");
        }

        private void UpdateLabel2(TimeSpan timeLeft)
        {
            label2.Text = string.Format($"До экзамена: {timeLeft.Days} дней, {timeLeft.Hours}:{timeLeft.Minutes}:{timeLeft.Seconds}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AnimateBackgroundColor(Color startColor, Color endColor)
        {
            int animationSteps = 50;
            int animationDuration = 100;

            int rStep = (endColor.R - startColor.R) / animationSteps;
            int gStep = (endColor.G - startColor.G) / animationSteps;
            int bStep = (endColor.B - startColor.B) / animationSteps;

            for (int i = 0; i < animationSteps; i++)
            {
                int r = startColor.R + (rStep * i);
                int g = startColor.G + (gStep * i);
                int b = startColor.B + (bStep * i);

                Color intermediateColor = Color.FromArgb(r, g, b);

                this.BackColor = intermediateColor;

                Application.DoEvents();
                System.Threading.Thread.Sleep(animationDuration / animationSteps);
            }

            this.BackColor = endColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if((int)(DateTime.Now - startTime).TotalMilliseconds <= 20000)
            {
                numberOfClicks++;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            SaveNumberToFile(numberOfClicks, "text.txt");
            int maxNumber = ReadFromFile("text.txt").Max();
            MessageBox.Show($"Вы нажали на кнопку {numberOfClicks} раз\nРекорд: {maxNumber}");
        }

        private static void SaveNumberToFile(int number, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(numberOfClicks);
            }
        }

        private static List<int> ReadFromFile(string fileName)
        {
            List<int> numbers = new List<int>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int number;
                    try
                    {
                        if (int.TryParse(line, out number))
                        {
                            numbers.Add(number);
                        }
                        else
                        {
                            throw new Exception("Wrong format");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e.Message}");
                    }
                }
            }
            return numbers;
        }
    }
}
