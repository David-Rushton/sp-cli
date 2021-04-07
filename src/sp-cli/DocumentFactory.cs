namespace SpCli
{
    public class DocumentFactory
    {
        public Document New(string content) => new Document(content);
    }
}
