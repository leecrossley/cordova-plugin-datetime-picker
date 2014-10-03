## Date Time Picker Plugin

**for Apache Cordova (Windows Phone 8)**

**There is an issue with this plugin and the latest version of Cordova, please see [issue #2](https://github.com/leecrossley/cordova-plugin-datetime-picker/issues/2)**

## Install

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

Once the date is selected, the callback function `onDateSelected` will log the selected date (as a JavaScript Date object) to the console.

## Platforms

Windows Phone 8 support only.

### Credits

This plugin is based on code written by sbregnov and hypermurea.

## License

[MIT License](http://ilee.mit-license.org)
