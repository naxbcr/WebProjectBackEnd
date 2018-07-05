using project1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using project1.DAL.UnitOfWork;
using project1.DAL;
using AutoMapper;
using System.Transactions;

namespace project1.BLL.Services
{
    
    public class CommentService : ICommentService
    {
        IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CreateComment(Comment comment)
        {
            using (var scope = new TransactionScope())
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<Comment, t_comment>();    
                });

                t_comment com =  Mapper.Map<t_comment>(comment);

                _unitOfWork.CommentRepository.Add(com);
                _unitOfWork.Save();
                scope.Complete();
                return com.id;
            }
        }

        public bool DeleteComment(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var com = _unitOfWork.CommentRepository.Get(id);
                    if (com != null)
                    {

                        _unitOfWork.CommentRepository.Delete(com.id);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Comment> GetAllComentsOfTranslate(int translateid)
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_comment, Comment>()
                  .ForMember(dest => dest.postion, opt => opt.MapFrom(z => z.t_user.t_position.position))
                  .ForMember(dest => dest.userfname, opt => opt.MapFrom(z => z.t_user.first_name))
                  .ForMember(dest => dest.userlname, opt => opt.MapFrom(z => z.t_user.last_name));

            });

            var dalcoments = _unitOfWork.CommentRepository.FindAllWhere(x => x.id_translate == translateid).ToArray();
            if (dalcoments != null)
            {
                List<Comment> list = Mapper.Map<t_comment[], List<Comment>>(dalcoments);
                return list;
            }
            return null;
        }

        public IEnumerable<Comment> GetAllComentsOfUser(int userid)
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_comment, Comment>()
                  .ForMember(dest => dest.postion, opt => opt.MapFrom(z => z.t_user.t_position.position))
                  .ForMember(dest => dest.userfname, opt => opt.MapFrom(z => z.t_user.first_name))
                  .ForMember(dest => dest.userlname, opt => opt.MapFrom(z => z.t_user.last_name));

            });

            var dalcoments = _unitOfWork.CommentRepository.FindAllWhere(x => x.id_user == userid).ToArray();
            if (dalcoments != null)
            {
                List<Comment> list = Mapper.Map<t_comment[], List<Comment>>(dalcoments);
                return list;
            }
            return null;
        }

        public Comment GetCommentById(int id)
        {
            
            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_comment, Comment>()
                  .ForMember(dest => dest.postion, opt => opt.MapFrom(z => z.t_user.t_position.position))
                  .ForMember(dest => dest.userfname, opt => opt.MapFrom(z => z.t_user.first_name))
                  .ForMember(dest => dest.userlname, opt => opt.MapFrom(z => z.t_user.last_name));

                  });

            if (id > 0)
            {
                t_comment tcom = _unitOfWork.CommentRepository.Get(id);
                Comment dto = Mapper.Map<Comment>(tcom);
                return dto;
            }
            return null; 


        }

        public bool UpdateComment(int id, Comment comment)
        {
            //poka nenado
            throw new NotImplementedException();
        }
    }
}