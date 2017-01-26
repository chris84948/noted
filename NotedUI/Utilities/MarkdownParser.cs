using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.Utilities
{
    class MarkdownParser
    {
        private static MarkdownPipeline _pipeline;
        public static MarkdownPipeline Pipeline
        {
            get
            {
                if (_pipeline == null)
                    _pipeline = MarkdownParser.Create();

                return _pipeline;
            }
        }

        public static MarkdownPipeline Create()
        {
            var builder = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UseEmphasisExtras()
                .UseAutoLinks()
                .UsePipeTables()
                .UseGridTables()
                .UseFooters()
                .UseFootnotes()
                .UseCitations()
                .UseFigures();

            return builder.Build();
        }

        public static string Parse(string markdown)
        {
            if (string.IsNullOrEmpty(markdown))
                return string.Empty;

            return Markdown.ToHtml(markdown, Pipeline);
        }
    }
}
