namespace winexpext.winapp
{
	public partial class frmMain : Form
	{
		enum TimestampingAction : int
		{
			Copy,
			Rename
		}

		readonly int NoDelay = -1;
		readonly int UserMustClose = 0;
		readonly bool LogToFile = true;
		readonly bool LogToConsole = false;

		string Filename = string.Empty;

		public frmMain(string[] args)
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{

		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{

		}

		private void btnClose_Click(object sender, EventArgs e)
		{

		}
	}
}
