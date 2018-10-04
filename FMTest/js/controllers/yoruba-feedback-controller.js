angular
    .module('YorubaIntonationApp')
    .controller('YorubaFeedbackController', function ($scope, YorubaFeedbackService, $window, ngToast) {
        activate();

        function activate() {
            $scope.feedback = {};
        }

        $scope.feedback = {};

        $scope.submitFeedback = function () {
            YorubaFeedbackService
                .submitFeedback($scope.feedback)
                .then(feedbackSuccessFn, feedbackErrorFn);

            function feedbackSuccessFn(data, status, headers, config) {
                ngToast.create({
                    content: "<p>Thanks for reaching out to me. I will get back to you in earnest.</p>",
                    className: 'success',
                    timeout: '5000'
                });

                $scope.feedback = {};
            }

            function feedbackErrorFn() {
                ngToast.create({
                    content: "<p>I failed you</p> <p>I'm sorry</p> <p>You can send me a direct e-mail. Just look around this page for an envelope and click it</p>",
                    className: 'danger',
                    timeout: '5000'
                });
            }
        };
    });