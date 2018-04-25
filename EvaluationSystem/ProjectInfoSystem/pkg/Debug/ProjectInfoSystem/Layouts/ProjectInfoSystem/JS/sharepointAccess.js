app.factory('SharePointService', ['$q', function ($q) {
    var SharePointService = {};
    SharePointService.executeQuery = function (context) {
        var deferred = $q.defer();
        context.executeQueryAsync(deferred.resolve, function (o, args) {
            deferred.reject(args);
        });
        return deferred.promise;
    };
    return SharePointService;
}]);