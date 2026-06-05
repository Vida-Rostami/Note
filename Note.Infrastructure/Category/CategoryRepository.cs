using Dapper;
using Microsoft.Extensions.Options;
using Note.Domain;
using Note.Domain.Category;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Note.Domain.Pagination;
using System.Net;
using Oracle.ManagedDataAccess.Types;
using Note.Infrastructure.Common.Retry;
using Polly;

namespace Note.Infrastructure.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private DatabaseSettings _databaseOptions;
        private readonly IAsyncPolicy _policy;
        public CategoryRepository(IOptions<DatabaseSettings> options)
        {
            _databaseOptions = options.Value;
            _policy = PollyHelper.CreateOracleRetryPolicy();
        }

        public async Task<PaginationBaseResposne<List<GetCategoryModel>>> Get(int pageNumber, int pageSize)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                using var connection = new OracleConnection(_databaseOptions.OracleConnection);
                await connection.OpenAsync();

                using var command = new OracleCommand("Sp_GetAllCategory", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("p_PageNumber", OracleDbType.Int32).Value = pageNumber;
                command.Parameters.Add("p_PageSize", OracleDbType.Int32).Value = pageSize;
                command.Parameters.Add("p_TotalCount", OracleDbType.Int32).Direction = ParameterDirection.Output;
                command.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                var categories = new List<GetCategoryModel>();
                while (await reader.ReadAsync())
                {
                    categories.Add(new GetCategoryModel
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                        CreateDateTime = reader.IsDBNull(reader.GetOrdinal("CreateDateTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        ModifyDateTime = reader.IsDBNull(reader.GetOrdinal("ModifyDateTime")) ? null : (reader.GetDateTime(reader.GetOrdinal("ModifyDateTime"))),
                    });
                }
                var totalObj = command.Parameters["p_TotalCount"].Value;
                var totalCount = totalObj == null || totalObj == DBNull.Value
                    ? 0
                    : ((OracleDecimal)totalObj).ToInt32();
                return new PaginationBaseResposne<List<GetCategoryModel>>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    IsSuccess = categories.Any(),
                    Data = categories,
                    Code = categories.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NoContent,
                    Message = categories.Any() ? "اطلاعات با موفقیت دریافت گردید." : "اطلاعاتی یافت نشد"
                };
            });
        }

        public async Task<BaseResponse<GetCategoryModel>> Get(int categoryId)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                using var connection = new OracleConnection(_databaseOptions.OracleConnection);
                await connection.OpenAsync();

                using var command = new OracleCommand("SP_GetCategoryById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("p_CategoryId", OracleDbType.Int32).Value = categoryId;
                command.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                GetCategoryModel category = null;
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    category = new GetCategoryModel
                    {
                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                }

                if (category == null)
                {
                    return new BaseResponse<GetCategoryModel>
                    {
                        IsSuccess = false,
                        Message = "اطلاعاتی یافت نشد",
                        Code = (int)HttpStatusCode.NoContent
                    };
                }

                return new BaseResponse<GetCategoryModel>
                {
                    Data = category,
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "با موفقیت دریافت شد"
                };
            });
        }
        
        public async Task<BaseResponse> Insert(AddCategoryModel model)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                using (var connection = new OracleConnection(_databaseOptions.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_CategoryName", model.CategoryName, DbType.String, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_InsertCategory", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = (int)HttpStatusCode.Created,
                        Message = "با موفقیت درج شد",
                    };
                }
            });
        }

        public async Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                var getCategory = await Get(model.CategoryId);
                if (getCategory != null)
                {
                    if (getCategory.Code == (int)HttpStatusCode.NoContent)
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Code = (int)HttpStatusCode.NoContent,
                            Message = "با شناسه وارد شده داده ای یافت نشد."
                        };
                    }
                }
                using (var connection = new OracleConnection(_databaseOptions.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_CategoryId", model.CategoryId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("P_CategoryName", model.CategoryName, DbType.String, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_UpdateCategory", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = (int)HttpStatusCode.OK,
                        Message = "با موفقیت اپدیت شد",
                    };
                }
            });
        }

        public async Task<BaseResponse> Delete(int categoryId)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                var getCategory = await Get(categoryId);
                if (getCategory != null)
                {
                    if (getCategory.Code == (int)HttpStatusCode.NoContent)
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Code = (int)HttpStatusCode.NoContent,
                            Message = "با شناسه وارد شده داده ای یافت نشد."
                        };
                    }
                }
                using (var connection = new OracleConnection(_databaseOptions.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_CategoryId", categoryId, DbType.Int32, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_DeleteCategory", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = (int)HttpStatusCode.OK,
                        Message = "با موفقیت حذف شد",
                    };
                }
            });
        }
    }
}
