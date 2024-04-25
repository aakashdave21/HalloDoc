using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
namespace HalloDocMVC.Controllers.Admin;
using ClosedXML.Excel;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class RecordsController : Controller
{

    private readonly IRecordsService _recordsService;
    public RecordsController(IRecordsService recordsService)
    {
        _recordsService = recordsService;
    }
    public IActionResult Index(RecordsView Parameters, int PageNum = 1, int PageSize = 5)
    {
        try
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_RecordsTable", _recordsService.GetAllRecords(Parameters, PageNum, PageSize));
            }
            else
            {
                return View(_recordsService.GetAllRecords(Parameters, PageNum, PageSize));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    public IActionResult Delete(int Id)
    {
        try
        {
            _recordsService.DeleteRecord(Id);
            TempData["Success"] = "Deleted SuccessFully";
            return RedirectToAction("Index");
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


    public IActionResult Download(RecordsView Parameters, int PageNum = 1, int PageSize = 5)
    {
        try
        {
            RecordsViewModel recordsView = _recordsService.GetAllRecords(Parameters, PageNum, PageSize);
            byte[] excelContent = GenerateExcel(recordsView);
            return File(excelContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }
    private static byte[] GenerateExcel(RecordsViewModel recordsView)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("DataSheet1");

        // Set header text, style, and borders
        var headerStyle = worksheet.Range("A1:N1").Style;
        headerStyle.Font.Bold = true;
        headerStyle.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
        headerStyle.Font.FontColor = XLColor.FromHtml("#DCE6F1");
        headerStyle.Border.BottomBorder = XLBorderStyleValues.Thin;
        headerStyle.Border.BottomBorderColor = XLColor.Black;

        string[] headers = { "RequestId", "Patient Name", "Requestor", "Service Date", "Clsoe Case Date", "Email", "Phone", "Address", "Status", "Physician", "Physician Note", "Cancelled By Physician Note", "Admin Note", "Patient Note" };
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Column(i + 1).Width = i == 0 ? 15 : 20;
        }

        int row = 2;
        foreach (var item in recordsView.RecordsRequestList)
        {
            // Check for null references in the item and related entities
            if (item != null)
            {
                worksheet.Cell(row, 1).Value = item.RequestId;
                worksheet.Cell(row, 2).Value = item.PatientName;
                worksheet.Cell(row, 3).Value = item.Requestor;
                worksheet.Cell(row, 4).Value = item.DateOfService;
                worksheet.Cell(row, 5).Value = "-";
                worksheet.Cell(row, 6).Value = item.Email;
                worksheet.Cell(row, 7).Value = item.PhoneNumber;
                worksheet.Cell(row, 8).Value = item.Address;
                worksheet.Cell(row, 9).Value = item.RequestStatus;
                worksheet.Cell(row, 10).Value = item.PhysicianName;
                worksheet.Cell(row, 11).Value = item.PhysicianNote;
                worksheet.Cell(row, 12).Value = item.CancelledByProviderNote;
                worksheet.Cell(row, 13).Value = item.AdminNote;
                worksheet.Cell(row, 14).Value = item.PatientNote;

                var dataRange = worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 14));
                var dataCellStyle = dataRange.Style;
                dataCellStyle.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");
                dataCellStyle.Font.FontColor = XLColor.FromHtml("#4F81BD");
                row++;
            }
            else
            {
                Console.WriteLine("Null reference detected in item: " + item);
            }
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}