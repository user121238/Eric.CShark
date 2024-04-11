using Eric.CShark.Extensions;

namespace Eric.CShark.Tests.ExtensionsTest
{
	[TestClass]
	public class ActionExtensionsTest
	{
		[TestMethod]
		public async Task TestDebounce()
		{
			int execCount = 0;
			Action action = () => execCount++;

			Action debounceAction = action.Debounce(1000);

			debounceAction.Invoke();
			debounceAction.Invoke();

			await Task.Delay(1500);

			Assert.AreEqual(execCount, 1);

		}

		[TestMethod]
		public async Task TestThrottle()
		{
			int execCount = 0;
			Action action = () => execCount++;

			var throttleAction = action.Throttle(1000);

			throttleAction.Invoke();

			throttleAction.Invoke();

			await Task.Delay(1500);

			Assert.AreEqual(execCount, 1);

			throttleAction.Invoke();

			await Task.Delay(1000);

			Assert.AreEqual(execCount, 2);
		}
	}
}
