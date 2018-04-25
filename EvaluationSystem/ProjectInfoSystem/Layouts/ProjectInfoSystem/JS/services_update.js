

app.factory('operationSrv', ['serverCall', function (serverCall) {
    {

        function getContractInfoItems() {
            return serverCall('../services.aspx/GetContractInfoItems', {});
        }
        function getContracts() {
            return serverCall("/projectsinfo/_api/web/lists(guid'29e865cf-792e-48b7-b28c-3e634e090d76')/items");
        }
        function getBlocks(contractId) {

            var url = "/projectsinfo/_api/web/lists(guid'E1064C12-79E8-4B1E-843A-BC448A5654AF')/items";
            if (contractId)
                url += "?$filter=Contract/Id eq " + contractId + ""

            return serverCall(url);
        }
        function getFarms(contractId) {

            var url = "/projectsinfo/_api/web/lists(guid'B2FD6798-DE86-46E8-B83D-A122F574225D')/items";
            if (contractId)
                url += "?$filter=Contract/Id eq " + contractId + ""
            return serverCall(url);
        }
        function getMainOperations() {
            return serverCall("/projectsinfo/_api/web/lists(guid'CA43400F-28C4-4525-B38E-7E248F3DFD4B')/items");
        }
        function getSubOperations() {
            return serverCall("/projectsinfo/_api/web/lists(guid'0DEB6337-7B57-4392-90D0-8464F588D8B8')/items");
        }
        function getContractOperations(contractId) {
            var url = "/projectsinfo/_api/web/lists(guid'4DF5A39F-5857-4908-99A3-5D8B4FF4DAA8')/items?$select=Id,Title,ExecutiveOperationId,ExecutiveOperation/Title,SubExecutiveOperation,Measurement,ChangeValue,ChangeAmount,Amount,OrgValue,Farm/Id&$expand=ExecutiveOperation&$top=999999999";
            if (contractId)
                url += "&$filter=Contract/Id eq " + contractId + "";
            return serverCall(url);
        }
        function getContractLevelOperation(contractId) {
            var url = "/projectsinfo/_api/web/lists(guid'37779A27-883D-44C5-9B24-4E3B99786776')/items?$select=Id,Title,BaseContractOprId,BaseContractOpr/Title,ChangeValue,ChangeAmount,Amount,Value&$expand=BaseContractOpr";
            if (contractId)
                url += "&$filter=Contract/Id eq " + contractId + "";
            return serverCall(url);
        }
        function getActivityOperations(id) {

            var url = "/projectsinfo/_api/web/lists(guid'4DF5A39F-5857-4908-99A3-5D8B4FF4DAA8')/items?$select=Id,Title,ContractId,ExecutiveOperationId,ExecutiveOperation/Title,SubExecutiveOperation,Measurement,ChangeValue,ChangeAmount,Amount,OrgValue&$expand=ExecutiveOperation&$top=999999999";
            if(id)
                url += "&$filter=Contract/Id ne " + id + "";
            return serverCall(url);
        }
        function save(items,contractItems) {

            return serverCall('../services.aspx/SaveItems', {items:items,contractItems:contractItems});
        }
        
        return {
            getContractInfoItems: getContractInfoItems,
            getContracts: getContracts,
            getBlocks: getBlocks,
            getFarms: getFarms,
            getMainOperations: getMainOperations,
            getSubOperations: getSubOperations,
            getContractOperations: getContractOperations,
            getContractLevelOperation:getContractLevelOperation,
            getActivityOperations: getActivityOperations,
            save:save

        };
    }
}]);