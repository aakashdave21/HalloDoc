using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.Admin.Implementation;
public class InvoicingRepo : IInvoicingRepo
{
     private readonly HalloDocContext _dbContext;
        public InvoicingRepo(HalloDocContext dbContext)
        {
            _dbContext = dbContext;
        }


}