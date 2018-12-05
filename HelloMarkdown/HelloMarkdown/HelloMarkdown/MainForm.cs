using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Markdig;
using ColorCode;
using Markdig.SyntaxHighlighting;

namespace HelloMarkdown
{
    public partial class MainForm : Form
    {
        RichTextArea richTextAreaMd;
        RichTextArea richTextAreaHtml;
        WebView previewer;
        Markdig.MarkdownPipeline pipeline;
        public MainForm()
        {
            Title = "Markdown Previewer";
            ClientSize = new Size(1024, 768);
            Create();
        }
        private void Create()
        {
            this.pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSyntaxHighlighting()
                .Build();

            richTextAreaMd = new RichTextArea() { Width=320, Height=600 };
            richTextAreaHtml = new RichTextArea() { Width=320, Height=600 };
            previewer = new WebView() { Width=320, Height=600 };

            richTextAreaMd.Focus();
            richTextAreaMd.TextChanged += (object sender, EventArgs e) =>
            {
                richTextAreaHtml.Text = Markdig.Markdown.ToHtml((sender as RichTextArea).Text, pipeline);
                previewer.LoadHtml(richTextAreaHtml.Text);
            };
            //richTextAreaMd.Text = new StreamReader(new FileStream("/tmp/work/Projects/HelloMarkdown/HelloMarkdown/HelloMarkdown/DefaultMarkdown.md", FileMode.Open)).ReadToEnd();
            //richTextAreaMd.Text = new StreamReader(new FileStream("./DefaultMarkdown.md", FileMode.Open)).ReadToEnd();
            richTextAreaMd.Text = new StreamReader(new FileStream(System.IO.Path.Combine(System.AppContext.BaseDirectory, "DefaultMarkdown.md"), FileMode.Open)).ReadToEnd();
            Content = new TableLayout
            {
                Padding = 0,
                Spacing = new Size(0, 0),
                Rows =
                {
                    new TableRow(richTextAreaMd, richTextAreaHtml, previewer),
                }
            };
        }
    }
}
