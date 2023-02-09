using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Google_chat_reader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedFilePath = (string[])e.Data.GetData(DataFormats.FileDrop);
                var Content = JsonConvert.DeserializeObject<jsonFormat>(File.ReadAllText(droppedFilePath.FirstOrDefault()));
                Discussion.SuspendLayout();
                Discussion.Enabled = false;
                if (Content.messages.Any())
                {
                    Discussion.Clear();
                    string chatFile;
                    var firstname = Content.messages.FirstOrDefault().creator.name;
                    var previousname = String.Empty;
                    var previousfulldate = String.Empty;
                    var previousdate = String.Empty;
                    foreach (var item in Content.messages)
                    {
                        var currentname = item.creator.name;
                        var writename = currentname != previousname;
                        previousname = currentname;

                        var currentfulldate = item.created_date.Substring(
                        0,
                        item.created_date.Length - 7);
                        var writefulldate = currentfulldate != previousfulldate;
                        previousfulldate = currentfulldate;

                        var currentdate = item.created_date.Substring(
                        0,
                        item.created_date.Length - 15);
                        var writedate = currentdate != previousdate;
                        previousdate = currentdate;

                        Discussion.BackColor = Color.White;
                        if(writedate)
                        {
                            Discussion.SelectionColor = Color.Black;
                            Discussion.SelectionAlignment = HorizontalAlignment.Center;
                            Discussion.AppendText(previousdate + Environment.NewLine);                            
                        }
                        //Chat.SelectionBackColor = currentname != firstname ? Color.Pink : Color.LightBlue;
                        //Chat.SelectionBackColor = Color.White;
                        //var test = Chat.Find("");
                        Discussion.SelectionColor = currentname != firstname ? Color.DarkViolet : Color.Blue;
                        Discussion.SelectionAlignment = HorizontalAlignment.Left; 
                        string text = String.Empty;
                        if (writefulldate || writename)
                            text+= Environment.NewLine + item.creator.name + " " + currentfulldate + Environment.NewLine;
                        /*else if (writedate)
                            text += " " + item.created_date + Environment.NewLine;*/
                        Discussion.AppendText(
                            text +
                            item.text + Environment.NewLine);
                    }
                }
                Discussion.Enabled = true;
                Discussion.ResumeLayout();
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }

    public class jsonCreator
    {
        public string name { get; set; }
        public string email { get; set; }
        public string user_type { get; set; }
    }
    public class jsonMessage
    {
        public jsonCreator creator { get; set; }
        public string created_date { get; set; }
        public string text { get; set; }
        public string topic_id { get; set; }

        public override string ToString()
        {
            return text;
        }
    }
    public class jsonFormat
    {
        public List<jsonMessage> messages { get; set; }
    }
}
