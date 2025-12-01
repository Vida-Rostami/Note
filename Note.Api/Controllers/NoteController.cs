using Microsoft.AspNetCore.Mvc;
using Note.Application.Note;
using Note.Model;
using Note.Model.Note;

namespace Note.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : INoteServices
    {

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
        public async Task<BaseResponse> DeleteNote(int NoteId)
        {
            ///return await 
            throw new NotImplementedException();
        }
    }
}
