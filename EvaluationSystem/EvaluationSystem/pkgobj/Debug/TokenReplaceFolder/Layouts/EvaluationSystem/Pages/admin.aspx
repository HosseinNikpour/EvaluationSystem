<%@ Assembly Name="EvaluationSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0d96cb7700ff0a65" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="EvaluationSystem.Layouts.EvaluationSystem.Pages.admin" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
   <link href="../JS/select/select.css" rel="stylesheet" />
    <link href="../CSS/style3.css" rel="stylesheet" />
    <link href="../CSS/loading.css" rel="stylesheet" />
    <link href="../CSS/bootstrap-datepicker.css" rel="stylesheet" />
    <%-- <link href="../CSS/disable-items.css" rel="stylesheet" />--%>

   
    <script src="../JS/jquery-1.11.3.min.js"></script>
   <script src="../JS/moment.js"></script>
    <script src="../JS/moment-jalaali.js"></script>


     <script src="../JS/angular.js"></script>
     <script src="../JS/select/angularjs-dropdown-multiselect.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../JS/ui-bootstrap-tpls.js"></script>
   
     <script src="admin.js"></script>
    <script src="../JS/persianDatePicker.js"></script>
    <script src="../JS/bootstrap-datepicker.min.js"></script>
    <script src="../JS/bootstrap-datepicker.fa.min.js"></script>

    <script src="../JS/serverCall.js"></script>
    <script src="../JS/services.js"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
   <div ng-app="App" ng-controller="adminCtrl">
       <div  class="loading" ng-show="loading">

       </div>
        <div id="my-admin-app" dir="rtl">

        <div id="my-qustions-form">
        <div id="my-lable">انواع ارزیابی :</div>
        <select  ng-options="item.Title for item in evaluationTypes" ng-model="evalType"   ng-change="changeEvalType()">
         
        </select>
         </div>
            <div id="my-qustions-form" ng-show="!evalType.IsCompany">
        <div id="my-lable" >پیمان ها : </div>
        <div ng-dropdown-multiselect="" options="contracts" selected-model="selectedContracts" 
                                 extra-settings="participantsSettings" translation-texts="secectTrnslationText" 
                                style="display: inline-block; cursor: pointer; " >
                            </div>
                </div>

            <div   ng-show="evalType.IsCompany">
                 <div id="my-lable" >شرکت ها : </div>
        <div ng-dropdown-multiselect="" options="companies" selected-model="selectedCompanies" 
                                 extra-settings="participantsSettings" translation-texts="secectTrnslationText" 
                                style="display: inline-block; cursor: pointer; " >
                            </div>
            </div>
       <%-- <select   ng-options="item.Title for item in contracts" ng-model="selectedContracts" ></select>--%>
 <div id="my-qustions-form">
        <div id="my-lable" >دوره :  </div>

      <select ng-options=" p.Title for p in periods" ng-model="period"></select>
      </div>


            <div id="my-submit-form">
        <input  type="button" ng-click="createItems()" value="ذخیره سازی"/>

       
</div>
            </div>
    
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    صفحه ادمین ارزیابی
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
   صفحه ادمین ارزیابی
</asp:Content>
