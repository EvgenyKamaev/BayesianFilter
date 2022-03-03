using BayesianFilter.Core.Models;
using BayesianFilter.Core.Models.Entity;
using BayesianFilter.Core.Services.Interfaces;
using System.Collections.Generic;

namespace BayesianFilter.Core.Services
{
    /// <summary>
    /// Основной сервис
    /// </summary>
    public class BayesianService : IBayesianService
    {
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly IBayesianClassifier _bayesianClassifier;
        private readonly IBayesianRepository _bayesianRepository;
        private readonly IBayesianContainer _bayesianContainer;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="exceptionsRepository"></param>
        /// <param name="bayesianClassifier"></param>
        /// <param name="bayesianRepository"></param>
        /// <param name="bayesianContainer"></param>
        public BayesianService(IExceptionsRepository exceptionsRepository, IBayesianClassifier bayesianClassifier, IBayesianRepository bayesianRepository, IBayesianContainer bayesianContainer)
        {
            _exceptionsRepository = exceptionsRepository;
            _bayesianClassifier = bayesianClassifier;
            _bayesianRepository = bayesianRepository;
            _bayesianContainer = bayesianContainer;
        }

        /// <summary>
        /// Проверка строки алготимом Байеса
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public CheckResponse Check(string subject)
        {
            if (string.IsNullOrEmpty(subject))
                return new CheckResponse()
                {
                    Result = 0,
                    Message = "String is null or empty"
                };

            var except = _exceptionsRepository.FindSubject(subject);

            if (except != null)
                return new CheckResponse()
                {
                    Result = 0,
                    Message = "Target string in exceptions list"
                };

            var subjectParts = SplitString(subject.ToLower()).ToArray();
            var checkResult = _bayesianClassifier.IsSubjectSpam(subjectParts);

            return new CheckResponse()
            {
                Result = checkResult,
                Message = "OK"
            };
        }

        /// <summary>
        /// Добавить "черный" список
        /// </summary>
        /// <param name="spams"></param>
        /// <returns></returns>
        public InsertResult AddSpams(List<string> spams)
        {
            if (spams == null)
                return null;

            var result = _bayesianRepository.BulkUpdate(spams, true);

            if (result == null)
                return null;

            _bayesianContainer.LoadData();

            return new InsertResult()
            {
                Inserted = result.Inserted
            };
        }

        /// <summary>
        /// Добавить "белый" список
        /// </summary>
        /// <param name="hams"></param>
        /// <returns></returns>
        public InsertResult AddHams(List<string> hams)
        {
            if (hams == null)
                return null;

            var result = _bayesianRepository.BulkUpdate(hams, false);

            if (result == null)
                return null;

            _bayesianContainer.LoadData();

            return new InsertResult()
            {
                Inserted = result.Inserted
            };
        }

        /// <summary>
        /// Добавить исключение
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public ExceptionsModel AddException(string subject)
        {
            var checkExist = _exceptionsRepository.FindSubject(subject);

            if (checkExist != null)
                return null;

            var result = _exceptionsRepository.Insert(subject);

            if (result == null)
                return null;

            return new ExceptionsModel()
            {
                Id = result.Id,
                CreatedDate = result.CreatedDate,
                Subject = subject,
            };
        }

        /// <summary>
        /// Удалить исключение
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result DeleteException(int id)
        {
            var result = _exceptionsRepository.DeleteSubject(id);

            return new Result()
            {
                StatusCode = result.StatusCode,
                Message = result.Message,
            };
        }

        /// <summary>
        /// Получение страницы исключений
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageModel GetExceptionsPage(int page, int pageSize)
        {
            var result = _exceptionsRepository.GetPage(page, pageSize);

            if (result == null)
                return null;

            return new PageModel()
            {
                Exceptions = result.Exceptions,
                Page = result.Page,
                TotalExceptionsCount = result.TotalExceptionsCount,
                TotalPageCount = result.TotalPageCount
            };
        }

        /// <summary>
        /// Деление строки на биграммы
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<string> SplitString(string str)
        {
            List<string> list = new List<string>();
            int i = 0;
            while (i < str.Length - 1)
            {
                list.Add(str.Substring(i, 2));
                i += 2;
            }
            return list;
        }
    }
}
