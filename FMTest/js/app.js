angular.module('YorubaIntonationApp', ['blockUI', 'ngToast', 'ngClickCopy', 'angularModalService'])
    .config(['ngToastProvider', function (ngToastProvider) {
        ngToastProvider.configure({
            horizontalPosition: 'center', // or 'fade',
            verticalPosition: 'top'
        });
    }])
    .config(["blockUIConfig", function (blockUIConfig) {
        blockUIConfig.requestFilter = function (config) {
            //Perform a global, case-insensitive search on the request url for 'noblockui' ...
            if (config.url.match(/noblockui/gi)) {
                return false; // ... don't block it.
            }
        };
    }])
    .controller('JsEditorController', function ($scope, JsEditorService, $window) {
        $scope.processArray = processArray;
        $scope.columnChanged = columnChanged;
        $scope.addRow = addNewRow;
        $scope.addColumn = addColumn;
        $scope.hiddenProps = [];
        $scope.root = {
            editing: null
        };
        $scope.deletedRows = [];
        $scope.deleteRow = deleteRow;
        $scope.includeRow = includeRow;
        $scope.finish = finish;

        function processArray() {
            $scope.arrayText = $scope.input;
            $scope.array = JSON.parse($scope.arrayText);
            $scope.backupArray = angular.copy($scope.array);
            $scope.arrayProps = [];

            for (var i = 0; i < $scope.array.length; i++) {
                var member = $scope.array[i];
                for (property in member) {
                    if (member.hasOwnProperty(property) && $scope.arrayProps.indexOf(property) == -1) {
                        $scope.arrayProps.push(property);
                    }
                }
            }

            //$scope.backupArrayProps = angular.copy($scope.arrayProps);
        }

        function columnChanged(column) {
            var index = $scope.hiddenProps.indexOf(column);
            if (index == -1) {
                $scope.hiddenProps.push(column);
            }

            else {
                $scope.hiddenProps.splice(index, 1);
            }
        }

        function addNewRow() {
            $scope.array.push({});
        }

        function addColumn() {
            if ($scope.newColumn) {
                $scope.arrayProps.push($scope.newColumn);
                $scope.newColumn = '';
            }
        }

        function deleteRow(rowIndex) {
            $scope.deletedRows.push(rowIndex);
        }

        function includeRow(rowIndex) {
            $scope.deletedRows.splice($scope.deletedRows.indexOf(rowIndex), 1);
        }

        function finish() {
            // Retrieve the array
            $scope.outputArray = []; //= angular.copy($scope.array);

            // Remove all deletedRows
            var index = 0;
            for (var item in $scope.array) {
                if ($scope.deletedRows.indexOf(index) == -1)
                    $scope.outputArray.push(angular.copy($scope.array[index]));
                index++;
            }

            // Remove instance of hidden props in each object of array
            $scope.outputArray.forEach(function (v) {
                $scope.hiddenProps.forEach(function (w) {
                    if (v.hasOwnProperty(w))
                        delete v[w];
                });
            });

            $scope.finished = true;
            $scope.outputArrayText = JSON.stringify($scope.outputArray, undefined, 4);
            $window.location.href = "#page-wrapper";
        }
    })
    .factory('JsEditorService', function ($http) {
        var service = {};
        return service;
    })
    .factory('focus', function ($timeout, $window) {
        return function (id) {
            // timeout makes sure that it is invoked after any other event has been triggered.
            // e.g. click events that need to run before the focus or
            // input elements that are in a disabled state but are enabled when those events are triggered
            $timeout(function () {
                var element = $window.document.getElementById(id);
                if (element)
                    element.focus()
            });
        };
    })
    .directive('eventFocus', function (focus) {
        return function (scope, elem, attr) {
            elem.on(attr.eventFocus, function () {
                focus(attr.eventFocusId);
            });

            scope.$on('$destroy', function () {
                elem.off(attr.eventFocus);
            });
        };
    })
    .directive('jsonText', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                function into(input) {
                    return JSON.parse(input);
                }
                function out(data) {
                    return JSON.stringify(data);
                }
                ngModel.$parsers.push(into);
                ngModel.$formatters.push(out);

            }
        };
    });