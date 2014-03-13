
var exec = require("cordova/exec");

var DateTimePicker = function () {
    this.name = "DateTimePicker";
};

DateTimePicker.prototype.selectDate = function (onSuccess, onError, value) {
    exec(onSuccess.bind(null), onError, "DateTimePicker", "selectDate", [value]);
};

DateTimePicker.prototype.selectTime = function (onSuccess, onError, value) {
    exec(onSuccess.bind(null), onError, "DateTimePicker", "selectTime", [value]);
};

module.exports = new DateTimePicker();