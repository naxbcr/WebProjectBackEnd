using project1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using AutoMapper;
using project1.DAL;

namespace project1.BLL.Services
{
    public class LanguageService : ILanguageService
    {
        IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Language> GetAll()
        {
            var daltlangs = _unitOfWork.LanguageRepository.GetAll().ToArray();
            if (daltlangs != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<t_language, Language>());
                List<Language> listtypes = Mapper.Map<t_language[], List<Language>>(daltlangs);
                return listtypes;
            }
            return null;
        }

        public Language GetLanguagenById(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<t_language, Language>());
            t_language daltype = _unitOfWork.LanguageRepository.Get(id);
            Language dto = Mapper.Map<Language>(daltype);
            return dto;
        }
    }
}