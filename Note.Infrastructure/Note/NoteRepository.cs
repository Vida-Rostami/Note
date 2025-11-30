using Note.Model;
using Note.Model.Note;

namespace Note.Infrastructure.Note
{
    public class NoteRepository : INoteRepository
    {
        public NoteRepository()
        {
            
        }

        public Task<BaseResponse<GetNoteModel>> GetNote()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetNoteModel>> GetNote(int catgoryId)
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
        public Task<BaseResponse> DeleteNote(int NoteId)
        {
            throw new NotImplementedException();
        }
    }
}
