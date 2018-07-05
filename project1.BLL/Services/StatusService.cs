using project1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using project1.DAL;
using AutoMapper;

namespace project1.BLL.Services
{
    public class StatusService : IStatusService
    {

        IUnitOfWork _unitOfWork;

        public StatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Status> GetAll()
        {
            var daltlangs = _unitOfWork.StatusRepository.GetAll().ToArray();
            if (daltlangs != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<t_status, Status>());
                List<Status> listtypes = Mapper.Map<t_status[], List<Status>>(daltlangs);
                return listtypes;
            }
            return null;
        }

        public Status GetById(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<t_status, Status>());
            t_status daltype = _unitOfWork.StatusRepository.Get(id);
            Status dto = Mapper.Map<Status>(daltype);
            return dto;
        }
    }
}