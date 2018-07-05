using project1.BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1.BLL.Interfaces
{
    public interface ICommentService
    {
        Comment GetCommentById(int id);
        IEnumerable<Comment> GetAllComentsOfTranslate(int translateid);
        IEnumerable<Comment> GetAllComentsOfUser(int userid);
        int CreateComment(Comment comment);
        bool UpdateComment(int id,Comment comment);
        bool DeleteComment(int id);
    }
}
