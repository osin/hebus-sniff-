using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        public int value = 0;
        public static int varNb = 0;
        public int offset = 0;
        public int limit = 2;
        public static string path = @"c:\Images\";
        public static string currentList = "";
        DateTime time = new DateTime();
        Hebus item;
        WebClient ei = new WebClient();
        int hebusMaximage = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            hebusMaximage = Hebus.getMaxImages();
            item = new Hebus();
            pagination();
            currentList = path + comboBox1.Text + ".txt";
            textBlockInfos.Content = "Il y'a " + Convert.ToString(hebusMaximage) + " images sur Hebus";
            try
            {
                StreamReader read = new StreamReader(path + "log");
                textBoxDebut.Text = read.ReadLine();
                read.Close();
            }
            catch (Exception)
            {
                textBoxDebut.Text = "0";
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            readResult();
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {

                DateTime time = new DateTime();
                time = DateTime.Now;
                hebusMaximage = Hebus.getMaxImages();
                item.process();
                if (cbRewriteFile.IsChecked == false)
                {
                    MessageBoxResult result = MessageBox.Show("Souhaitez vous supprimer le dossier " + path + " ?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        try
                        {
                            Directory.Delete(path, true);
                        }
                        catch (Exception i)
                        {
                            MessageBox.Show("Erreur : " + i.ToString() + "\n Certains dossiers peuvent ne pas être supprimer", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }
                buttonGo.IsEnabled = false;
                for (value = int.Parse(textBoxDebut.Text); value <= int.Parse(textBoxDebut.Text)+int.Parse(comboBoxLimit.Text); value++)
                {
                    if (value <= hebusMaximage) ;
                    operate();
                    progressBar1.Value = (100 / ((int.Parse(textBoxDebut.Text)) + (int.Parse(comboBoxLimit.Text))) * value);
                }
                buttonGo.IsEnabled = true;
                textBlockVarNB.Text = (Convert.ToString(varNb));
                DateTime elapse = DateTime.Now;
                textBlockDuree.Text = elapse.Subtract(time).Hours.ToString() + "H -" + elapse.Subtract(time).Minutes.ToString() + "M -" + elapse.Subtract(time).Seconds.ToString() + 'S';
                try
                {
                    StreamWriter logger = new StreamWriter(path + "log", false);
                    logger.WriteLine(value);
                    logger.Close();
                }
                catch (Exception i)
                {
                    MainWindow.log(i);
                }
                textBlockInfos.Content = "Les images sont copiées dans: " + path;
                textBlockState.Text = "Etat: Terminé";
                textBoxDebut.Text = Convert.ToString(value+1);
                readResult();
                pagination();
                ei.Dispose();
                hebusMaximage = Hebus.getMaxImages();
        }

        public void operate()
        {
            {
                String URL = item.getImageURL(value);
                writeImageURL(URL);
                getImage(URL);
                //item.getName(value);

            }
        }
        public void getImage(String url)
        {
            try
            {
                {
                    if (!Directory.Exists(path + item.Categorize(value))) Directory.CreateDirectory(path + item.Categorize(value));
                    ei.DownloadFile(url, path + item.Categorize(value) + value + (url.Substring(url.LastIndexOf('.'))));
                }
            }
            catch (Exception e)
            {
                log(e);
            }
        }
        
        private void btPrec_Click(object sender, RoutedEventArgs e)
        {
            offset = (offset == 0) ? 0 : offset - limit;
            readResult();
            pagination();

        }

        private void btSuiv_Click(object sender, RoutedEventArgs e)
        {
            offset += limit;
            readResult();
            pagination();
        }

        private void btVoir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(tbPage.Text) <= (Hebus.countMyLink() / limit) && int.Parse(tbPage.Text) >= 0)
                {
                    offset = limit * int.Parse(tbPage.Text);
                    readResult();
                }
                else tbPage.Text = "?";
            }
            catch (Exception i)
            {
                tbPage.Text = "?";
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(tbImgPage.Text) > 0)
                {
                    limit = int.Parse(tbImgPage.Text);
                    pagination();
                    readResult();
                }
                else tbImgPage.Text = "?";
            }
            catch (Exception i)
            {
                tbImgPage.Text = "?";
            }
        }

        private void cbRewriteFile_Checked(object sender, RoutedEventArgs e)
        {
            cbRewriteFile.Content = "Ajouter";
        }

        private void cbRewriteFile_Unchecked(object sender, RoutedEventArgs e)
        {
            cbRewriteFile.Content = "Ré-ecrire";
        }

        protected String getSource(int limit, String source)
        {
            try
            {
                if (limit != 0)
                    source += limit + ".html";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(source);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                String Line = reader.ReadToEnd();
                response.Close();
                return Line;
            }
            catch (Exception e)
            {
                log(e);
                String Line = "";
                return Line;
            }
        }

        public void writeImageURL(string URL)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                StreamWriter stream = new StreamWriter(currentList, true);
                if (URL != "")
                    stream.WriteLine(value + " : " + item.Categorize(value) + value + URL.Substring(URL.LastIndexOf('.')));
                stream.Flush();
                stream.Close();
                if (cbRewriteFile.IsChecked == false) cbRewriteFile.IsChecked = true;
            }
            catch (Exception e)
            {
                log(e);
            }
        }

        private void Window_gotFocus(object sender, RoutedEventArgs e)
        {
                textBlockState.Text = "Traitement...";
        }

        public static void log(Exception e)
        {
            if (!Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);
            StreamWriter stream = new StreamWriter(path + "errors", true);
            stream.WriteLine(">" + DateTime.Now + " : " + e.ToString());
            stream.Flush();
            stream.Close();
        }

        public void readResult()
        {
            try
            {
                StreamReader str = new StreamReader(currentList);
                string line = "<html><body bgColor='black'><center>";
                int initCount = 0;
                double width=0;
                while (initCount <= offset)
                {
                    if (initCount != 0) str.ReadLine();
                    initCount++;
                }
                for (int count = 0; count < limit; count++)
                {
                    if (limit<=2) width = webBrowser1.Width / limit-1; else width = webBrowser1.Width / (Math.Sqrt(limit));
                    String itemURL = str.ReadLine();
                    line += "<a href=\"" + path+itemURL.Substring(itemURL.IndexOf(':')+2) + "\"><img style=\"border:0px\" "
                    +"width=\"" + width + "\""
                    + "src=\"" + path + itemURL.Substring(itemURL.IndexOf(':') + 2) + "\"/></a>&nbsp;";
                }
                line = line + "</center><body></html>";
                webBrowser1.NavigateToString(line);
                str.Close();
            }
            catch (Exception e)
            {
                log(e);
            }
        }

        public void pagination()
        {
            if (File.Exists(path + comboBox1.Text + ".txt"))
            {
                if (offset> 0) btPrec.IsEnabled = true;
                button1.IsEnabled = true;
                StreamReader readMore = new StreamReader(currentList);
                for (int count = 0; count < limit + 1; count++)
                    if (readMore.ReadLine() != null && count >= limit)
                    {
                        btImgPage.IsEnabled = true;
                        btSuiv.IsEnabled = true;
                        btVoir.IsEnabled = true;
                        tbPage.IsEnabled = true;
                        if (offset == 0) btPrec.IsEnabled = false;
                        if ((offset / limit) + 1==((Hebus.countMyLink() / limit) + 1)) btSuiv.IsEnabled = false;
                    }
                int page = (offset / limit) + 1;
                tbPage.Text = Convert.ToString(page);
                lPage.Content = "/ " + Convert.ToString((Hebus.countMyLink()/limit)+1);
                readMore.Close();
            }
            else webBrowser1.NavigateToString("<center>Vous n'avez pas encore recupere d'imagess</center>");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            item.Close();
            this.Close();
        }

        private void progressBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //not implemented
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentList = path + comboBox1.Text +".txt";
        }
    }
}