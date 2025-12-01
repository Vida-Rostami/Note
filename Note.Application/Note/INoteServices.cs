using Note.Model.Note;
using Note.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Application.Note
{
    public interface INoteServices
    {
        Task<BaseResponse<GetNoteModel>> GetNote();
        Task<BaseResponse<GetNoteModel>> GetNote(int catgoryId);
        Task<BaseResponse> InsertNote(AddNoteModel model);
        Task<BaseResponse> UpdateNote(UpdateNoteModel model);
        Task<BaseResponse> DeleteNote(int NoteId);
    }
}
