using System.Threading;
using System;

namespace Eric.CShark.Utility
{
	public class ActionHelper
	{
		private Timer _throttleTimer;
		private bool _isThrottled = false;


		/// <summary>
		/// 节流
		/// </summary>
		/// <param name="action"></param>
		/// <param name="delayMilliseconds"></param>
		public void Throttle(Action action, int delayMilliseconds)
		{
			if (!_isThrottled)
			{
				action.Invoke();
				_isThrottled = true;

				// 创建计时器以在一段时间后重置isThrottled标志
				_throttleTimer = new Timer(_ =>
				{
					_isThrottled = false;
				}, null, delayMilliseconds, Timeout.Infinite);
			}
		}


		private Timer _debounceTimer;


		/// <summary>
		/// 防抖
		/// </summary>
		/// <param name="action"></param>
		/// <param name="delayMilliseconds"></param>
		public void Debounce(Action action, int delayMilliseconds)
		{
			// 清除之前的计时器
			_debounceTimer?.Change(Timeout.Infinite, Timeout.Infinite);

			// 创建新的计时器
			_debounceTimer = new Timer(_ =>
			{
				action.Invoke();
			}, null, delayMilliseconds, Timeout.Infinite);
		}
	}
}
