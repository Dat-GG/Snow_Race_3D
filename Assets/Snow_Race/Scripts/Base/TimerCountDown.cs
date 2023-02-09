using System;
using UniRx;

namespace SimpleTimer
{
    public class TimerCountDown : IDisposable
    {
        public CountStatus Status => _countStatus;

        private readonly Func<DateTime> _now;
        private readonly Action<TimeSpan> _step;
        private readonly Action _finish;
        private readonly TimeSpan _timeGap;
        private readonly DateTime _startTime;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private CountStatus _countStatus;
        private DateTime _beginDateTime;

        public enum CountStatus
        {
            Begin,
            Counting,
            Finished
        }

        /// <summary>
        /// Overload for start time is current time
        /// </summary>
        public TimerCountDown(Func<DateTime> now, TimeSpan timeGap, Action<TimeSpan> step, Action finish)
        {
            _now = now;
            _step = step;
            _finish = finish;
            _timeGap = timeGap;
            _countStatus = CountStatus.Begin;
        }

        /// <summary>
        /// Overload for start time is startTime
        /// </summary>
        public TimerCountDown(Func<DateTime> now, TimeSpan timeGap, DateTime startTime, Action<TimeSpan> step,
            Action finish)
        {
            _now = now;
            _step = step;
            _finish = finish;
            _timeGap = timeGap - (_now.Invoke() - startTime);
            _startTime = startTime;
            _countStatus = CountStatus.Begin;
        }

        public void Start()
        {
            _countStatus = CountStatus.Counting;

            if (_timeGap < TimeSpan.Zero)
            {
                _countStatus = CountStatus.Finished;
                _finish();
                return;
            }

            _beginDateTime = _now.Invoke();
            _step.Invoke(_timeGap);
            Observable.Interval(TimeSpan.FromSeconds(1f), Scheduler.MainThreadIgnoreTimeScale)
                      .Select(_ => _timeGap - (_now.Invoke() - _beginDateTime))
                      .Subscribe(t => {
                          if (t >= TimeSpan.Zero)
                              _step(t);
                          else
                          {
                              _countStatus = CountStatus.Finished;
                              _finish();
                              Dispose();
                          }
                      })
                      .AddTo(_disposables);
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}