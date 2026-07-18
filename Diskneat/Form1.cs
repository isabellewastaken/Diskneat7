using System.IO;
using System.Runtime.InteropServices;


namespace Diskneat
{
    public partial class Form1 : Form
    {
        int filesremoved;

        [DllImport("Shell32.dll")]
        private static extern uint SHEmptyRecycleBin(
    IntPtr hwnd,
    string? pszRootPath,
    RecycleFlags dwFlags);

        [Flags]
        private enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteTemporaryFiles();
        }

        private void DeleteTemporaryFiles()
        {
            
            if (MessageBox.Show("Are you sure you want to delete all temporary files?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

                filesremoved = 0;
                DirectoryInfo directory = new DirectoryInfo(Path.GetTempPath());
                foreach (FileInfo file in directory.GetFiles())
                {
                    try
                    {
                        file.Delete();
                        filesremoved++;
                    }
                    catch (IOException)
                    {
                        // Handle the exception if needed
                    }
                }

                foreach (DirectoryInfo directory2 in directory.GetDirectories())
                {
                    try
                    {
                        directory2.Delete(true);
                        filesremoved++;
                    }

                    catch
                    {
                        // Handle the exception if needed
                    }


                }

                MessageBox.Show($"Deleted {filesremoved} temporary files and directories.", "Cleanup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EmptyRecycleBin();
        }

        private void EmptyRecycleBin()
        {
           if (MessageBox.Show("Are you sure you want to empty the Recycle Bin?", "Confirm Empty Recycle Bin", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                filesremoved = 0;
                try
                {
                    SHEmptyRecycleBin(
                        IntPtr.Zero,
                        null,
                        RecycleFlags.SHERB_NOCONFIRMATION |
                        RecycleFlags.SHERB_NOPROGRESSUI |
                        RecycleFlags.SHERB_NOSOUND);

                    MessageBox.Show(
                        $"Recycle Bin has been emptied.\n {filesremoved} items removed.",
                        "Cleanup Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to empty Recycle Bin.\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

    }
  
}
