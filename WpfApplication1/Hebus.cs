using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WpfApplication1
{
    class Hebus : MainWindow
    {
        public void hebus()
        {
            int hebusMaxImages = getMaxImages();
            if (int.Parse(textBoxLimit.Text) > (hebusMaxImages))
                textBoxLimit.Text = Convert.ToString(hebusMaxImages);
            webBrowser1.NavigateToString("<html><body background color='black'></body></html>");
            textBlockInfos.Content = "Il y'a " + Convert.ToString(hebusMaxImages + " images sur Hebus");
        }
        public static int getMaxImages()
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
            }
            catch (Exception e)
            {
                MainWindow.log(e);
                return 0;
            }
        }

        public String getImageURL(int value)
        {
            try
            {
                String source = this.getSource(value, "http://www.hebus.com/imagefull-");
                int index = source.IndexOf("<img class=\"tn\" src=\"", 2000);
                string line = source.Substring(index, 255);
                line = line.Substring(line.IndexOf("http"), line.IndexOf("alt") - 23);
                if (line.Contains("jpg") || line.Contains("png"))
                {
                    MainWindow.varNb++;
                    return line;
                }
                else return "";
            }
            catch (Exception e)
            {
                textBlockState.Text = "Etat: Erreur";
                log(e);
                return "";
            }
        }
        
        public static int countMyLink()
        {
            StreamReader str = new StreamReader(MainWindow.path + "List.txt");
            int count = 0;
            while (!str.EndOfStream)
            {
                str.ReadLine();
                count++;
            }
            str.Close();
            return count;
        }

        public bool control()
        {
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
                return false;
            }
        }

        public void process()
        {
            if (this.control())
            {
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
                    MainWindow.log(i);
                }

            }
        }
    }
}