using Eric.CShark.Utility;

namespace Eric.CShark.Tests.ActionHelperTest
{
	[TestClass]
	public class ActionHelperTests
	{
		private ActionHelper _actionHelper;


		[TestInitialize]
		public void Initialize()
		{
			_actionHelper = new ActionHelper();
		}



		[TestMethod]
		public async Task TestDebounce()
		{

			int execCount = 0;

			Action action = () => execCount++;

			_actionHelper.Debounce(action, 1000);
			_actionHelper.Debounce(action, 1000);

			await Task.Delay(3000);

			Assert.AreEqual(execCount, 1);
		}


		[TestMethod]
		public async Task TestThrottle()
		{
			int execCount = 0;

			Action action = () => execCount++;

			_actionHelper.Throttle(action, 1000);
			_actionHelper.Throttle(action, 1000);

			await Task.Delay(1500);

			Assert.AreEqual(execCount, 1);

			_actionHelper.Throttle(action, 500);

			Assert.AreEqual(execCount, 2);



		}

		[TestMethod("GetThrottleAction")]
		public async Task TestGetThrottleAction()
		{
			int execCount = 0;

			Action action = () => execCount++;

			var newAction = _actionHelper.GetThrottleAction(action, 1000);

			newAction.Invoke();
			newAction.Invoke();
			newAction.Invoke();
			newAction.Invoke();

			await Task.Delay(2000);

			Assert.AreEqual(execCount, 1);

			newAction.Invoke();

			Assert.AreEqual(execCount, 2);

		}



		/// <summary>
		/// 测试防抖
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task TestDebounce2()
		{

			//action 总共会被调用5次，但实际执行只有2次

			int count = 0;

			Action action = () => count++;

			_actionHelper.Debounce(action, 1000);
			_actionHelper.Debounce(action, 1000);
			_actionHelper.Debounce(action, 1000);
			_actionHelper.Debounce(action, 1000);


			await Task.Delay(2000);

			Assert.AreEqual(count, 1);


			_actionHelper.Debounce(action, 1000);

			await Task.Delay(2000);

			Assert.AreEqual(count, 2);

		}


		[TestMethod]
		public async Task TestGetDebounceAction()
		{
			int count = 0;

			void Action() => count++;

			var newAction = _actionHelper.GetDebounceAction(Action, 1000);

			newAction.Invoke();
			newAction.Invoke();
			newAction.Invoke();
			newAction.Invoke();
			
			await Task.Delay(2000);

			Assert.AreEqual(count, 1);

			newAction.Invoke();

			await Task.Delay(2000);

			Assert.AreEqual(count, 2);
		}

	}
}
