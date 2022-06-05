using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCI.Application.Services.Users.Command.EditeUser;
using UCI.Application.Services.Users.Command.RegisterUser;
using UCI.Application.Services.Users.Command.RemoveUser;
using UCI.Application.Services.Users.Command.UserstatusChenge;
using UCI.Application.Services.Users.Query.GetRoles;
using UCI.Application.Services.Users.Query.GetUser;
using UCI.Common.Dto;
using static UCI.Application.Services.Users.Command.EditeUser.Editeuser;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        //13
        //نمونه سازی از سرویس مورد نظر برای فرخوانی لیست مربوطه

        //ببینید اینجاباخود کلاس کار نمیکنیم با اینترفیس کار میکنیم
        //ctor +dobar tab                                               
        //ایجاد سازنده کلاس و سپس اینجکشن
        private readonly IGetUserService _getUserService;
        //(2-ابتدا مراحل بالا را انجام دادیم . حالا میخواهیم نقش رول ها رو به ویوو خودمون بفرستیم. پس میاییم این سرویس رو هم اینجا استفده میکنیم)
        private readonly IGetRolesService _getRolesService;
        //مرحله3
        //در این مرحله ما برای اینکه بتونیم یوزر خودمون رو اضافه کنیم ی نمونه از اون اینجا ایجاد می کنیم.
        //همون نمونه سازی از کلاس و سرویس آی رجیستر یوزر سرویس
        private readonly IRegisterUserService _registerUserService;
        //4
        //برای حذف کردن یوزور باید متد مربوطه را اینجا نمونه سازی کنیم تا بتونیم از توابع و متد هاش استفاده کنیم
        private readonly IRemoveUserService _removeUserService;
        //5
        // برای تغییر وضعیت فعال یا فعال نبودن یوز باید نمونه سازی متد مربوطه را اینجا انجام دهیم
        private readonly IUserstatusChenge _userstatusChenge;
        //6
        //برای ویرایش کاربران
        private readonly IEditeUser _editeUser;

        public UserController(IGetUserService getUserService, IGetRolesService getRolesService,
            IRegisterUserService registerUserService,
            IRemoveUserService removeUserService,
            IUserstatusChenge userstatusChenge,
            IEditeUser editeUser)
        {
            _getUserService = getUserService;
            _getRolesService = getRolesService;//پر کردن کامبوباکس مربوط به نقش ها
            _registerUserService = registerUserService;
            _removeUserService = removeUserService;
            _userstatusChenge = userstatusChenge;//تغییر وضعیت یوزر
        }


        
        //14
        //خب حالا به عنوان تابع ورودی ایندکس زیر میتونیم یدونه سرچ کی بگیریم و یدونه پیج
        public IActionResult Index(string serchkey,int page)
        {
            //خب حالا به ویوو خودمون اون کلاسی رو که برای ما خروجی لیست رو تولید میکرد رو اضافه میکنیم
            return View(_getUserService.Excute(new RequestGetUserDto
            {
                serchkey=serchkey,
                page=page,
            }
                ));
            //15
            //خب ما ی اشتباه کوچیک انجام دادیم تو سرویس یوزر،خروجی ما اونجا به صورت ی لیست هست 
            //که باید بریم خروجی رو از نوع ریزالت که براش کلاس ایجاد کردیم در نظر بگیریم
        }
        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        //اضافه کردن یک اکشن برای ثبت نام کاربر
       //16
       [HttpGet]
        public IActionResult Create()
        {
            //در زیر میخواهیم با ویووبگ دیتامون رو به کامبوباکس پاس بدیم

            ViewBag.Rols = new SelectList(_getRolesService.Excutee().Data, "Id", "Name");
            return View();
        }



        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        //ایجاد یک متد برای ثبت اطلاعات کاربر
      
        [HttpPost]
        public IActionResult Create(string fullname, string emailAddress, long RolId, string inputPassword, string inputRePassword)
        {

          
            var result = _registerUserService.Excute(new RequestRegisterUserDto
            {
                fullname = fullname,
                emailAddress = emailAddress,
                UserRolsss = new List<RolseInRegisterUserDto>()
              {
               new RolseInRegisterUserDto
               {
                   RolId=RolId
               }
              },
                inputPassword=inputPassword,
                inputRePassword = inputRePassword,
            });
            return Json(result);
        }

        //######################################################################################################################
       //اکشن دلت برای پاک کردن کاربر
       [HttpPost]
       public IActionResult Delete(long UserId)
        {
            return Json(_removeUserService.Execute(UserId) );
           

        }
        //######################################################################################################################
        //اکشن تغییر وضعیت دادن یوزر
        [HttpPost]
        public IActionResult ChengeuserStatus(long USID)
        {
            return Json(_userstatusChenge.Excute(USID));
        }
        //######################################################################################################################
        //برای ویرایش کاربران
        [HttpPost]
        public IActionResult Edite(long UserId,string Fullname)
        {
            return Json(_editeUser.Execute(new RequestEditeUserDto { FullName = Fullname, UserID = UserId ,}));
        }
    }
}
