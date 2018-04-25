app.factory('serverCall', ['$http', function ($http) {
	

	return function (address, param) {
		return $http({
			method: param? 'POST':'GET',
			url: address,
			headers: {
				'Content-Type': 'application/json;odata=verbose',
				'Accept': 'application/json;odata=verbose'
			},
			data: param
		});
	}
}]);