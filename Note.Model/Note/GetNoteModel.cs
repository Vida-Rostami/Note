using Note.Model.Common;

namespace Note.Model.Note
{
    public class GetNoteModel
    {
        public int NoteId { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<int>? Tag { get; set; }
        public List<String>? TagName { get; set; }
        public int IsEditable { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string ShamsiCreateDateTime => CreateDateTime.ToShamsi();
        public DateTime? ModifyDateTime { get; set; }
        public string ShamsiModifyDateTime => ModifyDateTime.ToShamsi();
    }
}
