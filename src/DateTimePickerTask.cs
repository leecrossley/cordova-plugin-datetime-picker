using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Coding4Fun.Toolkit.Controls.Primitives;
using Microsoft.Phone.Controls.Primitives;
using System.Windows.Navigation;

namespace Cordova.Extension.Commands
{
    public class DateTimePickerTask
    {
        private PhoneApplicationFrame _frame;
        private object _frameContentWhenOpened;
        private NavigationInTransition _savedNavigationInTransition;
        private NavigationOutTransition _savedNavigationOutTransition;
        private IDateTimePickerPage _dateTimePickerPage;
        private ITimeSpanPickerPage<TimeSpan> _timeSpanPickerPage;

        public enum DateTimePickerType
        {
            Date = 0,
            Time = 1
        }

        public class DateTimeResult : TaskEventArgs
        {
            public DateTimeResult(TaskResult taskResult) : base(taskResult) { }

            public DateTime? Value { get; internal set; }
        }

        public event EventHandler<DateTimeResult> Completed;

        public DateTime? Value { get; set; }

        public int? Step { get; set; }

        public void Show(DateTimePickerType type)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => OpenPickerPage(type));
        }

        private void OpenPickerPage(DateTimePickerType type)
        {
            var page = type == DateTimePickerType.Date
                           ? "/Microsoft.Phone.Controls.Toolkit;component/DateTimePickers/DatePickerPage.xaml?dummy="
                           : "/Coding4Fun.Toolkit.Controls;component/ValuePicker/TimeSpanPicker/TimespanPickerPage.xaml?dummy=";

            page += Guid.NewGuid();

            var pickerPageUri = new Uri(page, UriKind.Relative);

            if (_frame != null) return;
            _frame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (_frame == null) return;

            _frameContentWhenOpened = _frame.Content;

            var uiElement = _frameContentWhenOpened as UIElement;
            if (null != uiElement)
            {
                _savedNavigationInTransition = TransitionService.GetNavigationInTransition(uiElement);
                TransitionService.SetNavigationInTransition(uiElement, null);
                _savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(uiElement);
                TransitionService.SetNavigationOutTransition(uiElement, null);
            }

            if (type == DateTimePickerType.Date)
            {
                _frame.Navigated += OnDateFrameNavigated;
            }
            else
            {
                _frame.Navigated += OnTimeFrameNavigated;
            }
            _frame.NavigationStopped += OnFrameNavigationStoppedOrFailed;
            _frame.NavigationFailed += OnFrameNavigationStoppedOrFailed;

            _frame.Navigate(pickerPageUri);
        }

        private void ClosePickerPage()
        {
            if (_frame != null)
            {
                _frame.Navigated -= OnDateFrameNavigated;
                _frame.Navigated -= OnTimeFrameNavigated;
                _frame.NavigationStopped -= OnFrameNavigationStoppedOrFailed;
                _frame.NavigationFailed -= OnFrameNavigationStoppedOrFailed;

                var uiElement = _frameContentWhenOpened as UIElement;
                if (uiElement != null)
                {
                    TransitionService.SetNavigationInTransition(uiElement, _savedNavigationInTransition);
                    _savedNavigationInTransition = null;
                    TransitionService.SetNavigationOutTransition(uiElement, _savedNavigationOutTransition);
                    _savedNavigationOutTransition = null;
                }

                _frame = null;
                _frameContentWhenOpened = null;
            }

            var taskResult = new DateTimeResult(TaskResult.Cancel);

            if (_dateTimePickerPage != null)
            {
                if (_dateTimePickerPage.Value.HasValue)
                {
                    Value = _dateTimePickerPage.Value.Value;
                    taskResult = new DateTimeResult(TaskResult.OK) { Value = Value.Value };
                }
            }
            if (_timeSpanPickerPage != null)
            {
                if (_timeSpanPickerPage.Value.HasValue)
                {
                    Value = new DateTime(1970, 1, 1, 0, 0, 0, 0) + _timeSpanPickerPage.Value.Value;
                    taskResult = new DateTimeResult(TaskResult.OK) { Value = Value.Value };
                }
            }

            _dateTimePickerPage = null;
            _timeSpanPickerPage = null;

            Completed(this, taskResult);
        }

        private void OnDateFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _frameContentWhenOpened)
            {
                ClosePickerPage();
            }
            else if (_dateTimePickerPage == null)
            {
                _dateTimePickerPage = e.Content as IDateTimePickerPage;
                if (_dateTimePickerPage != null)
                {
                    _dateTimePickerPage.Value = Value.GetValueOrDefault(DateTime.Now);
                }
            }
        }

        private void OnTimeFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _frameContentWhenOpened)
            {
                ClosePickerPage();
            }
            else if (_timeSpanPickerPage == null)
            {
                _timeSpanPickerPage = e.Content as ITimeSpanPickerPage<TimeSpan>;
                if (_timeSpanPickerPage != null)
                {
                    if (Step != null)
                    {
                        _timeSpanPickerPage.StepFrequency = new TimeSpan(0, (int) (Step / 60), 0);
                    }
                    _timeSpanPickerPage.Maximum = new TimeSpan(23, 59, 0);
                    _timeSpanPickerPage.Minimum = new TimeSpan(0, 0, 0);
                    _timeSpanPickerPage.Value = DateTimeToTimeSpan(Value);
                }
            }
        }

        private static TimeSpan DateTimeToTimeSpan(DateTime? dt)
        {
            return dt.HasValue
                       ? new TimeSpan(0, dt.Value.Hour, dt.Value.Minute, dt.Value.Second, dt.Value.Millisecond)
                       : TimeSpan.Zero;
        }

        private void OnFrameNavigationStoppedOrFailed(object sender, EventArgs e)
        {
            ClosePickerPage();
        }

    }
}
