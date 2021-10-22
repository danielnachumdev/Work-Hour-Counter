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
        private void setup_new_form_size(Form this_form)
        {
            Size ScreenSize = Screen.FromControl(this_form).Bounds.Size;
            int ASPECT_A = 10, ASPECT_B = 5, SCALE = 16; //aspect ration A/B as in 16/9
            double
                NEW_HEIGHT = ScreenSize.Height / SCALE * ASPECT_A,
                NEW_WIDTH = ScreenSize.Width / SCALE * ASPECT_B;
            this_form.Height = (int)NEW_HEIGHT;
            this_form.Width = 700;//(int)NEW_WIDTH;
            this_form.MaximizeBox = false;
            this_form.MinimumSize = this_form.Size;
            this_form.MaximumSize = this_form.Size;
            this_form.Location = new Point((ScreenSize.Width - this_form.Width) / 2, (ScreenSize.Height - this_form.Height) / 2);
        }
        private void initialize_global_variables()
        {
            cPacket = DateTime.Now.Date.ToString("dd/MM/yyyy") + " ";
            SquareSize = new Size((int)((double)this.Width / 800 * 50), (int)((double)this.Width / 900 * 50));
        }
        private void initialize_controls_properties()
        {
            PathLabel.Text = "";
        }
        private void color_panels()
        {
            foreach (Control control in this.Controls)
                if (control is Panel)
                    if (control.BackColor == Color.FromName("control"))
                        control.BackColor = Color.DarkRed;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            setup_new_form_size(this);
            initialize_global_variables();
            initialize_controls_properties();
            //local variables
            int padding = 10;
            Size default_button_size = new Size((this.Width / 3 - padding * 7 / 2) / 3, 30);

            #region Panels
            #region Open/Save
            Panel os_panel = new Panel();//Open Save panel
            os_panel.Name = "os_panel";
            os_panel.Location = new Point(this.Width / 3 * 2 + padding, padding);
            os_panel.Size = new Size(os_panel.Width, default_button_size.Height);
            OpenButton.Size = default_button_size;
            SaveButton.Size = default_button_size;
            SaveAsButton.Size = default_button_size;
            OpenButton.Location = new Point(0, 0);
            SaveButton.Location = new Point(OpenButton.Location.X + OpenButton.Width, 0);
            SaveAsButton.Location = new Point(SaveButton.Location.X + SaveButton.Width, 0);
            //PathLabel.Location = new Point(0, OpenButton.Height);
            os_panel.Controls.AddRange(new Control[] { OpenButton, SaveButton, SaveAsButton });
            #endregion
            this.Controls.Add(os_panel);
            #region DateTime
            Panel dt_panel = new Panel();//Date Time panel
            dt_panel.Name = "dt_panel";
            dt_panel.Location = new Point(this.Width / 3 * 2 + padding, os_panel.Location.Y + os_panel.Height + padding);

            dt = new DateTimePicker();
            dt.Size = new Size(this.Width / 3 - padding * 2 - 15, 30);
            dt.ValueChanged += new EventHandler(dateTimePicker_ValueChanged);
                    
            Button PreviousButton = new Button();
            PreviousButton.Text = "Previous Day";
            PreviousButton.Size = new Size(default_button_size.Width * 3 / 2, default_button_size.Height);
            PreviousButton.Click += new EventHandler(PreviousButton_Click);
            PreviousButton.BackColor = Color.FromName("control");
            
            Button NextButton = new Button();
            NextButton.Text = "Next Day";
            NextButton.Size = new Size(default_button_size.Width * 3 / 2, default_button_size.Height);
            NextButton.Click += new EventHandler(NextButton_Click);
            NextButton.BackColor = Color.FromName("control");

            dt.Location = new Point(0, 0);
            PreviousButton.Location = new Point(0, dt.Height + padding);
            NextButton.Location = new Point(PreviousButton.Location.X + PreviousButton.Width, PreviousButton.Location.Y);

            dt_panel.Size = new Size(dt_panel.Width, dt.Height + padding + default_button_size.Height);
            dt_panel.Controls.AddRange(new Control[] { dt, PreviousButton, NextButton });
            #endregion
            this.Controls.Add(dt_panel);
            #region Select Area
            Panel sa_panel = new Panel();
            sa_panel.Name = "sa_panel";
            sa_panel.Location = new Point(1,1);
            sa_panel.Size = new Size(this.Width / 3 * 2 - padding *2, this.Height - padding * 2 - 20);

            //sa_panel.Location = new Point(0, 0);

            //Preview Area
            SelectArea = new Rectangle(new Point(padding, padding), new Size(this.Width / 3 * 2 - padding * 2, this.Height - padding * 2 - 20));
            
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
                                /*SelectArea.X + */50 + quarter * (SquareSize.Width + 1) + (hour > 11 ? SelectArea.Width / 2 : 0),
                                /*SelectArea.Y + */(hour % 12 * SelectArea.Height / 12));
                    labels[quarter, hour] = l;
            sa_panel.Controls.Add(l);
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
                sa_panel.Controls.Add(l);
            }
            #endregion
            this.Controls.Add(sa_panel);
            #region Export
            Panel ex_panel = new Panel();
            ex_panel.Name = "ex_panel";
            ex_panel.Location = new Point(dt_panel.Location.X , dt_panel.Location.Y + dt_panel.Height + padding);

            ExportButton.Location = new Point(0, 0);
            ExportButton.Size = new Size(this.Width / 3 - padding * 7 / 2, 30);

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
                r.Location = new Point(padding, ExportButton.Height+padding+ num * r.Size.Height);
                r.Font = new Font("arial", 12);
                r.AutoSize = true;
                r.BringToFront();
                r.Visible = true;
                r.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                RadioButtons[num] = r;
                ex_panel.Controls.Add(r);
            }

            DayCheckBox.Location = new Point(dt_panel.Width/2, RadioButtons[1].Location.Y);
            WeekCheckBox.Location = new Point(dt_panel.Width / 2, RadioButtons[2].Location.Y);
            MonthCheckBox.Location = new Point(dt_panel.Width / 2, RadioButtons[3].Location.Y);
            YearCheckBox.Location = new Point(dt_panel.Width / 2, RadioButtons[4].Location.Y);
            
            ex_panel.Size=new Size(dt_panel.Width, RadioButtons[0].Size.Height * (names.Length+2)+padding);
            ex_panel.Controls.AddRange(new Control[] { ExportButton,DayCheckBox,WeekCheckBox,MonthCheckBox,YearCheckBox });
            #endregion
            this.Controls.Add(ex_panel);
            #endregion
            PreviewLabel = new Label();
            PreviewLabel.Text = "Preview";
            PreviewLabel.Font = new Font("Arial", this.Size.Width/100*2);
            PreviewLabel.Location = new Point(ex_panel.Location.X,ex_panel.Location.Y+ex_panel.Height+padding);
            PreviewLabel.AutoSize = false;
            PreviewLabel.Size = new Size(this.Width / 3 - padding * 2, this.Height / 3 * 2 - padding * 2);
            this.Controls.Add(PreviewLabel);
            //assigns the graphics object
            g = this.CreateGraphics();
            //color all of the difrrent panels so it will be easier to see each of them
            //color_panels();

            //testpost();
            //MessageBox.Show(this.Size.ToString());
            CreaditLabel.Location = new Point(this.Size.Width - CreaditLabel.Size.Width - 30, this.Size.Height - CreaditLabel.Size.Height - 50);

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
            
            /*g.DrawLine(new Pen(BlackBrush), new Point(this.Width / 3 * 2, 0), new Point(this.Width / 3 * 2, this.Height));
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width / 3 * 2, this.Height / 3), new Point(this.Width, this.Height / 3));
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width / 3 * 2, 70), new Point(this.Width, 70));
            g.DrawLine(new Pen(BlackBrush), new Point(this.Width/3*2,this.Height/3/2), new Point(this.Width,this.Height/3/2));
            g.DrawString("See Into", new Font("Arial", 15), BlackBrush, new Point(this.Width / 6 * 5, ExportButton.Location.Y+ExportButton.Size.Height));
*/
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
            if(IndexOfcPacket == -1)
            {
                int month = dt.Value.Month, year = dt.Value.Year;
                for (int i = SortedData.Length - 1; i >= 0 && SortedData.Length - 1 - i < 31; i--)
                    if (SortedData[i].getDate().Year == year && SortedData[i].getDate().Month == month)
                    {
                        IndexOfcPacket = i;
                        break;
                    }
            }
            if(IndexOfcPacket!=-1)
            {
                try
                {
                    double Weekly = 0, Monthly = 0, Yearly = 0;
                    byte cWeek;
                    int cdays_to_startofweek = (int)SortedData[IndexOfcPacket].getDate().DayOfWeek;
                    int weekminRange = Math.Min(IndexOfcPacket, cdays_to_startofweek);
                    int weekmaxRange = 7 - cdays_to_startofweek;
                    int days_in_month = DateTime.DaysInMonth(SortedData[IndexOfcPacket].getDate().Year, SortedData[IndexOfcPacket].getDate().Month);
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
                            for (int i = Math.Max(0, IndexOfcPacket - monthminRange); i < Math.Min(SortedData.Length, IndexOfcPacket + monthmaxRange) + (days_in_month == 31 ? 1 : 0); i++)
                            {
                                if (i >= SortedData.Length)
                                    break;
                                if (SortedData[i].getDate().Month != SortedData[IndexOfcPacket].getDate().Month)
                                    continue;
                                Monthly += SortedData[i].getTotalTiemValue();
                                if (DayCheckBox.Checked == true)
                                    ExportData.Add("\t\t" + SortedData[i].getExportString());
                            }
                            ExportData.Insert(0, "Total Hours For " + SortedData[IndexOfcPacket].getDate().Month + "/" + SortedData[IndexOfcPacket].getDate().Year + " is " + Monthly);
                            //DateTime.month
                            break;
                        case 4://Year
                            int length = Math.Min(SortedData.Length, IndexOfcPacket + yearmaxRange);
                            int records_last_month_count = 0;
                            for (int i = Math.Max(0, IndexOfcPacket - yearminRange); i < length; i++)
                            {
                                if (MonthCheckBox.Checked == true)
                                {
                                    int sLength = DateTime.DaysInMonth(SortedData[i].getDate().Year, SortedData[i].getDate().Month)+i;
                                    int cmonth = SortedData[i].getDate().Month;
                                    int istart = i;
                                    while (i < SortedData.Length && SortedData[i].getDate().Month == cmonth && i < sLength)
                                    {
                                        Monthly+= SortedData[i].getTotalTiemValue();
                                        if (DayCheckBox.Checked == true)
                                            ExportData.Add("\t\t\t" + SortedData[i].getExportString());
                                        i++;
                                    }
                                    //i-records_last_month_count== Math.Max(0, IndexOfcPacket - yearminRange)?0:records_last_month_count
                                    ExportData.Insert(records_last_month_count, "\tTotal Hours For " + cmonth + "/" + SortedData[IndexOfcPacket].getDate().Year + " is " + Monthly);
                                    records_last_month_count += i - istart + 1;
                                    Yearly += Monthly;
                                    Monthly = 0;
                                    i--;
                                }
                                else
                                {
                                    Yearly += SortedData[i].getTotalTiemValue();
                                    if (DayCheckBox.Checked == true)
                                        ExportData.Add("\t\t\t" + SortedData[i].getExportString());
                                }
                            }
                            ExportData.Insert(0, "Total Hours For this Year " + Yearly);
                            break;
                        case 5://All
                            break;
                    }
                    //exporting
                    SaveFileDialog s = new SaveFileDialog();
                    if (s.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllLines(s.FileName + ".txt", ExportData.ToArray());
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
            else
            {
                MessageBox.Show("There are no record for this month..");
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
