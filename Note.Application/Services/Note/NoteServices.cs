using Note.Infrastructure.Note;
using Note.Domain;
using Note.Domain.Note;
using Note.Domain.Pagination;
using Note.Infrastructure.Caching;
using System.Net;

namespace Note.Application.Services.Note
{
    public class NoteServices : INoteServices
    {
        private readonly INoteRepository _noteRepository;
        private readonly ICacheService _cacheService;
        public NoteServices(INoteRepository noteRepository, ICacheService cacheService)
        {
            _noteRepository = noteRepository;
            _cacheService = cacheService;
        }
        public async Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote(NoteFilterModel model)
        {
            var cacheKey = $"note_" +
               $"{model.Title ?? "all"}_" +
               $"{model.Text ?? "all"}_" +
               $"{model.CategoryId?.ToString() ?? "all"}_" +
               $"{model.TagId?.ToString() ?? "all"}_" +
               $"{model.PersianFromDate ?? "all"}_" +
               $"{model.PersianToDate ?? "all"}_" +
               $"{model.PageNumber}_" +
               $"{model.PageSize}";
            var cachedData = await _cacheService.Get<PaginationBaseResposne<List<GetNoteModelResponse>>>(cacheKey);

            if (cachedData != null)
                return cachedData;

            var result = await _noteRepository.GetNote(model);

            if (result == null)
                return new PaginationBaseResposne<List<GetNoteModelResponse>>
                {
                    Code = (int)HttpStatusCode.NoContent,
                    IsSuccess = false,
                    Message = "اطلاعاتی یافت نشد."
                };

            await _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<BaseResponse<GetNoteModelResponse>> GetNote(int noteId)
        {
            var cacheKey = $"note_detail_{noteId}";

            var cachedNote = await _cacheService.Get<BaseResponse<GetNoteModelResponse>>(cacheKey);

            if (cachedNote != null)
                return cachedNote;

            var result = await _noteRepository.GetNote(noteId);

            if (result == null)
            {
                return new BaseResponse<GetNoteModelResponse>
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NoContent,
                    Message = "اطلاعاتی یافت نشد."
                };
            }

            await _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
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
