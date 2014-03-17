using System;
using System.Runtime.Serialization;
using Microsoft.Phone.Tasks;

namespace WPCordovaClassLib.Cordova.Commands
{
    public class DateTimePicker : BaseCommand
    {
        private DateTimePickerTask _dateTimePickerTask;
        private DateTimePickerOptions _dateTimePickerOptions;

        [DataContract]
        public class DateTimePickerOptions
        {
            [DataMember(IsRequired = false, Name = "value")]
            public DateTime Value { get; set; }

            [DataMember(IsRequired = false, Name = "step")]
            public int Step { get; set; }

            public DateTimePickerOptions()
            {
                SetDefaultValues(new StreamingContext());
            }

            [OnDeserializing]
            public void SetDefaultValues(StreamingContext context)
            {
                Value = DateTime.Now;
                Step = 60;
            }
        }

        public void selectDate(string options)
        {
            try 
            {
                if (!GetDefaults(options)) return;

                _dateTimePickerTask = new DateTimePickerTask { Value = _dateTimePickerOptions.Value };

                _dateTimePickerTask.Completed += dateTimePickerTask_Completed;
                _dateTimePickerTask.Show(DateTimePickerTask.DateTimePickerType.Date);
            }
            catch (Exception e)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, e.Message));
            }
        }

        public void selectTime(string options)
        {
            try
            {
                if (!GetDefaults(options)) return;

                _dateTimePickerTask = new DateTimePickerTask
                    {
                        Value = _dateTimePickerOptions.Value,
                        Step = _dateTimePickerOptions.Step
                    };

                _dateTimePickerTask.Completed += dateTimePickerTask_Completed;
                _dateTimePickerTask.Show(DateTimePickerTask.DateTimePickerType.Time);
            }
            catch (Exception e)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, e.Message));
            }
        }

        private bool GetDefaults(string options)
        {
            try
            {
                _dateTimePickerOptions = new DateTimePickerOptions();
                var args = JSON.JsonHelper.Deserialize<string[]>(options);
                var value = args[0];
                var step = args.Length > 2 ? args[1] : null;

                if (!String.IsNullOrEmpty(value))
                {
                    _dateTimePickerOptions.Value = FromUnixTime(long.Parse(value));
                }
                if (!String.IsNullOrEmpty(step))
                {
                     _dateTimePickerOptions.Step = int.Parse(step);
                }
            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION, ex.Message));
                return false;
            }
            return true;
        }

        private static DateTime FromUnixTime(long unixtime)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dateTime.AddMilliseconds(unixtime).ToLocalTime();
        }

        private void dateTimePickerTask_Completed(object sender, DateTimePickerTask.DateTimeResult e)
        {
            if (e.Error != null)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR));
                return;
            }            

            switch (e.TaskResult)
            {
                case TaskResult.OK:
                    try
                    {
                        var result = (long) e.Value.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                        DispatchCommandResult(new PluginResult(PluginResult.Status.OK, result + ""));
                    }
                    catch (Exception ex)
                    {
                        DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "Datetime picker error. " + ex.Message));
                    }
                    break;

                case TaskResult.Cancel:
                    DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "Cancelled."));
                    break;               
            }

            _dateTimePickerTask = null;
        }       

    }
}