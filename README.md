## Date Time Picker Plugin

**for Apache Cordova (Windows Phone 8)**

## Install

#### Latest published version on npm (with Cordova CLI >= 5.0.0)

```
cordova plugin add cordova-plugin-datetime-picker
```

#### Latest version from GitHub

```
cordova plugin add https://github.com/leecrossley/cordova-plugin-datetime-picker.git
```

Currently requires you to include the Microsoft.Phone.Controls.Toolkit and Coding4Fun.Toolkit.Controls (both available via nuget), although I'm working to break this dependency.

Coding4Fun.Toolkit.Controls is used for the timepicker, as the default WPToolkit implementation does not support intervals (steps), such as being able to only select 15 minute intervals.

## Usage

You **do not** need to reference any JavaScript, the Cordova plugin architecture will add a datetimepicker object to your root automatically when you build.

Ensure you use the plugin after your deviceready event has been fired.

### Example

```js
var onDateSelected = function (date) {
    console.log(new Date(parseInt(date, 10)));
}

datetimepicker.selectDate(onDateSelected);
```

var onTimeSelected = function (time) {
    console.log(new Date(parseInt(time, 10)));
}

datetimepicker.selectTime(onTimeSelected);
```

Once the date is selected, the callback function `onDateSelected` will log the selected date (as a JavaScript Date object) to the console.

### Screenshot

![Screenshot](https://raw.githubusercontent.com/leecrossley/cordova-plugin-datetime-picker/master/screenshot.jpg)

## Platforms

Windows Phone 8 support only.

### Credits

This plugin is based on code written by sbregnov and hypermurea.

## License

[MIT License](http://ilee.mit-license.org)
