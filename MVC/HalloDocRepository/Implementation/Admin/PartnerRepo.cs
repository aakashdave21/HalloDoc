using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using System.Data.Common;

namespace HalloDocRepository.Admin.Implementation;
public class PartnerRepo : IPartnerRepo
{
     private readonly HalloDocContext _dbContext;
        public PartnerRepo(HalloDocContext dbContext)
        {
            _dbContext = dbContext;
        }

    public IEnumerable<Healthprofessional> GetVendorList(string vendorName = "", int ProfessionId = 0){
        IEnumerable<Healthprofessional> query = _dbContext.Healthprofessionals.Include(v => v.ProfessionNavigation).Where(prof => prof.Isdeleted!=true);
        if(ProfessionId!=0){
            query = query.Where(prof => prof.Profession == ProfessionId);
        }
        if(!string.IsNullOrEmpty(vendorName)){
            query = query.Where(prof => prof.Vendorname.ToLower().Contains(vendorName));
        }
        query = query.OrderByDescending(prof => prof.Createddate);
        return query.ToList();
    }

    public void AddVendor(Healthprofessional vendorInfo){
        if(vendorInfo?.Id != 0 && vendorInfo?.Id != null){
            Healthprofessional? vendorDetail = _dbContext.Healthprofessionals.FirstOrDefault(v => v.Id == vendorInfo.Id && v.Isdeleted!=true);
            if(vendorDetail != null){
                vendorDetail.Vendorname = vendorInfo.Vendorname;
                vendorDetail.Profession = vendorInfo.Profession;
                vendorDetail.Email = vendorInfo.Email;
                vendorDetail.Faxnumber = vendorInfo.Faxnumber;
                vendorDetail.Phonenumber = vendorInfo.Phonenumber;
                vendorDetail.Address = vendorInfo.Address;
                vendorDetail.City = vendorInfo.City;
                vendorDetail.Regionid = vendorInfo.Regionid;
                vendorDetail.Zip = vendorInfo.Zip;
                vendorDetail.Businesscontact = vendorInfo.Businesscontact;

                _dbContext.SaveChanges();
                return;
            }else{
                throw new Exception();
            }
        }else{
            _dbContext.Healthprofessionals.Add(vendorInfo);
            _dbContext.SaveChanges();
        }
    }

    public void DeleteVendor(int Id){
        Healthprofessional? details = _dbContext.Healthprofessionals.FirstOrDefault(prof => prof.Id == Id && prof.Isdeleted!=true);
        if(details!=null){
            details.Isdeleted = true;
            details.Modifieddate = DateTime.Now;

            _dbContext.SaveChanges();
            return;
        }
        throw new Exception();
    }

    public Healthprofessional GetSingleBusiness(int Id){
        Healthprofessional? details = _dbContext.Healthprofessionals.FirstOrDefault(prof => prof.Id == Id && prof.Isdeleted!=true);
        if(details!=null){
            return details;
        }
        throw new Exception();
    }

}