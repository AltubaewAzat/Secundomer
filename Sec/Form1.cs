using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sec
{
    public partial class Form : System.Windows.Forms.Form
    {
        //start timer
        private DateTime _dateStart; // время старта таймера
        //time array timers
        private List<double> _times; // сюда сохраняются интервалы времени    

        public Form()
        {
            _dateStart = new DateTime();
            _times = new List<double>();
            InitializeComponent();            
        }

        // во время работы таймера у нас:
        // 1) работает метод GetTimeTimer(), который подсчитывает нам прошедшие миллисекунды
        // 2) работает основное табло и показывает нам прошедшие секунды
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            var timeTimer = GetTimeTimer(); // Дёргаем прошедшее время в миллисекундах
            label1.Text = ConvertToTextTime(timeTimer); // конвертируем всё в string и засовываем в основное табло
        }

        // конвертируем миллисекунды типа double в тип string, для того чтобы вывести их на listBox
        private string ConvertToTextTime(double time)
        {
            int msec = Convert.ToInt32(time); // воткнул все миллисекунды, преобразованные в int
            int sec = 0;
            int min = 0;
            if (time <= 100.0)
            {
                msec = Convert.ToInt32(time);
            }
            else
            {
                sec = msec / 100;
                min = sec / 60;
                sec %= 60;
                msec %= 100;
            }

            string msecText = msec.ToString();
            while (msecText.Length < 2)
            {
                msecText = "0" + msecText;
            }

            string secText = sec.ToString();
            while (secText.Length < 2)
            {
                secText = "0" + secText;
            }

            string minText = min.ToString();
            while (minText.Length < 2)
            {
                minText = "0" + minText;
            }
            return $"{minText}:{secText}:{msecText}";
        }

        // подсчитываем колличество миллисекунд, которое прошло от момента старта до финиша
        private double GetTimeTimer()
        {
            var start = _dateStart;
            var time = DateTime.Now - start;
            return time.TotalMilliseconds / 10;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
               
        private void StartTimer()
        {
            btnReset.Enabled = false;
            btnDelete.Enabled = false;            
            _dateStart = DateTime.Now;
            timer1.Enabled = true;
        }

        private void StopTimer()
        {
            btnReset.Enabled = true;
            btnDelete.Enabled = true;
            timer1.Enabled = false;                        
            var time = GetTimeTimer();
            _times.Add(time);
            var textTime = ConvertToTextTime(time);
            listBox.Items.Add(textTime);
            lblMidValue.Text = ConvertToTextTime(GetTimeMiddle(_times));
            listBox.SelectedIndex = listBox.Items.Count - 1;
            if (listBox.Items.Count > 1)
            {
                listBox.SetSelected(listBox.Items.Count -2, false);
            }
        }

        private double GetTimeMiddle(List<double> times)
        {
            if (times.Count == 0)
            {
                return 0;
            }
            else if (times.Count == 1)
            {
                return times[0];
            }
            else if (times.Count == 2)
            {
                return (times[0] + times[1]) / 2.0;
            }
            else
            {
                return times.Sum() / Convert.ToDouble(times.Count);
            }
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            _times.Clear();
            lblMidValue.Text = ConvertToTextTime(0);
            this.listBox.Focus();                   
        }

        private void Form1_Load(object Sender, EventArgs e)
        {
            //btnDelete.TabIndex = 0;
            //label1.TabIndex = 1;
            //listBox.TabIndex = 2;            
            //btnReset.TabIndex = 3;                    
        }

        //удаление строчек из listbox
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] indexes = listBox.SelectedIndices.OfType<int>().OrderByDescending(i => i).ToArray();
                foreach (int j in indexes)
                {
                    listBox.Items.RemoveAt(j);
                    _times.RemoveAt(j);
                    lblMidValue.Text = ConvertToTextTime(GetTimeMiddle(_times));
                }                
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {            
            if (e.KeyCode == Keys.Space)
            {
                try
                {
                    bool workTimer = timer1.Enabled;                    
                    if (workTimer)
                    {
                        StopTimer();
                    }
                    else                    
                    {
                        StartTimer();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        private void saveValue_Click(object sender, EventArgs e)
        {
            //var saveValueTime = ConvertToTextTime(_times);
            using (var sw = new StreamWriter("test.txt", true))
            {
                //bool workTimer = timer1.Enabled;
                //if (!workTimer)
                foreach (double i in _times)
                {
                    var timeText = ConvertToTextTime(i);
                    sw.Write(timeText);
                }
            }
        }
    }
}
