# UnityThreading
Wrapper of .NET Thread for Unity.
## How to start
1) Import into your Unity project UnityThreading.dll in Assets/Plugins folder OR import source codes;
2) Create object of UnityThread type and put into constructor your delegate. For example:
```C#
private void Start()
{
  //...
  UnityThread unityThread = new UnityThread(SomeAction);
  //...
}

private void SomeAction()
{
  //Some action in thread
}
```
3) You can subscribe on Completed and Aborted thread events. **These events invoke in main thread!** For example:
```C#
private void Start()
{
  //...
  UnityThread unityThread = new UnityThread(SomeAction);
  unityThread.Completed += () =>
            {
                // Here you can use Unity features
            };
  unityThread.Aborted += () =>
            {
                // Here you can use Unity features
            };
  //...
}
//...
```
4) In order to control the state of the thread, you have the functions: Start(), Abort() and readonly property IsAlive. For example:
```C#
//...
private UnityThread _unityThread;
//...
private void Start()
{
  //...
  _unityThread = new UnityThread(SomeAction);
  _unityThread.Completed += () =>
            {
                // Here you can use Unity features
            };
  _unityThread.Aborted += () =>
            {
                // Here you can use Unity features
            };
  _unityThread.Start();
  //...
}
//...
private void LateUpdate()
{
  if(_unityThread.IsAlive && Input.GetKeyDown(KeyCode.S))
  {
    _unityThread.Abort();
  }
}
//...
```
**You do not need to Abort the UnityThread when you turn off the Unity application, this logic is already implemented in the core.**
