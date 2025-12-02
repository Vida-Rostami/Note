using Note.Infrastructure.Tag;
using Note.Model;
using Note.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Application.Tag
{
    internal class TagServices : ITagServices
    {
        private readonly ITagRepository _tagRepository;

        public TagServices(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<BaseResponse<GetTagModel>> GetTag()
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
