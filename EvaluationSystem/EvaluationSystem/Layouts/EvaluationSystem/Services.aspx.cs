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

namespace EvaluationSystem.Layouts.EvaluationSystem
{
    public partial class Services : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        [WebMethod]
        public static string Approve(int itemId, string listName, string comment, bool isCompany)
        {
            int iD = SPContext.Current.Web.CurrentUser.ID;
            return Utility.Approve(comment, itemId, listName, iD, isCompany);
        }

        [WebMethod]
        public static int CanApprove(int itemId, string listName, bool isCompany)
        {
            int creatorId = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            return Utility.CanApprove(itemId, listName, isCompany, out creatorId, out num2, out num3, out num4);
        }

        [WebMethod]
        public static string CreateEvaluationContracts(string evalTypeTitle, List<int> contractsId, int periodId, bool isCompany)
        {
            string str2 = Utility.SelectEvaluationList(evalTypeTitle);
            SPWeb web = SPContext.Current.Web;
            SPList list = web.GetList("/Lists/Contracts");
            SPList list2 = web.GetList("/Lists/Companies");
            SPList list3 = web.GetList("/Lists/" + str2);
            SPList list4 = web.GetList("/Lists/Criterions");
            SPList list5 = web.GetList("/Lists/FormPermissions");
            SPList list6 = web.GetList("/Lists/FormsNames");
            foreach (int num in contractsId)
            {
                int lookupId;
                SPListItem item = isCompany ? list2.GetItemById(num) : list.GetItemById(num);
                SPQuery query = new SPQuery
                {
                    Query = string.Format(@"<Where>
                                          <Eq>
                                             <FieldRef Name='ListName' />
                                             <Value Type='Text'>{0}</Value>
                                          </Eq>
                                       </Where>", str2)
                };
                SPListItem item2 = (list5.GetItems(query).Count > 0) ? list5.GetItems(query)[0] : null;
                if (((item2 == null) || (item2["Creator"] == null)) || (item2["Approver1"] == null))
                {
                    return (" برای " + item["Title"].ToString() + " اطلاعات دسترسی ارزشیابی تکمیل نشده است ");
                }
                int userId = Utility.GetRelatedUser(new SPFieldLookupValue(item2["Creator"].ToString()).LookupId, num, isCompany);
                int num3 = Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver1"].ToString()).LookupId, num, isCompany);
                int num4 = (item2["Approver2"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver2"].ToString()).LookupId, num, isCompany) : 0;
                int num5 = (item2["Approver3"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item2["Approver3"].ToString()).LookupId, num, isCompany) : 0;
                List<int> viewerIds = new List<int>();
                SPFieldLookupValueCollection values = (item2["Viewers"] != null) ? new SPFieldLookupValueCollection(item2["Viewers"].ToString()) : null;
                if (values != null)
                {
                    foreach (SPFieldLookupValue value2 in values)
                    {
                        lookupId = Utility.GetRelatedUser(value2.LookupId, num, isCompany);
                        viewerIds.Add(lookupId);
                    }
                }
                List<int> viewerContractIds = new List<int>();
                if (!isCompany)
                {
                    SPFieldUserValueCollection values2 = (item["Viewers"] != null) ? new SPFieldUserValueCollection(web, item["Viewers"].ToString()) : null;
                    if (values2 != null)
                    {
                        foreach (SPFieldUserValue value3 in values2)
                        {
                            lookupId = value3.LookupId;
                            viewerContractIds.Add(lookupId);
                        }
                    }
                }
                List<int> editorsIds = new List<int>();
                SPFieldLookupValueCollection values3 = (item2["Editors"] != null) ? new SPFieldLookupValueCollection(item2["Editors"].ToString()) : null;
                if (values3 != null)
                {
                    foreach (SPFieldLookupValue value2 in values3)
                    {
                        lookupId = Utility.GetRelatedUser(value2.LookupId, num, isCompany);
                        editorsIds.Add(lookupId);
                    }
                }
                int result = 0;
                string s = Utility.CreateEvaluationContract(num, "ایجاد شده", userId, periodId, evalTypeTitle, isCompany);
                int.TryParse(s, out result);
                if (result < 0)
                {
                    return s;
                }
                int iD = web.Groups["تیم راهبری"].ID;
                int num10 = web.Groups["تیم راهبری-ویرایش"].ID;
                SPListItem itemById = list3.GetItemById(int.Parse(s));
                Utility.SetListItemPermission(itemById, userId, 0x40000003, true);
                Utility.SetListItemPermission(itemById, iD, 0x40000002, false);
                Utility.SetListItemPermission(itemById, iD, 0x40000002, false);
                Utility.SetListItemPermission(itemById, num10, 0x40000003, false);
                foreach (int num11 in viewerIds)
                {
                    Utility.SetListItemPermission(itemById, num11, 0x40000002, false);
                }
                foreach (int num11 in viewerContractIds)
                {
                    Utility.SetListItemPermission(itemById, num11, 0x40000002, false);
                }
                foreach (int num12 in editorsIds)
                {
                    Utility.SetListItemPermission(itemById, num12, 0x40000003, false);
                }
                string str4 = Utility.CreateEvaluationContractItem(itemById.ID, evalTypeTitle, isCompany, num, userId, num3, num4, num5, viewerIds, viewerContractIds, editorsIds);
                int.TryParse(str4, out result);
                if (result < 0)
                {
                    return str4;
                }
                string str5 = "/_Layouts/15/EvaluationSystem/Pages/EditForm.aspx?ListName=" + Utility.SelectEvaluationList(evalTypeTitle) + "&ID=" + s;
            }
            return "";
        }

        [WebMethod]
        public static object GetEvaluationContract(int id, string listName)
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.GetList("/Lists/" + listName);
            SPList list2 = web.GetList("/Lists/" + listName + "Details");
            SPListItem itemById = list.GetItemById(id);
            EvaluationContract contract = new EvaluationContract
            {
                id = id,
                title = itemById["Title"].ToString(),
                period = new SPFieldLookupValue(itemById["Period"].ToString()).LookupValue
            };
            bool isCompany = false;
            if (((listName == "EvaluationSupplier") || (listName == "EvalutaionProjectManager")) || (listName == "EvalutaionTechnicalInspector"))
            {
                isCompany = true;
            }
            if (isCompany)
            {
                contract.contractId = new SPFieldLookupValue(itemById["CompanyNasr"].ToString()).LookupId;
                contract.contract = new SPFieldLookupValue(itemById["CompanyNasr"].ToString()).LookupValue;
            }
            else
            {
                contract.contractId = new SPFieldLookupValue(itemById["Contract"].ToString()).LookupId;
                contract.contract = new SPFieldLookupValue(itemById["Contract"].ToString()).LookupValue;
            }
            contract.details = Utility.GetEvaluationContractDetails(id, listName, isCompany);
            List<HistoryDetail> list3 = Utility.GetHistory(web, id, listName);
            return new
            {
                evalContract = contract,
                histories = list3
            };
        }

      
        [WebMethod(EnableSession = true)]
        public static string Reject(int itemId, string listName, string comment, bool isCompany)
        {
            int iD = SPContext.Current.Web.CurrentUser.ID;
            return Utility.Reject(comment, itemId, listName, iD, isCompany);
        }

        [WebMethod]
        public static string SaveEvaluation(EvaluationContract evalContract, bool isTemp, string listName, bool isCompany)
        {
            int lookupId;
            string str = "";
            SPWeb web = SPContext.Current.Web;
            SPUser currentUser = web.CurrentUser;
            SPList parentList = web.GetList("/Lists/" + listName);
            SPList list = web.GetList("/Lists/" + listName + "Details");
            SPListItem itemById = parentList.GetItemById(evalContract.id);
            SPList list3 = isCompany ? web.GetList("/Lists/Companies") : web.GetList("/Lists/Contracts");
            SPList list4 = web.GetList("/Lists/FormPermissions");
            SPList list5 = web.GetList("/Lists/FormsNames");
            string str2 = Utility.SelectEvaluationTypeFromList(listName);
            int id = isCompany ? new SPFieldLookupValue(itemById["CompanyNasr"].ToString()).LookupId : new SPFieldLookupValue(itemById["Contract"].ToString()).LookupId;
            SPListItem item2 = list3.GetItemById(id);
            string str3 = itemById["Status"].ToString();
            SPQuery query = new SPQuery();
            SPQuery query2 = new SPQuery
            {
                Query = string.Format(@"<Where>
                                          <Eq>
                                             <FieldRef Name='ListName' />
                                             <Value Type='Text'>{0}</Value>
                                          </Eq>
                                       </Where>", listName)
            };
            SPListItem item3 = (list4.GetItems(query2).Count > 0) ? list4.GetItems(query2)[0] : null;
            int userId = Utility.GetRelatedUser(new SPFieldLookupValue(item3["Creator"].ToString()).LookupId, id, isCompany);
            int num3 = Utility.GetRelatedUser(new SPFieldLookupValue(item3["Approver1"].ToString()).LookupId, id, isCompany);
            int num4 = (item3["Approver2"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item3["Approver2"].ToString()).LookupId, id, isCompany) : 0;
            int num5 = (item3["Approver3"] != null) ? Utility.GetRelatedUser(new SPFieldLookupValue(item3["Approver3"].ToString()).LookupId, id, isCompany) : 0;
            List<int> list6 = new List<int>();
            SPFieldLookupValueCollection values = (item3["Viewers"] != null) ? new SPFieldLookupValueCollection(item3["Viewers"].ToString()) : null;
            if (values != null)
            {
                foreach (SPFieldLookupValue value2 in values)
                {
                    lookupId = Utility.GetRelatedUser(value2.LookupId, id, isCompany);
                    list6.Add(lookupId);
                }
            }
            List<int> list7 = new List<int>();
            SPFieldLookupValueCollection values2 = (item3["Editors"] != null) ? new SPFieldLookupValueCollection(item3["Editors"].ToString()) : null;
            if (values2 != null)
            {
                foreach (SPFieldLookupValue value2 in values2)
                {
                    lookupId = Utility.GetRelatedUser(value2.LookupId, id, isCompany);
                    list7.Add(lookupId);
                }
            }
            List<int> list8 = new List<int>();
            if (!isCompany)
            {
                SPFieldUserValueCollection values3 = (item2["Viewers"] != null) ? new SPFieldUserValueCollection(web, item2["Viewers"].ToString()) : null;
                if (values3 != null)
                {
                    foreach (SPFieldUserValue value3 in values3)
                    {
                        lookupId = value3.LookupId;
                        list8.Add(lookupId);
                    }
                }
            }
            str = Utility.UpadetDetailScores(evalContract.details, list);
            if (str == "")
            {
                if (isTemp)
                {
                    str = Utility.UpdateStatus(parentList, evalContract.id, null, "ثبت موقت", userId);
                }
                else
                {
                    str = Utility.UpdateStatus(parentList, evalContract.id, new decimal?(evalContract.totalScore), "در انتظار تایید", num3);
                }
                if (str != "")
                {
                    return str;
                }
                str = Utility.CreateHistory(web, itemById.ID, DateTime.Now.ToString(), isTemp ? "ثبت موقت" : "ثبت", "", listName);
                if ((str != "") || isTemp)
                {
                    return str;
                }
                Utility.SetListItemPermission(itemById, userId, 0x40000002, true);
                Utility.SetListItemPermission(itemById, num3, 0x40000002, false);
                if (num4 != 0)
                {
                    Utility.SetListItemPermission(itemById, num4, 0x40000002, false);
                }
                if (num5 != 0)
                {
                    Utility.SetListItemPermission(itemById, num5, 0x40000002, false);
                }
                foreach (int num8 in list6)
                {
                    Utility.SetListItemPermission(itemById, num8, 0x40000002, false);
                }
                foreach (int num9 in list7)
                {
                    Utility.SetListItemPermission(itemById, num9, 0x40000003, false);
                }
                SPQuery query3 = new SPQuery
                {
                    Query = string.Format(@"<Where>
                                              <Eq>
                                                 <FieldRef Name='Title' />
                                                 <Value Type='Text'>{0}</Value>
                                              </Eq>
                                           </Where>", id.ToString())
                };
                int iD = web.Groups["تیم راهبری"].ID;
                int num11 = web.Groups["تیم راهبری-ویرایش"].ID;
                SPListItem item = list.GetItems(query3)[0];
                Utility.SetListItemPermission(item, userId, 0x40000002, true);
                Utility.SetListItemPermission(item, num3, 0x40000002, false);
                if (num4 != 0)
                {
                    Utility.SetListItemPermission(item, num4, 0x40000002, false);
                }
                if (num5 != 0)
                {
                    Utility.SetListItemPermission(item, num5, 0x40000002, false);
                }
                Utility.SetListItemPermission(item, iD, 0x40000002, false);
                Utility.SetListItemPermission(item, num11, 0x40000003, false);
                foreach (int num8 in list6)
                {
                    Utility.SetListItemPermission(item, num8, 0x40000002, false);
                }
                foreach (int num9 in list7)
                {
                    Utility.SetListItemPermission(item, num9, 0x40000003, false);
                }
                string str4 = string.Concat(new object[] { "/_Layouts/15/EvaluationSystem/Pages/DisplayForm.aspx?ListName=", listName, "&ID=", evalContract.id });
            }
            return str;
        }
    }
}
