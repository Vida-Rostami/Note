using Note.Infrastructure.Note;
using Note.Model;
using Note.Model.Note;

namespace Note.Application.Note
{
    public class NoteServices : INoteServices
    {
        private readonly INoteRepository _noteRepository;

        public NoteServices(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        public async Task<BaseResponse<List<GetNoteModel>>> GetNote()
        {
            return await _noteRepository.GetNote();
        }

        public async Task<BaseResponse<GetNoteModel>> GetNote(int noteId)
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
