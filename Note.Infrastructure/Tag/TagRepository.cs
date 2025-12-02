using Note.Model;
using Note.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Infrastructure.Tag
{

    public class TagRepository : ITagRepository
    {
        private DatabaseSettings _option;
        public TagRepository(IOptions<DatabaseSettings> options)
        {
            _option = options.Value;
        }

        public Task<BaseResponse<GetTagModel>> GetTag()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetTagModel>> GetTag(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> InsertTag(AddTagModel model)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateTag(UpdateTagModel model)
        {
            throw new NotImplementedException();
        }
        public Task<BaseResponse> DeleteTag(int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
