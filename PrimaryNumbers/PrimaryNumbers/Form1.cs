using System;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace PrimaryNumbers
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            _taskModel = new TaskModel();
            _taskModel.NewTaskAdded += UpdateListBox;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            _taskModel.AddTask((ulong)valueBox.Value);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ActiveControl = buttonRun;

            var timer = new Timer {Interval = 1000};
            timer.Tick += UpdateListBox;
            timer.Start();
        }

        private void UpdateListBox(object sender, EventArgs e)
        {
            var i = 0;
            foreach (var tsk in _taskModel.Tasks)
            {
                if (listBox1.Items.Count > i)
                {
                    listBox1.Items[i++] = new TaskPresenter(tsk);
                }
                else
                {
                    listBox1.Items.Add(new TaskPresenter(tsk));
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            var selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }

            var task = (TaskPresenter) listBox1.Items[selectedIndex];
            task.Cancel();
        }

        private readonly ITaskModel _taskModel;
    }
}
