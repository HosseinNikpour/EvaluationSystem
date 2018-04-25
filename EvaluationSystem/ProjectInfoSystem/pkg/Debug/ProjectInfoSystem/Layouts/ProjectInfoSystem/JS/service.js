
app.factory('service', function ($q, $http) {
    {

        var listIds = {
            Contracts: '0179B06A-7CE9-42C1-8273-8F2B9CBB9FD2',
            PeriodWeeklies: '0198EDEB-A03D-4D0F-B611-622F54DA83F5',
            MainOperations: 'F4E0ED87-3C77-40DE-9959-FC1317BBC5D8',
            Operations: '4A577901-EE6B-4653-9297-686F68C29F52',
            SubOperations: 'F99E8EFE-E5AF-4925-A8FE-8C92B1EEFA4A',

//
            OperationsOnFarms: '6D634BF8-0FB6-42A1-A9FF-5846B5FA3F39',
            WeeklyOperations: 'E27960E8-5275-44F0-BE77-A1FBA5444B2D',
            WeeklyOperationsDetails: '3B38600D-0EF3-4521-B9F4-9EC9CECEFC4F',

            WeeklyPlanOperations: 'FC749863-5AE3-4F48-8304-7442B202A1D9',
            WeeklyPlanDetails: 'B7BB3EDC-E791-4D91-80F7-6407E4405AE9'
        }

        function serverCall(address, param) {
            return $http({
                method: param ? 'POST' : 'GET',
                url: address,
                headers: {
                    "Accept": "application/json; odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                    "content-type": "application/json; odata=verbose"
                },

                data: JSON.stringify(param)
            });
        }


        function getPeriods() {
            return serverCall("/_api/web/lists(guid'" + listIds.PeriodWeeklies + "')/items?$select=Title,Id,StartDate,EndDate");
        }

        function getContracts(id) {
            var url = "/_api/web/lists(guid'" + listIds.Contracts + "')/items";
            url = id ? url + "?$filter=Id eq " + id : url+"?$filter=Status eq 'جاری'";
            return serverCall(url);
        }

        
      //  function getWeeklyOperations(contractId, period) {
      ////Todo: اطلاعات تا این دوره بیاید
      //      var url = "/_api/web/lists(guid'" + listIds.WeeklyOperationsDetails + "')/getitems";
      //      var queryPayload = {
      //          'query': {
      //              '__metadata': { 'type': 'SP.CamlQuery' },
      //              ViewXml: "<View Scope='RecursiveAll'></View>",
      //              FolderServerRelativeUrl: "/Lists/WeeklyConstructionsDetails/" + contractId
      //          }
      //      };
      //      return serverCall(url, queryPayload);

      //  }
       
    
        function getWeeklyItemDetails(weeklyItemId) {
            var url = "/_api/web/lists(guid'" + listIds.WeeklyOperationsDetails + "')/items?$select=Id,Title,Measurement,Constructed,OperationId,Operation/Title,SubOperation/Title,SubOperationId,PeriodId,WeeklyConstructionId&$expand=Operation/Title,SubOperation/Title&$filter=WeeklyConstruction/Id eq " + weeklyItemId + "";
            return serverCall(url);

        }
        function getWeeklyPlanDetails(weeklyItemId) {
            var url = "/_api/web/lists(guid'" + listIds.WeeklyPlanDetails + "')/items?$select=Id,Title,Measurement,Constructed,OperationId,Operation/Title,SubOperation/Title,SubOperationId,PeriodId,WeeklyConstructionId&$expand=Operation/Title,SubOperation/Title&$filter=WeeklyConstruction/Id eq " + weeklyItemId + "";
            return serverCall(url);

        }
        function getWeeklyItem(weeklyItemId) {
            var url = "/_api/web/lists(guid'" + listIds.WeeklyOperations + "')/items?$select=Id,Title,ContractId,Contract/Title,PeriodId,Period/Title,Period/StartDate,Status&$expand=Period/Title,Contract/Title&$top=999999999&$filter=ID eq " + weeklyItemId + "";
            return serverCall(url);

        }
                 
        function getWeeklyPlanItem(weeklyItemId) {
            var url = "/_api/web/lists(guid'" + listIds.WeeklyPlanOperations + "')/items?$select=Id,Title,ContractId,Contract/Title,PeriodId,Period/Title,Period/StartDate,Status&$expand=Period/Title,Contract/Title&$top=999999999&$filter=ID eq " + weeklyItemId + "";
            return serverCall(url);

        }
        //end get data for display & edit  form


        function getOperationsOnFarmsFilterContract(contractId) {
            var url = "/_api/web/lists(guid'" + listIds.OperationsOnFarms + "')/items?$select=Id,Title,Contract,Contract/Id,Operation,Operation/Id,Operation/Title,SubOperation/Title,SubOperation,SubOperation/Id,Measurement,CheckValue,FirstVolume,FinalVolume,FirstCost,FinalCost,ItemWeight,ItemWeightOperation,TotalItemWeight,TotalWeightOperation,EqAcre&$expand=Contract,Contract/Id,Operation,Operation/Id,Operation/Title,SubOperation/Title,SubOperation,SubOperation/Id&$top=999999999&$filter=Contract/Id eq " + contractId + "";
            return serverCall(url);
        }

        function getMainOperations() {
            return serverCall("/_api/web/lists(guid'" + listIds.MainOperations + "')/items");
        }
        function getSubOperations() {
            return serverCall("/_api/web/lists(guid'" + listIds.SubOperations + "')/items");
        }
       

        
       function getWeeklyDetailPrevius(contractId,startDate)
        {
          
        //    var url = "/sites/jmis/_api/web/lists(guid'" + listIds.WeeklyOperationsDetails + "')/items?$select=Id,Title,OperationId,Operation/Title,ContractOperationId,ContractOperation/Title,PeriodId,Period/Title,Period/StartDate&$expand=Operation,ContractOperation,Period,Files&$filter=Period/StartDate le  '" + startDate + "'";
            
        //    var queryPayload = {
        //        'query': {
        //            '__metadata': { 'type': 'SP.CamlQuery' },
        //            ViewXml: "<View Scope='RecursiveAll'></View>",
        //            FolderServerRelativeUrl: "/sites/JMIS/Lists/WeeklyConstructionsDetails/" + contractId
        //        }
        //    };
        //    return serverCall(url, queryPayload);
          
           return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/GetWeeklyDetailPrevius', { contractId: contractId, startDate: startDate });
           }
        //ba estefade az method save fieldha saveFields
        function save(weeklyItem,isTemp) {

            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/SaveWeeklyReport', { weeklyItem: weeklyItem ,isTemp:isTemp});
        }
        function savePlan(weeklyItem) {

            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/SaveWeeklyPlan', { weeklyItem: weeklyItem });
        }
        function canApprove(id,listName)
        {
            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/canApprove', { itemId: id, listName:listName});
        }
        function getHistories(id, listName)
        {
            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/GetHistories', { itemId: id, listName: listName })
        }
        function approve(id, comment,listName) {
            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/Approve', { itemId: id, comment: comment,listName:listName });
        }
        function reject(id, comment,listName) {
            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/Reject', { itemId: id, comment: comment,listName:listName });
        }


        function isExitSameWeeklyReport(contractId,periodId)
        {

            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/IsExitSameWeeklyReport', { contractId: contractId, periodId: periodId });
        }
        function isExitSameWeeklyPlanReport(contractId, periodId)
        {
            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/IsExitSameWeeklyPlan', { contractId: contractId, periodId: periodId });
        }

    }

    return {
        getPeriods: getPeriods,
        getContracts: getContracts,
        getWeeklyItem: getWeeklyItem,
        getWeeklyPlanItem:getWeeklyPlanItem,
        getWeeklyItemDetails: getWeeklyItemDetails,
        getWeeklyPlanDetails: getWeeklyPlanDetails,
        getOperationsOnFarmsFilterContract: getOperationsOnFarmsFilterContract,
        getMainOperations:getMainOperations,
        getSubOperations: getSubOperations,
        getWeeklyDetailPrevius:getWeeklyDetailPrevius,
        save: save,
        savePlan:savePlan,
        canApprove: canApprove,
        getHistories:getHistories,
        approve: approve,
        reject:reject,
        isExitSameWeeklyReport: isExitSameWeeklyReport,
        isExitSameWeeklyPlanReport: isExitSameWeeklyPlanReport

       
    };

});