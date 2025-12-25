using Note.Infrastructure.Tag;
using Note.Domain;
using Note.Domain.Tag;
using Note.Infrastructure.Caching;
using Note.Domain.Category;

namespace Note.Application.Tag
{
    public class TagServices : ITagServices
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICacheService _cacheService;

        public TagServices(ITagRepository tagRepository, ICacheService cacheService)
        {
            _tagRepository = tagRepository;
            _cacheService = cacheService;
        }

        public async Task<BaseResponse<List<GetTagModel>>> GetTag()
        {
            return await _tagRepository.GetTag();
        }

        public async Task<BaseResponse<GetTagModel>> GetTag(int tagId)
        {
            var cachedKey = $"tag_{tagId}";
            var tagCached = await _cacheService.Get<GetTagModel>(cachedKey);
            if(tagCached != null)
            {
                return new BaseResponse<GetTagModel>
                {
                    Data = tagCached,
                    IsSuccess = true,
                    Code = 200,
                    Message = "با موفقیت دریافت گردید."
                };
            }

            var result =  await _tagRepository.GetTag(tagId);
            if(result == null)
            {
                return new BaseResponse<GetTagModel>
                {
                    Code = 204,
                    IsSuccess = true,
                    Message = "اطلاعاتی یافت نگردید."
                };
            }
            await _cacheService.Set(cachedKey, result.Data, TimeSpan.FromMinutes(5));
            return result;
        }

        public async Task<BaseResponse> InsertTag(AddTagModel model)
        {
            return await _tagRepository.InsertTag(model);
        }

        public async Task<BaseResponse> UpdateTag(UpdateTagModel model)
        {
            return await _tagRepository.UpdateTag(model);
        }
        public async Task<BaseResponse> DeleteTag(int tagId)
        {
            return await _tagRepository.DeleteTag(tagId);
        }
    }
}
