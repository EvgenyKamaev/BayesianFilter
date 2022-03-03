using BayesianFilter.Core.Models;
using BayesianFilter.Core.Models.Entity;
using System.Collections.Generic;

namespace BayesianFilter.Core.Services.Interfaces
{
    public interface IExceptionsRepository
    {
        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <returns></returns>
        List<ExceptionsModel> GetAll();

        /// <summary>
        /// Найти по строке
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        ExceptionsModel FindSubject(string subject);

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="id"></param>
        Result DeleteSubject(int id);

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="isSpam"></param>
        /// <returns></returns>
        ExceptionsModel Insert(string subject);

        /// <summary>
        /// Постраничный вывод
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageModel GetPage(int page, int pageSize);
    }
}
