using Note.Model;
using Note.Model.Note;

namespace Note.Application.Note
{
    public interface INoteServices
    {
        Task<BaseResponse<List<GetNoteModel>>> GetNote();
        Task<BaseResponse<GetNoteModel>> GetNote(int noteId);
        Task<BaseResponse> InsertNote(AddNoteModel model);
        Task<BaseResponse> UpdateNote(UpdateNoteModel model);
        Task<BaseResponse> DeleteNote(int noteId);
    }
}
