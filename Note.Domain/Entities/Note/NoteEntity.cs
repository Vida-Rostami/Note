using Note.Domain.Entities.Category;

namespace Note.Domain.Entities.Note
{
    public class NoteEntity
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int CategoryId { get; set; }

        public CategoryEntity Category { get; set; } 

        public bool IsEditable { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

    }
}
