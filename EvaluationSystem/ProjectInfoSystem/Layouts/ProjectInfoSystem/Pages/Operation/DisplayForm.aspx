<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" Inherits="ProjectInfoSystem.Layouts.ProjectInfoSystem.Pages.Operation.DisplayForm" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <link href="../../CSS/loading.css" rel="stylesheet" />
     
    <link href="../../CSS/style3.css" rel="stylesheet" />
     <script src="../../JS/jquery-3.2.1.min.js"></script>
    <script src="../../JS/moment.js"></script>
    <script src="../../JS/moment-jalaali.js"></script>
    <script src="../../JS/angular.min.js"></script>
    <script src="DisplayForm.js"></script>
     <script src="../../JS/service.js"></script>
      <script src="../../JS/QueryString.js"></script>
  
   
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <div  ng-app="opApp" ng-controller="opCtrl">
        
        <div id="select">
           
        <div class="loading" ng-show="loading"></div>
    <div id="mycontainer" class="display" style=" padding-right: 19%;" >

          <div class="newhead-priod" >
           <span>دوره </span>
           <div id="priod" class="display-head-data" >{{weeklyItem.Period.Title}}</div>
          </div>
          <div class="newhead-contract" >
           <span>پیمان</span>
         <div id="contractt" class="display-head-data"  >  {{weeklyItem.Contract.Title}}   </div>
               </div>
         <%-- <div class="newhead-block" >
           <span>بلوک</span>
         <div class="display-head-data" > {{Block}} </div>
          </div>--%>
             
       <%--  <div class="newhead-priod" >
           <span>واحد زراعی</span>
         <div class="display-head-data" >  {{Farm}} </div>
            </div>--%>
          
          <div id="show-operation-bott" >
        
              </div>
           </div>
     </div>
         <div id="tbl"  >
            <table  class="container" dir="rtl" >
                <thead>
                    <tr>
                        <th class="operation-run" >عملیات اجرایی</th>
                        <th class="item-run">آیتم</th>
                        <th class="Measurement">واحد</th>
                        <th class="TotalAmount" >مقدار کل کار</th>
                        <th class="Constructed" >انجام شده تا کنون</th>
                        <th class="done-work" >انجام شده در این دوره</th>
                        <th class="total">مجموع</th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="o in weeklyItem.Items | orderBy : ['OperationId', 'SubOperationId','Measurement']">
                        <td>{{o.Operation.Title}}</td>
                        <td>{{o.SubOperation.Title}}</td>
                        <td>{{o.Measurement}}</td>
                        <td>{{o.CheckValue| number:1}}</td>
                        <td>{{o.Constructed|number:1}}</td>
                      
                        <td>{{o.doneWork|number:1}}</td>
                        <td>{{o.Constructed+o.doneWork|number:1}}</td>
                    </tr>
                </tbody>
            </table>
        </div>

         <div id="historydiv" ng-if="histories.length>0">
					<table class="container history" >
						<thead>
							<tr>
								<th>ردیف</th>
								<th>کاربر</th>
								<th>رویداد</th>
								<th>تاریخ</th>
								<th>توضیحات</th>
							</tr>
						</thead>
						<tbody>
							<tr ng-repeat="s in histories">
								<td class="footer-history-1" id="number-row" >{{($index)+1}}</td>
								<td class="footer-history-2">{{s.UserName}}</td>
								<td class="footer-history-3">{{s.state}}</td>
								<td class="footer-history-4" id="number-row">{{s.HistoryDate}}</td>
								<td class="footer-history-5">{{s.Description}}</td>
							</tr>
						</tbody>
                        
					</table>
	 </div>

        <div class="bottom-container">
       
            <input ng-show="canApprove!=-1"  ng-model="comment" class="form-coment" ></input>
            <div id="form-botem">
                 <input type="button" ng-click="close()" value="انصراف" />
               
                <input ng-show="canApprove!=-1" type="button" value="رد" ng-click="reject()" />
                 <input ng-show="canApprove!=-1" type="button" value="تایید" ng-click="approve()" />
               
            </div>

        </div>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
فرم گزارش هفتگی
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
فرم گزارش هفتگی
</asp:Content>

