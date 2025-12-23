using Note.Infrastructure.Tag;
using Note.Domain;
using Note.Domain.Tag;

namespace Note.Application.Tag
{
    public class TagServices : ITagServices
    {
        private readonly ITagRepository _tagRepository;

        public TagServices(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<BaseResponse<List<GetTagModel>>> GetTag()
        {
            return await _tagRepository.GetTag();
        }

        public async Task<BaseResponse<GetTagModel>> GetTag(int tagId)
        {
            return await _tagRepository.GetTag(tagId);
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
