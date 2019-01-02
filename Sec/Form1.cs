using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sec
{
    public partial class Form1 : Form
    {
        //start timer
        private DateTime _dateStart;
        //time array timers
        private List<double> _times;
        public Form1()
        {
            _dateStart = new DateTime();
            _times = new List<double>();
            InitializeComponent();
        }
        private void timer1_Tick_1(object sender, EventArgs e) //тут таймер трудится
        {
            var timeTimer = GetTimeTimer();
            label1.Text = ConvertToTextTime(timeTimer);
        }

        private string ConvertToTextTime(double time)
        {
            int msec = Convert.ToInt32(time);
            int sec = 0;
            int min = 0;
            if (time < 1000.0)
            {
                msec = Convert.ToInt32(time);
            }
            else
            {
                //if (second < 0)
                //{
                //    return "";
                //}
                //else
                //{
                //    if (second == 0)
                //    {
                //        return "00:00";
                //    }
                //    else
                //    {
                //        int minute = 0;
                //        int hour = 0;

                //        minute = second / 60;
                //        hour = minute / 60;
                //        minute %= 60;
                //        second %= 60;

                //        if (hour > 0)
                //        {
                //            return $"{hour:d2}:{minute:d2}:{second:d2}";
                //        }
                //        else
                //        {
                //            return $"{minute:d2}:{second:d2}";
                //        }
                //    }
                //}                           

                sec = msec / 1000;
                min = sec / 60;
                sec %= 60;
                msec %= 1000;

                //min = sec / 60;
                //min %= 60;
                //sec %= 60;
                //msec %= 1000;
            }

            string msecText = msec.ToString();
            while (msecText.Length < 3)
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

        private double GetTimeTimer()
        {
            var start = _dateStart;
            var time = DateTime.Now - start;
            return time.TotalMilliseconds;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //throw new ArgumentException("Текст Error");
                bool workTimer = timer1.Enabled;
                //таймер останавливаем
                if (workTimer)
                {
                    StopTimer();
                }
                else
                //запускаем таймер
                {
                    StartTimer();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartTimer()
        {
            btnReset.Enabled = false;
            btnStart.Text = "Остановить";
            _dateStart = DateTime.Now; //это я так обнуляю время
            timer1.Enabled = true;
        }
        private void StopTimer()
        {
            btnReset.Enabled = true;

            timer1.Enabled = false;
            btnStart.Text = "Старт";
            var time = GetTimeTimer();
            _times.Add(time);
            var textTime = ConvertToTextTime(time);
            label1.Text = ConvertToTextTime(0);
            listBox.Items.Add(textTime);
            lblMidValue.Text = ConvertToTextTime(GetTimeMiddle(_times));
            listBox.SelectedIndex = listBox.Items.Count - 1;
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


        private void btnTest_Click(object sender, EventArgs e)
        {
            label1.Text = ConvertToTextTime(500000.0);
        }


        private bool flagPress = false;

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                btnStart_Click(null, null);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            _times.Clear();
            lblMidValue.Text = ConvertToTextTime(0.0);
        }
    }
}
