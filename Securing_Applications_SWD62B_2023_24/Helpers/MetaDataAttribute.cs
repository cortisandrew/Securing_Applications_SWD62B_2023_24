namespace Securing_Applications_SWD62B_2023_24.Helpers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class MetaDataAttribute : Attribute
    {
        public MetaDataAttribute(string author, string dateModified, string description) {
            this.Author = author;
            this.DateModified = dateModified;
            this.Description = description;
        }
        public string Author { get; set; }

        public string DateModified { get; set; }

        public string Description { get; set; }

        public string SummariseInformation()
        {
            return String.Empty;
        }
    }
}
