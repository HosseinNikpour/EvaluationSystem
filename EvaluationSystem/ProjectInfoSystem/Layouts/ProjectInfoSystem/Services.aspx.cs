using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Web.Hosting.Administration;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Globalization;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;
using System.Threading;
using System.Text;


namespace ProjectInfoSystem.Layouts.ProjectInfoSystem
{
    public partial class Services : LayoutsPageBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {


        }

       // [WebMethod]
        //public static object GetContractInfoItems()
        //{
            
        //    List<OperationItem> operations = new List<OperationItem>();
        //    string _siteUrl = SPContext.Current.Site.Url + "/ProjectsInfo";
        //    SPSite siteColl = new SPSite(_siteUrl);
        //    SPWeb web  = siteColl.OpenWeb();
        //    SPList list = web.GetList("/ProjectsInfo/Lists/ContractExecutiveOperation");
        //    SPListItemCollection col = list.GetItems();
        //    foreach (SPListItem item in col)
        //    {

        //       OperationItem oi = new OperationItem();
             
               
        //        oi.title =item["Title"]!=null? item["Title"].ToString():"";
        //        oi.id = item.ID;
        //        oi.operateTitle=new SPFieldLookupValue(item["ExecutiveOperation"].ToString()).LookupValue;
        //        oi.operateId=new SPFieldLookupValue(item["ExecutiveOperation"].ToString()).LookupId;
        //        oi.itemId = new SPFieldLookupValue(item["SubExecutiveOperation"].ToString()).LookupId;
        //        oi.itemTitle =new SPFieldLookupValue(item["SubExecutiveOperation"].ToString()).LookupValue;
        //        oi.unit = item["Measurement"].ToString();
        //        oi.value=item["OrgValue"]!=null?decimal.Parse(item["OrgValue"].ToString()):0;
        //        oi.variableValue=item["ChangeValue"]!=null?decimal.Parse(item["ChangeValue"].ToString()):0;
        //        oi.price= item["Amount"]!=null?decimal.Parse(item["Amount"].ToString()):0;
        //        oi.changePrice=item["ChangeAmount"]!=null?decimal.Parse(item["ChangeAmount"].ToString()):0;
        //        oi.contractItemWeight =item["TotalWeightContract"]!=null?decimal.Parse(item["TotalWeightContract"].ToString()):0;
        //        oi.taskItemWeight =item["ItemWeightAction"]!=null?decimal.Parse(item["ItemWeightAction"].ToString()):0;
        //        oi.contractOperateWeight = item["TotalWeightOperation"]!=null?decimal.Parse(item["TotalWeightOperation"].ToString()):0;
        //        oi.taskOperateWeight =item["WeightAction"]!=null?decimal.Parse(item["WeightAction"].ToString()):0;
        //        oi.hektar =item["EqHectar"]!=null?decimal.Parse(item["EqHectar"].ToString()):0;
        //        oi.contract = new SPFieldLookupValue(item["Contract"].ToString()).LookupValue; 
        //        oi.contractId=new SPFieldLookupValue(item["Contract"].ToString()).LookupId;
        //        oi.block = new SPFieldLookupValue(item["Block"].ToString()).LookupValue;
        //        oi.blockId=new SPFieldLookupValue(item["Block"].ToString()).LookupId;
        //        oi.farmUnit = new SPFieldLookupValue(item["Farm"].ToString()).LookupValue;
        //        oi.farmUnitId =new SPFieldLookupValue(item["Farm"].ToString()).LookupId;
        //        operations.Add(oi);
        //    }
        //    return new {operations=operations};
        //}

        //[WebMethod]
        //public static string SaveItems(List<OperationItemUpdate> items,List<OperationItemUpdate> contractItems)
        //{
        //    string strError = "";
        //  strError=  SaveFarmOperations(items);
        //  if (strError == "")
        //  {
        //      strError = SaveContractOperations(contractItems);
        //  }
          
        //    return strError;
        //}

        //private static string SaveFarmOperations(List<OperationItemUpdate> items)
        //{
        //    string strError = "";
        //    StringBuilder methodBuilder = new StringBuilder();
           
        //    string batch = string.Empty;
           
        //    string newValue = "mmmm";
        //    string updateColumn = "SampleColumn";
        //    try
        //    {
        //        string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
        //                                "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
        //        string methodFormat = "<Method ID='{0}' >" +
        //                                "<SetList>{1}</SetList>" +
        //                                "<SetVar Name='Cmd'>Save</SetVar>" +
        //                                "<SetVar Name='ID'>{2}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#ChangeAmount'>{3}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#TotalWeightContract'>{4}</SetVar>" +
        //                                 "<SetVar Name='urn:schemas-microsoft-com:office:office#ItemWeightAction'>{5}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#TotalWeightOperation'>{6}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#WeightAction'>{7}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#EqHectar'>{8}</SetVar>" +
                                        
        //                                "</Method>";
        //        string _siteUrl = SPContext.Current.Site.Url + "/ProjectsInfo";
        //        SPSite siteColl = new SPSite(_siteUrl);
        //        SPWeb web = siteColl.OpenWeb();
        //        web.AllowUnsafeUpdates = true;
        //        SPList list = web.GetList("/ProjectsInfo/Lists/ContractExecutiveOperation");
                
        //        string listGuid = list.ID.ToString();
        //        // Build the CAML update commands.
        //        for (int i = 0; i < items.Count; i++)
        //        {
        //            int itemID = items[i].Id;

        //            methodBuilder.AppendFormat(methodFormat,itemID, listGuid, itemID,items[i].ChangeAmount,items[i].TotalWeightContract, items[i].ItemWeightAction, items[i].TotalWeightOperation, items[i].OperateWeightAction, items[i].Hektar);
        //        }

        //        web.AllowUnsafeUpdates = true;

        //        // Generate the CAML
        //        batch = string.Format(batchFormat, methodBuilder.ToString());
        //       // batch2 = string.Format(batchFormat2, methodBuilder2.ToString());

        //        // Process the batch 
        //       string batchReturn = web.ProcessBatchData(batch);
        //       // string batch2Return = web.ProcessBatchData(batch2);

        //    }
        //    catch (Exception ex)
        //    {
        //        strError = ex.Message;
        //    }
        //    return strError;


        //}


        //private static string SaveContractOperations(List<OperationItemUpdate> items)
        //{

        //    string strError = "";
        //    StringBuilder methodBuilder = new StringBuilder();

        //    string batch = string.Empty;

        //    string newValue = "mmmm";
        //    string updateColumn = "SampleColumn";
        //    try
        //    {
        //        string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
        //                                    "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
        //        string methodFormat = "<Method ID='{0}' >" +
        //                               "<SetList>{1}</SetList>" +
        //                               "<SetVar Name='Cmd'>Save</SetVar>" +
        //                               "<SetVar Name='ID'>{2}</SetVar>" +
        //                               "<SetVar Name='urn:schemas-microsoft-com:office:office#ChangeAmount'>{3}</SetVar>" +
        //                               "<SetVar Name='urn:schemas-microsoft-com:office:office#TotalWeightContract'>{4}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#ItemWeightAction'>{5}</SetVar>" +
        //                               "<SetVar Name='urn:schemas-microsoft-com:office:office#TotalWeightOperation'>{6}</SetVar>" +
        //                               "<SetVar Name='urn:schemas-microsoft-com:office:office#WeightAction'>{7}</SetVar>" +
        //                               "<SetVar Name='urn:schemas-microsoft-com:office:office#RenewEqHectar'>{8}</SetVar>" +
        //                                "<SetVar Name='urn:schemas-microsoft-com:office:office#DrainEqHectar'>{9}</SetVar>" +
        //                                 "<SetVar Name='urn:schemas-microsoft-com:office:office#NetworkEqHectar'>{10}</SetVar>" +
        //                               "</Method>";


        //        string _siteUrl = SPContext.Current.Site.Url + "/ProjectsInfo";
        //        SPSite siteColl = new SPSite(_siteUrl);
        //        SPWeb web = siteColl.OpenWeb();
        //        web.AllowUnsafeUpdates = true;
        //        SPList baseContractOperations = web.GetList("/ProjectsInfo/Lists/ContractOperation");
        //        string baseContractListGuid = baseContractOperations.ID.ToString();
        //        for (int i = 0; i < items.Count; i++)
        //        {
        //            int itemID = items[i].Id;

        //            methodBuilder.AppendFormat(methodFormat, itemID, baseContractListGuid, itemID, items[i].ChangeAmount, items[i].TotalWeightContract, items[i].ItemWeightAction, items[i].TotalWeightOperation, items[i].OperateWeightAction, items[i].RenewEqHectar, items[i].DrainEqHectar, items[i].NetworkEqHectar);
        //        }


        //        web.AllowUnsafeUpdates = true;

        //        // Generate the CAML
        //        batch = string.Format(batchFormat, methodBuilder.ToString());
        //        // batch2 = string.Format(batchFormat2, methodBuilder2.ToString());

        //        // Process the batch 
        //        string batchReturn = web.ProcessBatchData(batch);
        //    }
        //    catch (Exception ex)
        //    {

        //        strError = ex.Message;
        //    }
        //    // string batch2Return = web.ProcessBatchData(batch2);
        //    return strError;
        //}


       [WebMethod]
    public static string Approve(int itemId, string comment,string listName)
    {
        SPWeb web = SPContext.Current.Web;
        int iD = web.CurrentUser.ID;
        return Utility.Approve(web, comment, listName, itemId, iD);
    }

    [WebMethod]
    public static int CanApprove(int itemId, string listName)
    {
        int creatorId = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        return Utility.CanApprove(SPContext.Current.Web, itemId, listName, out creatorId, out num2, out num3, out num4);
    }

    [WebMethod]
    public static object GetHistories(int itemId, string listName)
    {
        SPWeb web = SPContext.Current.Web;
        return new { histories = Utility.GetHistory(itemId, listName) };
    }

    [WebMethod]
    public static List<WeeklyOperationItem> GetWeeklyDetailPrevius(int contractId, string startDate)
    {
        List<WeeklyOperationItem> list = new List<WeeklyOperationItem>();
        SPWeb web = SPContext.Current.Web;
        SPList list2 = web.GetList("/Lists/PeriodWeeklies");
        SPList list3 = web.GetList("/Lists/WeeklyConstructionsDetails");
        SPQuery query = new SPQuery {
            Query = string.Format(@"<Where>
                                        <Lt>
                                                     <FieldRef Name='StartDate' />
                                                     <Value IncludeTimeValue='False' Type='DateTime'>{0}</Value>
                                                  </Lt>
                                               </Where>",SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(startDate)))
        };
        SPListItemCollection items = list2.GetItems(query);
        int[] source = new int[items.Count];
        int num = 0;
        if (items.Count != 0)
        {
            foreach (SPListItem item in items)
            {
                source[num++] = item.ID;
            }
            string str2 = "<Where><In><FieldRef Name='Period' LookupId='TRUE'/><Values>";
            for (int i = 0; i < source.Count<int>(); i++)
            {
                object obj2 = str2;
                str2 = string.Concat(new object[] { obj2, "<Value Type='Lookup'>", source[i], "</Value>" });
            }
            str2 = str2 + "</Values></In></Where>";
            SPQuery query2 = new SPQuery {
                Query = string.Format(@"<Where>
                                                  <Lt>           
                                                     <FieldRef Name='Period_x003a__x062a__x0627__x063' />
                                                     <Value Type='DateTime'>{0}</Value>
                                                  </Lt>
                                               </Where>",SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(startDate))),
                ViewAttributes = "Scope='RecursiveAll'"
            };
            SPFolder folder = web.GetFolder("/Lists/WeeklyConstructionsDetails/" + contractId.ToString());
            query2.Folder = folder;
            SPListItemCollection items2 = list3.GetItems(query2);
            foreach (SPListItem item2 in items2)
            {
                WeeklyOperationItem item3 = new WeeklyOperationItem {
                    Id = item2.ID,
                    Title = item2["Title"].ToString(),
                    DoneWork = decimal.Parse(item2["Constructed"].ToString()),
                    ExecutiveOperation = new SPFieldLookupValue(item2["Operation"].ToString()).LookupValue,
                    ExecutiveOperationId = new SPFieldLookupValue(item2["Operation"].ToString()).LookupId,
                    SubExecutiveOperation = (item2["SubOperation"] != null) ? new SPFieldLookupValue(item2["SubOperation"].ToString()).LookupValue : "",
                    SubExecutiveOperationId = (item2["SubOperation"] != null) ? new SPFieldLookupValue(item2["SubOperation"].ToString()).LookupId : 0,
                    Measure = (item2["Measurement"] != null) ? item2["Measurement"].ToString() : ""
                };
                list.Add(item3);
            }
        }
        return list;
    }

    [WebMethod]
    public static bool IsExitSameWeeklyReport(int contractId, int periodId)
    {
        SPWeb web = SPContext.Current.Web;
        Guid siteId = web.Site.ID;
        SPListItemCollection col = null;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    SPList list = Web.GetList("/Lists/PeriodWeeklies");
                    SPList list2 = Web.GetList("/Lists/WeeklyConstructions");
                    SPQuery query = new SPQuery {
                        ViewAttributes = "Scope='RecursiveAll'",
                        Query =string.Format(@"<Where>
                                                  <And>
                                                     <Eq>
                                                        <FieldRef Name='Contract' LookupId='TRUE'/>
                                                        <Value Type='Lookup'>{0}</Value>
                                                     </Eq>
                                                     <Eq>
                                                        <FieldRef Name='Period' LookupId='TRUE'/>
                                                        <Value Type='Lookup'>{1}</Value>
                                                     </Eq>
                                                  </And>
                                               </Where>",contractId,periodId)
                    };
                    col = list2.GetItems(query);
                }
            }
        });
        return (col.Count > 0);
    }

   
       [WebMethod]
    public static bool IsExitSameWeeklyPlan(int contractId, int periodId)
    {
        SPWeb web = SPContext.Current.Web;
        Guid siteId = web.Site.ID;
        SPListItemCollection col = null;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    SPList list = Web.GetList("/Lists/PeriodWeeklies");
                    SPList list2 = Web.GetList("/Lists/WeeklyPlanConstructions");
                    SPQuery query = new SPQuery {
                        ViewAttributes = "Scope='RecursiveAll'",
                        Query =string.Format(@"<Where>
                                                  <And>
                                                     <Eq>
                                                        <FieldRef Name='Contract' LookupId='TRUE'/>
                                                        <Value Type='Lookup'>{0}</Value>
                                                     </Eq>
                                                     <Eq>
                                                        <FieldRef Name='Period' LookupId='TRUE'/>
                                                        <Value Type='Lookup'>{1}</Value>
                                                     </Eq>
                                                  </And>
                                               </Where>",contractId,periodId)
                    };
                    col = list2.GetItems(query);
                }
            }
        });
        return (col.Count > 0);
    }

    [WebMethod(EnableSession=true)]
    public static string Reject(int itemId, string comment,string listName)
    {
        SPWeb web = SPContext.Current.Web;
        int iD = web.CurrentUser.ID;
        return Utility.Reject(web, comment, listName, itemId, iD);
    }

    private static string SaveWeeklyOpertionItems(int id, int contractId, string period, int periodId, List<WeeklyOperationItem> items, int creatorId, int approver1Id, int approver2Id, int approver3Id, List<int> viewersIds, List<int> editorsIds)
    {
        string msg = "";
        SPWeb web = SPContext.Current.Web;
        Guid siteID = web.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    SPList list = Web.GetList("/Lists/WeeklyConstructionsDetails");
                    Web.AllowUnsafeUpdates = true;
                    SPFolder folder = Web.GetFolder("/Lists/WeeklyConstructionsDetails/" + contractId.ToString());
                    if (!folder.Exists)
                    {
                        SPListItem item = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                        item["Title"] = contractId.ToString();
                        Web.AllowUnsafeUpdates = true;
                        item.Update();
                        folder = Web.GetFolder("/Lists/WeeklyConstructionsDetails/" + contractId.ToString());
                        Utility.SetListItemPermission(folder.Item, creatorId, 0x40000002, true);
                        Utility.SetListItemPermission(folder.Item, approver1Id, 0x40000002, false);
                        if (approver2Id != 0)
                        {
                            Utility.SetListItemPermission(folder.Item, approver2Id, 0x40000002, false);
                        }
                        if (approver3Id != 0)
                        {
                            Utility.SetListItemPermission(folder.Item, approver3Id, 0x40000002, false);
                        }
                        int iD = Web.Groups["تیم راهبری"].ID;
                        int userId = Web.Groups["تیم راهبری-ویرایش"].ID;
                        int num3 = Web.Groups["Ejra_viewer"].ID;
                        Utility.SetListItemPermission(folder.Item, iD, 0x40000002, false);
                        Utility.SetListItemPermission(folder.Item, num3, 0x40000002, false);
                        Utility.SetListItemPermission(folder.Item, userId, 0x40000003, false);
                        foreach (int num4 in viewersIds)
                        {
                            Utility.SetListItemPermission(folder.Item, num4, 0x40000002, false);
                        }
                        foreach (int num5 in editorsIds)
                        {
                            Utility.SetListItemPermission(folder.Item, num5, 0x40000003, false);
                        }
                    }
                    SPFolder folder2 = Web.GetFolder("/Lists/WeeklyConstructionsDetails/" + contractId.ToString());
                    SPQuery query = new SPQuery {
                        Query =string.Format(@"<Where>
                                              <Eq>
                                                 <FieldRef Name='WeeklyConstruction' LookupId='TRUE' />
                                                 <Value Type='Lookup'>{0}</Value>
                                              </Eq>
                                           </Where>",id),
                        Folder = folder2
                    };
                    SPListItemCollection collection1 = list.GetItems(query);
                    foreach (SPListItem item in collection1)
                    {
                        list.GetItemById(item.ID).Delete();
                    }
                    foreach (WeeklyOperationItem item3 in items)
                    {
                        SPListItem item4 = list.AddItem(folder.ServerRelativeUrl, SPFileSystemObjectType.File);
                        item4["WeeklyConstruction"] = new SPFieldLookupValue(id, "");
                        item4["Constructed"] = item3.DoneWork;
                        item4["Measurement"] = item3.Measure;
                        item4["Operation"] = new SPFieldLookupValue(item3.ExecutiveOperationId, "");
                        if (item3.SubExecutiveOperationId != 0)
                        {
                            item4["SubOperation"] = new SPFieldLookupValue(item3.SubExecutiveOperationId, "");
                        }
                        item4["Title"] = "جزییات گزارش هفتگی " + item3.ExecutiveOperation + " " + item3.SubExecutiveOperation;
                        item4["Period"] = new SPFieldLookupValue(periodId, "");
                        try
                        {
                            Web.AllowUnsafeUpdates = true;
                            item4.Update();
                            msg = item4.ID.ToString();
                            Web.AllowUnsafeUpdates = false;
                        }
                        catch (Exception exception)
                        {
                            msg = exception.Message;
                        }
                    }
                }
            }
        });
        return msg;
    }
    private static string SaveWeeklyPlanItems(int id, int contractId, string period, int periodId, List<WeeklyOperationItem> items, int creatorId, int approver1Id, int approver2Id, int approver3Id, List<int> viewersIds, List<int> editorsIds)
    {
        string msg = "";
        SPWeb web = SPContext.Current.Web;
        Guid siteID = web.Site.ID;
        SPSecurity.RunWithElevatedPrivileges(delegate
        {
            using (SPSite site = new SPSite(siteID))
            {
                using (SPWeb Web = site.OpenWeb())
                {
                    SPList list = Web.GetList("/Lists/WeeklyPlanConstructionsDetails");
                    Web.AllowUnsafeUpdates = true;
                    SPFolder folder = Web.GetFolder("/Lists/WeeklyPlanConstructionsDetails/" + contractId.ToString());
                    if (!folder.Exists)
                    {
                        SPListItem item = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                        item["Title"] = contractId.ToString();
                        Web.AllowUnsafeUpdates = true;
                        item.Update();
                        folder = Web.GetFolder("/Lists/WeeklyPlanConstructionsDetails/" + contractId.ToString());
                        Utility.SetListItemPermission(folder.Item, creatorId, 0x40000002, true);
                        Utility.SetListItemPermission(folder.Item, approver1Id, 0x40000002, false);
                        if (approver2Id != 0)
                        {
                            Utility.SetListItemPermission(folder.Item, approver2Id, 0x40000002, false);
                        }
                        if (approver3Id != 0)
                        {
                            Utility.SetListItemPermission(folder.Item, approver3Id, 0x40000002, false);
                        }
                        int iD = Web.Groups["تیم راهبری"].ID;
                        int userId = Web.Groups["تیم راهبری-ویرایش"].ID;
                        int num3 = Web.Groups["Ejra_viewer"].ID;
                        Utility.SetListItemPermission(folder.Item, iD, 0x40000002, false);
                        Utility.SetListItemPermission(folder.Item, num3, 0x40000002, false);
                        Utility.SetListItemPermission(folder.Item, userId, 0x40000003, false);
                        foreach (int num4 in viewersIds)
                        {
                            Utility.SetListItemPermission(folder.Item, num4, 0x40000002, false);
                        }
                        foreach (int num5 in editorsIds)
                        {
                            Utility.SetListItemPermission(folder.Item, num5, 0x40000003, false);
                        }
                    }
                    SPFolder folder2 = Web.GetFolder("/Lists/WeeklyPlanConstructionsDetails/" + contractId.ToString());
                    SPQuery query = new SPQuery
                    {
                        Query = string.Format(@"<Where>
                                              <Eq>
                                                 <FieldRef Name='WeeklyConstruction' LookupId='TRUE' />
                                                 <Value Type='Lookup'>{0}</Value>
                                              </Eq>
                                           </Where>", id),
                        Folder = folder2
                    };
                    SPListItemCollection collection1 = list.GetItems(query);
                    foreach (SPListItem item in collection1)
                    {
                        list.GetItemById(item.ID).Delete();
                    }
                    foreach (WeeklyOperationItem item3 in items)
                    {
                        SPListItem item4 = list.AddItem(folder.ServerRelativeUrl, SPFileSystemObjectType.File);
                        item4["WeeklyConstruction"] = new SPFieldLookupValue(id, "");
                        item4["Constructed"] = item3.DoneWork;
                        item4["Measurement"] = item3.Measure;
                        item4["Operation"] = new SPFieldLookupValue(item3.ExecutiveOperationId, "");
                        if (item3.SubExecutiveOperationId != 0)
                        {
                            item4["SubOperation"] = new SPFieldLookupValue(item3.SubExecutiveOperationId, "");
                        }
                        item4["Title"] = "جزییات پلن هفتگی " + item3.ExecutiveOperation + " " + item3.SubExecutiveOperation;
                        item4["Period"] = new SPFieldLookupValue(periodId, "");
                        try
                        {
                            Web.AllowUnsafeUpdates = true;
                            item4.Update();
                            msg = item4.ID.ToString();
                            Web.AllowUnsafeUpdates = false;
                        }
                        catch (Exception exception)
                        {
                            msg = exception.Message;
                        }
                    }
                }
            }
        });
        return msg;
    }
    [WebMethod]
    public static string SaveWeeklyReport(WeeklyOperation weeklyItem, bool isTemp)
    {
        int lookupId;
        SPSecurity.CodeToRunElevated secureCode = null;
        string message = "";
        SPWeb web = SPContext.Current.Web;
        Guid siteID = web.Site.ID;
        SPList list = web.GetList("/Lists/WeeklyConstructions");
        SPList list2 = web.GetList("/Lists/FormPermissions");
        SPListItem itemById = web.GetList("/Lists/Contracts").GetItemById(weeklyItem.ContractId);
        SPQuery query = new SPQuery {
            Query = string.Format(@"<Where>
                                     <Eq><FieldRef Name='ListName' />
                                       <Value Type='Text'>WeeklyConstructions</Value>
                                   </Eq>
                                    </Where>")
        };
        SPListItem item2 = (list2.GetItems(query).Count > 0) ? list2.GetItems(query)[0] : null;
        if (((item2 == null) || (item2["Creator"] == null)) || (item2["Approver1"] == null))
        {
            return (" برای " + itemById["Title"].ToString() + " اطلاعات دسترسی گزارش هفتگی تکمیل نشده است ");
        }
        int relatedUser = Utility.GetRelatedUser(new SPFieldLookupValue(item2["Creator"].ToString()).LookupId, weeklyItem.ContractId);
        int userId = Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver1"].ToString()).LookupId, weeklyItem.ContractId);
        int num3 = (item2["Approver2"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver2"].ToString()).LookupId, weeklyItem.ContractId) : 0;
        int num4 = (item2["Approver3"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver3"].ToString()).LookupId, weeklyItem.ContractId) : 0;
        List<int> viewersIds = new List<int>();
        SPFieldUserValueCollection values = (itemById["Viewers"] != null) ? new SPFieldUserValueCollection(web, itemById["Viewers"].ToString()) : null;
        if (values != null)
        {
            foreach (SPFieldUserValue value2 in values)
            {
                lookupId = value2.LookupId;
                viewersIds.Add(lookupId);
            }
        }
        SPFieldLookupValueCollection values2 = (item2["Viewers"] != null) ? new SPFieldLookupValueCollection(item2["Viewers"].ToString()) : null;
        if (values2 != null)
        {
            foreach (SPFieldLookupValue value3 in values2)
            {
                lookupId = value3.LookupId;
                viewersIds.Add(Utility.GetRelatedUser(lookupId, itemById.ID));
            }
        }
        List<int> editorsIds = new List<int>();
        SPFieldLookupValueCollection values3 = (item2["Editors"] != null) ? new SPFieldLookupValueCollection(item2["Editors"].ToString()) : null;
        if (values3 != null)
        {
            foreach (SPFieldLookupValue value3 in values3)
            {
                lookupId = value3.LookupId;
                editorsIds.Add(Utility.GetRelatedUser(lookupId, itemById.ID));
            }
        }
        SPFolder folder = web.GetFolder("/Lists/WeeklyConstructions/" + weeklyItem.ContractId.ToString());
        int navigatorGroupId = 0;
        int navigator_editorId = 0;
        int ejraGroupId = 0;
        if (!folder.Exists)
        {
            SPListItem item3 = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
            item3["Title"] = weeklyItem.ContractId.ToString();
            web.AllowUnsafeUpdates = true;
            item3.Update();
            folder = web.GetFolder("/Lists/WeeklyConstructions/" + weeklyItem.ContractId.ToString());
            if (secureCode == null)
            {
                secureCode = delegate {
                    using (SPSite site = new SPSite(siteID))
                    {
                        using (SPWeb Web = site.OpenWeb())
                        {
                            navigatorGroupId = Web.Groups["تیم راهبری"].ID;
                            navigator_editorId = Web.Groups["تیم راهبری-ویرایش"].ID;
                            ejraGroupId =Web.Groups["Ejra_viewer"].ID;
                        }
                    }
                };
            }
            SPSecurity.RunWithElevatedPrivileges(secureCode);
            Utility.SetListItemPermission(folder.Item, relatedUser, 0x4000006b, true);
            Utility.SetListItemPermission(folder.Item, userId, 0x40000002, false);
            if (num3 != 0)
            {
                Utility.SetListItemPermission(folder.Item, num3, 0x40000002, false);
            }
            if (num4 != 0)
            {
                Utility.SetListItemPermission(folder.Item, num4, 0x40000002, false);
            }
            Utility.SetListItemPermission(folder.Item, navigatorGroupId, 0x40000002, false);
            Utility.SetListItemPermission(folder.Item, ejraGroupId, 0x40000002, false);
            Utility.SetListItemPermission(folder.Item, navigator_editorId, 0x40000003, false);
            foreach (int num6 in viewersIds)
            {
                Utility.SetListItemPermission(folder.Item, num6, 0x40000002, false);
            }
            foreach (int num7 in editorsIds)
            {
                Utility.SetListItemPermission(folder.Item, num7, 0x40000003, false);
            }
        }
        int result = 0;
        SPListItem item = (weeklyItem.Id > 0) ? list.GetItemById(weeklyItem.Id) : list.AddItem(folder.ServerRelativeUrl, SPFileSystemObjectType.File);
        item["Title"] = string.Concat(new object[] { " گزارش اجرای پیمان", weeklyItem.Contract, " دوره ", weeklyItem.Period });
        item["Contract"] = new SPFieldLookupValue(weeklyItem.ContractId, "");
        item["Period"] = new SPFieldLookupValue(weeklyItem.Period, "");
        item["Status"] = isTemp ? "ثبت موقت" : "در انتظار تایید";
        item["CurrentUser"] = isTemp ? new SPFieldUserValue(web, relatedUser.ToString()) : new SPFieldUserValue(web, userId.ToString());
        try
        {
            web.AllowUnsafeUpdates = true;
            item.Update();
            if (isTemp)
            {
                Utility.SetListItemPermission(item, relatedUser, 0x40000003, false);
            }
            else
            {
                Utility.ResetItemPermission(item);
            }
            web.AllowUnsafeUpdates = false;
        }
        catch (Exception exception)
        {
            message = exception.Message;
        }
        if (message == "")
        {
            string s = SaveWeeklyOpertionItems(item.ID, weeklyItem.ContractId, new SPFieldLookupValue(item["Period"].ToString()).LookupValue, new SPFieldLookupValue(item["Period"].ToString()).LookupId, weeklyItem.Items, relatedUser, userId, num3, num4, viewersIds, editorsIds);
            int.TryParse(s, out result);
            if (result < 0)
            {
                return s;
            }
            string str3 = "/_Layouts/15/ProjectInfoSystem/Pages/Operation/EditForm.aspx?ListName=WeeklyConstructions&ID=" + item.ID;
            if (!isTemp)
            {
            }
            if (message == "")
            {
                message = Utility.CreateHistory(web, item.ID, DateTime.Now.ToString(), isTemp ? "ثبت موقت" : "ثبت", "", "WeeklyConstructions", web.CurrentUser.ID);
            }
        }
        return message;
    }

    

     [WebMethod]
    public static string SaveWeeklyPlan(WeeklyOperation weeklyItem)
    {
        int lookupId;
        SPSecurity.CodeToRunElevated secureCode = null;
        string message = "";
        SPWeb web = SPContext.Current.Web;
        Guid siteID = web.Site.ID;
        SPList list = web.GetList("/Lists/WeeklyPlanConstructions");
        SPList list2 = web.GetList("/Lists/FormPermissions");
        SPListItem itemById = web.GetList("/Lists/Contracts").GetItemById(weeklyItem.ContractId);
        SPQuery query = new SPQuery {
            Query = string.Format(@"<Where>
                                     <Eq><FieldRef Name='ListName' />
                                       <Value Type='Text'>WeeklyPlanConstructions</Value>
                                   </Eq>
                                    </Where>")
        };
        SPListItem item2 = (list2.GetItems(query).Count > 0) ? list2.GetItems(query)[0] : null;
        if (((item2 == null) || (item2["Creator"] == null)) || (item2["Approver1"] == null))
        {
            return (" برای " + itemById["Title"].ToString() + " اطلاعات دسترسی پلن هفتگی تکمیل نشده است ");
        }
        int relatedUser = Utility.GetRelatedUser(new SPFieldLookupValue(item2["Creator"].ToString()).LookupId, weeklyItem.ContractId);
        int userId = Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver1"].ToString()).LookupId, weeklyItem.ContractId);
        int num3 = (item2["Approver2"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver2"].ToString()).LookupId, weeklyItem.ContractId) : 0;
        int num4 = (item2["Approver3"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver3"].ToString()).LookupId, weeklyItem.ContractId) : 0;
        List<int> viewersIds = new List<int>();
        SPFieldUserValueCollection values = (itemById["Viewers"] != null) ? new SPFieldUserValueCollection(web, itemById["Viewers"].ToString()) : null;
        if (values != null)
        {
            foreach (SPFieldUserValue value2 in values)
            {
                lookupId = value2.LookupId;
                viewersIds.Add(lookupId);
            }
        }
        SPFieldLookupValueCollection values2 = (item2["Viewers"] != null) ? new SPFieldLookupValueCollection(item2["Viewers"].ToString()) : null;
        if (values2 != null)
        {
            foreach (SPFieldLookupValue value3 in values2)
            {
                lookupId = value3.LookupId;
                viewersIds.Add(Utility.GetRelatedUser(lookupId, itemById.ID));
            }
        }
        List<int> editorsIds = new List<int>();
        SPFieldLookupValueCollection values3 = (item2["Editors"] != null) ? new SPFieldLookupValueCollection(item2["Editors"].ToString()) : null;
        if (values3 != null)
        {
            foreach (SPFieldLookupValue value3 in values3)
            {
                lookupId = value3.LookupId;
                editorsIds.Add(Utility.GetRelatedUser(lookupId, itemById.ID));
            }
        }
        SPFolder folder = web.GetFolder("/Lists/WeeklyPlanConstructions/" + weeklyItem.ContractId.ToString());
        int navigatorGroupId = 0;
        int navigator_editorId = 0;
        int ejraGroupId = 0;
        if (!folder.Exists)
        {
            SPListItem item3 = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
            item3["Title"] = weeklyItem.ContractId.ToString();
            web.AllowUnsafeUpdates = true;
            item3.Update();
            folder = web.GetFolder("/Lists/WeeklyPlanConstructions/" + weeklyItem.ContractId.ToString());
            if (secureCode == null)
            {
                secureCode = delegate {
                    using (SPSite site = new SPSite(siteID))
                    {
                        using (SPWeb Web = site.OpenWeb())
                        {
                            navigatorGroupId = Web.Groups["تیم راهبری"].ID;
                            navigator_editorId = Web.Groups["تیم راهبری-ویرایش"].ID;
                            ejraGroupId =Web.Groups["Ejra_viewer"].ID;
                        }
                    }
                };
            }
            SPSecurity.RunWithElevatedPrivileges(secureCode);
            Utility.SetListItemPermission(folder.Item, relatedUser, 0x4000006b, true);
            Utility.SetListItemPermission(folder.Item, userId, 0x40000002, false);
            if (num3 != 0)
            {
                Utility.SetListItemPermission(folder.Item, num3, 0x40000002, false);
            }
            if (num4 != 0)
            {
                Utility.SetListItemPermission(folder.Item, num4, 0x40000002, false);
            }
            Utility.SetListItemPermission(folder.Item, navigatorGroupId, 0x40000002, false);
            Utility.SetListItemPermission(folder.Item, ejraGroupId, 0x40000002, false);
            Utility.SetListItemPermission(folder.Item, navigator_editorId, 0x40000003, false);
            foreach (int num6 in viewersIds)
            {
                Utility.SetListItemPermission(folder.Item, num6, 0x40000002, false);
            }
            foreach (int num7 in editorsIds)
            {
                Utility.SetListItemPermission(folder.Item, num7, 0x40000003, false);
            }
        }
        int result = 0;
        SPListItem item = (weeklyItem.Id > 0) ? list.GetItemById(weeklyItem.Id) : list.AddItem(folder.ServerRelativeUrl, SPFileSystemObjectType.File);
        item["Title"] = string.Concat(new object[] { " گزارش اجرای پیمان", weeklyItem.Contract, " دوره ", weeklyItem.Period });
        item["Contract"] = new SPFieldLookupValue(weeklyItem.ContractId, "");
        item["Period"] = new SPFieldLookupValue(weeklyItem.Period, "");
        item["Status"] =  "در انتظار تایید";
        item["CurrentUser"] =  new SPFieldUserValue(web, userId.ToString());
        try
        {
            web.AllowUnsafeUpdates = true;
            item.Update();
          
            Utility.ResetItemPermission(item);
            
            web.AllowUnsafeUpdates = false;
        }
        catch (Exception exception)
        {
            message = exception.Message;
        }
        if (message == "")
        {
            string s = SaveWeeklyPlanItems(item.ID, weeklyItem.ContractId, new SPFieldLookupValue(item["Period"].ToString()).LookupValue, new SPFieldLookupValue(item["Period"].ToString()).LookupId, weeklyItem.Items, relatedUser, userId, num3, num4, viewersIds, editorsIds);
            int.TryParse(s, out result);
            if (result < 0)
            {
                return s;
            }
            string str3 = "/_Layouts/15/ProjectInfoSystem/Pages/Operation/EditForm.aspx?ListName=WeeklyPlanConstructions&ID=" + item.ID;
           
            if (message == "")
            {
                message = Utility.CreateHistory(web, item.ID, DateTime.Now.ToString(), "ثبت", "", "WeeklyPlanConstructions", web.CurrentUser.ID);
            }
        }
        return message;
    }

    }
}
