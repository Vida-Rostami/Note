using Microsoft.Extensions.Options;
using Note.Model;
using Note.Model.Note;

namespace Note.Infrastructure.Note
{
    public class NoteRepository : INoteRepository
    {
        private readonly IOptions<DatabaseSettings> _options;

        public NoteRepository(IOptions<DatabaseSettings> options)
        {
            _options = options;
        }

        public Task<BaseResponse<GetNoteModel>> GetNote()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetNoteModel>> GetNote(int noteId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> InsertNote(AddNoteModel model)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateNote(UpdateNoteModel model)
        {
            throw new NotImplementedException();
        }
        public Task<BaseResponse> DeleteNote(int noteId)
        {
            throw new NotImplementedException();
        }
    }
}
