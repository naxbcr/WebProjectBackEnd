using project1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using AutoMapper;
using project1.DAL;
using project1.DAL.UnitOfWork;

namespace project1.BLL.Services
{
    public class PositionService : IPositionService
    {
        IUnitOfWork _unitOfWork;

        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Position> GetAll()
        {
            var list = _unitOfWork.PositionRepository.GetAll().ToArray();
            if (list != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<t_position, Position>());
                List<Position> listpos = Mapper.Map<t_position[], List<Position>>(list);
                return listpos;
            }
            return null;
        }

        public Position GetPositionById(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<t_position, Position>());
            var position = _unitOfWork.PositionRepository.Get(id);
            Position dto = Mapper.Map<Position>(position);
            return dto;

        }
    }
}