using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using ProjectInfoSystem.Layouts.ProjectInfoSystem;

namespace ProjectInfoSystem.Layouts
{
   public class Utility
    {


        public static string Approve(SPWeb web, string comment, string listName, int itemId, int currentUserID)
    {
        string stImprove = "";
        Guid siteID = web.Site.ID;
        int creatorId = 0;
        int approver1Id = 0;
        int approver2Id = 0;
        int approver3Id = 0;
        string status = "";
        int nextUserId = 0;
        int canApprove = CanApprove(web, itemId, listName, out creatorId, out approver1Id, out approver2Id, out approver3Id);
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web1 = site.OpenWeb())
                {
                    SPListItem itemById = web1.GetList("/Lists/" + listName).GetItemById(itemId);
                    try
                    {
                        if ((canApprove == 1) && (currentUserID == approver1Id))
                        {
                            if (approver2Id != 0)
                            {
                                status = "در انتظار تایید";
                                nextUserId = approver2Id;
                            }
                            else
                            {
                                status = "پایان فرآیند";
                                nextUserId = 0;
                            }
                        }
                        else if ((canApprove == 2) && (currentUserID == approver2Id))
                        {
                            if (approver3Id != 0)
                            {
                                status = "در انتظار تایید";
                                nextUserId = approver3Id;
                            }
                            else
                            {
                                status = "پایان فرآیند";
                                nextUserId = 0;
                            }
                        }
                        else if ((canApprove == 3) && (currentUserID == approver3Id))
                        {
                            status = "پایان فرآیند";
                            nextUserId = 0;
                        }
                        else
                        {
                            stImprove = "شما دسترسی لازم را ندارید.";
                        }
                        if (stImprove == "")
                        {
                            web1.AllowUnsafeUpdates = true;
                            itemById["Status"] = status;
                            itemById["CurrentUser"] = (nextUserId != 0) ? new SPFieldUserValue(web, nextUserId, "") : null;
                            itemById.Update();
                            CreateHistory(web1, itemById.ID, DateTime.Now.ToString(), "تایید اطلاعات", comment, itemById.Url.Split(new char[] { '/' })[1], currentUserID);
                            if (nextUserId != 0)
                            {
                                string str = string.Concat(new object[] { "/_Layouts/15/ProjectInfoSystem/operation/Pages/EditForm.aspx?ListName=", listName, "&ID=", itemById.ID });
                            }
                            web1.AllowUnsafeUpdates = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        stImprove = exception.Message;
                    }
                }
            }
        });
        return stImprove;
    }

    public static int CanApprove(SPWeb web, int itemId, string listName, out int creatorId, out int approver1Id, out int approver2Id, out int approver3Id)
    {
        int iD = web.CurrentUser.ID;
        SPList list = web.GetList("/Lists/Contracts");
        SPList list2 = web.GetList("/Lists/FormPermissions");
        SPList list3 = web.GetList("/Lists/" + listName);
        SPList list4 = web.GetList("/Lists/" + listName + "Details");
        SPListItem itemById = list3.GetItemById(itemId);
        int lookupId = new SPFieldLookupValue(itemById["Contract"].ToString()).LookupId;
        int num3 = 0;
        string str2 = "";
        if (itemById["CurrentUser"] != null)
        {
            num3 = new SPFieldLookupValue(itemById["CurrentUser"].ToString()).LookupId;
        }
        if (itemById["Status"] != null)
        {
            str2 = itemById["Status"].ToString();
        }
        SPQuery query = new SPQuery {
            Query = string.Format(@"<Where>
                                    <Eq><FieldRef Name='ListName' />
                                       <Value Type='Text'>{0}</Value>
                                    </Eq>
                                   </Where>", listName)
        };
        SPListItem item2 = (list2.GetItems(query).Count > 0) ? list2.GetItems(query)[0] : null;
        creatorId = GetRelatedUser(new SPFieldLookupValue(item2["Creator"].ToString()).LookupId, lookupId);
        approver1Id = GetRelatedUser(new SPFieldLookupValue(item2["Approver1"].ToString()).LookupId, lookupId);
        approver2Id = (item2["Approver2"] != null) ? GetRelatedUser(new SPFieldLookupValue(item2["Approver2"].ToString()).LookupId, lookupId) : 0;
        approver3Id = (item2["Approver3"] != null) ? GetRelatedUser(new SPFieldLookupValue(item2["Approver3"].ToString()).LookupId, lookupId) : 0;
        if (itemById["CurrentUser"] != null)
        {
            if (((str2 == "در انتظار تایید") && (approver1Id == iD)) && (iD == num3))
            {
                return 1;
            }
            if ((((approver2Id != 0) && (str2 == "در انتظار تایید")) && (approver2Id == iD)) && (iD == num3))
            {
                return 2;
            }
            if ((((approver3Id != 0) && (str2 == "در انتظار تایید")) && (approver3Id == iD)) && (iD == num3))
            {
                return 3;
            }
        }
        return -1;
    }

    public static string CompleteTask(SPWeb web, int itemId, string listName, int userId)
    {
        string strError = "";
        int iD = web.CurrentUser.ID;
        Guid siteID = web.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    Web.AllowUnsafeUpdates = true;
                    string strUrl = "/Lists/WorkflowTasks";
                    SPList list = Web.GetList(strUrl);
                    SPQuery query = new SPQuery {
                        Query =string.Format(@"<Where>
                                                      <And>
                                                         <Eq>
                                                            <FieldRef Name='ItemId' />
                                                            <Value Type='Number'>{0}</Value>
                                                         </Eq>
                                                         <And>
                                                            <Eq>
                                                               <FieldRef Name='ListName' />
                                                               <Value Type='Text'>{1}</Value>
                                                            </Eq>
                                                            <And>
                                                               <Neq>
                                                                  <FieldRef Name='PercentComplete' />
                                                                  <Value Type='Number'>100</Value>
                                                               </Neq>
                                                               <Contains>
                                                                  <FieldRef Name='AssignedTo' LookupId='TRUE' />
                                                                  <Value Type='LookupMulti'>{2}</Value>
                                                               </Contains>
                                                            </And>
                                                         </And>
                                                      </And>
                                                   </Where>",itemId,listName,userId)
                    };
                    SPListItem item = list.GetItems(query)[0];
                    item["PercentComplete"] = 100;
                    try
                    {
                        item.Update();
                    }
                    catch (Exception exception)
                    {
                        strError = exception.Message;
                    }
                }
            }
        });
        return strError;
    }

    public static string CreateHistory(SPWeb weba, int itemID, string date, string eventString, string description, string listname, int userId)
    {
        string strError = "";
        int iD = weba.CurrentUser.ID;
        Guid siteID = weba.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    string strUrl = "/Lists/WorkflowHistories";
                    SPListItem item = web.GetList(strUrl).AddItem();
                    item["Title"] = eventString + date;
                    item["ListName"] = listname;
                    item["ItemID"] = itemID;
                    item["User"] = new SPFieldUserValue(web, userId, "");
                    item["Date"] = DateTime.Now;
                    item["Event"] = eventString;
                    item["Comment"] = description;
                    try
                    {
                        item.Update();
                    }
                    catch (Exception exception)
                    {
                        strError = exception.Message;
                    }
                }
            }
        });
        return strError;
    }

    public static string CreateTask(SPWeb web, string taskName, int assignToID, string description, string linkItem, string list, int itemid)
    {
        string strError = "";
        int iD = web.CurrentUser.ID;
        Guid siteID = web.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    string strUrl = "/Lists/WorkflowTasks";
                    SPListItem item = web.GetList(strUrl).AddItem();
                    item["Title"] = taskName;
                    item["StartDate"] = DateTime.Now;
                    item["ListName"] = list;
                    item["ItemId"] = itemid;
                    SPFieldUserValueCollection values = new SPFieldUserValueCollection();
                    values.Add(new SPFieldUserValue(Web, assignToID, ""));
                    item["AssignedTo"] = values;
                    SPFieldUrlValue value2 = new SPFieldUrlValue {
                        Description = description,
                        Url = linkItem
                    };
                    item["ItemLink"] = value2;
                    try
                    {
                        item.Update();
                        int num = 0;
                        foreach (SPFieldUserValue value3 in values)
                        {
                            if (num == 0)
                            {
                                SetListItemPermission(item, value3.LookupId, 0x40000002, true);
                            }
                            else
                            {
                                SetListItemPermission(item, value3.LookupId, 0x40000002, false);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        strError = exception.Message;
                    }
                }
            }
        });
        return strError;
    }

    public static List<HistoryDetail> GetHistory(int dailyItemID, string listName)
    {
        List<HistoryDetail> HistoryDetailList = new List<HistoryDetail>();
        SPWeb web = SPContext.Current.Web;
        string siteURL = web.Site.Url;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web1 = site.OpenWeb())
                {
                    web1.AllowUnsafeUpdates = true;
                    SPList list = web.GetList("/Lists/WorkflowHistories");
                    SPListItem itemById = web.GetList("/Lists/" + listName).GetItemById(dailyItemID);
                    SPQuery query = new SPQuery {
                        Query =string.Format(@"<Where>
                                                  <And>
                                                     <Eq>
                                                        <FieldRef Name='ItemID' />
                                                        <Value Type='Number'>{0}</Value>
                                                     </Eq>
                                                     <Eq>
                                                        <FieldRef Name='ListName' />
                                                        <Value Type='Text'>{1}</Value>
                                                     </Eq>
                                                  </And>
                                                  </Where>
                                                  <OrderBy>
                                                      <FieldRef Name='ID' Ascending='TRUE' />
                                                  </OrderBy>",dailyItemID,listName)
                    };
                    SPListItemCollection items = list.GetItems(query);
                    foreach (SPListItem item2 in items)
                    {
                        HistoryDetail item = new HistoryDetail {
                            HistoryID = item2.ID
                        };
                        try
                        {
                            item.UserName = new SPFieldUserValue(web, item2["User"].ToString()).User.Name;
                            item.ListName = item2["ListName"].ToString();
                            item.state = item2["Event"].ToString();
                            item.HistoryDate = DateTime.Parse(item2["Created"].ToString());
                            item.Description = Convert.ToString(item2["Comment"]).Replace("\"", "'");
                            HistoryDetailList.Add(item);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        });
        return HistoryDetailList;
    }

    public static int GetRelatedUser(int userLookupId, int contractId)
    {
        SPWeb web = SPContext.Current.Web;
        SPList list = web.GetList("/Lists/Contracts");
        SPList list2 = web.GetList("/Lists/Areas");
        SPList list3 = web.GetList("/Lists/ContractUsers");
        SPListItem contract = (contractId > 0) ? list.GetItemById(contractId) : null;
        SPListItem userItem = list3.GetItemById(userLookupId);
        if (userLookupId == 1)
        {
            return new SPFieldLookupValue(contract["ContractorUser"].ToString()).LookupId;
        }
        if (userLookupId == 2)
        {
            return new SPFieldLookupValue(contract["ConsultantUser"].ToString()).LookupId;
        }
        if (userLookupId == 4)
        {
            return new SPFieldLookupValue(contract["ManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 5)
        {
            return new SPFieldLookupValue(list2.GetItemById(new SPFieldLookupValue(contract["Area"].ToString()).LookupId)["AreaManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 9)
        {
            SPQuery query = new SPQuery
            {
                Query = string.Format("<Where>\r\n                                                          <Eq>\r\n                                                                <FieldRef Name='Company' LookupId='TRUE' />\r\n                                                                <Value Type='Lookup'>{0}</Value>\r\n                                                            </Eq>\r\n                                                        </Where>", contractId)
            };
            SPListItem item3 = list2.GetItems(query)[0];
            return new SPFieldLookupValue(item3["AreaManagerUser"].ToString()).LookupId;
        }
      
        return new SPFieldLookupValue(userItem["UserName"].ToString()).LookupId;

    }

    public static string Reject(SPWeb web, string comment, string listName, int itemId, int currentUserID)
    {
        string stReject = "";
        Guid siteID = web.Site.ID;
        int creatorId = 0;
        int approver1Id = 0;
        int approver2Id = 0;
        int num = 0;
        string status = "";
        int nextUserId = 0;
        int canApprove = CanApprove(web, itemId, listName, out creatorId, out approver1Id, out approver2Id, out num);
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web1 = site.OpenWeb())
                {
                    SPListItem item = web1.GetList("/Lists/" + listName).GetItemById(itemId);
                    try
                    {
                        if ((canApprove == 1) && (currentUserID == approver1Id))
                        {
                            status = "در انتظار ویرایش";
                            nextUserId = creatorId;
                            SetListItemPermission(item, nextUserId, 0x40000003, false);
                        }
                        else if (canApprove == 2)
                        {
                            status = "در انتظار تایید";
                            nextUserId = approver1Id;
                        }
                        else if (canApprove == 3)
                        {
                            status = "در انتظار تایید";
                            nextUserId = approver2Id;
                        }
                        else
                        {
                            stReject = "شما دسترسی لازم را ندارید.";
                        }
                        if (stReject == "")
                        {
                            string str;
                            web1.AllowUnsafeUpdates = true;
                            item["Status"] = status;
                            item["CurrentUser"] = (nextUserId != 0) ? new SPFieldUserValue(web, nextUserId, "") : null;
                            item.Update();
                            CreateHistory(web1, item.ID, DateTime.Now.ToString(), "رد اطلاعات", comment, item.Url.Split(new char[] { '/' })[1], currentUserID);
                            if (canApprove != 1)
                            {
                                str = string.Concat(new object[] { "/_Layouts/15/ProjectInfoSystem/Operation/Pages/DisplayForm.aspx?ListName=", listName, "&ID=", item.ID });
                            }
                            else
                            {
                                str = string.Concat(new object[] { "/_Layouts/15/ProjectInfoSystem/Operation/Pages/EditForm.aspx?ListName=", listName, "&ID=", item.ID });
                            }
                            web1.AllowUnsafeUpdates = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        stReject = exception.Message;
                    }
                }
            }
        });
        return stReject;
    }

    public static void ResetItemPermission(SPListItem item)
    {
        string siteURL = item.ParentList.ParentWeb.Url;
        Guid listId = item.ParentList.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItem itemById = web.Lists[listId].GetItemById(item.ID);
                    web.AllowUnsafeUpdates = true;
                    itemById.ResetRoleInheritance();
                    web.AllowUnsafeUpdates = false;
                }
            }
        });
    }

    public static string SetListItemPermission(SPListItem Item, int userId, int PermissionID, bool ClearPreviousPermissions)
    {
        string strError = "";
        string siteURL = Item.ParentList.ParentWeb.Url;
        Guid listId = Item.ParentList.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPPrincipal byID;
                    Exception exception;
                    web.AllowUnsafeUpdates = true;
                    SPListItem itemById = web.Lists[listId].GetItemById(Item.ID);
                    if (!itemById.HasUniqueRoleAssignments)
                    {
                        itemById.BreakRoleInheritance(!ClearPreviousPermissions);
                    }
                    try
                    {
                        byID = web.SiteUsers.GetByID(userId);
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        byID = web.SiteGroups.GetByID(userId);
                    }
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(byID);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetById(PermissionID);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                    itemById.RoleAssignments.Remove(byID);
                    itemById.RoleAssignments.Add(roleAssignment);
                    try
                    {
                        itemById.SystemUpdate(false);
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        strError = exception.Message;
                    }
                }
            }
        });
        return strError;
    }


    }
}
