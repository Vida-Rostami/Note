using Note.Model.Tag;
using Note.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Note.Model.Note;

namespace Note.Infrastructure.Note
{
    public interface INoteRepository
    {
        Task<BaseResponse<GetNoteModel>> GetNote();
        Task<BaseResponse<GetNoteModel>> GetNote(int noteId);
        Task<BaseResponse> InsertNote(AddNoteModel model);
        Task<BaseResponse> UpdateNote(UpdateNoteModel model);
        Task<BaseResponse> DeleteNote(int noteId);
    }
}
