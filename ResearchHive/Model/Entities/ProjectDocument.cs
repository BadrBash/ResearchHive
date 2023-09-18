using Model.Common.Contracts;
using Model.Constants;
using Model.ModelException;

namespace Model.Entities
{
    public class ProjectDocument : BaseEntity
    {
        public ProjectDocument(int chapterNumber, Guid projectId, string documentPath, string folder)
        {
            ChapterNumber = chapterNumber;
            ProjectId = projectId;
            DocumentPath = documentPath;
            Folder = folder;
            if(chapterNumber == 0 ) 
            {
                throw new ArgumentException(ExceptionMessage.CannotBeZero);
            }
            if (string.IsNullOrEmpty(DocumentPath) || string.IsNullOrEmpty(Folder))
            {
                throw new ValueCannotBeNullException(ExceptionMessage.CannotBeNull);
            }
        }

        public int ChapterNumber { get; private set; }
        public string Folder { get; private set; }
        public string DocumentPath { get; private set; }

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }

        public ProjectDocument Update(string documentPath)
        {
            DocumentPath = documentPath;
            return this;
        }


    }
}
