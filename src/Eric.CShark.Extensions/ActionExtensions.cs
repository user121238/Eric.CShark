namespace Eric.CShark.Extensions
{
    public static class ActionExtensions
    {
	public static Action Debounce(this Action action, int delayMilliseconds)
	{
	    CancellationTokenSource? cts = null;

	    void DebounceAction()
	    {
		    cts?.Cancel();
		    cts = new CancellationTokenSource();
		    Task.Delay(delayMilliseconds, cts.Token).ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

	    return DebounceAction;
	}


	public static Action Throttle(this Action action, int delayMilliseconds)
	{
	    bool isThrottled = false;

	    void ThrottleAction()
	    {
		    if (!isThrottled)
		    {
			    action();
			    isThrottled = true;
			    Task.Delay(delayMilliseconds).ContinueWith(_ => isThrottled = false, TaskContinuationOptions.OnlyOnRanToCompletion);
		    }
	    }

	    return ThrottleAction;
	}
    }
}
