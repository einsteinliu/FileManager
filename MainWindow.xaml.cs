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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TagLib;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace MusicManager
{

    public partial class MyDic : Dictionary<int,string>
    {
        public void showCount()
        {
            MessageBox.Show(this.Keys.Count.ToString());
            return;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const UInt32 WM_KEYDOWN = 0x0100;
        private const UInt32 WM_KEYUP = 0x0101;
        const int VK_LEFT = 0x25;

        TextBlock tb = null;
        Dictionary<CheckBox, TreeViewItem> cbToItem = new System.Collections.Generic.Dictionary<CheckBox, TreeViewItem>();
        MyDic dic = new MyDic();

        public void showCount(MyDic dic_)
        {
            MessageBox.Show(dic_.Keys.Count.ToString());
        }

        //[DllImport("user32.dll")]
        //private static extern IntPtr FindWindow(
        //string lpClassName,
        //string lpWindowName);

        //[DllImport("user32.dll")]
        //private static extern IntPtr FindWindowEx(
        //IntPtr hwndParent,
        //IntPtr hwndChildAfter,
        //string lpszClass,
        //string lpszWindows);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        public MainWindow()
        {
            InitializeComponent();
            tree.Items.Add(new TreeViewItem(){Header="aaa2"});

            //dic[0] = "hahah";
            //dic.showCount();
            //showCount(dic);
           // showCount(dic);
            //dic.showCount();

            

            TreeViewItem item = tree.Items[0] as TreeViewItem;
            StackPanel sp = new StackPanel();
            item.Header = sp;
            CheckBox cb = new CheckBox();
            sp.Children.Add(cb);
            cbToItem[cb] = item;
            sp.Children.Add(new TextBlock() { Text = "hhah" });
            sp.Orientation = Orientation.Horizontal;
            cb.Click += cb_Click;

            item.ItemsSource = new string[] { "Pants", "Shirt", "Hat", "Socks" };

            TagLib.File ape = TagLib.File.Create(@"D:\classical\Backhaus\Backhaus.-.[Beethoven.Piano.Sonatas.Disc8(Decca.mono.recording)].专辑.(APE)\Beethoven-Backhaus 8\Beethoven-Backhaus 8.ape");
            string time = ape.Properties.Duration.ToString();
 
        }

        void cb_Click(object sender, RoutedEventArgs e)
        {
            cbToItem[sender as CheckBox].IsSelected = true;
        }

        void tb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void MainWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }

        void MainWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("unchecked");
        }

        void MainWindow_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("checked");
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;

                MessageBox.Show(path);
            }
        }

        private void MainWindow_Click(object sender, RoutedEventArgs e)
        {    
            tb.Background = Brushes.Red;
        }

        
        public int It
        {
            get;
            set;
        }

        public static XElement GetDirectoryXml(DirectoryInfo dir)
        {
            var info = new XElement("dir",
                           new XAttribute("name", dir.Name));

            foreach (var file in dir.GetFiles())
                info.Add(new XElement("file",
                             new XAttribute("name", file.Name)));

            foreach (var subDir in dir.GetDirectories())
                info.Add(GetDirectoryXml(subDir));

            return info;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //////////////////////保存文件树//////////////////////
            //根目录
            string rootPath = @"C:\";
            var dir = new DirectoryInfo(rootPath);
            //递归调用获得文件树
            var doc = new XDocument(GetDirectoryXml(dir));
            //将文件树保存入haha.txt文件
            System.IO.File.WriteAllText("haha.txt", doc.ToString());
            ///////////////////////读入文件树/////////////////////
            XDocument xdoc = XDocument.Load("haha.txt");
            List<XElement> els = xdoc.Elements().ToList<XElement>();
            //获得根节点
            List<XElement> es = els[0].Elements().ToList<XElement>();
            for(int i=0;i<es.Elements().Count<XElement>();i++)
            {//遍历根节点的所有子节点
                XElement xe = es.Elements().ElementAt<XElement>(i);
                string path = xe.FirstAttribute.Value.ToString();
                string name = xe.Name.ToString();
            }


            //Process[] processes = Process.GetProcessesByName("AIRPLAY");
            //if(processes.Length>0)
            //{
            //    bool re = PostMessage(processes[0].MainWindowHandle, WM_KEYDOWN, VK_LEFT, 0);
            //    //PostMessage(processes[0].MainWindowHandle, WM_KEYUP, VK_LEFT, 0);
            //}
            //System.Threading.Thread.Sleep(500);



            //IntPtr hWnd = FindWindow(null, "AIRPLAY 3");
            //if (!hWnd.Equals(IntPtr.Zero))
            //{
            //    // retrieve Edit window handle of Notepad 
            //    // send WM_SETTEXT message with "Hello World!"
            //    IntPtr leftK = new IntPtr(0x25);
            //    //endMessage(hWnd, WM_KEYDOWN, leftK, IntPtr.Zero);
            //    //PostMessage(hWnd, 0x0101, leftK, IntPtr.Zero);
            //}
        }
    }

}
