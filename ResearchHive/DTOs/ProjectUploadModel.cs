namespace Application.DTOs
{
    public class ProjectDocumentUploadModel
    {
        public string? ProjectTitle { get; set; }
        public string Author { get; set; }
        public int MaxNumberOfChapters { get; set; }
    }

    public class ResearchDocumentModel
    {
        public string Title { get; set; }

    }
    public class UploadResearchResponse
    {
        public string DocumentPath { get; set; } 
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }

    public class UploadProjectDocumentResponse
    {
        public List<DocumentPathAndNumber> DocumentPathsAndNumber { get; set; } = new List<DocumentPathAndNumber>();
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }

    public class DocumentPathAndNumber
    {
        public string Folder { get; set; }
        public string DocumentPath { get; set; }
        public int ChapterNumber { get; set; }
    }
}
