using Note.Domain.Models.Note;
using Note.Domain.Response;

namespace Note.Infrastructure.Note
{
    public interface INoteRepository
    {
        Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote(NoteFilterModel model);
        Task<BaseResponse<GetNoteModelResponse>> GetNote(int noteId);
        Task<BaseResponse> InsertNote(AddNoteModel model);
        Task<BaseResponse> UpdateNote(UpdateNoteModel model);
        Task<BaseResponse> DeleteNote(int noteId);
    }
}
