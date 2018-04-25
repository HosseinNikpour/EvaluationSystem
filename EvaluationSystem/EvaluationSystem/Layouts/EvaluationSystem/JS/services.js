/// <reference path="../FoodOrderPage.aspx" />
/// <reference path="../services.aspx" />

app.factory('evalSrv', ['serverCall', function (serverCall) {
    {

        function getContractIndexes(contractId) {
            return serverCall('../NewForm.aspx/GetContractIndexes', {contractId:contractId});
        }
        
        function getEvaluationTypes()
        {
            return serverCall("/_api/web/lists(guid'765D3B7A-D216-493C-A875-CB54033BC6D0')/items");
        }
        function getPeriods() {
            return serverCall("/_api/web/lists(guid'6DD6B913-0E00-46A1-83F3-DC76164BAC6C')/items");
        }
        //todo:change list id and query
        function getContracts(type) {
            var url = "/_api/web/lists(guid'0179B06A-7CE9-42C1-8273-8F2B9CBB9FD2')/items"
            if (type) 
                url += "?$filter=(ContractType eq '" + type.ContractType + "')" + " and (Status eq 'جاری')";

            return serverCall(url);
        }

        function getCompanies(type) {
            var url = "/_api/web/lists(guid'05FD239F-C7E4-41BF-BB59-1B6FAC4F4645')/items"
            if (type)
                url += "?$filter=CompanyType/Id eq '" + type.ContractType + "'"

            return serverCall(url);
        }


        function createEvaluationContracts(evaltype, contractId,period,isCompany) {

            return serverCall('../services.aspx/CreateEvaluationContracts', { evalTypeTitle: evaltype, contractsId: contractId, periodId:period, isCompany: isCompany });
        }
        function getEvaluationContract(id,listname)
        {

            return serverCall('../services.aspx/GetEvaluationContract', { id:id,listName:listname});
        }
        function saveEvaluation(evalContract, isTemp,listname,isCompany)
        {
            return serverCall('../services.aspx/SaveEvaluation', { evalContract: evalContract, isTemp: isTemp,listName:listname,isCompany:isCompany });
        }
        function canApprove(itemid,listname,isCompany)
        {
            return serverCall('../services.aspx/CanApprove', { itemId:itemid,  listName:listname,isCompany:isCompany});
        }
        function approve(id, listname,comment,isCompany)
        {
            return serverCall('../services.aspx/Approve', { itemId: id, listName: listname,comment:comment,isCompany:isCompany });
        }
        function reject(id, listname,comment,isCompany)
        {
            return serverCall('../services.aspx/Reject', { itemId: id, listName: listname,comment:comment,isCompany:isCompany });
        }
        return {
            getContractIndexes: getContractIndexes,
            getEvaluationTypes: getEvaluationTypes,
            getPeriods:getPeriods,
            getContracts: getContracts,
            createEvaluationContracts: createEvaluationContracts,
            getEvaluationContract: getEvaluationContract,
            getCompanies:getCompanies,
            saveEvaluation: saveEvaluation,
            canApprove: canApprove,
            approve: approve,
            reject:reject
        };
    }
}]);