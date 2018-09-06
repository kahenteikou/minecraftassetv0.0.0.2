using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using Forms = System.Windows.Forms;
using MCAssetsDOWNA;
using System.Windows.Interop;

namespace GUI版


{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpData;
    }
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>



    public partial class MainWindow : Window
    {
        
        private System.Threading.Thread myThread;
        string ver;
        string outPatH;
        IntPtr hwnd;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_click(object sender, RoutedEventArgs e)
        {
            var dlg = new Forms.FolderBrowserDialog();
            dlg.Description = "フォルダーを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスをメッセージボックスに表示
                PathTextbox.Text = dlg.SelectedPath;
            }

        }

        private void StartButton_click(object sender, RoutedEventArgs e)
        {
            if (PathTextbox.Text == "")
            {
                MessageBox.Show("パスを入れるんだ");
                return;

            }

            string outpath3 = PathTextbox.Text.TrimEnd('\\', '"');
            Core ASCore = new Core();
            MCVERJSONLib.Core VCore = new MCVERJSONLib.Core();
            string assetsver = VCore.JSONAssetVer(ListBox1.SelectedValue.ToString());
            hwnd= new WindowInteropHelper(this).Handle;
            ver = assetsver;
            outPatH = outpath3;
            ProgressBar1.IsIndeterminate = true;
            myThread.Start();

            

        }

        private void VerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            MCVERJSONLib.Core VCore = new MCVERJSONLib.Core();
            string assetsver = VCore.JSONAssetVer(ListBox1.SelectedValue.ToString());
            MessageBox.Show(assetsver);



        }

        private void Windows1_init(object sender, EventArgs e)
        {
            string patha = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // Appdataを取得
            string patha2 = patha + "\\.minecraft\\versions"; //Versionsフォルダを取得
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(patha2);
            System.IO.DirectoryInfo[] subFolders =
                di.GetDirectories("*", System.IO.SearchOption.AllDirectories);
            foreach (System.IO.DirectoryInfo subFolder in subFolders)
            {
                string jsonfilenamed = System.IO.Path.GetFileName(subFolder.FullName);
                if (System.IO.File.Exists(subFolder.FullName + "\\" + jsonfilenamed + ".jar"))
                {
                    ListBox1.Items.Add(jsonfilenamed);

                }
            }
            ProgressBar1.IsIndeterminate = false;
            myThread = new System.Threading.Thread(start);

        }
        private void start() {
            Core ASCore = new Core();
            ASCore.Assets2(ver, outPatH, hwnd);
            ASCore.SendString(hwnd, "終了");
            ASCore.SendString(hwnd, "Exit");
            do
            {

            } while (true) ;

            

        }
        private  IntPtr WndProc(IntPtr hwnd, int msg,
    IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x004A)
            {

                string sParam = ReceiveString(lParam);
                if (sParam == "Exit")
                {
                    MessageBox.Show("終了!");
                    ProgressBar1.IsIndeterminate = false;
                    myThread.Abort();

                }
                else { 
                    syoriLabel.Content = sParam;
                syoriLabel.UpdateLayout();
            }
            }

            return IntPtr.Zero;
        }

        //SendString()で送信された文字列を取り出す
        private string ReceiveString(IntPtr m)
        {
            string str = null;
            try
            {
                COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(m, typeof(COPYDATASTRUCT));
                str = cds.lpData;
                str = str.Substring(0, cds.cbData / 2);
            }
            catch { str = null; }
            return str;
        }
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        private void Window1_loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }
    }
}
