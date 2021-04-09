using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SpCli
{
    public class Document
    {
        record Correction(int Offset, int Length, bool Temporary, string Value);

        readonly List<Correction> _corrections = new();


        public Document(string content) => (OriginalContent) = (content);


        public string OriginalContent { get; init; }


        public void AddCorrection(int offset, int length, string value) =>
            _corrections.Add(new Correction(offset, length, Temporary: false, value))
        ;


        public string PrettyPrintCorrectedContent(int offset, int length)
        {
            var textToHightlight = OriginalContent.Substring(offset, length);
            textToHightlight = $"[slowblink yellow]{ textToHightlight }[/]";

            _corrections.Add(new Correction(offset, length, Temporary: true, textToHightlight));

            return PrettyPrintCorrectedContent();
        }

        public string PrettyPrintCorrectedContent()
        {
            var sb = new StringBuilder(OriginalContent);

            foreach(var correction in _corrections.OrderByDescending(c => c.Offset))
            {
                sb.Remove(correction.Offset, correction.Length);
                sb.Insert(correction.Offset, $"[green]{ correction.Value }[/]");
            }

            _corrections.RemoveAll(r => r.Temporary);


            return sb.ToString();
        }
    }
}
