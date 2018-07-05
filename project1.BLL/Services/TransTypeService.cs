
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using project1.BLL.Interfaces;
using project1.DAL.UnitOfWork;
using project1.BLL.Model;
using project1.DAL;
using project1.DAL.Interfaces;
using AutoMapper;

namespace project1.BLL.Services
{
    public class TransTypeService : ITypeService
    {
        IUnitOfWork _unitOfWork;

        public TransTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TransType> GetAllTypes()
        {
            var daltypes = _unitOfWork.TypeRepository.GetAll().ToArray();
            if (daltypes != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<t_type, TransType>());
                List<TransType> listtypes = Mapper.Map<t_type[], List<TransType>>(daltypes);
                return listtypes;
            }
            return null;
        }

        public TransType GetTypeById(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<t_type, TransType>());
            t_type daltype = _unitOfWork.TypeRepository.Get(id);
            TransType dto = Mapper.Map<TransType>(daltype);
            return dto;

        }
    }
}