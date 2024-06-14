using Microsoft.AspNetCore.Mvc;
using MVC_ASM_AHTBCinema_NHOM4_SD18301.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MVC_ASM_AHTBCinema_NHOM4_SD18301.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QlNhanVien : Controller
    {
        private readonly string url = "http://localhost:27936/api/NhanVien";
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<NhanVien> list = new List<NhanVien>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        // Chỉ định dạng ngày tháng của JSON.NET khi deserialize
                        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                        {
                            DateFormatString = "dd-MM-yyyy",
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        };
                        list = JsonConvert.DeserializeObject<List<NhanVien>>(apiResponse);
                    }
                    else
                    {
                        // Xử lý trường hợp không thành công
                        ViewBag.ErrorMessage = "Lỗi khi lấy dữ liệu từ API";
                    }
                }
            }
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVien dangKy)
        {
            if (!ModelState.IsValid)
            {
                return View(dangKy);
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(dangKy);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiRes = await response.Content.ReadAsStringAsync();
                        var createdCategory = JsonConvert.DeserializeObject<NhanVien>(apiRes);
                        return RedirectToAction("GetAll");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("IdKH", errorResponse);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"API Error: {response.ReasonPhrase}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return View(dangKy);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            NhanVien dangKy = null;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"{url}/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        dangKy = JsonConvert.DeserializeObject<NhanVien>(apiResponse);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Không tìm thấy dữ liệu";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Lỗi: {ex.Message}";
                    return View();
                }
            }
            return View(dangKy);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, NhanVien dangKy)
        {
            if (!ModelState.IsValid)
            {
                return View(dangKy);
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(dangKy);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync($"{url}/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetAll");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"API Error: {response.ReasonPhrase}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return View(dangKy);
        }
    }
}
