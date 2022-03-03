using BayesianFilter.Core.Logger;
using BayesianFilter.Core.Models.Entity;
using BayesianFilter.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using BayesianFilter.Core.Models;

namespace BayesianFilter.Core.Services
{
    public class BayesianRepository : IBayesianRepository
    {
        private readonly ICoreConfig _configuration;
        private readonly ICoreLogger _logger;

        public BayesianRepository(ICoreConfig configuration, ICoreLogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <returns></returns>
        public List<BayesianModel> GetAll()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"SELECT * FROM [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian]";

                    return connection.Query<BayesianModel>(command).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"BayesianRepository. Error get from table [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian]: \r\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// Найти по строке
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public BayesianModel FindSubject(string subject)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"SELECT Id,
                                                    CreatedDate,
                                                    Subject,
                                                    IsSpam
                                             FROM [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian] (NOLOCK)
                                             WHERE Subject = @subject";

                    return connection.QueryFirstOrDefault<BayesianModel>(command, new { subject });
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"BayesianRepository. Error get subject: '{subject}'\r\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="subject"></param>
        public void DeleteSubject(string subject)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    var command = "DELETE FROM [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian] WHERE Subject = @subject";
                    connection.Execute(command, new { subject });
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"BayesianRepository. Error delete from table [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian]: subject '{subject}'\r\n{ex}");
            }
        }

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="isSpam"></param>
        /// <returns></returns>
        public BayesianModel Insert(InsertModel model)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                {
                    const string command = @"INSERT INTO [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian] (CreatedDate,Subject,IsSpam)
                                             OUTPUT inserted.Id, inserted.CreatedDate, inserted.Subject, inserted.IsSpam                                                    
                                             VALUES (@CreatedDate, @Subject, @IsSpam)";

                    var parameters = new
                    {
                        CreatedDate = DateTime.Now,
                        Subject = model.Subject,
                        IsSpam = model.IsSpam
                    };

                    return connection.QuerySingle<BayesianModel>(command, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"BayesianRepository. Error insert into table [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian]: subject '{model.Subject}'\r\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// Добавить список
        /// </summary>
        /// <param name="insertData"></param>
        public void InsertList(List<InsertModel> insertData)
        {
            foreach (var insert in insertData)
            {
                try
                {
                    using (IDbConnection connection = new SqlConnection(_configuration.ConnectionString))
                    {
                        const string command = @"INSERT INTO [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian] (CreatedDate,Subject,IsSpam),                                                    
                                             VALUES (@CreatedDate, @Subject, @IsSpam)";

                        var parameters = new
                        {
                            CreatedDate = DateTime.Now,
                            Subject = insert.Subject,
                            IsSpam = insert.IsSpam
                        };

                        connection.QuerySingle<BayesianModel>(command, parameters);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"BayesianRepository. Error insert into table [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian]: subject '{insert.Subject}'\r\n{ex}");
                }
            }
        }

        /// <summary>
        /// Обновление списка
        /// </summary>
        /// <param name="subjects"></param>
        /// <param name="isSpam"></param>
        /// <returns></returns>
        public InsertResult BulkUpdate(List<string> subjects, bool isSpam)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        ///Удаление существующих записей
                        var deleted = connection.Execute("DELETE FROM [PayOnlineSystem.BayesianFilter].[dbo].[Bayesian] WHERE IsSpam = @isSpam", new { isSpam }, transaction);

                        var tbl = CreateDataTable(subjects, isSpam);

                        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.BatchSize = 1000;

                            FillColumnMappings(bulkCopy.ColumnMappings);

                            bulkCopy.DestinationTableName = "Bayesian";

                            bulkCopy.WriteToServerAsync(tbl);
                        }

                        transaction.Commit();
                        return new InsertResult { Inserted = subjects.Count() };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"BayesianRepository. BulkInsert {ex}");
                return null;
            }
        }

        private DataTable CreateDataTable(List<string> subjects, bool isSpam)
        {
            var tbl = new DataTable("Bayesian");
            var columns = GetColumns();
            tbl.Columns.AddRange(columns);

            foreach (var subject in subjects)
            {
                var newRow = tbl.NewRow();
                newRow = GetRow(newRow, subject, isSpam);
                tbl.Rows.Add(newRow);
            }
            tbl.AcceptChanges();
            return tbl;
        }

        private DataColumn[] GetColumns()
        {
            var columns = new[]
            {
                new DataColumn("Subject", typeof(string)),
                new DataColumn("CreatedDate", typeof(DateTime)),
                new DataColumn("IsSpam", typeof(bool))
            };
            return columns;
        }

        private DataRow GetRow(DataRow newRow, string entry, bool isSpam)
        {
            newRow["Subject"] = entry;
            newRow["CreatedDate"] = DateTime.Now;
            newRow["IsSpam"] = isSpam;
            return newRow;
        }

        private static void FillColumnMappings(SqlBulkCopyColumnMappingCollection columnMappings)
        {
            columnMappings.Add("Subject", "Subject");
            columnMappings.Add("CreatedDate", "CreatedDate");
            columnMappings.Add("IsSpam", "IsSpam");
        }
    }
}
