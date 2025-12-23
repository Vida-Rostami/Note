using Note.Domain;
using Note.Domain.Tag;

namespace Note.Infrastructure.Tag
{
    public interface ITagRepository
    {
        Task<BaseResponse<List<GetTagModel>>> GetTag();
        Task<BaseResponse<GetTagModel>> GetTag(int tagId);
        Task<BaseResponse> InsertTag(AddTagModel model); 
        Task<BaseResponse> UpdateTag(UpdateTagModel model);
        Task<BaseResponse> DeleteTag(int tagId);
    }
}
