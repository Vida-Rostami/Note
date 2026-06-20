using Microsoft.AspNetCore.Mvc;
using Note.Application.Services.Note;
using Note.Domain.Models.Note;
using Note.Domain.Response;

namespace Note.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class NoteController
    {
        private readonly INoteServices _noteServices;

        public NoteController(INoteServices noteServices)
        {
            _noteServices = noteServices;
        }

        [HttpGet]
        public async Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote([FromQuery] NoteFilterModel model)
        {
            return await _noteServices.GetNote(model);
        }

        [HttpGet("noteId")]
        public async Task<BaseResponse<GetNoteModelResponse>> GetNote([FromQuery] int noteId)
        {
            return await _noteServices.GetNote(noteId);
        }

        [HttpPost]
        public async Task<BaseResponse> InsertNote(AddNoteModel model)
        {
            return await _noteServices.InsertNote(model);
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateNote(UpdateNoteModel model)
        {
            return await _noteServices.UpdateNote(model);
        }

        [HttpDelete]
        public async Task<BaseResponse> DeleteNote(int noteId)
        {
            return await _noteServices.DeleteNote(noteId);
        }
    }
}
