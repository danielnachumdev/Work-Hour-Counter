using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Net.Http;

namespace Work_Hour_Counter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        DateTimePicker dt;
        private void MainForm_Load(object sender, EventArgs e)
        {
            Size ScreenSize = Screen.FromControl(this).Bounds.Size;
            int aspectA = 10, aspectB = 5,aspectConst=16; //aspect ration A/B as in 16/9
            double 
                nHeight = ScreenSize.Height / aspectConst * aspectA,
                nWidth = ScreenSize.Width/aspectConst*aspectB;
            this.Height = (int)nHeight;
            this.Width = 700;//(int)nWidth;
            this.MaximizeBox = false;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            SquareSize = new Size((int)((double)this.Width / 800 * 50), (int)((double)this.Width / 900 * 50));
            this.Location = new Point((ScreenSize.Width - this.Width) / 2, (ScreenSize.Height - this.Height) / 2);
            PathLabel.Text = "";
            cPacket = DateTime.Now.Date.ToString("dd/MM/yyyy") + " ";
            int padding = 10;

            //Buttons

            Point OSAnchor = new Point(this.Width / 3 * 2 + padding, padding);
            OpenButton.Location = OSAnchor;
            OpenButton.Size = new Size((this.Width / 3 - padding * 7 / 2) / 3, 30);
            SaveButton.Size = new Size((this.Width / 3 - padding * 7 / 2) / 3, 30);
            SaveAsButton.Size = new Size((this.Width / 3 - padding * 7 / 2) / 3, 30);
            SaveButton.Location = new Point(OpenButton.Location.X + OpenButton.Width, OSAnchor.Y);
            SaveAsButton.Location = new Point(SaveButton.Location.X + SaveButton.Width, OSAnchor.Y);
            PathLabel.Location = new Point(OSAnchor.X, OSAnchor.Y + 40);

            //DateTimePicker & Controls
            Point DTAnchor = new Point(this.Width / 3 * 2 + padding, padding+70);

            dt = new DateTimePicker();
            dt.Location = DTAnchor;
            dt.Size = new Size(this.Width / 3 - padding * 2-15, 30);
            dt.ValueChanged += new EventHandler(dateTimePicker_ValueChanged);
            this.Controls.Add(dt);
            Button PreviousButton = new Button();
            PreviousButton.Text = "Previous Day";
            PreviousButton.Size = new Size((this.Width / 3 - padding * 7/2) / 2, 30);
            PreviousButton.Location = new Point(DTAnchor.X, DTAnchor.Y + 30);
            PreviousButton.Click += new EventHandler(PreviousButton_Click);
            this.Controls.Add(PreviousButton);
            Button NextButton = new Button();
            NextButton.Text = "Next Day";
            NextButton.Size = new Size((this.Width / 3 - padding * 7/2) / 2, 30);
            NextButton.Location = new Point(PreviousButton.Location.X + PreviousButton.Width, DTAnchor.Y + 30);
            NextButton.Click += new EventHandler(NextButton_Click);
            this.Controls.Add(NextButton);

            ExportButton.Location = new Point(PreviousButton.Location.X, PreviousButton.Location.Y + PreviousButton.Height + padding * 2);
            //ExportButton.Size = new Size(this.Width/3/2-padding*2,this.Height/3/2-padding*2);
            ExportButton.Size = new Size(this.Width / 3 - padding * 7 / 2, 30);

            //Preview Area

            SelectArea = new Rectangle(new Point(padding, padding), new Size(this.Width / 3 * 2 - padding * 2, this.Height - padding * 2 - 20));
            PreviewLabel = new Label();
            PreviewLabel.Text = "Preview";
            PreviewLabel.Font = new Font("Arial", 15);
            PreviewLabel.Location = new Point(this.Width / 3 * 2 + padding, this.Height / 3 + padding);
            PreviewLabel.AutoSize = false;
            PreviewLabel.Size = new Size(this.Width / 3 - padding * 2, this.Height / 3 * 2 - padding * 2);
            this.Controls.Add(PreviewLabel);
            for (int hour = 0; hour < 24; hour++)
            {
                for (int quarter = 0; quarter < 4; quarter++)
                {
                    Label l = new Label();
                    l.AutoSize = false;
                    l.Size = SquareSize;
                    l.BackColor = UnSelectedColor;
                    l.MouseClick += new MouseEventHandler(Label_MouseClick);
                    l.Name = "Label" + hour + "." + quarter;
                    l.Location = new Point(
                                SelectArea.X + 50 + quarter * (SquareSize.Width + 1) + (hour > 11 ? SelectArea.Width / 2 : 0),
                                SelectArea.Y + (hour % 12 * SelectArea.Height / 12));
                    labels[quarter, hour] = l;
                    this.Controls.Add(l);
                }
            }

            //Text Labels

            for (int hour = 0; hour < 24; hour++)
            {
                Label l = new Label();
                l.Text = hour + "";
                l.Font = new Font("Arial", 20);
                l.Name = "Label" + hour;
                l.Location = new Point(
                    SelectArea.X + (hour > 11 ? SelectArea.Width / 2 : 0),
                    SelectArea.Y + (hour % 12 * SelectArea.Height / 12));
                l.AutoSize = true;
                l.MouseClick += new MouseEventHandler(TextLabel_MouseClick);
                this.Controls.Add(l);
            }

            //RadioButtons
            int[] names = new int[5] { 1, 2, 3, 4, 5 };
            string[] texts = new string[5] { "Day", "Week", "Month", "Year", "All" };
            RadioButtons = new RadioButton[names.Length];
            for (int num = 0; num < names.Length; num++)
            {
                RadioButton r = new RadioButton();
                if (num == 2)
                    r.Checked = true;
                if (num == 4)
                    r.Enabled = false;
                r.Name = "RadioButton" + names[num];
                r.Text = texts[num];
                r.Location = new Point(this.Width / 3 * 2 +padding , ExportButton.Location.Y+ExportButton.Size.Height+num*20);
                r.Font = new Font("arial", 12);
                r.AutoSize = true;
                r.BringToFront();
                r.Visible = true;
                r.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                RadioButtons[num] = r;
                this.Controls.Add(r);
            }
            DayCheckBox.Location = new Point(this.Width / 6 * 5, RadioButtons[1].Location.Y);
            WeekCheckBox.Location = new Point(this.Width / 6 * 5, RadioButtons[2].Location.Y);
            MonthCheckBox.Location = new Point(this.Width / 6 * 5, RadioButtons[3].Location.Y);
            YearCheckBox.Location = new Point(this.Width / 6 * 5, RadioButtons[4].Location.Y);

            g = this.CreateGraphics();
            //testpost();
            //MessageBox.Show(this.Size.ToString());

        }
        RadioButton[] RadioButtons;
        Label PreviewLabel;
        Graphics g;
        SolidBrush BlackBrush = new SolidBrush(Color.Black);
        Size SquareSize;//defualt at size(800,900) is (50,50)
        Rectangle SelectArea;
        Label[,] labels = new Label[4, 24];
        int SelectedLabelsCount = 0;
        string cPacket = "";
        List<string> data= new List<string>();
        DataUnit[] SortedData;
        int IndexOfcPacket = -1;
        string path = "";
        Color SelectedColor = Color.Red, UnSelectedColor = Color.Gray;
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width / 3 * 2, 0), new Point(this.Width / 3 * 2, this.Height));
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width / 3 * 2, this.Height / 3), new Point(this.Width, this.Height / 3));
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width / 3 * 2, 70), new Point(this.Width, 70));
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width/3*2,this.Height/3/2), new Point(this.Width,this.Height/3/2));
            g.DrawString("See Into", new Font("Arial", 15), BlackBrush, new Point(this.Width / 6 * 5, ExportButton.Location.Y+ExportButton.Size.Height));
            /*
            for (int hour = 0; hour < 24; hour++)
            {
                g.DrawString("" + hour, new Font("Arial", 20), BlackBrush, new Point(
                    SelectArea.X + (hour > 11 ? SelectArea.Width / 2 : 0),
                    SelectArea.Y + (hour % 12 * SelectArea.Height / 12)));
            }*/
        }
        bool SomethingChanged = false;
        private void TextLabel_MouseClick(object sender, MouseEventArgs e)
        {
            int hour = int.Parse((sender as Label).Name.Substring(5));
            for (int quarter = 0; quarter < 4; quarter++)
            {
                Label l = labels[quarter, hour];
                Label_MouseClick(l, new MouseEventArgs(MouseButtons.Left, 1, l.Location.X, l.Location.Y, 0));
            }
        }
        private void Label_MouseClick(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            if (l.BackColor == UnSelectedColor)
            {
                l.BackColor = SelectedColor;
                SelectedLabelsCount++;
            }
            else
            {
                l.BackColor = UnSelectedColor;
                SelectedLabelsCount--;
            }
            SomethingChanged = true;
            updatePreview();
        }
        private string getPreview()
        {
            string result = "";
            bool foundfirst = false;
            for (int hour = 0; hour < 24; hour++)
            {
                for (int quarter = 0; quarter < 4; quarter++)
                {
                    Label l = labels[quarter, hour];
                    if (isSelected(l))
                    {
                        if (!foundfirst)
                        {
                            foundfirst = true;
                            result = getTime(l);
                        }
                        else
                        {
                            if (isSelected(labels[quarter == 0 ? 3 : quarter - 1, quarter == 0 ? hour - 1 : hour]))
                            {
                                string cTime = getTime(l);
                                result = result.Substring(0, result.LastIndexOf(' ') + 1) + cTime.Substring(getTime(l).LastIndexOf(' ') + 1);
                            }
                            else
                            {
                                result += " , " + getTime(l);
                            }
                        }
                    }
                }
            }
            return result;
        }
        private bool isSelected(Label l)
        {
            return l.BackColor == SelectedColor ? true : false;
        }
        private string getTime(Label l)
        {
            int quarter = getQuarter(l);
            return getHour(l) + ":" + quarter * 15 + " - " + (getHour(l)+(quarter==3?1:0)) + ":" + (quarter == 3 ? 00 : (quarter+1)*15);
        }
        private int getQuarter(Label l)
        {
            return int.Parse(l.Name[l.Name.Length - 1].ToString());
        }
        private int getHour(Label l)
        {
            return int.Parse(l.Name.Substring(5, l.Name.IndexOf('.')-5));
        }
        public void AskToSave()
        {
            if (SomethingChanged)
            {
                DialogResult d = MessageBox.Show("You Changes something and havent saved, do you want to save before you continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (d == DialogResult.Yes)
                    SaveButton.PerformClick();
            }
        }
        private void NextButton_Click(object sender, EventArgs e)
        {
            AskToSave();
            SomethingChanged = false;
            dt.Value = dt.Value.AddDays(1);
            cleanDisplay();
            PreviewLabel.Text = "";
            cPacket = dt.Value.Date.ToString("dd/MM/yyyy") + " ";

            SelectedLabelsCount = 0;
            if (SortedData != null)
                for (int i = 0; i < SortedData.Length; i++)
                    if (SortedData[i].getDate() == dt.Value.Date)
                    {
                        IndexOfcPacket = i;
                        displayData(SortedData[i].getData());
                        foreach (int item in SortedData[i].getData())
                            if (item == 1)
                                SelectedLabelsCount++;
                        updatePreview(SortedData[i].getTotalTimeString());
                        break;
                    }

        }
        private void PreviousButton_Click(object sender, EventArgs e)
        {
            AskToSave();
            SomethingChanged = false;
            dt.Value = dt.Value.AddDays(-1);
            cleanDisplay();
            PreviewLabel.Text = "";
            cPacket = dt.Value.Date.ToString("dd/MM/yyyy") + " ";
            SelectedLabelsCount = 0;
            if (SortedData != null)
                for (int i = 0; i < SortedData.Length; i++)
                    if (SortedData[i].getDate() == dt.Value.Date) 
                    {
                        IndexOfcPacket = i;
                        displayData(SortedData[i].getData());
                        foreach (int item in SortedData[i].getData())
                            if (item == 1)
                                SelectedLabelsCount++;
                        updatePreview(SortedData[i].getTotalTimeString());
                        break;
                    }
        }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AskToSave();
            SomethingChanged = false;
            cleanDisplay();
            PreviewLabel.Text = "";
            cPacket = dt.Value.Date.ToString("dd/MM/yyyy") + " ";
            SelectedLabelsCount = 0;
            if (SortedData != null)
                for (int i = 0; i < SortedData.Length; i++)
                    if (SortedData[i].getDate() == dt.Value.Date)
                    {
                        IndexOfcPacket = i;
                        displayData(SortedData[i].getData());
                        foreach (int item in SortedData[i].getData())
                            if (item == 1)
                                SelectedLabelsCount++;
                        updatePreview(SortedData[i].getTotalTimeString());
                        break;
                    }
        }
        public void displayData(int[] data)
        {
            for (int hour = 0; hour < 24; hour++)
                for (int quarter = 0; quarter < 4; quarter++)
                    if(data[hour*4+quarter]==1)
                        labels[quarter, hour].BackColor = SelectedColor;
        }
        public void cleanDisplay()
        {
            foreach (Label l in labels)
                l.BackColor = UnSelectedColor;
        }
        public void updatePreview(string TotalTime)
        {
            string SelectedTimes = getPreview();
            PreviewLabel.Text = "Selected Time Periods \n" + SelectedTimes + "\n" + TotalTime;
        }
        public void updatePreview()
        {
            string TotalTime = "Total Time " + SelectedLabelsCount / 4 + ":" + (SelectedLabelsCount % 4) * 15;
            string SelectedTimes = getPreview();
            PreviewLabel.Text = "Selected Time Periods \n" + SelectedTimes + "\n" + TotalTime;
        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.FileName = "C:\\Users\\danie\\Desktop\\Work";
            if (o.ShowDialog() == DialogResult.OK)
            {
                path = o.FileName;
                PathLabel.Text = path;
                foreach (string s in File.ReadAllLines(path))
                {
                    if (s.Length > 107)
                        data.Add(s.Substring(0, 107));
                    else
                        data.Add(s);
                }
                int sortedlength = 0;
                bool[] skip = new bool[data.Count];
                for (int i = 0; i < data.Count; i++)
                {
                    if (!new DataUnit(data[i]).isEmpty())
                        sortedlength++;
                    else
                        skip[i] = true;
                }
                SortedData = new DataUnit[sortedlength];
                for (int i = 0; i < sortedlength; i++)
                {
                    if (!skip[i])
                    {
                        SortedData[i] = new DataUnit(data[i]);
                        if (SortedData[i].isToday())
                        {
                            displayData(SortedData[i].getData());
                            foreach (int item in SortedData[i].getData())
                                if (item == 1)
                                    SelectedLabelsCount++;
                            updatePreview();
                            //cPacket = SortedData[i].toString();
                        }
                    }
                }
            }
        }
        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            if(s.ShowDialog()==DialogResult.OK)
            {
                path = s.FileName + ".dat";
                PathLabel.Text = path;
                SaveButton.PerformClick();
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (path != "")
            {
                string dayData = "";
                for (int hour = 0; hour < 24; hour++)
                {
                    for (int quarter = 0; quarter < 4; quarter++)
                    {
                        if (isSelected(labels[quarter, hour]))
                            dayData += "1";
                        else
                            dayData += "0";

                    }
                }
                cPacket = dt.Value.Date.ToString("dd/MM/yyyy")+" "+ dayData;
                //data.Add(cPacket);
                if (SortedData != null)
                {
                    bool found = false;
                    for (int i = 0; i < SortedData.Length; i++)
                    {
                        if (SortedData[i].getDate().Date.ToString("dd/MM/yyyy") == cPacket.Substring(0, cPacket.IndexOf(' ')))
                        {
                            SortedData[i] = new DataUnit(cPacket);
                            IndexOfcPacket = i;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        DataUnit[] tmp = new DataUnit[SortedData.Length + 1];
                        int index = 0;
                        foreach (DataUnit d in SortedData)
                        {
                            tmp[index] = d;
                            index++;
                        }
                        tmp[tmp.Length - 1] = new DataUnit(cPacket);
                        SortedData = tmp;
                        IndexOfcPacket = SortedData.Length - 1;
                    }
                    data.Clear();
                    foreach (DataUnit d in SortedData)
                        data.Add(d.toString());
                    File.WriteAllLines(path, data.ToArray());
                    SomethingChanged = false;
                }
                else
                {
                    SortedData = new DataUnit[1];
                    SortedData[0] = new DataUnit(cPacket);
                }
            }
            else
                SaveAsButton.PerformClick();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;
            if (r.Checked == true)
            {
                DayCheckBox.Enabled = false;
                WeekCheckBox.Enabled = false;
                MonthCheckBox.Enabled = false;
                YearCheckBox.Enabled = false;
                int num = int.Parse(r.Name[r.Name.Length - 1].ToString());
                if (num > 1)
                    DayCheckBox.Enabled = true;
                if (num > 2)
                    WeekCheckBox.Enabled = true;
                if (num > 3)
                    MonthCheckBox.Enabled = true;
                if (num > 4)
                    YearCheckBox.Enabled = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SomethingChanged)
            {
                DialogResult d = MessageBox.Show("Are you sure You want to quit without saving?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(d==DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (SortedData == null)
            {
                MessageBox.Show("Save file is empty, please fill some data");
                return;
            }
            int SelectedIndex = 0;
            for (int i = 0; i < RadioButtons.Length; i++)
                if (RadioButtons[i].Checked == true)
                {
                    SelectedIndex = i + 1;
                    break;
                }
            //getting the data
            List<string> ExportData = new List<string>();
            if (IndexOfcPacket == -1)
            {
                for (int i = 0; i < SortedData.Length; i++)
                {
                    if (SortedData[i].getDate().ToString("dd/MM/yyyy") == DateTime.Today.Date.ToString("dd/MM/yyyy"))
                        IndexOfcPacket = i;
                }
            }
            try
            {
                double Weekly = 0, Monthly = 0, Yearly = 0;
                byte cWeek;
                int cdays_to_startofweek = (int)SortedData[IndexOfcPacket].getDate().DayOfWeek;
                int weekminRange = Math.Min(IndexOfcPacket, cdays_to_startofweek);
                int weekmaxRange = 7 - cdays_to_startofweek;
                int cdays_to_startofmonth = (int)SortedData[IndexOfcPacket].getDate().Day;
                int monthminRange = Math.Min(IndexOfcPacket, cdays_to_startofmonth);
                int monthmaxRange = DateTime.DaysInMonth(SortedData[IndexOfcPacket].getDate().Year, SortedData[IndexOfcPacket].getDate().Month) - cdays_to_startofmonth;
                int cdays_to_startofyear = (int)SortedData[IndexOfcPacket].getDate().DayOfYear;
                int yearminRange = Math.Min(IndexOfcPacket, cdays_to_startofyear);
                int yearmaxRange = (DateTime.IsLeapYear(SortedData[IndexOfcPacket].getDate().Year) ? 366 : 365) - cdays_to_startofyear;
                switch (SelectedIndex)
                {
                    case 1://Day
                        ExportData.Add(SortedData[IndexOfcPacket].getExportString());
                        break;
                    case 2://Week
                        for (int i = Math.Max(0, IndexOfcPacket - weekminRange); i < Math.Min(SortedData.Length, IndexOfcPacket + weekmaxRange); i++)
                        {
                            Weekly += SortedData[i].getTotalTiemValue();
                            if (DayCheckBox.Checked == true)
                                ExportData.Add("\t" + SortedData[i].getExportString());
                        }
                        ExportData.Insert(0, "Total Hours For this Week " + Weekly);
                        break;
                    case 3://Month
                        for (int i = Math.Max(0, IndexOfcPacket - monthminRange); i < Math.Min(SortedData.Length, IndexOfcPacket + monthmaxRange); i++)
                        {
                            if (SortedData[i].getDate().Month != SortedData[IndexOfcPacket].getDate().Month)
                                continue;
                            Monthly += SortedData[i].getTotalTiemValue();
                            if (DayCheckBox.Checked == true)
                                ExportData.Add("\t\t" + SortedData[i].getExportString());
                        }
                        ExportData.Insert(0, "Total Hours For "+SortedData[IndexOfcPacket].getDate().Month+"/"+ SortedData[IndexOfcPacket].getDate().Year + " is " + Monthly);
                        //DateTime.month
                        break;
                    case 4://Year
                        for (int i = Math.Max(0, IndexOfcPacket - yearminRange); i < Math.Min(SortedData.Length, IndexOfcPacket + yearmaxRange); i++)
                        {
                            Yearly += SortedData[i].getTotalTiemValue();
                            if (DayCheckBox.Checked == true)
                                ExportData.Add("\t\t\t" + SortedData[i].getExportString());
                        }
                        ExportData.Insert(0, "Total Hours For this Year " + Yearly);
                        break;
                    case 5://All
                        break;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }

            //exporting
            SaveFileDialog s = new SaveFileDialog();
            if (s.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(s.FileName + ".txt", ExportData.ToArray());
            }
        }


        //web
        private static readonly HttpClient client = new HttpClient();
        private async void testpost()
        {
            string uri = "127.0.0.1:3000";
            
            var values = new Dictionary<string, string>
            {
                { "thing1", "hello" },
                { "thing2", "world" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(uri, content);
            var responseString = await response.Content.ReadAsStringAsync();
            }
    }
}
