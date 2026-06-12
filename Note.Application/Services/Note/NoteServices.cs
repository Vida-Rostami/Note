using Microsoft.Extensions.Options;
using Note.Application.Commons;
using Note.Domain;
using Note.Domain.Note;
using Note.Domain.Pagination;
using Note.Infrastructure.Caching;
using Note.Infrastructure.Note;
using System.Net;

namespace Note.Application.Services.Note
{
    public class NoteServices : INoteServices
    {
        private readonly INoteRepository _noteRepository;
        private readonly ICacheService _cacheService;
        private PaginationSettings _paginationOptions;
        public NoteServices(INoteRepository noteRepository, ICacheService cacheService, IOptions<PaginationSettings> paginationOptions)
        {
            _noteRepository = noteRepository;
            _cacheService = cacheService;
            _paginationOptions = paginationOptions.Value;
        }
        public async Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote(NoteFilterModel model)
        {
            var pageNumber = model.PageNumber <= 0 ? _paginationOptions.DefaultPageNumber : model.PageNumber;
            var pageSize = model.PageSize <= 0 ? _paginationOptions.DefaultPageSize : Math.Min(model.PageSize, _paginationOptions.MaxPageSize);
            model.PageNumber = pageNumber;
            model.PageSize = pageSize;
            var version = await _cacheService.GetVersion(CacheKeys.NoteVersion);
            var cacheKey = GenerateListCacheKey(version, model);

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
            var version = await _cacheService.GetVersion(CacheKeys.NoteVersion);
            var cacheKey =   GenerateDetailCacheKey(version, noteId);

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

            await _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<BaseResponse> InsertNote(AddNoteModel model)
        {
            var result = await _noteRepository.InsertNote(model);
            if (result.IsSuccess)
            {
                await _cacheService.IncrementVersion(CacheKeys.NoteVersion);
            }
            return result;
        }

        public async Task<BaseResponse> UpdateNote(UpdateNoteModel model)
        {
            var result = await _noteRepository.UpdateNote(model);

            if (result.IsSuccess)
            {
                await _cacheService.IncrementVersion(CacheKeys.NoteVersion);
            }

            return result;
        }
        public async Task<BaseResponse> DeleteNote(int noteId)
        {
            var result = await _noteRepository.DeleteNote(noteId);

            if (result.IsSuccess)
            {
                await _cacheService.IncrementVersion(CacheKeys.NoteVersion);
            }

            return result;
        }

        private string GenerateListCacheKey( int version,  NoteFilterModel model)
        {
            return
                $"note_v{version}_list_" +
                $"{model.Title ?? "all"}_" +
                $"{model.Text ?? "all"}_" +
                $"{model.CategoryId?.ToString() ?? "all"}_" +
                $"{model.TagId?.ToString() ?? "all"}_" +
                $"{model.PersianFromDate ?? "all"}_" +
                $"{model.PersianToDate ?? "all"}_" +
                $"{model.PageNumber}_" +
                $"{model.PageSize}";
        }
        private string GenerateDetailCacheKey(
            int version,
            int noteId)
        {
            return $"note_v{version}_detail_{noteId}";
        }
    }
}
