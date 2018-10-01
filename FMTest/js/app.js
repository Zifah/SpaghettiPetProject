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
    .controller('YorubaIntonationController', function ($scope, YorubaIntonationService, $window, ngToast, ModalService) {
        activate();

        function activate() {
            //$scope.greeting = YorubaIntonationService.getGreeting();
            $scope.editing = true;
        }

        $scope.processParagraph = processParagraph;
        $scope.setIndex = setIndex;
        $scope.setSyllableIndex = setSyllableIndex;
        $scope.setPreferredForm = setPreferredForm;
        $scope.finish = finish;
        $scope.reload = reload;
        $scope.restart = restart;
        $scope.notifyTextCopied = notifyTextCopied;
        $scope.syllableIndex = 0;
        $scope.finalForm = [];
        $scope.getRecentlyProcessed = getRecentlyProcessed;

        function restart() {
            $scope.editing = true;
            $scope.completed = false;
        }

        function getRecentlyProcessed() {
            YorubaIntonationService
                .getRecentlyProcessed()
                .then(paragraphSuccessFn, paragraphErrorFn);

            function paragraphSuccessFn(data, status, headers, config) {
                $scope.recentlyProcessed = data.data;
            }

            function paragraphErrorFn() {
                ngToast.create({
                    content: "Oops! Could not get recently processed Yoruba words. Please try again in a few minutes.",
                    className: 'danger',
                    timeout: '5000'
                });
            }
        }

        function processParagraph() {
            YorubaIntonationService
                .processParagraph($scope.paragraph)
                .then(paragraphSuccessFn, paragraphErrorFn);

            function paragraphSuccessFn(data, status, headers, config) {
                var p = data.data;
                $scope.words = p.words;
                $scope.syllables = p.wordSyllables;
                $scope.syllableForms = p.syllableForms;
                $scope.paragraphId = p.id;
                $scope.finalForm = [];

                for (var i in $scope.words) {
                    var currentWord = $scope.words[i];
                    $scope.finalForm.push(angular.copy($scope.syllables[currentWord]));
                }

                $scope.setIndex(0);
                $window.location.href = "#divCurrentWord";
            }

            function paragraphErrorFn() {
                ngToast.create({
                    content: "Oops! Error processing paragraph.",
                    className: 'danger',
                    timeout: '5000'
                });
            }
        }

        function setIndex(index) {
            $scope.currentWordIndex = index;
            $scope.setSyllableIndex(0);
        }

        function setSyllableIndex(index) {
            $scope.syllableIndex = index;
            var newSyllableForms = $scope.syllableForms[$scope.syllables[$scope.words[$scope.currentWordIndex]][index]];
            if (newSyllableForms.length == 1) {
                setPreferredForm(newSyllableForms[0]);
            }
        }

        function setPreferredForm(form) {
            var currentWordFinalForm = $scope.finalForm[$scope.currentWordIndex];
            var wordsLength = $scope.words.length;
            var syllableLength = currentWordFinalForm.length;

            $scope.finalForm[$scope.currentWordIndex][$scope.syllableIndex] = form;

            if ($scope.syllableIndex == syllableLength - 1) {
                if ($scope.currentWordIndex == wordsLength - 1) {
                    finish();
                }

                else {
                    $scope.setIndex($scope.currentWordIndex + 1);
                }
            }

            else {
                $scope.setSyllableIndex($scope.syllableIndex + 1);
            }
        }

        function finish() {
            var finalWords = [];
            for (var i = 0; i < $scope.finalForm.length; i++) {
                finalWords.push($scope.finalForm[i].join(""));
            }

            $scope.finalParagraph = finalWords.join(" ");
            $scope.completed = true;
            $scope.editing = false;
            $window.location.href = "#page-wrapper";


            YorubaIntonationService
                .notifyCompleted($scope.paragraphId, $scope.finalParagraph);

            ngToast.create({
                content: "<strong>You are done!</strong>",
                className: 'info',
                timeout: '2000'
            });
        }

        function reload() {
            $window.location.reload();
        }

        function notifyTextCopied() {
            ngToast.create({
                content: "The tone marked Yoruba text is now on your clipboard. <strong>Jọ̀ọ́ ṣá mi látẹ̀wọ́ọ́!</strong>",
                className: 'info',
                timeout: '5000'
            });

            ModalService.showModal({
                template: "<div>Fry lives in {{futurama.city}}</div>",
                controller: function (close) {
                    this.city = "New New York";
                },
                controllerAs: "futurama"
            }).then(function (modal) {
                // The modal object has the element built, if this is a bootstrap modal
                // you can call 'modal' to show it, if it's a custom modal just show or hide
                // it as you need to.
                modal.element.modal();
                modal.close.then(function (result) {
                    $scope.message = result ? "You said Yes" : "You said No";
                });
            });
        }
    })
    .factory('YorubaIntonationService', function ($http) {
        var service = {
            getGreeting: getGreeting,
            processParagraph: processParagraph,
            notifyCompleted: notifyCompleted,
            getRecentlyProcessed: getRecentlyProcessed
        };

        function getGreeting() {
            return "Hello world";
        }

        function processParagraph(word) {
            return $http.post('api/yoruba/paragraph/derivatives', { content: word });
        }

        function notifyCompleted(id, finalForm) {
            return $http.put('api/yoruba/paragraph/derivatives/?noblockui', { id: id, output: finalForm });
        }

        function getRecentlyProcessed() {
            return $http.get('/api/yoruba/paragraph/derivatives');
        }

        return service;
    })
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
                if($scope.deletedRows.indexOf(index) == -1)
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