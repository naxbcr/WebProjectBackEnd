using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        //только для ЕФ

        IRepository<t_user> UserRepository { get; }
        IRepository<t_position> PositionRepository { get; }
        IRepository<t_comment> CommentRepository { get; }
        IRepository<t_translate> TransRepository { get; }
        IRepository<t_type> TypeRepository { get; }
        IRepository<t_language> LanguageRepository { get; }
        IRepository<t_status> StatusRepository { get; }

        void Dispose();
        void Save();

        Task SaveAsync();
    }
}
