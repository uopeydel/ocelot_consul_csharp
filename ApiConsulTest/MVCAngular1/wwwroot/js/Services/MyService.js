
app = app
    .factory('MyService', function () {

        // private
        var value = 0;

        // public
        return {

            getValue: function () {
                return value;
            },

            setValue: function (val) {
                value = val;
            }
        };
    });

app = app
    .factory('MyService2', function () {

        // private
        var value = 0;

        // public
        return {

            getValue: function () {
                return value + "x";
            },

            setValue: function (val) {
                value = val;
            }

        };
    });

app = app.service('SomeService', function ($http) {
    this.someFunction = function () {
        $http.get('https://localhost:5001/api/values').then(function (data) {
            console.log("data response 1 => ", data);
        });
        console.log("some function 1 is debug");
    };
});

app = app.service('SomeService1', function ($http) {
    this.someFunction = function () {
        $http.get('https://localhost:5001/api/values').then(function (data) {
            console.log("data response 2 => ", data);
        });
        console.log("some function 2 is debug");
    };
});