using Microsoft.AspNetCore.Mvc;
using Note.Application.Note;
using Note.Model;
using Note.Model.Note;

namespace Note.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController 
    {
        private readonly INoteServices _noteServices;

        public NoteController(INoteServices noteServices)
        {
            _noteServices = noteServices;
        }

        public async Task<BaseResponse<GetNoteModel>> GetNote()
        {
            return await _noteServices.GetNote();
        }

        public async Task<BaseResponse<GetNoteModel>> GetNote(int noteId)
        {
           return await _noteServices.GetNote(noteId);
        }

        public async Task<BaseResponse> InsertNote(AddNoteModel model)
        {
            return await _noteServices.InsertNote(model);
        }

        public async Task<BaseResponse> UpdateNote(UpdateNoteModel model)
        {
           return await _noteServices.UpdateNote(model);
        }
        public async Task<BaseResponse> DeleteNote(int noteId)
        {
            return await _noteServices.DeleteNote(noteId);
        }
    }
}
