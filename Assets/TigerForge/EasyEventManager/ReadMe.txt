=======================================
 TIGERFORGE EASY EVENT MANAGER v.1.0.0
=======================================

Easy Event Manager is a lightweight Event Manager system very easy to use. All the Scripts are well commented so to make clear what they do and how they work.

HOW TO START
Simply create an Empty GameObject in your Scene and drag the EEventManager Script on it. Now you can use the Event Manager system in your C# Scripts.

TIGERFORGE NAMESPACE
To avoid class names conflicts, the Event Manager class is inside TigerForge namespace. Just put a "using TigerForge" at the beginning of your C# Scripts.

HOW IT WORKS
An Event can be seen as a message that is sent globally in the Scene. This message can be sent by anyone who is in the Scene and can be listened to by anyone who is in the Scene.
This means that the Event Manager is able to emit an Event (to send a message) and to listen to an Event (to listen to a message).
Moreover, the Event Manager can store any kind of data when an Event is emitted and can release this data to who is listening.

EMIT AN EVENT
You can emit an Event in any point of your C# Script simply using the EmitEvent() method specifying the name of this Event. The name is used to identify this specific Event among other Events:

EEventManger.EmitEvent("MY_EVENT_NAME");

Optionally, you can call this method attaching any kind of value (integer, float, boolean, string, GameObject, etc.). For example:

EEventManger.EmitEvent("MY_EVENT_NAME", 10);

LISTEN TO AN EVENT
Because an Event is emitted globally in the Scene, anyone is able to intercept emitted Events.
In your C# Script, use the StartListening() method specifying the name of the Event to listen to and the name of a function to call every time that Event is emitted:

EEventManger.StartListening("MY_EVENT_NAME", MyFunctionToCall);

Pay attention that the StartListening() method must be called once only inside your C# Script. Generally, can be a good way to put it in a Start() or Awake() Unity event.

READ AN EMITTED VALUE
If you use EmitEvent() method specifying a value, you can read this value after that Event is emitted.
The Event Manager has a generic GetData() method that releases the data as object. In this case, you must convert (cast) the value in the proper way. For example, in your MyFunctionToCall():

void MyFunctionToCall() {
    var myData = EEventManger.GetData("MY_EVENT_NAME");
}

However, the Event Manager has specific methods to get the emitted value already converted in a specific data type. Pay attention that you always have to know what kind of data you're managing.
For example, if your EmitEvent() method has emitted an integer value, you can use this method to get it:

void MyFunctionToCall() {
    int myIntegerData = EEventManger.GetInteger("MY_EVENT_NAME");
}

At the moment, the Event Manager has specific "Get" methods for Integer, Float, Boolean, String and GameObject data type.

STOP LISTENING
It's a good practice to stop listening to an Event if you don't need it anymore or, in general, if you need to suspend the listening:

EEventManger.StopListening("MY_EVENT_NAME", MyFunctionToCall);

Pay attention that you have to specify both the Event name and your call back function name.

OTHER METHODS
The Event Manager offers various methods to get information about the system, delete unneeded data, and so on.

MAKE PERSISTENT
The Event Manager exposes a "Make Persistent" option unselected by default.
If you select it, the GameObject the Event Managers is attached to will became persistent and always available in your whole game. 
