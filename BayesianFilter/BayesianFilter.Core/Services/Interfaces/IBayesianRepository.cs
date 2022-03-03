using BayesianFilter.Core.Models;
using BayesianFilter.Core.Models.Entity;
using System.Collections.Generic;

namespace BayesianFilter.Core.Services.Interfaces
{
    public interface IBayesianRepository
    {
        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <returns></returns>
        List<BayesianModel> GetAll();

        /// <summary>
        /// Найти по строке
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        BayesianModel FindSubject(string subject);

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="subject"></param>
        void DeleteSubject(string subject);

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BayesianModel Insert(InsertModel model);

        /// <summary>
        /// Добавить список
        /// </summary>
        /// <param name="insertData"></param>
        void InsertList(List<InsertModel> insertData);

        /// <summary>
        /// Добавить список
        /// </summary>
        /// <param name="subjects"></param>
        /// <param name="isSpam"></param>
        /// <returns></returns>
        InsertResult BulkUpdate(List<string> subjects, bool isSpam);
    }
}
