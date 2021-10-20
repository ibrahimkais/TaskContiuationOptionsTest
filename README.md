I see lots of developers are using the task continuation option in C# Asynchronous and parallel programming without knowing they can provide multiple continuation options to the same task to handle different scenarios and cases and only one continuation option will be executed according to the task state.
For example when we need to execute a task and we need to handle the happy case if it succeeds and worst case if it fails. see the following code.
Here I initialized a new task.

Task task = new Task((obj)=>{....});

now I'll add the happy case scenario continuation and this will be executed only when the task succeeds

_ = task.ContinueWith(prevtask=>{....}, TaskContinuationOptions.OnlyOnRanToCompletion);
_ = task.ContinueWith(prevtask=>{....}, TaskContinuationOptions.OnlyOnRanToCompletion);
_ = task.ContinueWith(prevtask=>{....}, TaskContinuationOptions.OnlyOnRanToCompletion);

notice I added multiple continuations for OnlyOnRanToCompletion option. they will randomly run according to the task scheduling and same if you do for the other options
here I'll add the worst case scenario continuation and this will be executed only if the task fails

_= task.ContinueWith(prevTask=>{....}, TaskContinuationOptions.OnlyOnFaulted);

Also what if it canceled, no problem just add the continuation option to the task and this will be executed if the task is canceled

_= task.ContinueWith(prevTask=>{....}, TaskContinuationOptions.OnlyOnCanceled);

at last you can start the task or pass it to other functions

await task;

just notice I'm still using the same task object I initialized at the beginning and neglected all the returned tasks from the continuation options. 
still you can used these returned tasks to handle more cases but this out of the scope of this post.

Source: CLR via C# 4th. PART V Threading page 706
