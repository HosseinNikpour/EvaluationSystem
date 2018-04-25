app.factory('service', function ($q, $http) {
    {

        var listIds = {
            Contracts: '2EB3D67F-A213-42A5-8838-F0452C1DD3FC',
            Blocks: 'C0E59060-B66D-4780-A1EA-80FB42DB64DB',
            Farms: '03728037-27C9-47B5-8EDA-0808ADD4EFC1',
            MainOperations: 'F01D9E87-4CBC-4A9B-B84A-0231CB7FF7FE',
            SubOperations: 'D94ED6B0-858B-4CAB-9CFE-8CD24A0E3803',
            WeeklyOperations: '9EC43D77-43E6-428B-93DB-767470E17FD4',
            OperationsOnContracts: 'E06A16C1-961D-442B-B052-6DA40CE674D1',
            OperationsOnFarms: 'CEE1C5B1-3D18-4493-9934-CC0187B4D633',
            PeriodWeeklies: '335AC69A-BBAD-4602-A308-334DCAF9C797',
            WeeklyOperationsDetails: '3897173A-2299-4FA7-8175-64066F582BDB'
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

        function getContractPeriod() {
            return $q.all([getPeriod(), getContracts()])
             .then(function (a) {
                 var Period = a[0].data.d.results;
                 var Contract = a[1].data.d.results;
                 return { Period: Period, Contract: Contract }

             })
        }


        function getPeriod() {
            return serverCall("/sites/JMIS/_api/web/lists(guid'" + listIds.PeriodWeeklies + "')/items");
        }

        function getContracts(id) {
            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.Contracts + "')/items";
            url = id ? url + "?$filter=Id eq " + id : url;
            return serverCall(url);
        }

        //get contract 
        function getContractData(contractId) {
            return $q.all([getFarms(contractId), getBlocks(contractId), getContracts(contractId)])
             .then(function (a) {
                 var farms = a[0].data.d.results;
                 var blocks = a[1].data.d.results;
                 var contract = a[2].data.d.results;
                 return { farms: farms, blocks: blocks, contract: contract }
             });
        }

        //get all Operations on farm
        function getOperationData(contractId, farmId, periodId) {
            return $q.all([getWeeklyOperations(contractId, farmId, periodId), getContractOperationsFilter(farmId)])
            .then(function (a) {
                var WeeklyOperations = a[0].data.d.results;
                var contractOperations = a[1].data.d.results;


                return {
                    contractOperations: contractOperations,
                    WeeklyOperations: WeeklyOperations
                    // gfhgfhg:asdsa
                }
            });

        }



        //get all Operations on contract
        function getOperationDataOnContract(contractId) {
            return $q.all([getWeeklyOperations(contractId), getOperationsOnContracts(contractId)])
            .then(function (a) {
                var WeeklyOperations = a[0].data.d.results;
                var contractOperations = a[1].data.d.results;


                return {
                    contractOperations: contractOperations,
                    WeeklyOperations: WeeklyOperations

                }
            });

        }


        //start getweekly opration details
        function getWeeklyOperations(contractId, farmId, periodId) {
            //var viewXml = '<View Scope="RecursiveAll"></View>';
            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.WeeklyOperationsDetails + "')/getitems";
            var queryPayload = {
                'query': {
                    '__metadata': { 'type': 'SP.CamlQuery' },
                    ViewXml: "<View Scope='RecursiveAll'></View>",
                    FolderServerRelativeUrl: "/sites/JMIS/Lists/WeeklyConstructionsDetails/" + contractId
                }
            };
            return serverCall(url, queryPayload);

        }
        //end getweekly opration details


        // start get data for display & edit  form 
        function DisplayFormdata(requestId) {
            return $q.all([getWeeklyOperationsMaster(requestId), getWeeklyOperationsDetails(requestId)])
            var masterDataForm = a[0].data.d.results;
            var DetailDataForm = a[1].data.d.results;
            return {
                masterDataForm: masterDataForm,
                masterDataForm: masterDataForm
            }
        }


        function getWeeklyOperationsDetails(requestId) {
            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.WeeklyOperationsDetails + "')/items?$select=Id,Title,Measurement,Constructed,SubOperation/Id,SubOperation/Title,WeeklyConstruction/Id,WeeklyConstruction/Title,Operation/Id,Operation/Title$expand=SubOperation/Id,SubOperation/Title,WeeklyConstruction/Id,WeeklyConstruction/Title,Operation/Id,Operation/Title&$top=999999999&$filter=WeeklyConstruction/Id eq " + requestId + "";
            return serverCall(url);

        }

        function getWeeklyOperationsMaster(requestId) {
            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.WeeklyOperations + "')/items?$select=Id,Title,Contract/Id,Contract/Title,Period/Id,Period/Title,Status,IsContractLevel,Farm/Id,Farm/Title,Block/Id,Block/Title$expand=Contract/Id,Contract/Title,Period/Id,Period/Title,Farm/Id,Farm/Title,Block/Id,Block/Title&$top=999999999&$filter=ID eq " + requestId + "";
            return serverCall(url);

        }
        //end get data for display & edit  form




        function getOperationsOnContracts(contractId) {
            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.OperationsOnContracts + "')/items?$select=Id,Title,Contract,Contract/Id,Operation,Operation/Id,Operation/Title,Measurement,FirstVolume,FinalVolume,FirstCost,FinalCost,ItemWeight,ItemWeightOperation,TotalItemWeight,TotalWeightOperation,EqAcre&$expand=Contract,Contract/Id,Operation,Operation/Id,Operation/Title&$top=999999999&$filter=Contract/Id eq " + contractId + "";
            return serverCall(url);
        }

        function getContractOperationsFilter(farmId) {
            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.OperationsOnFarms + "')/items?$select=Id,Title,Farm,Farm/Id,Block,Block/Id,Contract,Contract/Id,Operation,Operation/Id,Operation/Title,SubOperation/Title,SubOperation,SubOperation/Id,Measurement,FirstVolume,FinalVolume,FirstCost,FinalCost,ItemWeight,ItemWeightOperation,TotalItemWeight,TotalWeightOperation,EqAcre&$expand=Farm,Farm/Id,Block,Block/Id,Contract,Contract/Id,Operation,Operation/Id,Operation/Title,SubOperation/Title,SubOperation,SubOperation/Id&$top=999999999&$filter=Farm/Id eq " + farmId + "";
            return serverCall(url);
        }
        function getMainOperations() {
            return serverCall("/sites/JMIS/_api/web/lists(guid'" + listIds.MainOperations + "')/items");
        }
        function getSubOperations() {
            return serverCall("/sites/JMIS/_api/web/lists(guid'" + listIds.SubOperations + "')/items");
        }
        function getContractOperations(contractId) {

            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.WeeklyOperationsDetails + "')/items";
            return serverCall(url);
        }


        function getFarms(contractId) {

            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.Farms + "')/items";
            if (contractId)
                url += "?$filter=Contract/Id eq " + contractId + ""
            return serverCall(url);
        }
        function getBlocks(contractId) {

            var url = "/sites/JMIS/_api/web/lists(guid'" + listIds.Blocks + "')/items";
            if (contractId)
                url += "?$filter=Contract/Id eq " + contractId + ""

            return serverCall(url);
        }


        //ba estefade az method save fieldha saveFields
        function saveformsoperations(weeklyItem) {

            return serverCall('/_layouts/15/ProjectInfoSystem/services.aspx/SaveWeeklyReport', { weeklyItem: weeklyItem });
        }


    }

    return {
        getContractData: getContractData,
        getOperationData: getOperationData,
        getOperationDataOnContract: getOperationDataOnContract,
        getContractPeriod: getContractPeriod,
        getContractOperations: getContractOperations,
        DisplayFormdata: DisplayFormdata,
        saveformsoperations: saveformsoperations,
    };

});