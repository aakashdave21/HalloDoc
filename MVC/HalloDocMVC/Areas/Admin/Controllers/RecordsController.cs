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
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DataSheet1");

            // Set header text, style, and borders
            var headerStyle = worksheet.Range("A1:N1").Style;
            headerStyle.Font.Bold = true;
            headerStyle.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
            headerStyle.Font.FontColor = XLColor.FromHtml("#DCE6F1");
            headerStyle.Border.BottomBorder = XLBorderStyleValues.Thin;
            headerStyle.Border.BottomBorderColor = XLColor.Black;

            worksheet.Cell(1, 1).Value = "RequestId";
            worksheet.Cell(1, 2).Value = "Patient Name";
            worksheet.Column(2).Width = 20;
            worksheet.Cell(1, 3).Value = "Requestor";
            worksheet.Column(3).Width = 15;
            worksheet.Cell(1, 4).Value = "Service Date";
            worksheet.Column(4).Width = 15;
            worksheet.Cell(1, 5).Value = "Clsoe Case Date";
            worksheet.Column(5).Width = 15;
            worksheet.Cell(1, 6).Value = "Email";
            worksheet.Column(6).Width = 15;
            worksheet.Cell(1, 7).Value = "Phone";
            worksheet.Column(7).Width = 45;
            worksheet.Cell(1, 8).Value = "Address";
            worksheet.Column(8).Width = 40;
            worksheet.Cell(1, 9).Value = "Status";
            worksheet.Column(9).Width = 15;
            worksheet.Cell(1, 10).Value = "Physician";
            worksheet.Column(10).Width = 15;
            worksheet.Cell(1, 11).Value = "Physician Note";
            worksheet.Column(11).Width = 15;
            worksheet.Cell(1, 12).Value = "Cancelled By Physician Note";
            worksheet.Column(12).Width = 15;
            worksheet.Cell(1, 13).Value = "Admin Note";
            worksheet.Column(13).Width = 15;
            worksheet.Cell(1, 14).Value = "Patient Note";
            worksheet.Column(14).Width = 15;


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

                    // Apply data body styling
                    var dataRange = worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 14));
                    var dataCellStyle = dataRange.Style;
                    dataCellStyle.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");
                    dataCellStyle.Font.FontColor = XLColor.FromHtml("#4F81BD");
                    row++;
                }
                else
                {
                    // Log the null reference or handle it appropriately
                    Console.WriteLine("Null reference detected in item: " + item);
                }
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
            }
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }
}