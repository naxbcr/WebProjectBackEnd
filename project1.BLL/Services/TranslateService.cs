using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using project1.BLL.Interfaces;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using project1.DAL.UnitOfWork;
using project1.DAL;
using System.Transactions;
using AutoMapper;

namespace project1.BLL.Services
{
    public class TranslateService : ITranslateService
    {
        IUnitOfWork _unitOfWork;

        public TranslateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CreateTranslate(Translate newtrans)
        {

            using (var scope = new TransactionScope())
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<Translate, t_translate>();
                });

                t_translate tra = Mapper.Map<t_translate>(newtrans);

                _unitOfWork.TransRepository.Add(tra);
                _unitOfWork.Save();
                scope.Complete();
                return tra.id;
            }
        }

        public bool DeleteTrans(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var tra = _unitOfWork.TransRepository.Get(id);
                    if (tra != null)
                    {

                        _unitOfWork.TransRepository.Delete(tra.id);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Translate> GetAllTranslates()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_translate, Translate>()
                  .ForMember(dest => dest.typename, opt => opt.MapFrom(z => z.t_type.typename))
                  .ForMember(dest => dest.cs_userfname, opt => opt.MapFrom(z => z.t_user.first_name))
                  .ForMember(dest => dest.cs_userlname, opt => opt.MapFrom(z => z.t_user.last_name))
                  .ForMember(dest => dest.tr_userfname, opt => opt.MapFrom(z => z.t_user1.first_name))
                  .ForMember(dest => dest.tr_userlname, opt => opt.MapFrom(z => z.t_user1.last_name))
                  .ForMember(dest => dest.srclang_name, opt => opt.MapFrom(z => z.t_language.language))
                  .ForMember(dest => dest.destlang_name, opt => opt.MapFrom(z => z.t_language1.language))
                  .ForMember(dest => dest.status_name, opt => opt.MapFrom(z => z.t_status.status));
            });

            var trans = _unitOfWork.TransRepository.GetAll().ToArray();
            if (trans != null)
            {
                List<Translate> list = Mapper.Map<t_translate[], List<Translate>>(trans);
                return list;
            }
            return null;
        }

        public IEnumerable<Translate> GetAllUserTranslates(int userid)
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_translate, Translate>()
                  .ForMember(dest => dest.typename, opt => opt.MapFrom(z => z.t_type.typename))
                  .ForMember(dest => dest.cs_userfname, opt => opt.MapFrom(z => z.t_user.first_name))
                  .ForMember(dest => dest.cs_userlname, opt => opt.MapFrom(z => z.t_user.last_name))
                  .ForMember(dest => dest.tr_userfname, opt => opt.MapFrom(z => z.t_user1.first_name))
                  .ForMember(dest => dest.tr_userlname, opt => opt.MapFrom(z => z.t_user1.last_name))
                  .ForMember(dest => dest.srclang_name, opt => opt.MapFrom(z => z.t_language.language))
                  .ForMember(dest => dest.destlang_name, opt => opt.MapFrom(z => z.t_language1.language))
                  .ForMember(dest => dest.status_name, opt => opt.MapFrom(z => z.t_status.status));
            });

            var trans = _unitOfWork.TransRepository.FindAllWhere(x =>x.id_customer == userid | x.id_translator == userid).ToArray();
            if (trans != null)
            {
                List<Translate> list = Mapper.Map<t_translate[], List<Translate>>(trans);
                return list;
            }
            return null;
        }

        public Translate GetTransById(int id)
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_translate, Translate>()
                  .ForMember(dest => dest.typename, opt => opt.MapFrom(z => z.t_type.typename))
                  .ForMember(dest => dest.cs_userfname, opt => opt.MapFrom(z => z.t_user.first_name))
                  .ForMember(dest => dest.cs_userlname, opt => opt.MapFrom(z => z.t_user.last_name))
                  .ForMember(dest => dest.tr_userfname, opt => opt.MapFrom(z => z.t_user1.first_name))
                  .ForMember(dest => dest.tr_userlname, opt => opt.MapFrom(z => z.t_user1.last_name))
                  .ForMember(dest => dest.srclang_name, opt => opt.MapFrom(z => z.t_language.language))
                  .ForMember(dest => dest.destlang_name, opt => opt.MapFrom(z => z.t_language1.language))
                  .ForMember(dest => dest.status_name, opt => opt.MapFrom(z => z.t_status.status));
            });

            if (id > 0)
            {
                t_translate tra = _unitOfWork.TransRepository.Get(id);
                Translate dto = Mapper.Map<Translate>(tra);
                return dto;
            }
            return null;
        }

        public bool UpdateTrans(int id, Translate trans)
        {
            

            var success = false;
            if (trans != null)
            {
                using (var scope = new TransactionScope())
                {
                    var dbtrans = _unitOfWork.TransRepository.Get(id);
                    if (dbtrans != null)
                    {
                        dbtrans.title = trans.title;
                        dbtrans.update_date = trans.update_date;
                        dbtrans.decription = trans.decription;
                        dbtrans.src_lang = trans.src_lang;
                        dbtrans.dest_lang = trans.dest_lang;
                        dbtrans.translate_status = trans.translate_status;
                        dbtrans.end_date = trans.end_date;
                        dbtrans.id_translator = trans.id_translator;
                        dbtrans.id_type = trans.id_type;
                        dbtrans.link_customer = trans.link_customer;
                        dbtrans.link_translator = trans.link_translator;
                        dbtrans.translate_status = trans.translate_status;
                        int pricecount = DocReader.ReadDoc(trans.link_customer);
                        dbtrans.price = Convert.ToDecimal(pricecount);

                        _unitOfWork.TransRepository.Update(dbtrans);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}