## Date Time Picker Plugin

**for Apache Cordova (Windows Phone 8)**

## Install

```
cordova plugin add https://github.com/leecrossley/cordova-plugin-datetime-picker.git
```

Currently requires you to include the WPtoolkit (available via nuget), although I'm working to break this dependency.

## Usage

You **do not** need to reference any JavaScript, the Cordova plugin architecture will add a datetimepicker object to your root automatically when you build.

Ensure you use the plugin after your deviceready event has been fired.

## Platforms

Windows Phone 8 support only.

### Credits

This plugin is based on code written by sbregnov and hypermurea.

## License

[MIT License](http://ilee.mit-license.org)