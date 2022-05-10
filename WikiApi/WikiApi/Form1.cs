using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiApi
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        public struct Page
        {
            Page(string title, string address, List<string> path)
            {
                Title = title;
                Address = address;
                Path = path;
            }

            public string Title { get; set; }
            public string Address { get; set; }
            public List<string> Path { get; set; }
        }
        List<Page> pages = new List<Page>();
        public string getContenidoWeb(string address)
        {


            WebClient client = new WebClient();

            using (Stream stream = client.OpenRead("https://en.wikipedia.org/w/api.php?action=parse&format=xml&page="+ address+ "&prop=text"))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();

            }
            

        }
        int findID = -1;
        public void loadPage(object message) 
        {
            string Address = "";
            int i = (int)message;
            if (i == -1)
            {
                Address = textBox1.Text;
            }
            else
            {
                Address = pages[i].Address;
            }
            try
            {
                string pageText = getContenidoWeb(Address); ;

                while (pageText.Contains("href="))
                {
                    List<string> curentPath = new List<string>();
                    if (i == -1)
                    {
                        curentPath = new List<string>();
                    }
                    else
                    {
                        curentPath = pages[i].Path.Distinct().ToList();
                    }

                    Page page = new Page();
                    pageText = pageText.Substring(pageText.IndexOf("href") + 12);
                    page.Address = pageText.Substring(0, pageText.IndexOf('"'));
                    pageText = pageText.Substring(pageText.IndexOf("title") + 7);
                    page.Title = pageText.Substring(0, pageText.IndexOf('"'));
                    curentPath.Add(page.Address);
                    page.Path = curentPath;
                    if (!page.Title.Contains("."))
                        if (!page.Title.Contains("."))
                            if (!page.Title.Contains("File"))
                                if (!page.Title.Contains(";"))
                                    if (!page.Title.Contains("#"))
                                        if (!page.Title.Contains(":"))
                                            if (!page.Address.Contains("."))
                                                if (!page.Address.Contains("."))
                                                    if (!page.Address.Contains("File"))
                                                        if (!page.Address.Contains(";"))
                                                            if (!page.Address.Contains("#"))
                                                                if (!page.Address.Contains(":"))
                                                                {
                                                                    //Console.WriteLine
                                                                    pages.Add(page);
                                                                    //Console.WriteLine(page.Path.Count());
                                                                    //Console.WriteLine(page.Address);
                                                                    if ((page.Address == textBox2.Text) || (page.Title == textBox2.Text))
                                                                    {

                                                                        findID = pages.Count - 1;
                                                                        Console.WriteLine("Found");


                                                                        find = true;
                                                                        break;
                                                                    }
                                                                }

                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        bool find = false;
        int counter = 1;
        private int workerThreads;
        private int portThreads;

        private void button1_Click(object sender, EventArgs e)
        {
            loadPage(-1);
            timer1.Enabled = true;
            //label1.Text += textBox2.Text;
            /*for (int i = 0; i < pages.Count; i++)
            {
                Console.WriteLine(pages[i].Title + " " + pages[i].Address);
            }*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (find)
            {
                timer1.Enabled = false;
                List<string> pathResult = pages[findID].Path;
                for (int i1 = 0; i1 < pathResult.Count; i1++)
                {
                    label1.Text += pathResult[i1] + "=>";
                }
                label1.Text = textBox1.Text + "=>" + label1.Text;
                label1.Text = label1.Text.Substring(0, label1.Text.Length - 2);
            }
            Console.WriteLine(counter);
            for (int i = 0; i < pages.Count; i++)
            {
                //Console.WriteLine(pages.Count);
                if (pages[i].Path.Count == counter)
                {
                    loadPage(i);
                                        //loadPage(i);
                    if (find)
                        break;
                }
                Thread.Sleep(100);
            }
            counter++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
