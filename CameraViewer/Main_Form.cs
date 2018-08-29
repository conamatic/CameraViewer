using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace CameraViewer
{
    public partial class Main_Form : Form
    {
        List<UrlPanel> Panel_List = new List<UrlPanel>();
        List<VlcControl> VlcControlList = new List<VlcControl>();

        bool formOpen = false;
        int counter = 1;
        Form form3;

        int AttemptedRows = 0;
        int AttemptedCols = 0;
        DirectoryInfo vlcLibDirectory;

        public Main_Form()
        {
            InitializeComponent();

            string currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                ToolStripItem ScreenItem = runToolStripMenuItem.DropDownItems.Add(Screen.AllScreens[i].DeviceName);
                ScreenItem.Tag = i;
                ScreenItem.Click += new EventHandler(ScreenMenuClick);
            }
        }

        #region Event Handlers

        void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;

            switch (item.Text)
            {
                case "Exit":
                    foreach(VlcControl control in VlcControlList)
                    {
                        control.Stop();
                    }
                    form3.Close();
                    break;

            }

        }

        void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UrlPanel urlPanel = new UrlPanel();
            urlPanel.panel = New_Panel(null, false, null);
            Panel_List.Add(urlPanel);
        }

        void addMultipleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Form Add_Form = new Add_Form();
            Add_Form.SettingsUpdated += new Add_Form.AddMultipleUpdateHandler(addMultipleToolStripMenuItem_ButtonClicked);
            Add_Form.Show();
        }

        void addMultipleToolStripMenuItem_ButtonClicked(object sender, AddMultipleUpdateArgs Panels)
        {
            for (int i = 0; i < Panels.Quantity; i++)
            {
                UrlPanel urlPanel = new UrlPanel
                {
                    Url = Panels.Template,
                    InputLock = false,
                    Label = Panels.Label,
                    panel = New_Panel(Panels.Template, false, Panels.Label)
                };
                Panel_List.Add(urlPanel);
            }
        }

        //

        void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Import_File_Dialog.ShowDialog() == DialogResult.OK)
            {
                if (Panel_List.Count > 0)
                {
                    DialogResult Overwrite = MessageBox.Show("Remove existing panels first?", "Remove existing panels?", MessageBoxButtons.YesNoCancel);

                    if (Overwrite == DialogResult.Yes)
                    {
                        this.Controls.OfType<Panel>().ToList().ForEach(panel => this.Controls.Remove(panel));
                        counter = 1;
                        Panel_List.Clear();
                    }
                    else if (Overwrite == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                try
                {
                    List<UrlPanel> temp_PanelList = JsonConvert.DeserializeObject<List<UrlPanel>>(File.ReadAllText(Import_File_Dialog.FileName));

                    foreach (UrlPanel urlPanel in temp_PanelList)
                    {
                        urlPanel.panel = New_Panel(urlPanel.Url, urlPanel.InputLock, urlPanel.Label);
                        Panel_List.Add(urlPanel);
                    }

                }
                catch (Exception FileImportEx)
                {
                    MessageBox.Show(FileImportEx.ToString());
                }
            }
        }

        void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Panel_List.Count != 0)
            {
                DialogResult ExportResult = MessageBox.Show("Lock this configuration?", "Lock?", MessageBoxButtons.YesNoCancel);
                if (ExportResult != DialogResult.Cancel)
                {
                    if (Export_Config_Dialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(Export_Config_Dialog.OpenFile()))
                            {
                                if (ExportResult == DialogResult.Yes)
                                {
                                    foreach (UrlPanel urlPanel in Panel_List)
                                    {
                                        urlPanel.InputLock = true;
                                    }
                                }

                                foreach (UrlPanel urlPanel in Panel_List)
                                {
                                    urlPanel.Url = urlPanel.panel.Controls.OfType<TextBox>().ToList()[0].Text;
                                    urlPanel.Label = urlPanel.panel.Controls.OfType<TextBox>().ToList()[1].Text;
                                }

                                writer.WriteLine(JsonConvert.SerializeObject(Panel_List, Formatting.Indented));

                            }
                        }
                        catch (Exception FileExportEx)
                        {
                            MessageBox.Show(FileExportEx.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("What's the point of exporting nothing?");
            }
        }

        void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (UrlPanel urlPanel in Panel_List)
            {
                this.Controls.Remove(urlPanel.panel);
            }
            counter = 0;
            Panel_List.Clear();
        }

        void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings Settings_Form = new Settings(AttemptedRows, AttemptedCols);
            Settings_Form.SettingsUpdated += new Settings.SettingsUpdateHandler(NewSettings);
            Settings_Form.Show();
        }

        //

        void ScreenMenuClick(object sender, EventArgs e)
        {
            if (!formOpen)
            {
                ToolStripItem screen_item = (ToolStripItem)sender;
                OpenNewScreen(Convert.ToInt32(screen_item.Tag));

                MenuStrip strip = screen_item.GetCurrentParent() as MenuStrip;

            }
            else
            {
                DialogResult result = MessageBox.Show("Only one window supported - do you want to close it?", "Window already open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    form3.Close();

                    ToolStripItem screen_item = (ToolStripItem)sender;
                    OpenNewScreen(Convert.ToInt32(screen_item.Tag));

                    MenuStrip strip = screen_item.GetCurrentParent() as MenuStrip;
                }

            }
        }

        void Label_Clicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            VlcControl control = (VlcControl)label.Tag;

            string now = DateTime.Now.ToString("yyyy-MM-dd hhmmss");
            control.TakeSnapshot(now + ".jpg");
        }

        void Form_Closing(object sender, FormClosedEventArgs e)
        {
            try
            {
                formOpen = false;
            }
            catch (Exception CloseException)
            {
                MessageBox.Show(CloseException.Message);
            }
        }

        void Main_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        #endregion

        Panel New_Panel(string template, bool locked, string label)
        {
            Panel panel = new Panel()
            {
                Width = 440,
                Height = 40,

                AutoSize = true,

                Name = "Panel_" + counter.ToString(),
                Tag = counter,

                Top = (45 * counter) - 15,
                Left = 20
            };
            Label Counter_Label = new Label();
            Counter_Label.Left = 0;
            Counter_Label.Top = 8;
            Counter_Label.Width = 30;
            Counter_Label.Text = counter.ToString();

            panel.Controls.Add(Counter_Label);


            TextBox url_input = new TextBox();
            url_input.Left = 40;
            url_input.Top = 0;
            url_input.Width = 350;
            url_input.Font = new Font(url_input.Font.FontFamily, 15);

            url_input.Text = template;
            url_input.Name = "Text_Input";

            if (locked)
            {
                url_input.ReadOnly = true;
            }

            panel.Controls.Add(url_input);

            TextBox label_input = new TextBox();
            label_input.Left = 400;
            label_input.Top = 0;
            label_input.Width = 200;
            label_input.Font = new Font(label_input.Font.FontFamily, 15);

            label_input.Text = label;
            label_input.Name = "Label_Input";

            panel.Controls.Add(label_input);

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem menuRemove = new ToolStripMenuItem("Remove");
            menuRemove.Click += new EventHandler(RemovePanel);
            menu.Items.Add(menuRemove);

            panel.ContextMenuStrip = menu;

            counter++;
            this.Controls.Add(panel);
            return panel;
        }

        void OpenNewScreen(int screen)
        {
            foreach (UrlPanel urlPanel in Panel_List)
            {
                urlPanel.Url = urlPanel.panel.Controls.OfType<TextBox>().ToList()[0].Text;
                urlPanel.Label = urlPanel.panel.Controls.OfType<TextBox>().ToList()[1].Text;
            }

            form3 = new Form();
            form3.Controls.Clear();
            Screen screenToUse = Screen.AllScreens[screen];

            form3.FormBorderStyle = FormBorderStyle.None;
            //form3.Icon = Properties.Resources.DispatchViewer;
            form3.WindowState = FormWindowState.Maximized;
            form3.BackColor = Color.Black;
            form3.FormClosed += new FormClosedEventHandler(Form_Closing);

            form3.StartPosition = FormStartPosition.Manual;
            form3.Location = screenToUse.Bounds.Location;

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Exit");
            menu.ItemClicked += Menu_ItemClicked;
            form3.ContextMenuStrip = menu;

            int count = 0;
            int ScreenH = screenToUse.Bounds.Height; int ScreenW = screenToUse.Bounds.Width;
            int total = Panel_List.Count;

            int rows = 0;
            int cols = 0;

            if (AttemptedCols != 0 & AttemptedRows != 0)
            {
                cols = AttemptedCols;
                rows = AttemptedRows;
            }
            else
            {
                if (Panel_List.Count == 1)
                {
                    rows = 1;
                    cols = 1;
                }
                else
                {
                    if (ScreenH < ScreenW)
                    {
                        rows = 2;
                        cols = Convert.ToInt32(Math.Ceiling((decimal)total / 2));
                    }
                    else
                    {
                        cols = 2;
                        rows = Convert.ToInt32(Math.Ceiling((decimal)total / 2));
                    }
                }
            }

            try
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        UrlPanel urlPanel = Panel_List[count];

                        VlcControl vlcControl = new VlcControl();
                        vlcControl.BeginInit();
                        vlcControl.VlcLibDirectory = vlcLibDirectory;
                        vlcControl.VlcMediaplayerOptions = new[] { "-vvv" };
                        vlcControl.EndInit();

                        vlcControl.Play(urlPanel.Url);
                        vlcControl.Enabled = true;
                        vlcControl.VlcMediaPlayer.Play(urlPanel.Url);

                        vlcControl.Dock = DockStyle.None;
                        vlcControl.Size = new Size(ScreenW / cols, (ScreenH / rows) - 20);
                        vlcControl.Location = new Point((ScreenW / cols) * j, (ScreenH / rows) * i + 20);

                        #region Label
                        Label AddressLabel = new Label();
                        AddressLabel.Text = (urlPanel.Label != String.Empty) ? urlPanel.Label : urlPanel.Url;
                        AddressLabel.Height = 20;
                        AddressLabel.Font = new Font(AddressLabel.Font.FontFamily, 14);
                        AddressLabel.ForeColor = Color.White;
                        AddressLabel.Tag = vlcControl;
                        AddressLabel.Cursor = Cursors.Hand;
                        AddressLabel.AutoSize = true;

                        AddressLabel.Click += new EventHandler(Label_Clicked);
                        #endregion
                        AddressLabel.Location = new Point((ScreenW / cols) * j, (ScreenH / rows) * i);

                        form3.Controls.Add(AddressLabel);
                        form3.Controls.Add(vlcControl);
                        VlcControlList.Add(vlcControl);

                        count++;
                    }
                }
            }
            catch (Exception)
            {
            }

            form3.Show();
            formOpen = true;
        }

        void RemovePanel(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            ContextMenuStrip menu = (ContextMenuStrip)menuItem.GetCurrentParent();
            Panel panel = (Panel)menu.SourceControl;
            this.Controls.Remove(panel);
            Panel_List.RemoveAll(p => p.panel == panel);
            counter--;

            foreach (UrlPanel proceeding in Panel_List.Where(p => Convert.ToInt32(p.panel.Tag) > Convert.ToInt32(panel.Tag)))
            {
                List<Label> label = proceeding.panel.Controls.OfType<Label>().ToList();
                label[0].Text = (Convert.ToInt32(label[0].Text) - 1).ToString();
                proceeding.panel.Tag = Convert.ToInt32(proceeding.panel.Tag) - 1;
                proceeding.panel.Top = proceeding.panel.Top - 45;
            }
        }

        void NewSettings(object sender, SettingsUpdateArgs e)
        {
            AttemptedRows = e.Rows;
            AttemptedCols = e.Cols;
        }
    }
    class UrlPanel
    {
        public string Url { get; set; }
        public string Label { get; set; }
        public bool InputLock { get; set; }

        [JsonIgnore]
        public Panel panel { get; set; }
    }
}
