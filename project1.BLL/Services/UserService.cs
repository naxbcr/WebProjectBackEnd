using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using project1.BLL.Interfaces;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using project1.DAL.UnitOfWork;
using project1.DAL;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using BCrypt.Net;



namespace project1.BLL.Services
{
    public class UserService : IUserService
    {
         IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public int CreateUser(User item)
        {
            if (AllowEmail(item.email)){

                using (var scope = new TransactionScope())
                {

                 item.password = BCrypt.Net.BCrypt.HashPassword(item.password,BCrypt.Net.BCrypt.GenerateSalt(10));

                    Mapper.Initialize(cfg => {
                        cfg.CreateMap<User, t_user>();
                    });

                    t_user user = Mapper.Map<t_user>(item);

                    _unitOfWork.UserRepository.Add(user);
                    _unitOfWork.Save();
                    scope.Complete();
                    return user.id;
                }
            }

            return 0;

        }

        public bool DeleteUser(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.Get(id);
                    if (user != null)
                    {

                        _unitOfWork.UserRepository.Delete(user.id);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<User> GetAllUsers()
        {

            Mapper.Initialize(cfg => {
                cfg.CreateMap<t_user, User>()
                  .ForMember(dest => dest.postionname, opt => opt.MapFrom(z => z.t_position.position)); 
            });

            var users = _unitOfWork.UserRepository.GetAll().ToArray();
            if (users != null)
            {
                List<User> list = Mapper.Map<t_user[], List<User>>(users);
                return list;
            }
            return null;

        }

        public User GetUserById(int id)
        {
            if (id > 0)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<t_user, User>()
                      .ForMember(dest => dest.postionname, opt => opt.MapFrom(z => z.t_position.position));
                });

                t_user user = _unitOfWork.UserRepository.Get(id);
                User dto = Mapper.Map<User>(user);
                return dto;
            }
            return null;

        }

        //проверка почты для регистрации, существует ли почта вообще
        public bool AllowEmail(string email)
        {
            var user = _unitOfWork.UserRepository.GetFirstOrDefaultWhere(x => x.email.Equals(email));
            if (user != null) { return false; //zanet
            }
            return true;//ne zanet 
        }

        //проверка почты для изменения данных в профиле, не занета ли она кем нибудь, кроме самого себя.
        public bool CheckEmail(string email, int id)
        {
            var user = _unitOfWork.UserRepository.GetFirstOrDefaultWhere(x => x.email.Equals(email));
            if(user.id == id) { return true; } // эмайл занет тем же пользователем
            return false; // эмайл занет кем-то другим
        }

        public bool UpdateUser(int id, User user)
        {
            var success = false;
            if (user != null & CheckEmail(user.email,user.id))
            {
                using (var scope = new TransactionScope())
                {
                    var dbuser = _unitOfWork.UserRepository.Get(id);
                    if (dbuser != null)
                    {
                        dbuser.first_name = user.first_name;
                        dbuser.last_name = user.last_name;
                        dbuser.email = user.email;
                        dbuser.id_position = user.id_position;

                        if (user.password != null) // меняем пароль если указали новый пароль, в другом случае поле пассворд у пользователя будет пустым, т.е. остается прежним и менять его ненадо
                        {
                            dbuser.password = BCrypt.Net.BCrypt.HashPassword(user.password, BCrypt.Net.BCrypt.GenerateSalt(10));
                        }
                        
                        _unitOfWork.UserRepository.Update(dbuser);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public User GetUserByEmail(string email)
        {
            if (email !=null)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<t_user, User>()
                      .ForMember(dest => dest.postionname, opt => opt.MapFrom(z => z.t_position.position));
                });

                t_user user = _unitOfWork.UserRepository.GetFirstOrDefaultWhere(x =>x.email.Equals(email));
                if (user != null)
                {
                    User dto = Mapper.Map<User>(user);
                    return dto;
                }
                return null;
            }
            return null;

        }

        public async Task<User> GetUserByEmailAndPassAsync(string email,string pass)
        {
            if (email != null & pass !=null)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<t_user, User>()
                      .ForMember(dest => dest.postionname, opt => opt.MapFrom(z => z.t_position.position));
                });
                
                t_user user = await _unitOfWork.UserRepository.GetFirstOrDefaultWhereAsync(x => x.email.Equals(email));
                if (user != null)
                {
                    bool verify = BCrypt.Net.BCrypt.Verify(pass, user.password);
                    if (verify)
                    {
                        User dto = Mapper.Map<User>(user);
                        return dto;
                    }
                }
                return null;
            }
            return null;
        }

        public void ConfirmEmail (string id, bool confirm)
        {
            t_user user = _unitOfWork.UserRepository.Get(int.Parse(id));
            user.emailconfirm = confirm;
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }



        /////////////////////////////////////////////////////



    }
}