using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Globalization;

namespace EvaluationSystem.Layouts.EvaluationSystem
{
    public static class Utility
    {
         public static string Approve(string comment, int itemId, string listName, int currentUserID, bool isCompany)
    {
        string stImprove = "";
        Guid siteID = SPContext.Current.Site.ID;
        int creatorId = 0;
        int approver1Id = 0;
        int approver2Id = 0;
        int approver3Id = 0;
        string status = "";
        int nextUserId = 0;
        int canApprove = CanApprove(itemId, listName, isCompany, out creatorId, out approver1Id, out approver2Id, out approver3Id);
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItem itemById = web.GetList("/Lists/" + listName).GetItemById(itemId);
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
                            web.AllowUnsafeUpdates = true;
                            itemById["Status"] = status;
                            itemById["CurrentUser"] = (nextUserId != 0) ? new SPFieldUserValue(web, nextUserId, "") : null;
                            itemById.Update();
                            CreateHistory(web, itemById.ID, DateTime.Now.ToString(), "تایید اطلاعات", comment, itemById.Url.Split(new char[] { '/' })[1]);
                            if (nextUserId != 0)
                            {
                                string str = string.Concat(new object[] { "/_Layouts/15/EvaluationSystem/Pages/EditForm.aspx?ListName=", listName, "&ID=", itemById.ID });
                            }
                            web.AllowUnsafeUpdates = false;
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

    public static int CanApprove(int itemId, string listName, bool isCompany, out int creatorId, out int approver1Id, out int approver2Id, out int approver3Id)
    {
        SPWeb web = SPContext.Current.Web;
        int iD = web.CurrentUser.ID;
        SPList list = isCompany ? web.GetList("/Lists/Companies") : web.GetList("/Lists/Contracts");
        SPList list2 = web.GetList("/Lists/FormPermissions");
        SPList list3 = web.GetList("/Lists/" + listName);
        SPList list4 = web.GetList("/Lists/" + listName + "Details");
        SPList list5 = web.GetList("/Lists/EvaluationTypes");
        SPListItem itemById = list3.GetItemById(itemId);
        int contractId = isCompany ? new SPFieldLookupValue(itemById["CompanyNasr"].ToString()).LookupId : new SPFieldLookupValue(itemById["Contract"].ToString()).LookupId;
        int lookupId = 0;
        string str2 = "";
        if (itemById["CurrentUser"] != null)
        {
            lookupId = new SPFieldLookupValue(itemById["CurrentUser"].ToString()).LookupId;
        }
        if (itemById["Status"] != null)
        {
            str2 = itemById["Status"].ToString();
        }
        string str3 = SelectEvaluationTypeFromList(listName);
        SPQuery query = new SPQuery {
            Query =string.Format(@"<Where>
                                          <Eq>
                                             <FieldRef Name='ListName' />
                                             <Value Type='Text'>{0}</Value>
                                          </Eq>
                                       </Where>",listName)
        };
        SPListItem item2 = (list2.GetItems(query).Count > 0) ? list2.GetItems(query)[0] : null;
        creatorId = GetRelatedUser(new SPFieldLookupValue(item2["Creator"].ToString()).LookupId, contractId, isCompany);
        approver1Id = GetRelatedUser(new SPFieldLookupValue(item2["Approver1"].ToString()).LookupId, contractId, isCompany);
        approver2Id = (item2["Approver2"] != null) ? GetRelatedUser(new SPFieldLookupValue(item2["Approver2"].ToString()).LookupId, contractId, isCompany) : 0;
        approver3Id = (item2["Approver3"] != null) ? GetRelatedUser(new SPFieldLookupValue(item2["Approver3"].ToString()).LookupId, contractId, isCompany) : 0;
        if (itemById["CurrentUser"] != null)
        {
            if (((str2 == "در انتظار تایید") && (approver1Id == iD)) && (iD == lookupId))
            {
                return 1;
            }
            if ((((approver2Id != 0) && (str2 == "در انتظار تایید")) && (approver2Id == iD)) && (iD == lookupId))
            {
                return 2;
            }
            if ((((approver3Id != 0) && (str2 == "در انتظار تایید")) && (approver3Id == iD)) && (iD == lookupId))
            {
                return 3;
            }
        }
        return -1;
    }

    public static string CompleteTask(int itemId, string listName, int userId)
    {
        string strError = "";
        int iD = SPContext.Current.Web.CurrentUser.ID;
        Guid siteID = SPContext.Current.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    string strUrl = "/Lists/WorkflowTasks";
                    SPList list = web.GetList(strUrl);
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

    public static string CreateEvaluationContract(int contractId, string status, int userId, int periodId, string evalTypeTitle, bool isCompany)
    {
        string str = "";
        SPWeb web = SPContext.Current.Web;
        SPList list = web.GetList("/Lists/" + SelectEvaluationList(evalTypeTitle));
        SPList list2 = web.GetList("/Lists/EvaluationTypes");
        SPList list3 = web.GetList("/Lists/Contracts");
        SPList list4 = web.GetList("/Lists/Companies");
        SPListItem itemById = web.GetList("/Lists/Periods").GetItemById(periodId);
        SPListItem item2 = list.AddItem();
        item2["Title"] = evalTypeTitle + " " + (isCompany ? list4.GetItemById(contractId)["Title"].ToString() : list3.GetItemById(contractId)["Title"].ToString()) + " " + itemById["Title"].ToString();
        item2["Status"] = status;
        item2["CurrentUser"] = new SPFieldUserValue(web, userId.ToString());
        item2["Period"] = new SPFieldLookupValue(periodId, "");
        if (isCompany)
        {
            item2["CompanyNasr"] = new SPFieldLookupValue(contractId, "");
        }
        else
        {
            item2["Contract"] = new SPFieldLookupValue(contractId, "");
        }
        try
        {
            web.AllowUnsafeUpdates = true;
            item2.Update();
            str = item2.ID.ToString();
            web.AllowUnsafeUpdates = false;
        }
        catch (Exception exception)
        {
            str = "خطا در ذخیره سازی ایتم با شناسه" + item2.ID + exception.Message;
        }
        return str;
    }

    public static string CreateEvaluationContractItem(int parentId, string evalTypeTitle, bool isCompany, int contractId, int creatorId, int approver1Id, int approver2Id, int approver3Id, List<int> viewerIds, List<int> viewerContractIds, List<int> editorsIds)
    {
        string str = "";
        SPWeb web = SPContext.Current.Web;
        string str2 = SelectEvaluationList(evalTypeTitle);
        SPList list = web.GetList("/Lists/" + str2 + "Details");
        SPList list2 = web.GetList("/Lists/" + str2);
        SPList list3 = web.GetList("/Lists/Criterions");
        SPList list4 = web.GetList("/Lists/Indexes");
        SPFolder folder = web.GetFolder("/Lists/" + str2 + "Details/" + contractId.ToString());
        if (!folder.Exists)
        {
            SPListItem item = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
            item["Title"] = contractId.ToString();
            web.AllowUnsafeUpdates = true;
            item.Update();
            folder = web.GetFolder("/Lists/" + str2 + "Details/" + contractId.ToString());
            int iD = web.Groups["تیم راهبری"].ID;
            int userId = web.Groups["تیم راهبری-ویرایش"].ID;
            SetListItemPermission(folder.Item, creatorId, 0x40000002, true);
            SetListItemPermission(folder.Item, iD, 0x40000002, false);
            SetListItemPermission(folder.Item, userId, 0x40000003, false);
            foreach (int num3 in viewerIds)
            {
                SetListItemPermission(folder.Item, num3, 0x40000002, false);
            }
            foreach (int num3 in viewerContractIds)
            {
                SetListItemPermission(folder.Item, num3, 0x40000002, false);
            }
            foreach (int num4 in editorsIds)
            {
                SetListItemPermission(folder.Item, num4, 0x40000002, false);
            }
        }
        SPListItem itemById = list2.GetItemById(parentId);
        SPQuery query = new SPQuery {
            Query = string.Format(@"<Where>
                                          <Eq>
                                             <FieldRef Name='EvaluationType' />
                                             <Value Type='Text'>{0}</Value>
                                          </Eq>
                                       </Where>",evalTypeTitle)
        };
        SPListItemCollection items = list3.GetItems(query);
        int[] source = new int[items.Count];
        int num5 = 0;
        foreach (SPListItem item3 in items)
        {
            source[num5++] = item3.ID;
        }
        string str3 = "<Where><In><FieldRef Name='Criterion'  LookupId='TRUE'/><Values>";
        for (int i = 0; i < source.Count<int>(); i++)
        {
            object obj2 = str3;
            str3 = string.Concat(new object[] { obj2, "<Value Type='Lookup'>", source[i], "</Value>" });
        }
        str3 = str3 + "</Values></In></Where>";
        SPQuery query2 = new SPQuery {
            Query = str3
        };
        SPListItemCollection items2 = list4.GetItems(query2);
        foreach (SPListItem item4 in items2)
        {
            SPListItem item5 = list.AddItem(folder.ServerRelativeUrl, SPFileSystemObjectType.File);
            SPListItem item6 = list3.GetItemById(new SPFieldLookupValue(item4["Criterion"].ToString()).LookupId);
            if (isCompany)
            {
                item5["EvaluationCompany"] = new SPFieldLookupValue(parentId, "");
            }
            else
            {
                item5["EvaluationContract"] = new SPFieldLookupValue(parentId, "");
            }
            item5["Criterion"] = new SPFieldLookupValue(item4["Criterion"].ToString()).LookupValue;
            item5["Index"] = item4["Title"].ToString();
            item5["Weight"] = decimal.Parse(item4["Weight"].ToString());
            item5["Org_Weight"] = decimal.Parse(item4["Weight"].ToString());
            item5["Row"] = item6["Row"].ToString() + "-" + item4["Row"].ToString();
            item5["Score"] = 0;
            try
            {
                web.AllowUnsafeUpdates = true;
                item5.Update();
                str = item5.ID.ToString();
                web.AllowUnsafeUpdates = false;
            }
            catch (Exception exception)
            {
                return (exception.Message + " خطا در ذخیره سازی آیتم در لیست " + list.Title);
            }
        }
        return str;
    }

    public static string CreateHistory(SPWeb weba, int itemID, string date, string eventString, string description, string listname)
    {
        string strError = "";
        int currentUserID = SPContext.Current.Web.CurrentUser.ID;
        Guid siteID = SPContext.Current.Site.ID;
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
                    item["User"] = new SPFieldUserValue(web, currentUserID, "");
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

    public static string CreateTask(string taskName, int assignToID, string description, string linkItem, string list, int itemid)
    {
        string strError = "";
        int iD = SPContext.Current.Web.CurrentUser.ID;
        Guid siteID = SPContext.Current.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    string strUrl = "/Lists/WorkflowTasks";
                    SPListItem item = web.GetList(strUrl).AddItem();
                    item["Title"] = taskName;
                    item["StartDate"] = DateTime.Now;
                    item["ListName"] = list;
                    item["ItemId"] = itemid;
                    SPFieldUserValueCollection values = new SPFieldUserValueCollection();
                    values.Add(new SPFieldUserValue(web, assignToID, ""));
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

    public static List<IndexItem> GetEvaluationContractDetails(int evalContractId, string listName, bool isCompany)
    {
        List<IndexItem> list = new List<IndexItem>();
        SPList list2 = SPContext.Current.Web.GetList("/Lists/" + listName + "Details");
        SPQuery query = new SPQuery {
            ViewAttributes = "Scope=\"Recursive\""
        };
        if (isCompany)
        {
            query.Query =string.Format(@"<Where>
                                        <Eq>
                                            <FieldRef Name='EvaluationCompany' LookupId='TRUE' />
                                            <Value Type='Lookup'>{0}</Value>
                                        </Eq>
                                    </Where>
                                        <OrderBy>
                                            <FieldRef Name='Row' Ascending='True' />
                                        </OrderBy>",evalContractId);
        }
        else
        {
            query.Query =string.Format(@"<Where>
                                        <Eq>
                                            <FieldRef Name='EvaluationContract' LookupId='TRUE' />
                                            <Value Type='Lookup'>{0}</Value>
                                        </Eq>
                                    </Where>
                                        <OrderBy>
                                            <FieldRef Name='Row' Ascending='True' />
                                        </OrderBy>",evalContractId);
        }
        SPListItemCollection items = list2.GetItems(query);
        foreach (SPListItem item in items)
        {
            IndexItem item2 = new IndexItem {
                weight = decimal.Parse(item["Weight"].ToString()),
                org_weight = decimal.Parse(item["Org_Weight"].ToString()),
                id = item.ID,
                title = (item["Title"] != null) ? item["Title"].ToString() : "",
                criterion = item["Criterion"].ToString(),
                index = item["Index"].ToString(),
                isRelated = Convert.ToBoolean(item["IsRelevent"]),
                score = (item["Score"] != null) ? decimal.Parse(item["Score"].ToString()) : 0M,
                order = item["Row"].ToString()
            };
            list.Add(item2);
        }
        return list;
    }

    public static List<HistoryDetail> GetHistory(SPWeb web, int dailyItemID, string listName)
    {
        List<HistoryDetail> HistoryDetailList = new List<HistoryDetail>();
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
                        Query = string.Format(@"<Where>
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

    public static int GetRelatedUser(int userLookupId, int contractId, bool isCompany)
    {
        SPWeb web = SPContext.Current.Web;
        SPList list = web.GetList("/Lists/Contracts");
        SPList list2 = web.GetList("/Lists/Areas");
        SPList list3 = web.GetList("/Lists/ContractUsers");
        SPListItem item = !isCompany ? list.GetItemById(contractId) : null;
        SPListItem itemById = list3.GetItemById(userLookupId);
        if (userLookupId == 1)
        {
            return new SPFieldLookupValue(item["ContractorUser"].ToString()).LookupId;
        }
        if (userLookupId == 2)
        {
            return new SPFieldLookupValue(item["ConsultantUser"].ToString()).LookupId;
        }
        if (userLookupId == 4)
        {
            return new SPFieldLookupValue(item["ManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 5)
        {
            return new SPFieldLookupValue(list2.GetItemById(new SPFieldLookupValue(item["Area"].ToString()).LookupId)["AreaManagerUser"].ToString()).LookupId;
        }
        if (userLookupId == 9)
        {
            SPQuery query = new SPQuery {
                Query = string.Format(@"<Where>
                                                          <Eq>
                                                                <FieldRef Name='Company' LookupId='TRUE' />
                                                                <Value Type='Lookup'>{0}</Value>
                                                            </Eq>
                                                        </Where>",contractId)
            };
            SPListItem item3 = list2.GetItems(query)[0];
            return new SPFieldLookupValue(item3["AreaManagerUser"].ToString()).LookupId;
        }
        return new SPFieldLookupValue(itemById["UserName"].ToString()).LookupId;
    }

    public static void GetUsersContract(int contractId, out int contractorId, out int consultentId, out int managerId, out int domainManagerId)
    {
        SPListItem itemById = SPContext.Current.Web.GetList("/Lists/Contracts").GetItemById(contractId);
        contractorId = new SPFieldLookupValue(itemById["ContractorUser"].ToString()).LookupId;
        consultentId = new SPFieldLookupValue(itemById["ConsultentUser"].ToString()).LookupId;
        managerId = new SPFieldLookupValue(itemById["ManagerUser"].ToString()).LookupId;
        domainManagerId = new SPFieldLookupValue(itemById["DomainManagerUser"].ToString()).LookupId;
    }

    public static string Reject(string comment, int itemId, string listName, int currentUserID, bool isCompany)
    {
        string stReject = "";
        Guid siteID = SPContext.Current.Site.ID;
        int creatorId = 0;
        int approver1Id = 0;
        int approver2Id = 0;
        int num = 0;
        string status = "";
        int nextUserId = 0;
        int canApprove = CanApprove(itemId, listName, isCompany, out creatorId, out approver1Id, out approver2Id, out num);
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItem item = web.GetList("/Lists/" + listName).GetItemById(itemId);
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
                            web.AllowUnsafeUpdates = true;
                            item["Status"] = status;
                            item["CurrentUser"] = (nextUserId != 0) ? new SPFieldUserValue(web, nextUserId, "") : null;
                            item.Update();
                            CreateHistory(web, item.ID, DateTime.Now.ToString(), "رد اطلاعات", comment, item.Url.Split(new char[] { '/' })[1]);
                            if (canApprove != 1)
                            {
                                str = string.Concat(new object[] { "/_Layouts/15/EvaluationSystem/Pages/DisplayForm.aspx?ListName=", listName, "&ID=", item.ID });
                            }
                            else
                            {
                                str = string.Concat(new object[] { "/_Layouts/15/EvaluationSystem/Pages/EditForm.aspx?ListName=", listName, "&ID=", item.ID });
                            }
                            web.AllowUnsafeUpdates = false;
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

    public static string SelectEvaluationList(string type)
    {
        string siteURL = SPContext.Current.Site.Url;
        SPList list = SPContext.Current.Web.GetList("/Lists/FormPermissions");
        string listName = "";
        Guid listId = list.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList List = web.Lists[listId];
                    SPQuery query = new SPQuery {
                        Query =string.Format(@"<Where>
                                                      <Eq>
                                                         <FieldRef Name='Title' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                   </Where>",type)
                    };
                    SPListItem item = List.GetItems(query)[0];
                    listName = item["ListName"].ToString();
                }
            }
        });
        return listName;
    }

    public static string SelectEvaluationTypeFromList(string listName)
    {
        string siteURL = SPContext.Current.Site.Url;
        SPList list = SPContext.Current.Web.GetList("/Lists/FormPermissions");
        string formTitle = "";
        Guid listId = list.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList List = web.Lists[listId];
                    SPQuery query = new SPQuery {
                        Query = string.Format(@"<Where>
                                                      <Eq>
                                                         <FieldRef Name='ListName' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                   </Where>",listName)
                    };
                    SPListItem item = List.GetItems(query)[0];
                    formTitle = item["Title"].ToString();
                }
            }
        });
        return formTitle;
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

    public static string UpadetDetailScores(List<IndexItem> indexes, SPList detailList)
    {
        string strError = "";
        string siteURL = detailList.ParentWeb.Url;
        Guid listId = detailList.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    SPList list = web.Lists[listId];
                    foreach (IndexItem item in indexes)
                    {
                        SPListItem itemById = list.GetItemById(item.id);
                        itemById["Score"] = item.score;
                        itemById["Weight"] = item.weight;
                        itemById["IsRelevent"] = item.isRelated;
                        try
                        {
                            itemById.Update();
                        }
                        catch (Exception exception)
                        {
                            strError = "خطا در ذخیره سازی " + exception.Message;
                            return;
                        }
                    }
                }
            }
        });
        return strError;
    }

    public static string UpdateStatus(SPList parentList, int evalContractId, decimal? totalScore, string status, int userId)
    {
        string strError = "";
        string siteURL = parentList.ParentWeb.Url;
        Guid listId = parentList.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    SPListItem itemById = web.Lists[listId].GetItemById(evalContractId);
                    if (totalScore.HasValue)
                    {
                        itemById["TotalScore"] = totalScore;
                    }
                    if (status != "")
                    {
                        itemById["Status"] = status;
                    }
                    if (userId != 0)
                    {
                        itemById["CurrentUser"] = new SPFieldUserValue(web, userId, "");
                    }
                    try
                    {
                        itemById.Update();
                    }
                    catch (Exception exception)
                    {
                        strError = "خطا در UpdateStatus " + exception.Message;
                    }
                }
            }
        });
        return strError;
    }


    }
}
      