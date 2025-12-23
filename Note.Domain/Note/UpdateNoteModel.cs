namespace Note.Domain.Note
{
    public class UpdateNoteModel
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public List<int> Tags { get; set; }

    }
}
