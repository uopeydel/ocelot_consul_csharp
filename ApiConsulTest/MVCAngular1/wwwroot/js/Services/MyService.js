var app = angular.module('myApp', []);
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

app = app.service('SomeService', function () {
    this.someFunction = function () { console.log("some function 1 is debug");};
});

app = app.service('SomeService1', function () {
    this.someFunction = function () { console.log("some function 2 is debug");};
});