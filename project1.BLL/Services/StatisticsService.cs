using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using project1.BLL.Model;
using project1.DAL.Interfaces;
using AutoMapper;
using project1.DAL;
using project1.BLL.Interfaces;
using System.Transactions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace project1.BLL.Services
{
 
    public class StatisticsService : IStatistics 
    {
        IUnitOfWork _unitOfWork;
        const decimal priceK = 0.07M;

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<AdminStatistics> getAdminStatistics()
        {
                Stopwatch stopwatch = new Stopwatch();
                AdminStatistics astats = new AdminStatistics();
                stopwatch.Start();

            var res1 = await _unitOfWork.UserRepository.FindAllWhereAsync(x => x.id_position == 1);
            var res2 = await _unitOfWork.UserRepository.FindAllWhereAsync(x => x.id_position == 2);
            var res3 = await _unitOfWork.UserRepository.FindAllWhereAsync(x => x.id_position == 3);
            var res4 = await _unitOfWork.UserRepository.FindAllWhereAsync(x => x.emailconfirm == false);
            var res5 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.translate_status == 1);
            var res6 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.translate_status == 4);
            var res7 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.translate_status == 3);
            var res8 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.end_date > DateTime.Now);

            astats.totalusers = res1.Count();
            astats.totaltranslators = res2.Count();
            astats.totaladmins = res3.Count();
            astats.notredeemedusers = res4.Count();

            astats.posted = res5.Count();
            astats.translated = res6.Count();
            astats.inprogress = res7.Count();
            astats.expired = res8.Count();

            //astats.totalusers = _unitOfWork.UserRepository.FindAllWhere(x => x.id_position == 1).Count();
            //astats.totaltranslators = _unitOfWork.UserRepository.FindAllWhere(x => x.id_position == 2).Count();
            //astats.totaladmins = _unitOfWork.UserRepository.FindAllWhere(x => x.id_position == 3).Count();
            //astats.notredeemedusers = _unitOfWork.UserRepository.FindAllWhere(x => x.emailconfirm == false).Count();


              stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;

                return astats;
            
        }

        public async Task<TranslatorStatistics> getTranslatorStatistics(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            TranslatorStatistics stats = new TranslatorStatistics(id);
            stopwatch.Start();

            var res1 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.id_translator == id & x.translate_status == 4);
            var res2 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.id_translator == id & x.translate_status == 3);
            var res3 = await _unitOfWork.TransRepository.FindAllWhereAsync(x => x.id_translator == id & x.end_date > DateTime.Now);
         

            var result4 = res1.Select(x => x.price).ToList().Sum(); // zarabotannie babki
            var result5 = res3.Select(x => x.price).ToList().Sum(); // poterjannie babki
            var result6 = result4 / res1.Count(); // vesj dohod na kolvo uspeshniyh perevodov, sredniy dohod za operaciju.
            var result7 = res2.Select(x => x.price).ToList().Sum(); // summirovat dohod budushih perevodov, tekushih

            stats.translated = res1.Count();
            stats.inprogress = res2.Count();
            stats.expired = res3.Count();

            stats.totalgain = (Decimal)result4*priceK;
            stats.lostgain = (Decimal)result5 *priceK;
            stats.mediumprice = (Decimal)result6 *priceK;
            stats.futuregain = (Decimal)result7 *priceK;

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            return stats;

        }
    }
}