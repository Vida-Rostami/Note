using Dapper;
using Microsoft.Extensions.Options;
using Note.Model;
using Note.Model.Category;
using Note.Model.Note;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;

namespace Note.Infrastructure.Category
{


    public class CategoryRepository : ICategoryRepository
    {
        private DatabaseSettings _options;

        public CategoryRepository(IOptions<DatabaseSettings> options)
        {
            _options = options.Value;
        }
        public async Task<BaseResponse<List<GetCategoryModel>>> Get()
        {
            try
            {
                using var connection = new OracleConnection(_options.OracleConnection);
                await connection.OpenAsync();

                using var command = new OracleCommand("Sp_GetAllCategory", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                var tags = new List<GetCategoryModel>();
                while (await reader.ReadAsync())
                {
                    var tag = new GetCategoryModel
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        CategoryName = reader.GetString("CategoryName"),
                        CreateDateTime = reader.IsDBNull(reader.GetOrdinal("CreateDateTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        ModifyDateTime = reader.IsDBNull(reader.GetOrdinal("ModifyDateTime")) ? null : (reader.GetDateTime(reader.GetOrdinal("ModifyDateTime"))),
                    };
                    tags.Add(tag);
                }

                if (!tags.Any())
                {
                    return new BaseResponse<List<GetCategoryModel>>
                    {
                        IsSuccess = false,
                        Message = "اطلاعاتی یافت نگردید",
                        Code = 204
                    };
                }


                return new BaseResponse<List<GetCategoryModel>>
                {
                    IsSuccess = true,
                    Data = tags,
                    Message = "اطلاعات با موفقیت دریافت گردید.",
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<GetCategoryModel>>
                {
                    IsSuccess = false,
                    Message = $"خطا در دریافت اطلاعات",
                    Code = 500
                };
            }
        }
        public async Task<BaseResponse<GetCategoryModel>> Get(int categoryId)
        {
            try
            {
                using var connection = new OracleConnection(_options.OracleConnection);
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
                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                }
                if (category == null)
                {
                    return new BaseResponse<GetCategoryModel>
                    {
                        IsSuccess = false,
                        Message = "اطلاعاتی یافت نشد",
                        Code = 204
                    };
                }
                return new BaseResponse<GetCategoryModel>
                {
                    Data = category,
                    IsSuccess = true,
                    Code = 200,
                    Message = "با موفقیت دریافت شد"
                };
            }
            catch (Exception ex)
            {
                // TODO: log ex.Message
                return new BaseResponse<GetCategoryModel>
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = "خطایی رخ داده است"
                };
            }
        }
        public async Task<BaseResponse> Insert(AddCategoryModel model)
        {
            try
            {
                using (var connection = new OracleConnection(_options.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_CategoryName", model.CategoryName, DbType.String, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_InsertCategory", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = 201,
                        Message = "با موفقیت درج شد",
                    };
                }
            }
            catch (Exception ex)
            {
                //log
                return new BaseResponse
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = "خطایی رخ داده است"
                };
            }
        }

        public async Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            try
            {
                var getCategory = Get(model.CategoryId);
                if (getCategory != null)
                {
                    if (getCategory.Result.Code == 204)
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Code = 204,
                            Message = "با شناسه وارد شده داده ای یافت نشد."
                        };
                    }
                }
                using (var connection = new OracleConnection(_options.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_CategoryId", model.CategoryId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("P_CategoryName", model.CategoryName, DbType.String, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_UpdateCategory", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = 201,
                        Message = "با موفقیت اپدیت شد",
                    };
                }
            }
            catch (Exception ex)
            {
                //log
                return new BaseResponse
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = "خطایی رخ داده است"
                };
            }
        }
        public async Task<BaseResponse> Delete(int categoryId)
        {
            try
            {
                var getCategory = Get(categoryId);
                if (getCategory != null)
                {
                    if (getCategory.Result.Code == 204)
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Code = 204,
                            Message = "با شناسه وارد شده داده ای یافت نشد."
                        };
                    }
                }
                using (var connection = new OracleConnection(_options.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_CategoryId", categoryId, DbType.Int32, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_DeleteCategory", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = 201,
                        Message = "با موفقیت حدف شد",
                    };
                }
            }
            catch (Exception ex)
            {
                //log
                return new BaseResponse
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = "خطایی رخ داده است"
                };
            }
        }
    }
}
