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

			_actionHelper.Throttle(action,1000);
			_actionHelper.Throttle(action,1000);

			await Task.Delay(1500);

			Assert.AreEqual(execCount, 1);

			_actionHelper.Throttle(action, 500);

			Assert.AreEqual(execCount, 2);

		

		}

	}
}
