angular
    .module('YorubaIntonationApp')
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
    });