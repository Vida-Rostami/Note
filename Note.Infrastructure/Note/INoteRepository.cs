using Note.Domain;
using Note.Domain.Note;
using Note.Domain.Pagination;

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
