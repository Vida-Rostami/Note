using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Model.Note
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
