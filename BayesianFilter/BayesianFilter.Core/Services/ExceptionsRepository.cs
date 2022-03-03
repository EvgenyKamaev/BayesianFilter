using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using BayesianFilter.Core.Services.Interfaces;
using BayesianFilter.Core.Logger;
using BayesianFilter.Core.Models.Entity;
using System.Linq;
using BayesianFilter.Core.Models;

namespace BayesianFilter.Core.Services
{
    public class ExceptionsRepository : IExceptionsRepository
    {
        private readonly ICoreConfig _configuration;
        private readonly ICoreLogger _logger;

        public ExceptionsRepository(ICoreConfig configuration, ICoreLogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <returns></returns>
        public List<ExceptionsModel> GetAll()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"SELECT * FROM [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions]";

                    return connection.Query<ExceptionsModel>(command).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ExceptionsRepository. Error get from table [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions]: \r\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// Найти по строке
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public ExceptionsModel FindSubject(string subject)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"SELECT Id, CreatedDate, Subject
                                             FROM [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions] (NOLOCK) WHERE Subject = @subject";

                    return connection.QueryFirstOrDefault<ExceptionsModel>(command, new { subject });
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ExceptionsRepository. Error get subject: '{subject}'\r\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="id"></param>
        public Result DeleteSubject(int id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    var command = "DELETE FROM [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions] WHERE Id = @id";
                    connection.Execute(command, new { id });

                    return new Result()
                    {
                        Message = "SUCCESS",
                        StatusCode = 0
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ExceptionsRepository. Error delete from table [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions]: subject '{id}'\r\n{ex}");

                return new Result()
                {
                    Message = "ERROR",
                    StatusCode = -1
                };
            }
        }

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="isSpam"></param>
        /// <returns></returns>
        public ExceptionsModel Insert(string subject)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"INSERT INTO [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions] (CreatedDate,Subject)
                                             OUTPUT inserted.Id, inserted.CreatedDate, inserted.Subject                                                    
                                             VALUES (@CreatedDate, @Subject)";

                    var parameters = new
                    {
                        CreatedDate = DateTime.Now,
                        Subject = subject,
                    };

                    return connection.QuerySingle<ExceptionsModel>(command, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ExceptionsRepository. Error insert into table [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions]: subject '{subject}'\r\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// Постраничный вывод
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageModel GetPage(int page, int pageSize)
        {
            try
            {
                var offest = (pageSize - 1) * page;

                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"SELECT * FROM [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions] ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                    var parameters = new
                    {
                        Offset = offest,
                        PageSize = pageSize
                    };

                    var exceptions = connection.Query<ExceptionsModel>(command, parameters).ToList();

                    const string countCommand = @"SELECT COUNT(*) FROM [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions]";

                    var totalCount = connection.QueryFirst<int>(countCommand);

                    var totalPageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);

                    return new PageModel()
                    {
                        Exceptions = exceptions,
                        Page = page,
                        TotalExceptionsCount = totalCount,
                        TotalPageCount = totalPageCount
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ExceptionsRepository. Error get from table [PayOnlineSystem.BayesianFilter].[dbo].[Exceptions]: \r\n{ex}");
                return null;
            }
        }
    }
}
