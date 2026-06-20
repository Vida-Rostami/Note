using Note.Domain.Models.Note;
using Note.Domain.Response;

namespace Note.Application.Services.Note
{
    public interface INoteServices
    {
        Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote(NoteFilterModel model);
        Task<BaseResponse<GetNoteModelResponse>> GetNote(int id);
        Task<BaseResponse> InsertNote(AddNoteModel model);
        Task<BaseResponse> UpdateNote(UpdateNoteModel model);
        Task<BaseResponse> DeleteNote(int noteId);
    }
}
