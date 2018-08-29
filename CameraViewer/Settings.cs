using System;
using System.Windows.Forms;

namespace CameraViewer
{
    public partial class Settings : Form
    {
        public delegate void SettingsUpdateHandler(object sender, SettingsUpdateArgs e);
        public event SettingsUpdateHandler SettingsUpdated;

        public Settings(int AttemptedRows, int AttemptedCols)
        {
            InitializeComponent();
            Rows_Input.Text = AttemptedRows.ToString();
            Columns_Input.Text = AttemptedCols.ToString();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            SaveAndDispose();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SaveAndDispose();
            }
        }

        private void SaveAndDispose()
        {
            try
            {
                int newRows = (String.IsNullOrEmpty(Rows_Input.Text)) ? 1 : Convert.ToInt32(Rows_Input.Text);
                int newCols = (String.IsNullOrEmpty(Columns_Input.Text)) ? 1 : Convert.ToInt32(Columns_Input.Text);

                SettingsUpdateArgs args = new SettingsUpdateArgs(newRows, newCols);
                SettingsUpdated(this, args);
                this.Dispose();
            }
            catch (Exception AddNewException)
            {
                MessageBox.Show(AddNewException.Message);
            }
        }
    }

    public class SettingsUpdateArgs : System.EventArgs
    {
        private int newRows;
        private int newCols;

        public SettingsUpdateArgs(int Rows, int Cols)
        {
            this.newRows = Rows;
            this.newCols = Cols;
        }

        public int Rows
        {
            get
            {
                return newRows;
            }
        }
        public int Cols
        {
            get
            {
                return newCols;
            }
        }
    }

}
