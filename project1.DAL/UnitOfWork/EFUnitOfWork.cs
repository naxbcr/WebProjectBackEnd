using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project1.DAL.Interfaces;
using project1.DAL.Repository;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace project1.DAL.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly db_KulakovEntities _context;
        private IRepository<t_user> _userRepository;
        private IRepository<t_position> _positionRepository;
        private IRepository<t_comment> _commentRepository;
        private IRepository<t_translate> _transRepository;
        private IRepository<t_type> _typeRepository;

        private IRepository<t_language> _langRepository;
        private IRepository<t_status> _statusRepository;

        
        public EFUnitOfWork()
        {
            _context = new db_KulakovEntities();
            
        }

        public IRepository<t_comment> CommentRepository
        {
            get
            {
                if (this._commentRepository == null)
                    this._commentRepository = new EFGenericRepository<t_comment>(_context);
                return _commentRepository;
            }
        }

        public IRepository<t_position> PositionRepository
        {
            get
            {
                if (this._positionRepository == null)
                    this._positionRepository = new EFGenericRepository<t_position>(_context);
                return _positionRepository;
            }

        }

        public IRepository<t_translate> TransRepository
        {

            get
            {
                if (this._transRepository == null)
                    this._transRepository = new EFGenericRepository<t_translate>(_context);
                return _transRepository;
            }
        }

        public IRepository<t_type> TypeRepository
        {
            get
            {
                if (this._typeRepository == null)
                    this._typeRepository = new EFGenericRepository<t_type>(_context);
                return _typeRepository;
            }
        }

        public IRepository<t_user> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new EFGenericRepository<t_user>(_context);
                return _userRepository;
            }
        }



        public IRepository<t_status> StatusRepository
        {
            get
            {
                if (this._statusRepository == null)
                    this._statusRepository = new EFGenericRepository<t_status>(_context);
                return _statusRepository;
            }
        }

        public IRepository<t_language> LanguageRepository
        {
            get
            {
                if (this._langRepository == null)
                    this._langRepository = new EFGenericRepository<t_language>(_context);
                return _langRepository;
            }
        }



        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
               await  _context.SaveChangesAsync(); 
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       
    }
}