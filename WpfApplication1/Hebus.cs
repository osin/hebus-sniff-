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
            comboBoxLimit.Items.Add(Convert.ToString(hebusMaxImages));
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
        
        public String getName(int value)
        {
            try
            {
                String source = this.getSource(value, "http://www.hebus.com/imagefull-");
                int index = source.IndexOf("<title>", 100);
                string line = source.Substring(index, source.IndexOf(" - Hebus.com"));
                line = line.Substring(line.IndexOf('>'), line.IndexOf('-'));
                line = line.Substring(line.LastIndexOf('>')+2);
                line = line.Remove(line.IndexOf('-') - 1) ;
                return line;
            }
            catch (Exception e)
            {
                log(e);
                return "";
            }
        }
        public string Categorize(int value)
        {
            try
            {
                String source = this.getSource(value, "http://www.hebus.com/image-");
                int index = source.IndexOf("Accueil Hebus.com", 700);
                string line = source.Substring(index, 400);
                line = line.Substring(line.IndexOf(" class=\"fil\"") + 50, line.IndexOf("</div>") - 80);
                line = line.Substring(line.IndexOf("href") + 6);
                line = line.Remove(line.IndexOf("class"));
                line.Remove(line.LastIndexOf("html"));
                string[] lines = line.Split('/');
                line = "";
                for (int i = 0; i <= 2; i++)
                {
                    if (lines[i][lines[i].Length - 1] == '.')
                        lines[i] = lines[i].Remove(lines[i].Length - 1);
                    line += lines[i] + "\\";
                }
                return line;
            }
            catch (Exception e)
            {
                return "Unknow\\";
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
            StreamReader str = new StreamReader(MainWindow.currentList);
            int count = 0;
            while (!str.EndOfStream)
            {
                str.ReadLine();
                count++;
            }
            str.Close();
            return count;
        }


        public void process()
        {
            try
                {
                    if (!File.Exists(currentList))
                    {
                        System.IO.Directory.CreateDirectory(path);
                        StreamWriter stream = new StreamWriter(path + "log", true);
                        stream.WriteLine(limit);
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