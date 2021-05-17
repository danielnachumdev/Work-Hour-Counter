using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Work_Hour_Counter
{
    class DataUnit
    {
        private DateTime date;
        private int[] data;
        private double TotalTime;
        /*public DataUnit(DateTime de, int[] da)
        {
            this.date = de;
            this.data = da;
        }*/
        public DataUnit(string s)
        {
            this.date = Convert.ToDateTime(s.Substring(0, s.IndexOf(' ')));
            string dataastext = s.Substring(s.IndexOf(' ') + 1);
            int SelectedLabelsCount = 0;
            List<int> datalist = new List<int>();
            foreach (char c in dataastext)
            {
                if (c == '1'||c=='0')
                {
                    try
                    {
                        datalist.Add(int.Parse(c.ToString()));
                        if (c == '1')
                            SelectedLabelsCount++;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString() + "\n\n This Error Ocuured becuase somewhere in the data the numbers are not formatted propely");
                    }

                }
            }
            this.data = datalist.ToArray();
            this.TotalTime = (double)SelectedLabelsCount / 4;
        }
        public DateTime getDate()
        {
            return this.date;
        }
        public int[] getData()
        {
            return this.data;
        }
        public bool isToday()
        {
            return this.date == DateTime.Now.Date ? true : false;
        }
        public string toString()
        {
            return this.getDate().ToString("dd/MM/yyyy") + " " + string.Join("", this.getData());
        }
        public string getTotalTimeString()
        {
            return "Total Time " + (int)this.getTotalTiemValue() + ":" + (this.getTotalTiemValue()-(int)this.getTotalTiemValue()) * 4 * 15;
        }
        public double getTotalTiemValue()
        {
            return this.TotalTime;
        }
        public string getExportString()
        {
            return this.getDate().ToString("dd/MM/yyyy") + " " + this.getTotalTimeString();
        }
        public int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
        public bool isEmpty()
        {
            foreach (int item in this.data)
                if (item != 0)
                    return false;
            return true;
        }
    }
}
