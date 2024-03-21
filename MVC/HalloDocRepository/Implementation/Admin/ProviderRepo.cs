using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using System.Data.Common;

namespace HalloDocRepository.Admin.Implementation;
public class ProviderRepo : IProviderRepo
{
    private readonly HalloDocContext _dbContext;
    public ProviderRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Physician> GetAllPhysician(bool order = true, string? regionId = null)
    {
        IQueryable<Physician> query = _dbContext.Physicians.Include(phy => phy.Role);
        if (!string.IsNullOrEmpty(regionId))
        {
            query = query.Where(physician => physician.Regionid == int.Parse(regionId));
        }
        if (order)
        {
            return query.OrderBy(physician => physician.Firstname)
                        .ThenBy(physician => physician.Lastname)
                        .ToList();
        }
        else
        {
            return query.OrderByDescending(physician => physician.Firstname)
                        .ThenByDescending(physician => physician.Lastname)
                        .ToList();
        }
    }
    public void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds){
        //startnotificationId -> true
        // stopNotificationId -> false
        var stopIds = stopNotificationIds.Select(int.Parse).ToList();
        _dbContext.Physicians
            .Where(physician => stopIds.Contains(physician.Id))
            .ToList()
            .ForEach(physician => physician.IsNotificationStop = false);

        var startIds = startNotificationIds.Select(int.Parse).ToList();
        _dbContext.Physicians
            .Where(physician => startIds.Contains(physician.Id))
            .ToList()
            .ForEach(physician => physician.IsNotificationStop = true);

        _dbContext.SaveChanges();

    }
}