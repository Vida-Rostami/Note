using Note.Infrastructure.Note;
using Note.Domain;
using Note.Domain.Note;
using Note.Domain.Pagination;

namespace Note.Application.Services.Note
{
    public class NoteServices : INoteServices
    {
        private readonly INoteRepository _noteRepository;

        public NoteServices(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        public async Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote(NoteFilterModel model)
        {
            return await _noteRepository.GetNote(model);
        }

        public async Task<BaseResponse<GetNoteModelResponse>> GetNote(int noteId)
        {
            return await _noteRepository.GetNote(noteId);
        }

        public async Task<BaseResponse> InsertNote(AddNoteModel model)
        {
            return await _noteRepository.InsertNote(model);
        }

        public async Task<BaseResponse> UpdateNote(UpdateNoteModel model)
        {
            return await _noteRepository.UpdateNote(model);
        }
        public async Task<BaseResponse> DeleteNote(int NoteId)
        {
            return await _noteRepository.DeleteNote(NoteId);
        }
    }
}
