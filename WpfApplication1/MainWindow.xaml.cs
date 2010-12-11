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
            webBrowser1.NavigateToString("Bonjour, <br>Ce logiciel recupere les images, cliparts, fond d’ecrans, anime et dessins du site <a href=www.Hebus.com> Hebus.com </a>.<br>Il permet de casser la censure, et les pubs intempestives du site. <br> Vous naurez donc pas besoin de compte et pourrez récupérer les images à contenu adulte.<br> <br> <b> Premiere utilisation </b> <br> La première fois, aucun controle n’est actif, le site vous invite alors à télécharger les 10 premières images du site.  Vous pouvez choisir dans ‘télécharger’ le nombre d’images que vous souhaitez telecharger a partir de la position ‘commencer a’. Faites attentions, le téléchargement demande beaucoup de ressources, et une bonne bande passante. Lorsque vous êtes prêt appuyé sur GO. A la fin du processus, les images s’affichent et les options de navigation se deverrouillent. Vous pourrez alors aisement naviguer.<br> <br> <b> Utilisation </b> <br> Il suffit de choisir la position à laquelle commencer, par defaut, le programme vous suggere de reprendre à la dernière image telecharge. En appuyant sur ‘ajouter’ (qui devient réécrire) vous supprimerez le dossier de travail pour recommencer tout le processus de récupération. Attention, tout le contenu sera supprimé : images, paramètres et autre contenu utilisateur. Les images seront disponibles dans le chemin spécifie.<br><br><br><br><center><form name=\"_xclick\" action=\"https://www.paypal.com/fr/cgi-bin/webscr\" method=\"post\"><input type=\"hidden\" name=\"cmd\" value=\"_xclick\"><input type=\"hidden\" name=\"business\" value=\"osin@live.com\"><input type=\"hidden\" name=\"hebusSniff don\" value=\"don HebusSniff\"><input type=\"hidden\" name=\"currency_code\" value=\"EUR\"><input type=\"hidden\" name=\"amount\" value=\"\"><input type=\"image\" src=\"http://www.paypal.com/fr_FR/i/btn/x-click-butcc-donate.gif\" border=\"0\" name=\"submit\" alt=\"Effectuez vos paiements via PayPal : une solution rapide, gratuite et sécurisée\"></form></center>");
            try
            {
                hebusMaximage = Hebus.getMaxImages();
            }
            catch (Exception i)
            {
                textBlockState.Text = "Internet Manquant";
            }
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
            currentList = path + comboBox1.Text + ".txt";
            readResult();
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            if (control())
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
                for (value = int.Parse(textBoxDebut.Text); value <= int.Parse(textBoxDebut.Text) + int.Parse(comboBoxLimit.Text); value++)
                {
                    if (value <= hebusMaximage) ;
                    operate();
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
                    progressBar1.Value = (100 / ((int.Parse(textBoxDebut.Text)) + (int.Parse(comboBoxLimit.Text))) * value);
                }
                buttonGo.IsEnabled = true;
                textBlockVarNB.Text = (Convert.ToString(varNb));
                DateTime elapse = DateTime.Now;
                textBlockDuree.Text = elapse.Subtract(time).Hours.ToString() + "H -" + elapse.Subtract(time).Minutes.ToString() + "M -" + elapse.Subtract(time).Seconds.ToString() + 'S';
                textBlockInfos.Content = "Les images sont copiées dans: " + path;
                textBlockState.Text = "Etat: Terminé";
                textBoxDebut.Text = Convert.ToString(value + 1);
                readResult();
                pagination();
                ei.Dispose();
                hebusMaximage = Hebus.getMaxImages();
            }
        }

        public void operate()
        {
            {
                string URL = item.getImageURL(value);
                string name;
                if (cbNom.IsChecked==true)
                    name = item.getName(value);
                else
                    name = Convert.ToString(value) ;
                writeImageURL(URL, name);
                getImage(URL, name);
                

            }
        }
        public void getImage(string url, string name)
        {
            try
            {
                {
                    if (!Directory.Exists(path + item.Categorize(value))) Directory.CreateDirectory(path + item.Categorize(value));
                    ei.DownloadFile(url, path + item.Categorize(value) + name + (url.Substring(url.LastIndexOf('.'))));
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
            if (offset < 0) offset = 0;
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
                    offset = 0;
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

        public void writeImageURL(string URL, string name)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                StreamWriter stream = new StreamWriter(currentList, true);
                if (URL != "")
                    stream.WriteLine(value + " : " + item.Categorize(value) + name + URL.Substring(URL.LastIndexOf('.')));
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
                    width = webBrowser1.RenderSize.Width / (Math.Sqrt(limit));
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
        
        public bool control()
        {
            try
            {
                if (hebusMaximage == 0)
                {
                    hebusMaximage = Hebus.getMaxImages();
                    if (hebusMaximage == 0)
                    {
                        textBlockState.Text = "Erreur connexion";
                        return false;
                    }
                }
                if (int.Parse(textBoxDebut.Text) >= 0)
                    varNb = 0;
                else
                {
                    textBlockState.Text = "Valeurs incorrectes";
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                {
                    textBoxDebut.Text = "?";
                    textBlockState.Text = "Valeurs incorrectes";
                }
                return false;
            }
        }

        public void pagination()
        {
            currentList = path + comboBox1.Text + ".txt";
            if (File.Exists(currentList))
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
            else webBrowser1.NavigateToString("<center>Vous n'avez pas encore recupere d'images</center>");
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