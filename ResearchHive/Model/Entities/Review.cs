using Model .Common.Contracts;
using Model.Constants;
using Model.ModelException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Review : BaseEntity
    {
        public Review(string? comment, bool liked, Guid? projectId, Guid? researchId)
        {
            Comment = comment;
            Liked = liked;
            ProjectId = projectId;
            ResearchId = researchId;
        }
           
        public string? Comment { get; private set; }
        public bool Liked { get; private set; }
        public Guid? ProjectId { get; private set; }
        public Project? Project { get; private set; }
        public Guid? ResearchId { get; private set; }
        public Research? Research { get; private set; }

        public Review Update(string comment, bool liked)
        {
            Comment = comment;
            Liked = liked;
            return this;
        }

    }
}
