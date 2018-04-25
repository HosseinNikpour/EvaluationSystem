<%@ Assembly Name="ProjectInfoSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e1d8bb0b77db53e9" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="ProjectInfoSystem.Layouts.ProjectInfoSystem.Pages.Operation.EditForm" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <link href="../../CSS/loading.css" rel="stylesheet" />
    <link href="../../CSS/style3.css" rel="stylesheet" />
      <script src="../../JS/jquery-3.2.1.min.js"></script>
    <script src="../../JS/moment.js"></script>
    <script src="../../JS/moment-jalaali.js"></script>
    <script src="../../JS/angular.min.js"></script>
    <script src="EditForm.js"></script>
    <script src="../../JS/serverCall.js"></script>
    <script src="../../JS/service.js"></script>
    <script src="http://cdn.jsdelivr.net/ramda/latest/ramda.min.js"></script>
     <script src="../../JS/QueryString.js"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <asp:Literal runat="server" ID="lit1"></asp:Literal>
     <div  ng-app="opApp" ng-controller="opCtrl">
        <div ng-model="Title">{{Title.d.Title}}</div>
        <div id="select">
           
        <div class="loading" ng-show="loading"></div>
      <div id="mycontainer" style=" padding-right: 19%;" >

          <div class="newhead" >
           <div class="labale-period" >دوره</div>
           <input class="priod" ng-value="weeklyItem.Period.Title" />
          </div>

          <div class="newhead" >
           <span>پیمان</span>
              <input  ng-value="weeklyItem.Contract.Title"/>
         
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
                        <td>{{o.CheckValue|number:1}}</td>
                        <td>{{o.Constructed|number:1}}</td>
                      
                        <td><input type="number" ng-model='o.doneWork'/></td>
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
            <div class="footer-botten">
                 <input type="button" ng-click="close()" value="انصراف" />
                <input  type="button"  ng-click="save(false)" value="ثبت"/>
                
               
            </div>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
ویرایش گزارش هفتگی
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
ویرایش گزارش هفتگی
</asp:Content>


