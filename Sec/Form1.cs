using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sec
{
    public partial class Form : System.Windows.Forms.Form
    {
        private DateTime _dateStart; // время старта таймера
        private List<double> _times; // сюда сохраняются интервалы времени        
        private double timeTimerMilliseconds; // сюда передаём время в миллисекундах, а потом помещаем в label1 и listBox

        public Form()
        {
            _dateStart = new DateTime();
            _times = new List<double>();
            InitializeComponent();            
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            timeTimerMilliseconds = GetTimeTimer(); 
            label1.Text = ConvertToTextTime(timeTimerMilliseconds);                                     
        }

        private double GetTimeTimer()
        {
            var start = _dateStart;
            var time = DateTime.Now - start;
            return time.TotalMilliseconds / 10;
        }
        
        private string ConvertToTextTime(double time)
        {
            int msec = Convert.ToInt32(time);
            int sec = 0;
            int min = 0;
            if (msec < 100)
            {
                msec = Convert.ToInt32(time);
            }
            else
            {
                sec = msec / 100;
                min = sec / 60;
                msec %= 100;
                msec %= 60;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
               
        private void StartTimer()
        {
            btnSaveValue.Enabled = false;
            btnReset.Enabled = false;
            btnDelete.Enabled = false;            
            _dateStart = DateTime.Now;
            timer1.Enabled = true;
        }

        private void StopTimer()
        {
            btnSaveValue.Enabled = true;
            btnReset.Enabled = true;
            btnDelete.Enabled = true;
            timer1.Enabled = false;                      
            _times.Add(timeTimerMilliseconds);
            listBox.Items.Add(ConvertToTextTime(timeTimerMilliseconds));
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

        private void btnSaveValue_Click(object sender, EventArgs e)
        {
            btnSaveValue.Enabled = false;
            using (var sw = new StreamWriter("test.txt", true))
            {
                int count = 0;
                foreach (double i in _times)
                {
                    var timeText = ConvertToTextTime(i);
                    count++;
                    sw.WriteLine($"{count.ToString()}. {timeText}");
                }
            }            
        }
    }
}
