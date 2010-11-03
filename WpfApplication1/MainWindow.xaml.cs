using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;



namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        public int value=0;
        public int varNb=0;
        public int initCount=0;

        public int offset=0;
        public int limit=9;

        public string path = @"c:\Images\";

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(path + "list.txt")) button1.IsEnabled = true;
                else webBrowser1.NavigateToString("<center>Vous n'avez pas encore recupere de liens</center>");
                StreamReader read = new StreamReader(path + "log");
                textBoxDebut.Text = read.ReadLine();
                textBoxLimit.Text = read.ReadLine();
                if (int.Parse(textBoxLimit.Text) > (getHebusMaxImages()))
                    textBoxLimit.Text = Convert.ToString(getHebusMaxImages());
                webBrowser1.NavigateToString("<html><body background color='black'></body></html>");
                textBlockInfos.Content = "Il y'a " + Convert.ToString(getHebusMaxImages() + " images sur Hebus");
            }
            catch (Exception i)
            {
                textBoxDebut.Text = "0";
                textBoxLimit.Text = "100";
                textBlockInfos.Content = "Il y'a " + Convert.ToString(getHebusMaxImages() + " images sur Hebus");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            readResult();
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            if (control())
            {
                DateTime time = DateTime.Now;
                try
                {
                    if (!File.Exists(path + "List.txt"))
                    {
                        System.IO.Directory.CreateDirectory(path);
                        StreamWriter stream = new StreamWriter(path + "log", true);
                        stream.WriteLine(textBoxDebut.Text + "\n" + textBoxLimit.Text);
                        stream.Close();
                    }
                }
                catch (Exception i)
                {
                    textBlockState.Text = "Erreur accès au fichier";
                    log(i);
                }
                for (value = int.Parse(textBoxDebut.Text); value <= int.Parse(textBoxLimit.Text); value++)
                {
                    String URL = getImageURL(value);
                    writeImageURL(URL);
                    getImage(URL);
                }
                textBlockVarNB.Text = (Convert.ToString(varNb));
                DateTime elapse = DateTime.Now;
                textBlockDuree.Text = elapse.Subtract(time).Hours.ToString() + "H -" + elapse.Subtract(time).Minutes.ToString() + "M -" + elapse.Subtract(time).Seconds.ToString() + 'S';
                try
                {
                    StreamWriter logger = new StreamWriter(path + "log", false);
                    logger.WriteLine(textBoxLimit.Text + "\n" + (int.Parse(textBoxLimit.Text) + 100));
                    logger.Close();
                }
                catch (Exception i)
                {
                    log(i);
                }
                textBlockInfos.Content = "La liste d'image est copiée dans: " + path;
                textBlockState.Text = "Etat: Terminé";
                readResult();
            }
        }

        private String getImageURL(int value)
        {
            try
            {
                String source = getSource(value, "http://www.hebus.com/imagefull-");
                int index = source.IndexOf("<img class=\"tn\" src=\"", 2000);
                string line = source.Substring(index, 255);
                line = line.Substring(line.IndexOf("http"), line.IndexOf("alt")-22);
                if (line.Contains("jpg") || line.Contains("png"))
                {
                    varNb++;
                    return line;
                }
                else return " ";
            }
            catch (Exception e)
            {
                textBlockState.Text = "Etat: Erreur";
                Console.WriteLine("Erreur : "+e);
                return "invalide element, nothing has been taken : ";
            }
        }

        private void getImage(String URL)
        {
        /*
            int BufferSize = 512 * 4096;
            int BufferReadSize = 4096;

            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(URL);
            WebResponse serverResponse = imageRequest.GetResponse();
            
            Stream responseStream = serverResponse.GetResponseStream();
            byte[] buffer = new byte[BufferSize];
              
            //Nombre d'octets lus dans la portion courante du Flux.
            int read;
            //Nombre total des octets lus
            int parsedBytes = 0;
            while (true)
            {
                //Lecture d'un nouveau bloc de taille maximale 1024 octet (BufferReadSize)
                read = responseStream.Read(buffer, parsedBytes, BufferReadSize);
                if (read == 0) break; // TODO: might not be correct. Was : Exit While
                parsedBytes += read;
            }
            //Libération de ressources mémoire.
            responseStream.Close();
            //Transformation des données du buffer vers un Memory Stream
            MemoryStream pictureStream = new MemoryStream(buffer);
            //Transformation en Image
         */
        }

        private String getSource(int limit, String source)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(source + limit + ".html");
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

        private bool control()
        {
            if (checkboxHebus.IsChecked == true || checkboxWallpaper.IsChecked == true)
                try
                {
                    if ((int.Parse(textBoxDebut.Text) >= 0) && (int.Parse(textBoxLimit.Text) > int.Parse(textBoxDebut.Text)))
                            varNb = 0;
                        else
                        {
                            textBlockState.Text = "Valeurs incorrecte";
                            return false;
                        }
                    return true;
                }
                catch (Exception)
                {
                    {
                        textBoxDebut.Text = "?";
                        textBoxLimit.Text = "?";
                    }
                }
            else textBlockState.Text = "Vous n'avez pas choisi de site";
            return false;
        }

        private void writeImageURL(string URL)
        {
            try
            {
                StreamWriter stream = new StreamWriter(path + "List.txt", true);
                if (URL!=" ")
                stream.WriteLine("<img width=\"400\"src=\""+URL+"\" title=\""+value+"\"/>");
                stream.Close();
            }
            catch (Exception e)
            {
                log(e);
            }
        }
        
        private void Window_gotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBlockState.Text = "Traitement...";
            }
            catch (Exception i)
            {
                log(i);
            }
        }

        private int getHebusMaxImages()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.hebus.com/index-nouveautes.html");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                String Line = reader.ReadToEnd();
                response.Close();
                int index = Line.IndexOf("<div class=\"shadow\"  style=\"display:inline;\">");
                string line = Line.Substring(index, 255);
                line = line.Substring(line.IndexOf("image-") + 6, 6);
                return Int32.Parse(line);
            }catch(Exception e)
            {
                log(e);
                return 0;
            }
        }

        private void log(Exception e)
        {
            StreamWriter stream = new StreamWriter(path + "errors", true);
            stream.WriteLine(">"+DateTime.Now + " : " + e.ToString());
            stream.Close();
        }

        private void readResult()
        {
            StreamReader str = new StreamReader(path + "List.txt");
            string line = "<html><body bgColor='black'>";

            /*
             * Probleme il commence toujours à la ligne 1;
            */
            
            while (initCount<=offset){
                initCount++;
                str.ReadLine();
            }
            for (int count=0; count<limit; count++){
            line = line + str.ReadLine();
            }

            line = line + "<body></html>";
            webBrowser1.NavigateToString(line);
            str.Close();
        }

        private void btPrec_Click(object sender, RoutedEventArgs e)
        {
            offset = (offset==0) ? 0 :offset-limit;
            readResult();
            Console.WriteLine(">>>>>>>>>>>" + offset);
        }

        private void btSuiv_Click(object sender, RoutedEventArgs e)
        {
            offset += limit;
            readResult();
            Console.WriteLine(">>>>>>>>>>>"+offset);
        }

    }
}