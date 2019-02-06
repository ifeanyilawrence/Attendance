using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Areas.Admin.Models;
using Attendance.Web.Controllers;

namespace Attendance.Web.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private MenuViewModel viewModel;
        public ActionResult AddMenu()
        {
            try
            {
                viewModel = new MenuViewModel();
                PopulateAllDropDown(viewModel);
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddMenu(MenuViewModel viewModel)
        {
            try
            {
                if (viewModel != null)
                {
                    MenuLogic menuLogic = new MenuLogic();

                    List<MENU> MenuList = menuLogic.GetEntitiesBy(m => m.Display_Name == viewModel.Menu.Action && m.Controller == viewModel.Menu.Controller && m.Menu_Group_Id == viewModel.MenuGroup.Menu_Group_Id);
                    if (MenuList.Count > 0)
                    {
                        SetMessage("This Menu has already been added to this menuGroup!", Message.Category.Error);
                        RetainDropDown(viewModel);
                        return View(viewModel);
                    }

                    viewModel.Menu.Activated = true;
                    viewModel.Menu.Menu_Group_Id = viewModel.MenuGroup.Menu_Group_Id;
                    menuLogic.Create(viewModel.Menu);

                    SetMessage("Operation Successful! ", Message.Category.Information);
                    return RedirectToAction("AddMenu");
                }

            }
            catch (Exception ex)
            {
                SetMessage("Error!" + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }

        public ActionResult ViewMenuByMenuGroup()
        {
            try
            {
                viewModel = new MenuViewModel();
                PopulateAllDropDown(viewModel);
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ViewMenuByMenuGroup(MenuViewModel viewModel)
        {
            try
            {
                if (viewModel.MenuGroup != null && viewModel.MenuGroup.Menu_Group_Id > 0)
                {
                    MenuLogic menuLogic = new MenuLogic();
                    viewModel.MenuList = menuLogic.GetEntitiesBy(m => m.Menu_Group_Id == viewModel.MenuGroup.Menu_Group_Id);

                    RetainDropDown(viewModel);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }
        public ActionResult EditMenu(int mid)
        {
            try
            {
                viewModel = new MenuViewModel();
                if (mid > 0)
                {
                    MenuLogic menuLogic = new MenuLogic();
                    viewModel.Menu = menuLogic.GetEntityBy(x => x.Menu_Id == mid);
                    if (viewModel.Menu != null)
                    {
                        viewModel.MenuGroup = viewModel.Menu.MENU_GROUP;
                    }

                    RetainDropDown(viewModel);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditMenu(MenuViewModel viewModel)
        {
            try
            {
                MenuLogic menuLogic = new MenuLogic();
                viewModel.Menu.MENU_GROUP = viewModel.MenuGroup;
                bool isUpdated = menuLogic.Modify(viewModel.Menu);

                if (isUpdated == false)
                {
                    SetMessage("Edit Unsuccessful! ", Message.Category.Error);
                    return RedirectToAction("EditMenu", new { mid = viewModel.Menu.Menu_Id });
                }

                SetMessage("Operation Successful!", Message.Category.Information);
                return RedirectToAction("ViewMenuByMenuGroup");
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View("ViewMenuByMenuGroup", viewModel);
        }

        public ActionResult ConfirmDeleteMenu(int mid)
        {
            try
            {
                viewModel = new MenuViewModel();
                if (mid > 0)
                {
                    MenuLogic menuLogic = new MenuLogic();
                    viewModel.Menu = menuLogic.GetEntityBy(x => x.Menu_Id == mid);
                    if (viewModel.Menu != null)
                    {
                        viewModel.MenuGroup = viewModel.Menu.MENU_GROUP;
                    }

                    RetainDropDown(viewModel);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteMenu(MenuViewModel viewModel)
        {
            try
            {
                MenuLogic menuLogic = new MenuLogic();
                menuLogic.Delete(x => x.Menu_Id == viewModel.Menu.Menu_Id);

                SetMessage("Operation Successful!", Message.Category.Information);
                return RedirectToAction("ViewMenuByMenuGroup");

            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return RedirectToAction("ConfirmDeleteMenu", new { mid = viewModel.Menu.Menu_Id });
        }

        public void PopulateAllDropDown(MenuViewModel viewModel)
        {
            try
            {
                ViewBag.MenuGroup = viewModel.MenuGroupSelectList;
                ViewBag.Menu = viewModel.MenuSelectList;
                ViewBag.Role = viewModel.RoleSelectList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RetainDropDown(MenuViewModel viewModel)
        {
            try
            {
                if (viewModel.MenuGroup != null)
                {
                    ViewBag.MenuGroup = new SelectList(viewModel.MenuGroupSelectList, "Value", "Text", viewModel.MenuGroup.Menu_Group_Id);
                }
                else
                {
                    ViewBag.MenuGroup = viewModel.MenuGroupSelectList;
                }
                if (viewModel.Menu != null)
                {
                    ViewBag.Menu = new SelectList(viewModel.MenuSelectList, "Value", "Text", viewModel.Menu.Menu_Id);
                }
                else
                {
                    ViewBag.Menu = viewModel.MenuSelectList;
                }
                if (viewModel.Role != null)
                {
                    ViewBag.Role = new SelectList(viewModel.RoleSelectList, "Value", "Text", viewModel.Role.Id);
                }
                else
                {
                    ViewBag.Role = viewModel.RoleSelectList;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AddMenuInRole()
        {
            try
            {
                viewModel = new MenuViewModel();
                PopulateAllDropDown(viewModel);
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddMenuInRole(MenuViewModel viewModel)
        {
            try
            {
                if (viewModel != null)
                {
                    MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();

                    List<MENU_IN_ROLE> menuInRoleList = menuInRoleLogic.GetEntitiesBy(m => m.Menu_Id == viewModel.Menu.Menu_Id && m.Role_Id == viewModel.Role.Id);
                    if (menuInRoleList.Count > 0)
                    {
                        SetMessage("This Menu has already been added to this Role!", Message.Category.Error);
                        RetainDropDown(viewModel);
                        return View(viewModel);
                    }

                    viewModel.MenuInRole = new MENU_IN_ROLE();
                    viewModel.MenuInRole.Activated = true;
                    viewModel.MenuInRole.Menu_Id = viewModel.Menu.Menu_Id;
                    viewModel.MenuInRole.Role_Id = viewModel.Role.Id;
                    menuInRoleLogic.Create(viewModel.MenuInRole);

                    SetMessage("Operation Successful! ", Message.Category.Information);
                    return RedirectToAction("AddMenuInRole");
                }

            }
            catch (Exception ex)
            {
                SetMessage("Error!" + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }

        public ActionResult ViewMenuInRole()
        {
            try
            {
                viewModel = new MenuViewModel();
                PopulateAllDropDown(viewModel);
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ViewMenuInRole(MenuViewModel viewModel)
        {
            try
            {
                if (viewModel.Role != null && viewModel.Role.Id > 0)
                {
                    MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();
                    viewModel.MenuInRoleList = menuInRoleLogic.GetEntitiesBy(m => m.Role_Id == viewModel.Role.Id);

                    RetainDropDown(viewModel);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }
        public ActionResult EditMenuInRole(int mid)
        {
            try
            {
                viewModel = new MenuViewModel();
                if (mid > 0)
                {
                    MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();
                    viewModel.MenuInRole = menuInRoleLogic.GetEntityBy(x => x.Menu_In_Role_Id == mid);
                    if (viewModel.MenuInRole != null)
                    {
                        viewModel.Menu = viewModel.MenuInRole.MENU;
                        viewModel.Role = viewModel.MenuInRole.ROLE;
                    }

                    RetainDropDown(viewModel);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditMenuInRole(MenuViewModel viewModel)
        {
            try
            {
                MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();
                viewModel.MenuInRole.MENU = viewModel.Menu;
                viewModel.MenuInRole.ROLE = viewModel.Role;
                bool isUpdated = menuInRoleLogic.Modify(viewModel.MenuInRole);

                if (isUpdated == false)
                {
                    SetMessage("Edit Unsuccessful! ", Message.Category.Error);
                    return RedirectToAction("EditMenuInRole", new { mid = viewModel.Menu.Menu_Id });
                }

                SetMessage("Operation Successful!", Message.Category.Information);
                return RedirectToAction("ViewMenuInRole");
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View("ViewMenuInRole", viewModel);
        }

        public ActionResult ConfirmDeleteMenuInRole(int mid)
        {
            try
            {
                viewModel = new MenuViewModel();
                if (mid > 0)
                {
                    MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();
                    viewModel.MenuInRole = menuInRoleLogic.GetEntityBy(x => x.Menu_In_Role_Id == mid);
                    if (viewModel.MenuInRole != null)
                    {
                        viewModel.Menu = viewModel.MenuInRole.MENU;
                        viewModel.Role = viewModel.MenuInRole.ROLE;
                    }

                    RetainDropDown(viewModel);
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            RetainDropDown(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteMenuInRole(MenuViewModel viewModel)
        {
            try
            {
                MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();
                menuInRoleLogic.Delete(x => x.Menu_In_Role_Id == viewModel.MenuInRole.Menu_In_Role_Id);

                SetMessage("Operation Successful!", Message.Category.Information);
                return RedirectToAction("ViewMenuInRole");

            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return RedirectToAction("ConfirmDeleteMenuInRole", new { mid = viewModel.Menu.Menu_Id });
        }

    }
}