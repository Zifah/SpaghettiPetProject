angular
    .module('YorubaIntonationApp')
    .controller('YorubaIntonationController', function ($scope, YorubaIntonationService, $window, ngToast) {
        activate();

        function activate() {
            //$scope.greeting = YorubaIntonationService.getGreeting();
            $scope.editing = true;
        }

        $scope.processParagraph = processParagraph;
        $scope.setIndex = setIndex;
        $scope.setCurrentSyllable = setCurrentSyllable;
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
            $scope.setCurrentSyllable(0);
        }

        function setCurrentSyllable(index) {
            $scope.syllableIndex = index;
            $scope.currentSyllable = $scope.syllables[$scope.words[$scope.currentWordIndex]][index];
            $scope.currentSyllableForms = $scope.syllableForms[$scope.currentSyllable];

            if ($scope.currentSyllableForms.length === 1) {
                setPreferredForm(newSyllableForms[0]);
            }

            setSyllableFormLabels();
        }

        function setSyllableFormLabels() {
            $scope.emboldenEvenLabels = false;
            if ($scope.currentSyllableForms.length === 3) {
                $scope.syllableFormLabels = ["do", "re", "mi"];
            } else if ($scope.currentSyllableForms.length === 6) {
                $scope.syllableFormLabels = ["do", "DO", "re", "RE", "mi", "MI"];
                $scope.emboldenEvenLabels = true;
            } else {
                $scope.syllableFormLabels = [];
            }
        }

        function setPreferredForm(form) {
            var currentWordFinalForm = $scope.finalForm[$scope.currentWordIndex];
            var wordsLength = $scope.words.length;
            var syllableLength = currentWordFinalForm.length;

            $scope.finalForm[$scope.currentWordIndex][$scope.syllableIndex] = form;

            var isCurrentWordCompleted = $scope.syllableIndex === syllableLength - 1;
            if (isCurrentWordCompleted) {
                doWordFinishedAction();
            }

            else {
                // Move to the next syllable of the current word
                $scope.setCurrentSyllable($scope.syllableIndex + 1);
            }
        }

        /*
         * When a word has been tone marked, do we want to:
         * 1. Jump to the next word in the paragraph or 
         * 2. Conclude tonemarking, since the current word is the last word in the paragraph
         */
        function doWordFinishedAction() {
            var isCurrentWordTheLast = $scope.currentWordIndex === $scope.words.length - 1;
            if (isCurrentWordTheLast) {
                finish();
            }

            else {
                // Move to the next word of the paragraph
                $scope.setIndex($scope.currentWordIndex + 1);
                /*
                 * 1. Get the current word
                 * 2. Check if it exists previously in the array of words
                 * 3. If it doesn't, continue
                 * 4. Otherwise go into another method (Choose previous form)
                 * 
                 */
            }
        }


        function showPreviousForms() {
            // Important. How to deal with different cases?

            /*
             * 1. Display all previous forms of the word on a pop-up
             * 2. If the user chooses one, write over the final form entry of the current word with the final form entry of the chosen one, then either move to the next word or finish
             * 3. Give the user an option to close the pop-up as well
             * 4. 
             */
        }

        /*
         * Write marked form of previous word form on current word and call doWordFinishedAction()
         */
        function choosePreviousForm() {

        }

        /*
         * Concatenate all the tone marked parts of the paragraph and display
         */
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
        }
    });