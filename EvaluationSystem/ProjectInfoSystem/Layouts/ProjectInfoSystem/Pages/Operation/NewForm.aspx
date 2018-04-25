<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" Inherits="ProjectInfoSystem.Layouts.ProjectInfoSystem.Pages.Operation.NewForm" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="../../CSS/loading.css" rel="stylesheet" />

    <link href="../../CSS/style3.css" rel="stylesheet" />
    <script src="../../JS/jquery-3.2.1.min.js"></script>
    <script src="../../JS/moment.js"></script>
    <script src="../../JS/moment-jalaali.js"></script>
    <script src="../../JS/angular.min.js"></script>
    <script src="NewForm.js"></script>
    <script src="../../JS/service.js"></script>
    <script src="../../JS/serverCall.js"></script>

    <style>
        .error {
        border:solid 1px red !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div ng-app="opApp" ng-controller="opCtrl">

        <div id="select">

            <div class="loading" ng-show="loading"></div>
            <div id="mycontainer" style="padding-right: 19%;">
                <div class="newhead-priod">
                    <span>دوره </span>
                    <select  ng-model="period" ng-options="x.Title for x in periods"></select>

                    <div ng-show="period" class="show-date fade-in-out" ng-style="animation">تاریخ شروع : {{period.StartDate | jalaliDate:'jYYYY/jMM/jDD' }}</div>
                    <div ng-show="period" class="show-date fade-in-out" ng-style="animation">تاریخ پایان: {{period.EndDate | jalaliDate:'jYYYY/jMM/jDD' }}</div>

                </div>
                <div class="newhead-contract">
                    <span>پیمان</span>
                    <select ng-model="contract" ng-options="x.Title for x in contracts"></select>
                </div>
                <div id="show-operation-bott">
                    <input id="show-operation" type="button" ng-click="showOperations() " value="مشاهده عملیات" />
                </div>
            </div>
        </div>
     
            <table class="container" dir="rtl" ng-show="tableData.length>0">
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
                    <tr ng-repeat="r in tableData ">
                        <td>{{r.Operation.Title}}</td>
                        <td>{{r.SubOperation.Title}}</td>
                        <td>{{r.Measurement}}</td>
                        <td class="number">{{r.CheckValue|number:1}}</td>
                        <td  class="number">{{r.Constructed|number:1}}</td>
                        <td>
                           <%-- <input type="number" ng-model="r.doneWork" ng-model-options="{ updateOn: 'blur' }" ng-change='validateCell(r)' ng-class="{error : !r.hasError}"/></td>--%>
                             <input type="number" ng-model="r.doneWork" ng-model-options="{ updateOn: 'blur' }" ng-change='validateCell(r)' ng-class="{'error' : r.hasError}"/></td>
                           <td>{{r.sum = r.Constructed + r.doneWork|number:1 }}</td>
                    </tr>
                </tbody>
            </table>


      

        <div class="footer-botten">
            <input  type="button" ng-click="close()" value="انصراف" />
            <input ng-show="tableData.length>0" type="button" ng-click="save(false)" value="ثبت" />
            <input ng-show="tableData.length>0" type="button" ng-click="save(true)" value="ثبت موقت" />

        </div>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    فرم گزارش هفتگی
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    فرم گزارش هفتگی
</asp:Content>

